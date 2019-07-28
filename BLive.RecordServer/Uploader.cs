using BLive.RecordServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BLive.RecordServer
{
    public static class Uploader
    {
        public static bool DoUpload { get; set; } = false;
        public static void TryUpload(RecordTask task,string path)
        {
            if (DoUpload)
            {
                ProcessStartInfo ps = new ProcessStartInfo
                {
                    FileName = "BaiduPCS-Go",
                    Arguments = $"u {path} /{task.TaskName}_archives/",
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8
                };
                Process p = new Process();

                p.StartInfo = ps;
                p.Start();
                p.WaitForExit();
                Console.WriteLine(p.StandardOutput.ReadToEnd());
            }
        }
    }
}
