using System;
using System.Net.Http;
using System.Threading.Tasks;
using BLive.RecordServer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BLive.RecordServer
{
    public static class BilibiliAPI
    {
        public static async Task<RoomInfo> GetRoomInfoAsync(string roomid)
        {
            using (HttpClient hc = new HttpClient())
            {
                var response = await hc.GetAsync($"http://api.live.bilibili.com/room/v1/Room/get_info?id={roomid}");
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RoomInfo>(result);
            }
        }
        public static async Task<string> GetTrueUrl(string roomid)
        {
            using (var hc = new HttpClient())
            {
                if (roomid == null)
                {
                    throw new ArgumentNullException("roomid");
                }

                var url =
                    $"https://api.live.bilibili.com/room/v1/Room/playUrl?cid={roomid}&otype=json&quality=0&platform=web";
                var response = await hc.GetAsync(url);
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonResult = JObject.Parse(jsonString);
                var trueUrl = jsonResult["data"]["durl"][0]["url"].ToString();
                return trueUrl;
            }
        }
    }
}