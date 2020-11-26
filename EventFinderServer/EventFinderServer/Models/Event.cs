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
        static Dictionary<Interest, string> ImageDict = new Dictionary<Interest, string>
        {
            {Interest.Cinema, "cinema.jpg"},
            {Interest.Cooking, "cooking.jpg"},
            {Interest.Hiking, "hiking.jpg"},
            {Interest.Museum, "museum.jpg"},
            {Interest.PlaySports, "icehockey.jpg"},
            {Interest.WatchSports, "football.jpg"}
        };

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public Interest EventInterest { get; set; }
        public Language EventLanguages { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<UserFavorites> UserFavorites { get; set; }

        public EventDTO MakeDTO(bool favorite = false)
        {
            var res = new EventDTO { event_id = Id, title = Title, description = Description, isfavorite = favorite, beginning = BeginTime.ToString(), ending = EndTime.ToString() };
            res.messages = new MessageDTO[Messages.Count];
            res.image = "data:image/jpeg;base64, " + Convert.ToBase64String(File.ReadAllBytes(Path.Combine(ImageRoot, ImageDict[EventInterest])));

            for (int i = 0; i < Messages.Count; i++)
                res.messages[i] = Messages.ElementAt(i).MakeDTO();
            return res;
        }
    }
}
