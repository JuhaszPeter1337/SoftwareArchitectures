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

        public async Task SendMessage(int eventId, string user, string message)
        {
            await Clients.Others.SendAsync("Message", eventId, user, message);
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
