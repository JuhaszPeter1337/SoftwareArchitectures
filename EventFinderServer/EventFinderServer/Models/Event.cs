using EventFinderServer.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.Models
{
    public class Event
    {
        static string ImageRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageLocation { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Interests> EventInterests { get; set; }
        public List<Languages> EventLanguages { get; set; }
        public List<Message> Messages { get; set; }

        public EventDTO MakeDTO()
        {
            var res = new EventDTO { event_id = EventId, title = Title, description = Description, beginning = BeginTime.ToString(), ending = EndTime.ToString() };
            res.messages = new MessageDTO[Messages.Count];
            res.image = "data:image/jpeg;base64, " + Convert.ToBase64String(File.ReadAllBytes(Path.Combine(ImageRoot, ImageLocation)));

            for (int i = 0; i < Messages.Count; i++)
                res.messages[i] = Messages[i].MakeDTO();
            return res;
        }
    }
}
