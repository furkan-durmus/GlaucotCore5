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
        public object contents { get; set; }
        public object data { get; set; }
        public object headings { get; set; }
        public object buttons { get; set; }
        public string name { get; set; }
        public string android_channel_id { get; set; }
        public int priority { get; set; }
        public bool content_available { get; set; }
        public int ttl { get; set; }



    }
}
