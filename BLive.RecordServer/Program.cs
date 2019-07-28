using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BLive.RecordServer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Bilibili Live Recorder for Server Environment.");
            Dictionary<string, string> taskDictionary;
            Dictionary<string, SingleRoomRecorder> recoderDictionary = new Dictionary<string, SingleRoomRecorder>();
            if (!Directory.Exists("livearchives"))
                Directory.CreateDirectory("livearchives");
            if (File.Exists("tasks.json"))
                taskDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("tasks.json"));
            else
                taskDictionary = new Dictionary<string, string>();
            foreach (var pair in taskDictionary)
            {
                recoderDictionary.Add(pair.Key, new SingleRoomRecorder(pair.Value, pair.Key));
            }
            bool running = true;
            bool enable = false;
            Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>();
            commands.Add("help", (paras) =>
            {
                Console.WriteLine("not implemented yet..");
            });
            commands.Add("room", (paras) =>
            {
                switch (paras[1])
                {
                    case "add":
                        taskDictionary.Add(paras[2], paras[3]);
                        var recorder = new SingleRoomRecorder(paras[3], paras[2]);
                        recoderDictionary.Add(paras[2], recorder);
                        if (enable)
                            recorder.StartMonitor();
                        File.WriteAllText("tasks.json", JsonConvert.SerializeObject(taskDictionary));
                        Console.WriteLine("Room added.");
                        break;
                    case "remove":
                        if (enable)
                            recoderDictionary[paras[2]].StopMonitor();
                        taskDictionary.Remove(paras[2]);
                        recoderDictionary.Remove(paras[2]);
                        File.WriteAllText("tasks.json", JsonConvert.SerializeObject(taskDictionary));
                        Console.WriteLine("Room removed.");
                        break;
                    case "list":
                        foreach (var pair in taskDictionary)
                        {
                            Console.WriteLine($"Task \"{pair.Key}\": room {pair.Value}");
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown command for room.");
                        break;
                }
            });
            commands.Add("enable", (paras) =>
            {
                switch (paras[1])
                {
                    case "record":
                        enable = true;
                        foreach (var recorder in recoderDictionary.Values)
                        {
                            recorder.StartMonitor();
                        }
                        Console.WriteLine("Record service enabled.");
                        break;
                    case "upload":
                        Uploader.DoUpload = true;
                        Console.WriteLine("Upload service enabled. You should notice that this function needs BaiduPCS-Go to work.");
                        break;
                    default:
                        Console.WriteLine("Unknown service.");
                        break;
                }
            });
            commands.Add("disable", (paras) =>
            {
                switch (paras[1])
                {
                    case "record":
                        enable = false;
                        foreach (var recorder in recoderDictionary.Values)
                        {
                            recorder.StopMonitor();

                        }
                        Console.WriteLine("Record service disabled.");
                        break;
                    case "upload":
                        Uploader.DoUpload = true;
                        Console.WriteLine("Upload service disabled.");
                        break;
                    default:
                        Console.WriteLine("Unknown service.");
                        break;
                }
            });
            commands.Add("quit", (paras) =>
            {
                if (enable)
                {
                    Console.WriteLine("You should disable record service before quit.");
                }
                else
                {
                    running = false;
                    if (Uploader.DoUpload)
                    {
                        Console.WriteLine("We found that you enabled upload service, program will exit after all upload task finished");
                    }
                }
            });
            while (running)
            {
                Console.Write("> ");
                string command = Console.ReadLine();
                var splits = command.Split(" ");
                if (commands.ContainsKey(splits[0]))
                    commands[splits[0]](splits);
                else
                    Console.WriteLine("Unknown command.");
            }
        }
    }
}