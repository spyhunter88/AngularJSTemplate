using AngularJS.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using AngularJS.Web.Security.Models;

namespace AngularJS.Web.Areas.Admin.Api
{
    public class BaseController : ApiController
    {
        private AuthContext _ctx;

        protected ApplicationUserManager _userManager { get; set; }

        public BaseController()
        {
            _ctx = new AuthContext();
            _userManager = new ApplicationUserManager(new CustomUserStore(_ctx));
        }

        protected string GetUploadPath()
        {
            string res = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            return res;
        }

        protected short GetCurrentUserId()
        {
            var identity = (ClaimsIdentity) User.Identity;
            if (identity == null) return 0;
            //IEnumerable<Claim> claims = identity.Claims;
            //if (claims.Count() == 0) return 0;
            var claim = identity.FindFirst("id");
            if (claim == null) return 0;
            string id = claim.Value;

            return Int16.Parse(id);
        }

        protected string GetCurrentUserName()
        {
            return User.Identity.Name;
        }

        protected ApplicationUser GetCurrentUser()
        {

            ApplicationUser user = _userManager.FindById<ApplicationUser, int>(GetCurrentUserId());
            return user;
        }
    }
}
