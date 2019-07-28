using System;
using System.IO;
using System.Threading;

namespace BLive.RecordServer
{
    public class SingleRoomRecorder
    {
        private Timer _roomAliveChecker;
        private FlvDownloader _downloader;
        private string _realRoomID;
        private int _liveStatus;
        private bool _stopped = false;
        public void StartMonitor()
        {
            _roomAliveChecker.Change(0, 10000);
            _stopped = false;
        }
        public void StopMonitor()
        {
            _roomAliveChecker.Change(Timeout.Infinite, Timeout.Infinite);
            _stopped = true;
            _downloader.Stop();
        }
        public SingleRoomRecorder(string roomid,string nickname)
        {
            var info = BilibiliAPI.GetRoomInfoAsync(roomid).Result;
            _realRoomID = info.data.room_id.ToString();
            _roomAliveChecker = new Timer(RoomAliveCheck, _roomAliveChecker, Timeout.Infinite, Timeout.Infinite);
            _downloader = new FlvDownloader(BilibiliAPI.GetTrueUrl(_realRoomID).Result,
                Path.Combine(".", "livearchives"), nickname);
            _downloader.DownloadInterruptedEvent += (sender, e) =>
            {
                info = BilibiliAPI.GetRoomInfoAsync(_realRoomID).Result;
                _liveStatus = info.data.live_status;
                if (_liveStatus == 1)
                {
                    if (!_stopped)
                        _downloader.Start();
                }

                Uploader.TryUpload(new Models.RecordTask { RoomID = roomid, TaskName = nickname }, ((FlvDownloader)sender).FullPath);
            };
        }

        private void RoomAliveCheck(object sender)
        {
            var info = BilibiliAPI.GetRoomInfoAsync(_realRoomID).Result.data;
            _liveStatus = info.live_status;
            if (_liveStatus == 1) // is living
            {
                if (!_downloader.Running)
                    _downloader.Start();
            }
            else if (_downloader.Running)
            {
                _downloader.Stop();
            }
        }
    }
}