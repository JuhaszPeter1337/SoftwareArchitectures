using EventFinderServer.DTOs;
using System.Collections.Generic;

namespace EventFinderServer.Models
{
    public class User
    {
        public static int UserId = 0;

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool LoggedIn { get; set; }

        public bool[] Interests { get; set; }
        public bool[] Languages { get; set; }
        public List<int> Favourites { get; set; }

        public ProfileDTO MakeDTO()
        {
            return new ProfileDTO { username = Username, password = null, interests = Interests, languages = Languages, favourites = Favourites };
        }
    }
}
