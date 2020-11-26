using EventFinderServer.DataService;
using EventFinderServer.DTOs;
using EventFinderServer.Models;
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
        private static readonly Dictionary<string, string> ConnectedIds = new Dictionary<string, string>();

        private readonly EventService _dataService;
        private readonly AppSettings _appSettings;

        public DefaultHub(EventService service, IOptions<AppSettings> appSettings)
        {
            _dataService = service;
            _appSettings = appSettings.Value;
        }
        

        public override async Task OnConnectedAsync()
        {
            ConnectedIds.Add(Context.ConnectionId, "");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string username = ConnectedIds[Context.ConnectionId];
            if (!String.IsNullOrEmpty(username))
            {
                _dataService.Logout(username);
            }
            ConnectedIds.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }

        [Authorize]
        public async Task SendMessage(int eventId, string message)
        {
            string username = ConnectedIds[Context.ConnectionId];
            if (!String.IsNullOrEmpty(username))
            {
                bool success = _dataService.SendMessage(username, eventId, message);
                if (success)
                    await Clients.All.SendAsync("Message", eventId, message);
            }
        }

        [Authorize]
        public async Task AddFavorite(int eventId)
        {
            string username = ConnectedIds[Context.ConnectionId];
            if (!String.IsNullOrEmpty(username))
            {
                _dataService.AddFavorite(username, eventId);
                await Clients.Caller.SendAsync("AddFav", eventId);
            }
        }

        [Authorize]
        public async Task RemoveFavorite(int eventId)
        {
            string username = ConnectedIds[Context.ConnectionId];
            if (!String.IsNullOrEmpty(username))
            {
                _dataService.RemoveFavorite(username, eventId);
                await Clients.Caller.SendAsync("RemoveFav", eventId);
            }
        }

        [Authorize]
        public async Task GetFavorites()
        {
            string username = ConnectedIds[Context.ConnectionId];
            if (!String.IsNullOrEmpty(username))
            {
                var favorites = _dataService.GetFavorites(username);
                if (favorites != null)
                    await Clients.Caller.SendAsync("Favorites", favorites);
            }
        }

        [Authorize]
        public async Task GetEvents()
        {
            string username = ConnectedIds[Context.ConnectionId];
            if (!String.IsNullOrEmpty(username))
            {
                var events = _dataService.GetEvents(username);
                if (events != null)
                    await Clients.Caller.SendAsync("Events", events.ToArray());
            }
        }

        [Authorize]
        public async Task EditProfile(ProfileDTO user, ChangePassDTO pass = null)
        {
            string username = ConnectedIds[Context.ConnectionId];
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

        public async Task LoginReq(string username, string password)
        {
            User u = await _dataService.Login(username, password);
            if(u != null)
            {
                ConnectedIds.Add(Context.ConnectionId, username);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, u.UserName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                ProfileDTO p = new ProfileDTO
                {
                    username = u.UserName,
                    interests = u.Interests,
                    languages = u.Languages,
                    favorites = u.Favorites.Select(x => x.Id).ToArray(),
                    token = tokenString
                };
                await Clients.Caller.SendAsync("Login", p);
            }
            else
                await Clients.Caller.SendAsync("LoginFailed");
        }

        [Authorize]
        public async Task LogoutReq()
        {
            string username = ConnectedIds[Context.ConnectionId];
            if (!String.IsNullOrEmpty(username))
            {
                bool success = _dataService.Logout(username);
                if (success)
                    ConnectedIds[Context.ConnectionId] = "";
                await Clients.Caller.SendAsync("Logout", success);
            }
        }
    }
}
