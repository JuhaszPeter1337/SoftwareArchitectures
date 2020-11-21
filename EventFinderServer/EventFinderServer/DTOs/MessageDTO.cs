using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.DTOs
{
    public class MessageDTO
    {
        public int event_id { get; set; }
        public string username { get; set; }
        public string content { get; set; }
    }
}
