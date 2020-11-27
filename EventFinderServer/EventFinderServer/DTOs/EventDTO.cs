using EventFinderServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.DTOs
{
    public class EventDTO
    {
        public int event_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public Interest interests { get; set; }
        public Language languages { get; set; }
        public string beginning { get; set; }
        public string ending { get; set; }
        public bool isfavorite { get; set; }
        public MessageDTO[] messages { get; set; }
    }
}
