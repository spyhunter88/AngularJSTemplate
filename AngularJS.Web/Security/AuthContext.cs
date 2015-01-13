using System;
using System.Collections.Generic;
using System.Data.Entity;
using AngularJS.Web.Security.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AngularJS.Web.Security
{
    public class AuthContext : IdentityDbContext<ApplicationUser, CustomRole, int, 
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public AuthContext()
            : base("AuthContext")
        {
            // Database.SetInitializer<AuthContext>(null);
        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }
    }
}