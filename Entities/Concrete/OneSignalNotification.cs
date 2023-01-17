using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class OneSignalNotification
    {
        public string app_id { get; set; }
        public List<string> include_player_ids { get; set; }
        public Dictionary<string,string> contents { get; set; }
        public Dictionary<string,string> headings { get; set; }
        public List<Dictionary<string,string>> buttons { get; set; }
        public string name { get; set; }
        public string android_channel_id { get; set; }
        public int priority { get; set; }
        public bool content_available { get; set; }
        public int ttl { get; set; }


    }
}
