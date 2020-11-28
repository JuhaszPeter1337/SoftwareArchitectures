using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.Models
{
    [Flags]
    public enum Interest { WatchSports = 1, PlaySports = 2, Cinema = 4, Museum = 8, Hiking = 16, Cooking = 32 }
    [Flags]
    public enum Language { English = 1, German = 2, French = 4, Spanish = 8, Russian = 16, Hungarian = 32 }
}
