using EventFinderServer.DataService;
using EventFinderServer.DTOs;
using EventFinderServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventFinderServer.Hubs
{
    public class DefaultHub : Hub
    {
        private readonly EventService _dataService;
        private readonly AppSettings _appSettings;

        public DefaultHub(EventService service, IOptions<AppSettings> appSettings)
        {
            _dataService = service;
            _appSettings = appSettings.Value;
        }

        public async Task SendMessage(int eventId, string message)
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                bool success = _dataService.SendMessage(username, eventId, message);
                if (success)
                    await Clients.All.SendAsync("Message", eventId, new MessageDTO { username = username, content = message });
            }
        }

        public async Task AddEvent(EventDTO newevent)
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                bool success = _dataService.AddEvent(newevent);
                if (success)
                    await Clients.All.SendAsync("AddEvent", success);
            }
        }

        public async Task AddFavorite(int eventId)
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                _dataService.AddFavorite(username, eventId);
                await Clients.Caller.SendAsync("AddFav", eventId);
            }
        }

        public async Task RemoveFavorite(int eventId)
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                _dataService.RemoveFavorite(username, eventId);
                await Clients.Caller.SendAsync("RemoveFav", eventId);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task GetEvents()
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                var events = _dataService.GetEvents(username);
                if (events != null)
                    await Clients.Caller.SendAsync("Events", events.ToArray());
            }
        }

        public async Task GetUser()
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                var u = _dataService.GetUser(username);
                ProfileDTO user = u.MakeDTO();
                await Clients.Caller.SendAsync("GetUser", user);
            }
        }

        public async Task EditProfile(ProfileDTO user, ChangePassDTO pass = null)
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                bool success = await _dataService.Edit(username, user, pass);
                await Clients.Caller.SendAsync("Edit", success);
            }
        }

        public async Task RegisterReq(ProfileDTO user, string password)
        {
            bool success = await _dataService.Register(user, password);
            await Clients.Caller.SendAsync("Registered", success);
        }

        public async Task Login()
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                User u = _dataService.GetUser(username);
                if(u != null)
                {
                    ProfileDTO p = new ProfileDTO
                    {
                        username = u.UserName,
                        interests = u.Interests,
                        languages = u.Languages,
                        favorites = u.Favorites?.Select(x => x.EventId).ToArray(),
                        token = ""
                    };
                    await Clients.Caller.SendAsync("RedirectLogin", p);
                }
            }
        }

        public async Task LoginReq(string username, string password)
        {
            User u = await _dataService.Login(username, password);
            if(u != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var securekey = new SymmetricSecurityKey(key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, u.Id),
                    new Claim(ClaimTypes.Name, u.UserName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(securekey, SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                ProfileDTO p = new ProfileDTO
                {
                    username = u.UserName,
                    interests = u.Interests,
                    languages = u.Languages,
                    favorites = u.Favorites?.Select(x => x.EventId).ToArray(),
                    token = tokenString
                };
                await Clients.Caller.SendAsync("Login", p);
            }
            else
                await Clients.Caller.SendAsync("LoginFailed");
        }

        public async Task LogoutReq()
        {
            string username = Context.User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
            {
                bool success = _dataService.Logout(username);
                await Clients.Caller.SendAsync("Logout", success);
            }
        }
    }
}
