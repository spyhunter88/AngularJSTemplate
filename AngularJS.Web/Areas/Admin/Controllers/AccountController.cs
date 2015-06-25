using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJS.Web.Security;
using AngularJS.Web.Security.Models;

namespace AngularJS.Web.Areas.Admin.Api
{
    // [RoutePrefix("Admin/api/Account")]
    public class AccountController : ApiController
    {
        AuthContext authContext = new AuthContext();

        // GET api/<controller>
        public IHttpActionResult Get()
        {
            // return new string[] { "value1", "value2" };
            var users = authContext.Users.ToList();

            return users;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}