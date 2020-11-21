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
            new Event{ EventId = 0, Name = ""}
        };

        public ProfileDTO Login(string username, string password)
        {
            var u = Users.FirstOrDefault(u => u.Username.Equals(username));
            if (u == null || !PWHasher.Verify(password, u.Password))
                return null;

            u.LoggedIn = true;
            ProfileDTO p = new ProfileDTO { username = u.Username, password = null, interests = u.Interests, languages = u.Languages, favourites = u.Favourites };

            return p;
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
            User newuser = new User { Id = User.UserId++, Username = user.username, Password = PWHasher.Hash(user.password), LoggedIn = false, Interests = user.interests, Languages = user.languages, Favourites = new List<int>() };

            Users.Add(newuser);

            return true;
        }
    }
}
