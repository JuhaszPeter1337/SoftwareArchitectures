using EventFinderServer.DTOs;
using EventFinderServer.Models;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.DataService
{
    public class EventService
    {
        static List<User> Users = new List<User> { };
        static List<Event> Events = new List<Event>
        {
            new Event{
                EventId = 0, Title = "Football match", 
                Description = "Hungary plays against Serbia. " +
                "It will be a difficult match for the Hungarian team. " +
                "Hungary plays at home. Serbs are not the best nowadays " +
                "but they have a very good footbal team. The football" +
                " match takes place at Puskas Arena. The price is 15000 " +
                "Forint. You can buy the ticket online.",
                ImageLocation = "football.jpg",
                BeginTime = Convert.ToDateTime("2020.11.15. 20:45:00"), 
                EndTime = Convert.ToDateTime("2020.11.15. 23:00:00"),
                EventInterests = new List<Interests> 
                {
                    Interests.WatchSports,
                    Interests.PlaySports
                },
                EventLanguages = new List<Languages>
                {
                    Languages.Hungarian
                },
                Messages = new List<Message>
                {
                    new Message(){ Sender = "Peti", Content = "Kecske" },
                    new Message(){ Sender = "Bálint", Content = "Cica" },
                }
            },
            new Event{
                EventId = 1, Title = "Hiking tour",
                Description = "Hiking can help you relieve your stress and " +
                "clear your head for some time. Planning a hike over the " +
                "weekends can help in reducing your blood pressure and " +
                "cortisol levels that rise up due to the busy work schedule" +
                " you have for the whole week. But apart from the physical " +
                "benefits, hiking is also good food for your soul and here is why.",
                ImageLocation = "hiking.jpg",
                BeginTime = Convert.ToDateTime("2020.11.18. 08:00:00"),
                EndTime = Convert.ToDateTime("2020.11.19. 19:00:00"),
                EventInterests = new List<Interests>
                {
                    Interests.Hiking
                },
                EventLanguages = new List<Languages>
                {
                    Languages.English
                },
                Messages = new List<Message>
                {
                    new Message(){ Sender = "Peti", Content = "Kecske" },
                    new Message(){ Sender = "Bálint", Content = "Cica" },
                }
            },
            new Event{
                EventId = 2 , Title = "Museum tour",
                Description = "The Louvre or the Louvre Museum is the world's " +
                "largest art museum and a historic monument in Paris, France. " +
                "The museum is housed in the Louvre Palace, originally built as " +
                "the Louvre castle in the late 12th to 13th century under Philip II." +
                " Remnants of the fortress are visible in the basement of the museum. " +
                "Due to urban expansion, the fortress eventually lost its defensive function, " +
                "and in 1546 Francis I converted it into the primary residence of the French Kings.",
                ImageLocation = "museum.jpg",
                BeginTime = Convert.ToDateTime("2020.11.24. 14:00:00"),
                EndTime = Convert.ToDateTime("2020.11.24. 18:00:00"),
                EventInterests = new List<Interests>
                {
                    Interests.Museum
                },
                EventLanguages = new List<Languages>
                {
                    Languages.German
                },
                Messages = new List<Message> { }
            },
            new Event{
                EventId = 3 , Title = "Cooking course",
                Description = "It's safe to say people are cooking at home now more than ever." +
                " And since we're all stuck inside, world-class chefs to locals in Italy have gone online" +
                " to teach virtual cooking courses. The best part is, there’s a class for everyone," +
                " including total beginners in the kitchen, families," +
                " and even advanced cooks who have more time on their hands.",
                ImageLocation = "cooking.jpg",
                BeginTime = Convert.ToDateTime("2020.12.06. 8:00"),
                EndTime = Convert.ToDateTime("2020.12.06. 16:00"),
                EventInterests = new List<Interests>
                {
                    Interests.Cooking
                },
                EventLanguages = new List<Languages>
                {
                    Languages.Russian
                },
                Messages = new List<Message> { }
            },
             new Event{
                EventId = 4 , Title = "Cinema",
                Description = "We are pleased to announce that the renovated, redesigned Cinema City Mammut I." +
                 " is waiting for You...furhtermore its brand new VIP cinema section finally opened its doors" +
                 " as well! Watch Avengers: Engame in 3D.",
                ImageLocation = "cinema.jpg",
                BeginTime = Convert.ToDateTime("2020.12.03. 16:30"),
                EndTime = Convert.ToDateTime("2020.12.03. 19:00"),
                EventInterests = new List<Interests>
                {
                    Interests.Cinema
                },
                EventLanguages = new List<Languages>
                {
                    Languages.English
                },
                Messages = new List<Message> { }
            },
             new Event{
                EventId = 5 , Title = "Playing icehockey",
                Description = "Ice Hockey is one of the world’s fastest team sports full of skilful handling," +
                 " tactics, speed and determination. Learn how to play this exciting sport and you could join" +
                 " an ice hockey club and compete or simply enjoy ice hockey as a leisure activity, participating" +
                 " in games with players of all abilities.",
                ImageLocation = "icehockey.jpg",
                BeginTime = Convert.ToDateTime("2020.12.12. 18:00"),
                EndTime = Convert.ToDateTime("2020.12.12. 20:00"),
                EventInterests = new List<Interests>
                {
                    Interests.PlaySports
                },
                EventLanguages = new List<Languages>
                {
                    Languages.Hungarian
                },
                Messages = new List<Message> { }
            },
        };

        internal bool Edit(ProfileDTO user)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(user.username));
            if (u == null)
                return false;

            if (user.password != null)
                u.Password = PWHasher.Hash(user.password);

            u.Interests = user.interests;
            u.Languages = user.languages;

            return true;
        }

        public void AddFavorite(int eventId, string username)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(username));
            if (u == null || !u.LoggedIn)
                return;

            if(EventExists(eventId) && !u.Favorites.Contains(eventId))
                u.Favorites.Add(eventId);
        }

        public void RemoveFavorite(int eventId, string username)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(username));
            if (u == null || !u.LoggedIn)
                return;

            if (EventExists(eventId) && u.Favorites.Contains(eventId))
                u.Favorites.Remove(eventId);
        }

        public List<int> GetFavorites(string username)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(username));
            if (u == null || !u.LoggedIn)
                return null;

            List<int> res = new List<int>();

            foreach (var e in Events)
            {
                if (u.Favorites.Contains(e.EventId))
                {
                    res.Add(e.EventId);
                }
            }

            return res;
        }

        public ProfileDTO Login(string username, string password)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(username));
            if (u == null || !PWHasher.Verify(password, u.Password))
                return null;

            u.LoggedIn = true;
            ProfileDTO p = new ProfileDTO { username = u.Username, password = null, interests = u.Interests, languages = u.Languages, favorites = u.Favorites };

            return p;
        }

        private bool EventExists(int eventId)
        {
            return Events.FirstOrDefault(e => e.EventId.Equals(eventId)) != null;
        }

        public List<EventDTO> GetEvents(string username)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(username));
            if (u == null || !u.LoggedIn)
                return null;

            List<EventDTO> res = new List<EventDTO>();

            foreach(var e in Events)
            {
                if(e.EventInterests.Any(ei => u.Interests[(int)ei] && e.EventLanguages.Any(el => u.Languages[(int)el])))
                {
                    res.Add(e.MakeDTO(u.Favorites.Contains(e.EventId)));
                }
            }

            return res;
        }

        public bool SendMessage(int eventId, MessageDTO message)
        {
            try
            {
                Events[eventId].Messages.Add(new Message { Sender = message.username, Content = message.content });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Logout(string username)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(username));
            if (u == null || !u.LoggedIn)
                return false;

            u.LoggedIn = false;
            return true;
        }

        public bool Register(ProfileDTO user)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(user.username));
            if (u != null)
                return false;
            User newuser = new User { Id = User.UserId++, Username = user.username, Password = PWHasher.Hash(user.password), LoggedIn = false, Interests = user.interests, Languages = user.languages, Favorites = new List<int>() };

            Users.Add(newuser);

            return true;
        }
    }
}
