using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.Models
{
    public class Message
    {
        public int EventId { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; }
    }
}
