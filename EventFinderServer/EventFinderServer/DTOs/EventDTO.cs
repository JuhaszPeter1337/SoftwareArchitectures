using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.DTOs
{
    public class EventDTO
    {
        public int event_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string begin_time { get; set; }
        public string end_time { get; set; }
        public MessageDTO[] messages { get; set; }
    }
}
