using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace BugGroupByDatePart.Classes.Models
{
    public class CustomUser: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public long ClientId { get; set; } // Associated client

    }
}
