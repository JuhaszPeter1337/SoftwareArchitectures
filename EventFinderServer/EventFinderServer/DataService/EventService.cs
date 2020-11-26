using EventFinderServer.DAL;
using EventFinderServer.DTOs;
using EventFinderServer.Models;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            }

            return res.Succeeded;
        }

        public void AddFavorite(string username, int eventId)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));
            if (u == null)
                return;

            var f = u.Favorites.FirstOrDefault(f => f.Id.Equals(eventId));
            if (EventExists(eventId) && f == null)
                u.Favorites.Add(f);
        }

        public void RemoveFavorite(string username, int eventId)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));
            if (u == null)
                return;

            var f = u.Favorites.FirstOrDefault(f => f.Id.Equals(eventId));
            if (EventExists(eventId) && f != null)
                u.Favorites.Remove(f);
        }

        public List<int> GetFavorites(string username)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));
            if (u == null)
                return null;

            List<int> res = new List<int>();

            foreach (var e in _context.Events)
            {
                if (u.Favorites.Contains(e))
                {
                    res.Add(e.Id);
                }
            }

            return res;
        }

        public async Task<User> Login(string username, string password)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));

            var result = await _signInManager.PasswordSignInAsync(username, password, true, lockoutOnFailure: true);

            return u;
        }

        public User GetUser(string username)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));
            return u;
        }

        private bool EventExists(int eventId)
        {
            return _context.Events.FirstOrDefault(e => e.Id.Equals(eventId)) != null;
        }

        public List<EventDTO> GetEvents(string username)
        {
            var u = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));
            if (u == null)
                return null;

            List<EventDTO> res = new List<EventDTO>();

            foreach(var e in _context.Events)
            {
                if(u.Interests.HasFlag(e.EventInterest) && e.EventLanguages.HasFlag(u.Languages))
                {
                    res.Add(e.MakeDTO(u.Favorites.Contains(e)));
                }
            }

            return res;
        }

        public bool SendMessage(string username, int eventId, string message)
        {
            try
            {
                _context.Events.ElementAt(eventId).Messages.Add(new Message { Sender = username, Content = message });
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
            var u = new User { UserName = user.username, Interests = user.interests, Languages = user.languages, Favorites = new List<Event>() };
            var result = await _userManager.CreateAsync(u, password);
            //if(result.Succeeded)
            //    await _userManager.AddClaimAsync(u, new Claim(ClaimTypes.NameIdentifier, user.username));
            return result.Succeeded;
        }
    }
}
