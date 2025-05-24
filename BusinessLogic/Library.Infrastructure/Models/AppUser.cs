using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Models
{
    public class AppUser : IdentityUser
    {
        // Додаткові властивості користувача (опціонально)
        public string FullName { get; set; } = null!;
    }
}
