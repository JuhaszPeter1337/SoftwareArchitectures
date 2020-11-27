using EventFinderServer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public MessageDTO MakeDTO()
        {
            return new MessageDTO { username = Sender, content = Content };
        }
    }
}
