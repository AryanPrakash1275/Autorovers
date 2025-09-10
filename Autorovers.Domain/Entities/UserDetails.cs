using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorovers.Domain.Entities
{
    public class UserDetails
    {
        public int UserId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; } 
        public string ProfilePictureUrl { get; set; }
        public string Bio {  get; set; }
        public int PhoneNumber { get; set; }
        public string City { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Preferences {  get; set; }
    }
}
