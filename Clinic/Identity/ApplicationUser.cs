using Microsoft.AspNetCore.Identity;
using System;

namespace Clinic.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public int Experience { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
    }
}