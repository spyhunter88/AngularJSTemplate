using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJS.Web.Areas.Admin.Models.AngularJS.Web.Models;
using AngularJS.Web.Security;
using AngularJS.Web.Security.Models;

namespace AngularJS.Web.Areas.Admin.Api
{
    // [RoutePrefix("Admin/api/Account")]
    public class AccountController : ApiController
    {
        AuthContext authContext;

        public AccountController()
        {
            authContext = new AuthContext();
            AutoMapper.Mapper.CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
        

        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult GetAccounts()
        {
            List<ApplicationUser> users = authContext.Users.ToList();
            var _users = AutoMapper.Mapper.Map<List<UserViewModel>>(users);
            return Ok(_users);
        }

        // GET api/<controller>/5
        [HttpGet]
        public IHttpActionResult GetAccount([FromUri]int id)
        {
            if (id == 0) return Ok();

            var user = authContext.Users.Where(x => x.Id == id).FirstOrDefault();
            var _user = AutoMapper.Mapper.Map<UserViewModel>(user);
            return Ok(_user);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]UserViewModel user)
        {
            
        }

        // PUT api/<controller>/5
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public IHttpActionResult Delete([FromUri]int id)
        {
            if (id == 0) return Ok("Wrong user!");

            var user = authContext.Users.Find(new object[] { id });
            if (user == null) return Ok("User not found!");
            authContext.Users.Remove(user);
            var count = authContext.SaveChanges();

            return Ok("Delete User: " + count);
        }
    }
}