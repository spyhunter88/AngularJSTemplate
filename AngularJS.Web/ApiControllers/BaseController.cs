using AngularJS.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularJS.Web.Api
{
    public class BaseController : ApiController
    {
        private AuthContext _ctx;

        protected UserManager<IdentityUser> _userManager { get; set; }

        public BaseController()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        protected string GetUploadPath()
        {
            string res = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            return res;
        }

        protected short GetCurrentUserId()
        {
            return Int16.Parse(User.Identity.GetUserId());
        }

        protected string GetCurrentUserName()
        {
            return User.Identity.Name;
        }

        protected IdentityUser GetCurrentUser()
        {
            IdentityUser user = _userManager.FindById<IdentityUser, string>(User.Identity.GetUserId());
            return user;
        }
    }
}
