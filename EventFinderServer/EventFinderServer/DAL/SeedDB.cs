using EventFinderServer.DTOs;
using EventFinderServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.DAL
{
    public static class InitializeDB
    {
        public static void SeedDB(EventFinderDBC context)
        {
            context.Database.EnsureCreated();

            if (context.Events.Any())
            {
                return;   // DB has been seeded
            }

            Event[] events = new Event[]
            {
                new Event {
                    Title = "Football match",
                    Description = "Hungary plays against Serbia. " +
                    "It will be a difficult match for the Hungarian team. " +
                    "Hungary plays at home. Serbs are not the best nowadays " +
                    "but they have a very good footbal team. The football" +
                    " match takes place at Puskas Arena. The price is 15000 " +
                    "Forint. You can buy the ticket online.",
                    BeginTime = Convert.ToDateTime("2020.11.15. 20:45:00"),
                    EndTime = Convert.ToDateTime("2020.11.15. 23:00:00"),
                    EventInterest = Interest.WatchSports,
                    EventLanguages = Language.Hungarian,
                    Messages = new List<Message> { }
                },
                new Event{
                    Title = "Hiking tour",
                    Description = "Hiking can help you relieve your stress and " +
                    "clear your head for some time. Planning a hike over the " +
                    "weekends can help in reducing your blood pressure and " +
                    "cortisol levels that rise up due to the busy work schedule" +
                    " you have for the whole week. But apart from the physical " +
                    "benefits, hiking is also good food for your soul and here is why.",
                    BeginTime = Convert.ToDateTime("2020.11.18. 08:00:00"),
                    EndTime = Convert.ToDateTime("2020.11.19. 19:00:00"),
                    EventInterest = Interest.Hiking,
                    EventLanguages = Language.English,
                    Messages = new List<Message> { }
                },
                new Event{
                    Title = "Museum tour",
                    Description = "The Louvre or the Louvre Museum is the world's " +
                    "largest art museum and a historic monument in Paris, France. " +
                    "The museum is housed in the Louvre Palace, originally built as " +
                    "the Louvre castle in the late 12th to 13th century under Philip II." +
                    " Remnants of the fortress are visible in the basement of the museum. " +
                    "Due to urban expansion, the fortress eventually lost its defensive function, " +
                    "and in 1546 Francis I converted it into the primary residence of the French Kings.",
                    BeginTime = Convert.ToDateTime("2020.11.24. 14:00:00"),
                    EndTime = Convert.ToDateTime("2020.11.24. 18:00:00"),
                    EventInterest = Interest.Museum,
                    EventLanguages = Language.German,
                    Messages = new List<Message> { }
                },
                new Event{
                    Title = "Cooking course",
                    Description = "It's safe to say people are cooking at home now more than ever." +
                    " And since we're all stuck inside, world-class chefs to locals in Italy have gone online" +
                    " to teach virtual cooking courses. The best part is, there’s a class for everyone," +
                    " including total beginners in the kitchen, families," +
                    " and even advanced cooks who have more time on their hands.",
                    BeginTime = Convert.ToDateTime("2020.12.06. 8:00"),
                    EndTime = Convert.ToDateTime("2020.12.06. 16:00"),
                    EventInterest = Interest.Cooking,
                    EventLanguages = Language.Russian,
                    Messages = new List<Message> { }
                },
                 new Event{
                    Title = "Cinema",
                    Description = "We are pleased to announce that the renovated, redesigned Cinema City Mammut I." +
                     " is waiting for You...furhtermore its brand new VIP cinema section finally opened its doors" +
                     " as well! Watch Avengers: Engame in 3D.",
                    BeginTime = Convert.ToDateTime("2020.12.03. 16:30"),
                    EndTime = Convert.ToDateTime("2020.12.03. 19:00"),
                    EventInterest = Interest.Cinema,
                    EventLanguages = Language.English,
                    Messages = new List<Message> { }
                },
                 new Event{
                    Title = "Playing icehockey",
                    Description = "Ice Hockey is one of the world’s fastest team sports full of skilful handling," +
                     " tactics, speed and determination. Learn how to play this exciting sport and you could join" +
                     " an ice hockey club and compete or simply enjoy ice hockey as a leisure activity, participating" +
                     " in games with players of all abilities.",
                    BeginTime = Convert.ToDateTime("2020.12.12. 18:00"),
                    EndTime = Convert.ToDateTime("2020.12.12. 20:00"),
                    EventInterest = Interest.PlaySports,
                    EventLanguages = Language.Hungarian,
                    Messages = new List<Message> { }
                },
            };

            foreach (Event e in events)
            {
                context.Events.Add(e);
            }

            context.SaveChanges();
        }
    }
}
