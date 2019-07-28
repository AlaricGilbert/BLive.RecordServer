using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using FlvStream2File;

namespace BLive.RecordServer
{
    public class FlvDownloader
    {
        public string FullPath { get; private set; }
        private string _url;
        private string _path;
        private string _fname;
        private HttpClient _hc;
        private FlvStream2FileWriter _stream2FileWriter;
        private Stream _flvNetStream;
        public event EventHandler DownloadInterruptedEvent;
        public bool Running { get; private set; }
        public FlvDownloader(string url, string path, string fname)
        {
            _url = url;
            _path = path;
            _fname = fname;
        }

        public void Start()
        {
            Running = true;
            Thread downloadThread = null;
            downloadThread = new Thread(() =>
            {

                _hc = new HttpClient();
                FullPath = Path.Combine(_path, _fname + DateTime.Now.ToString("_yyyyMMdd_Hmmss") + ".flv");
                _stream2FileWriter =
                    new FlvStream2FileWriter(FullPath);
                try
                {
                    _flvNetStream = _hc.GetStreamAsync(_url).Result;
                }
                catch (Exception)
                {
                    DownloadInterruptedEvent?.Invoke(this, null);
                    return;
                }

                int buffer_size = 4096;
                byte[] buffer = new byte[buffer_size];
                AsyncCallback callback = null;
                callback = ar =>
                {
                    try
                    {
                        // Call EndRead.
                        int bytesRead = _flvNetStream.EndRead(ar);

                        // Process the bytes here.
                        if (bytesRead != buffer.Length)
                        {
                            if (bytesRead == 0)
                            {
                                _flvNetStream.Dispose();
                                _flvNetStream = null;
                                Running = false;
                                return;
                            }
                            _stream2FileWriter.Write(buffer.Take(bytesRead).ToArray());
                        }
                        else
                        {
                            _stream2FileWriter.Write(buffer);
                        }
                        _flvNetStream.BeginRead(buffer, 0, buffer_size, callback, null);

                    }
                    catch (Exception e)
                    {
                        // just close and return for now on error...
                        _flvNetStream?.Dispose();
                        _flvNetStream = null;
                        _hc?.Dispose();
                        _hc = null;
                        Running = false;
                        DownloadInterruptedEvent?.Invoke(this, null);
                        return;
                    }
                };
                _flvNetStream.BeginRead(buffer, 0, buffer_size, callback, null);
                while (_flvNetStream != null)
                {
                    Thread.Sleep(125);
                }
                _stream2FileWriter.FinallizeFile();
                downloadThread.Interrupt();
            });
            downloadThread.Start();
        }

        public void Stop()
        {
            _flvNetStream?.Close();
        }
    }
}