using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.DTOs
{
    public enum Interests { WatchSports, PlaySorts, Cinema, Museum, Hiking, Cooking }
    public enum Languages { English, German, French, Spanish, Russian, Hungarian}

    public class ProfileDTO
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool[] interests { get; set; }
        public bool[] languages { get; set; }
        public List<int> favourites { get; set; }
    }
}
