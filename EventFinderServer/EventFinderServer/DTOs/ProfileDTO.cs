using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventFinderServer.Models;

namespace EventFinderServer.DTOs
{
    public class ProfileDTO
    {
        public string username { get; set; }
        public Interest interests { get; set; }
        public Language languages { get; set; }
        public int[] favorites { get; set; }
        public string token { get; set; }
    }
}
