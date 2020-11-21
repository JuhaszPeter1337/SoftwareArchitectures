using EventFinderServer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Interests> EventInterests { get; set; }
        public List<Languages> EventLanguages { get; set; }
        public List<Message> Messages { get; set; }
    }
}
