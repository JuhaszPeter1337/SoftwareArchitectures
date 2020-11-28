using EventFinderServer.DAL;
using EventFinderServer.DTOs;
using EventFinderServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventFinderServer.DataService
{
    public class EventService
    {
        private readonly EventFinderDBC _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public EventService(EventFinderDBC context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        internal async Task<bool> Edit(string username, ProfileDTO user, ChangePassDTO pass)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));
            if (u == null)
                return false;

            IdentityResult res = IdentityResult.Success;
            if (pass != null)
            {
                res = await _userManager.ChangePasswordAsync(u, pass.currentpass, pass.newpass);
            }

            if (res.Succeeded)
            {
                u.Interests = user.interests;
                u.Languages = user.languages;
                _context.SaveChanges();
            }

            return res.Succeeded;
        }

        public void AddFavorite(string username, int eventId)
        {
            var u = _context.Users.Include(u => u.Favorites).FirstOrDefault(u => u.UserName.Equals(username));
            var e = _context.Events.FirstOrDefault(e => e.Id.Equals(eventId));
            if (u == null || e == null)
                return;

            if (u.Favorites == null)
                u.Favorites = new List<UserFavorites>();

            var f = u.Favorites.FirstOrDefault(f => f.EventId.Equals(eventId));
            if (f == null)
            {
                var uf = new UserFavorites { Event = e, EventId = e.Id, User = u, UserId = u.Id };
                u.Favorites.Add(uf);
                _context.SaveChanges();
            }
        }

        public void RemoveFavorite(string username, int eventId)
        {
            var u = _context.Users.Include(u => u.Favorites).FirstOrDefault(u => u.UserName.Equals(username));
            if (u == null)
                return;

            var f = u.Favorites?.FirstOrDefault(f => f.EventId.Equals(eventId));
            if (EventExists(eventId) && f != null)
            {
                u.Favorites.Remove(f);
                _context.SaveChanges();
            }
        }

        internal bool AddEvent(EventDTO newevent)
        {
            try
            {
                _context.Events.Add(new Event { Title = newevent.title, EventInterest = newevent.interests, EventLanguages = newevent.languages,
                    BeginTime = DateTime.ParseExact(newevent.beginning, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact(newevent.ending, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
                    Description = newevent.description, Messages = new List<Message>() });
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User> Login(string username, string password)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));

            var result = await _signInManager.CheckPasswordSignInAsync(u, password, false);

            return u;
        }

        public User GetUser(string username)
        {
            var u = _context.Users.Include(u => u.Favorites).FirstOrDefault(u => u.UserName.Equals(username));
            return u;
        }

        private bool EventExists(int eventId)
        {
            return _context.Events.FirstOrDefault(e => e.Id.Equals(eventId)) != null;
        }

        public List<EventDTO> GetEvents(string username)
        {
            var u = _context.Users.Include(u => u.Favorites).FirstOrDefault(u => u.UserName.Equals(username));
            if (u == null)
                return null;

            List<EventDTO> res = new List<EventDTO>();

            foreach(var e in _context.Events.Include(e => e.Messages))
            {
                if(((u.Interests & e.EventInterest) != 0 && (u.Languages & e.EventLanguages) != 0))
                {
                    bool? isfavorite = u.Favorites?.Any(f => f.EventId == e.Id);
                    bool f = isfavorite.HasValue ? isfavorite.Value : false;
                    res.Add(e.MakeDTO(f));
                }
            }

            return res;
        }

        public bool SendMessage(string username, int eventId, string message)
        {
            try
            {
                var e = _context.Events.FirstOrDefault(e => e.Id == eventId);
                if (e.Messages == null)
                    e.Messages = new List<Message>();
                e.Messages.Add(new Message { Sender = username, Content = message });
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Logout(string username)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));
            if (u == null)
                return false;

            _signInManager.SignOutAsync();
            
            return true;
        }

        public async Task<bool> Register(ProfileDTO user, string password)
        {
            if (_context.Users.Any(x => x.UserName == user.username))
                return false;
            var u = new User { UserName = user.username, Interests = user.interests, Languages = user.languages, Favorites = new List<UserFavorites>() };
            var result = await _userManager.CreateAsync(u, password);
            return result.Succeeded;
        }
    }
}
