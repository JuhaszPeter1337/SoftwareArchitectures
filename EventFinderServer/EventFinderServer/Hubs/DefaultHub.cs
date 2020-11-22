using EventFinderServer.DataService;
using EventFinderServer.DTOs;
using EventFinderServer.Models;
using Helpers;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.Hubs
{
    public class DefaultHub : Hub
    {
        EventService _dataService;

        public DefaultHub(EventService service)
        {
            _dataService = service;
        }

        public async Task SendMessage(int eventId, MessageDTO message)
        {
            bool success = _dataService.SendMessage(eventId, message);
            if(success)
                await Clients.All.SendAsync("Message", eventId, message);
        }

        public async Task AddFavorite(int eventId, string username)
        {
            _dataService.AddFavorite(eventId, username);
            await Clients.Caller.SendAsync("AddFav", eventId);
        }

        public async Task RemoveFavorite(int eventId, string username)
        {
            _dataService.RemoveFavorite(eventId, username);
            await Clients.Caller.SendAsync("RemoveFav", eventId);
        }

        public async Task GetFavorites(string username)
        {
            var favorites = _dataService.GetFavorites(username);
            if(favorites != null)
                await Clients.Caller.SendAsync("Favorites", favorites);
        }

        public async Task GetEvents(string username)
        {
            var events = _dataService.GetEvents(username);
            if(events != null)
                await Clients.Caller.SendAsync("Events", events.ToArray());
        }

        public async Task EditProfile(ProfileDTO user)
        {
            bool success = _dataService.Edit(user);
            await Clients.Caller.SendAsync("Edit", success);
        }

        public async Task RegisterReq(ProfileDTO user)
        {
            bool success = _dataService.Register(user);
            await Clients.Caller.SendAsync("Registered", success);
        }

        public async Task LoginReq(string username, string password)
        {
            ProfileDTO p = _dataService.Login(username, password);
            if(p != null)
                await Clients.Caller.SendAsync("Login", p);
            else
                await Clients.Caller.SendAsync("LoginFailed");
        }

        public async Task LogoutReq(string username)
        {
            bool success = _dataService.Logout(username);
            await Clients.Caller.SendAsync("Logout", success);
        }
    }
}
