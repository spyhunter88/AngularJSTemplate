using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AngularJS.Web.Models;
using AngularJS.Web.Security.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AngularJS.Web.Security.Repository
{
    public partial class AuthRepository : IDisposable
    {
        private AuthContext _ctx;
        private ApplicationUserManager _userManager;
        private CustomRoleStore _roleStore;


        public AuthRepository()
        {
            _ctx = new AuthContext();
            _roleStore = new CustomRoleStore(_ctx);
            _userManager = new ApplicationUserManager(new CustomUserStore(_ctx));
        }

        public Task<IdentityResult> RegisterUser(RegisterViewModel userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                SystemLoginEnabled = true,
                EmailReceiveEnabled = false
            };

            var result = _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}