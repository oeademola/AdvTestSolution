using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Advansio.API.Models
{
    public class Role: IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}