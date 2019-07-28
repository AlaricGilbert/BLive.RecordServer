using System.Collections.Generic;

namespace BLive.RecordServer.Models
{
    public class RoomInfo
    {
        public class Data
        {
            public class Frame
            {
                public string name { get; set; }
                public string value { get; set; }
                public int position { get; set; }
                public string desc { get; set; }
                public int area { get; set; }
                public int area_old { get; set; }
                public string bg_color { get; set; }
                public string bg_pic { get; set; }
                public bool use_old_area { get; set; }
            }

            public class Badge
            {
                public string name { get; set; }
                public int position { get; set; }
                public string value { get; set; }
                public string desc { get; set; }
            }

            public class MobileFrame
            {
                public string name { get; set; }
                public string value { get; set; }
                public int position { get; set; }
                public string desc { get; set; }
                public int area { get; set; }
                public int area_old { get; set; }
                public string bg_color { get; set; }
                public string bg_pic { get; set; }
                public bool use_old_area { get; set; }
            }

            public class NewPendants
            {
                public Frame frame { get; set; }
                public Badge badge { get; set; }
                public MobileFrame mobile_frame { get; set; }
                public object mobile_badge { get; set; }
            }

            public class StudioInfo
            {
                public int status { get; set; }
                public List<object> master_list { get; set; }
            }
            public int uid { get; set; }
            public int room_id { get; set; }
            public int short_id { get; set; }
            public int attention { get; set; }
            public int online { get; set; }
            public bool is_portrait { get; set; }
            public string description { get; set; }
            public int live_status { get; set; }
            public int area_id { get; set; }
            public int parent_area_id { get; set; }
            public string parent_area_name { get; set; }
            public int old_area_id { get; set; }
            public string background { get; set; }
            public string title { get; set; }
            public string user_cover { get; set; }
            public string keyframe { get; set; }
            public bool is_strict_room { get; set; }
            public string live_time { get; set; }
            public string tags { get; set; }
            public int is_anchor { get; set; }
            public string room_silent_type { get; set; }
            public int room_silent_level { get; set; }
            public int room_silent_second { get; set; }
            public string area_name { get; set; }
            public string pendants { get; set; }
            public string area_pendants { get; set; }
            public List<string> hot_words { get; set; }
            public int hot_words_status { get; set; }
            public string verify { get; set; }
            public NewPendants new_pendants { get; set; }
            public string up_session { get; set; }
            public int pk_status { get; set; }
            public int pk_id { get; set; }
            public int battle_id { get; set; }
            public int allow_change_area_time { get; set; }
            public int allow_upload_cover_time { get; set; }
            public StudioInfo studio_info { get; set; }
        }

        public int code { get; set; }
        public string msg { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
}