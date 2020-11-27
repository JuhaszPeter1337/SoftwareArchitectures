using EventFinderServer.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventFinderServer.Models
{
    public class User : IdentityUser
    {
        public Interest Interests { get; set; }
        public Language Languages { get; set; }
        public ICollection<UserFavorites> Favorites { get; set; }

        public ProfileDTO MakeDTO()
        {
            return new ProfileDTO
            {
                username = UserName,
                interests = Interests,
                languages = Languages,
                favorites = Favorites?.Select(f => f.EventId).ToArray()
            };
        }
    }
}
