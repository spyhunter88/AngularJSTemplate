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

namespace AngularJS.Web.Api
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
            // return Int16.Parse(User.Identity.GetUserId());
            var identity = (ClaimsIdentity) User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string id = identity.FindFirst("id").Value;

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
