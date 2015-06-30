using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AngularJS.Web.Areas.Admin.Models.AngularJS.Web.Models;
using AngularJS.Web.Security;
using AngularJS.Web.Security.Models;
using AngularJS.Web.Security.Repository;

namespace AngularJS.Web.Areas.Admin.Api
{
    // [RoutePrefix("Admin/api/Account")]
    public class AccountController : ApiController
    {
        AuthRepository authRepository;

        public AccountController()
        {
            authRepository = new AuthRepository();
            AutoMapper.Mapper.CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            AutoMapper.Mapper.CreateMap<CustomRole, RoleViewModel>().ReverseMap();
        }
        

        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult GetAccounts()
        {
            List<ApplicationUser> users = authRepository.GetUsers();
            var _users = AutoMapper.Mapper.Map<List<UserViewModel>>(users);

            foreach (UserViewModel _user in _users)
            {
                _user.RolesList = authRepository.GetRoles(_user.Id);
            }

            return Ok(_users);
        }

        // GET api/<controller>/5
        [HttpGet]
        public IHttpActionResult GetAccount([FromUri]int id)
        {
            if (id == 0) return Ok();

            var user = authRepository.FindById(id);
            var _user = AutoMapper.Mapper.Map<UserViewModel>(user);
            return Ok(_user);
        }

        // POST api/<controller>
        [HttpPost]
        public void PostAccount([FromUri] string action, [FromBody]UserViewModel user)
        {
            if (action == "create")
            {
                user.Id = 0;
            }
            else if (action == "delete")
            {

            }
            var result = authRepository.RegisterUser(user);
        }

        // PUT api/<controller>/5
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public IHttpActionResult DeleteAccount([FromUri]int id)
        {
            if (id == 0) return Ok("Wrong user!");

            var task = authRepository.DeleteUser(id).Result;

            return Ok("Delete User success!");
        }
    }
}