using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace TheWorld.Models
{
    public class WorldUser : IdentityUser
    {
        public DateTime DateJoined { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public string HomeAddress { get; set; }

        public DateTime FirstTrip{ get; set; }

        public string ProfilePic { get; set; }
        

        
    }
}