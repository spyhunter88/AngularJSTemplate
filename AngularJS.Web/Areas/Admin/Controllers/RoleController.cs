using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJS.Web.Areas.Admin.Models.AngularJS.Web.Models;
using AngularJS.Web.Security.Models;
using AngularJS.Web.Security.Repository;

namespace AngularJS.Web.Areas.Admin.Controllers
{
    public class RoleController : ApiController
    {
        AuthRepository authRepository;

        public RoleController()
        {
            authRepository = new AuthRepository();
            AutoMapper.Mapper.CreateMap<CustomRole, RoleViewModel>();
        }

        // GET api/<controller>
        public IHttpActionResult Get()
        {
            var roles = authRepository.GetRoles();
            var _roles = AutoMapper.Mapper.Map<List<RoleViewModel>>(roles);

            return Ok(_roles);
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