using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJS.Services;
using AngularJS.Web.Areas.Admin.Models;

namespace AngularJS.Web.Areas.Admin.Controllers
{
    public class PolicyController : ApiController
    {
        private readonly IObjectConfigService _objectConfigService;

        public PolicyController(IObjectConfigService objectConfigService)
            : base()
        {
            _objectConfigService = objectConfigService;
        }

        /// <summary>
        /// Use to get ObjectConfig, ObjectAction ... by UserID (prefer) or RoleID
        /// </summary>
        [HttpGet]
        public IHttpActionResult GetPolicy(int userId, int roleId)
        {
            PolicyViewModel policyModel = new PolicyViewModel(userId, roleId);

            var objectActions = _objectConfigService.GetObjectAction(userId, roleId);
            var objectConfigs = _objectConfigService.GetObjectConfig(userId, roleId);

            return Ok();
        }

        /// <summary>
        /// Save policy of each user: ObjectConfig, ObjectAction, Roles
        /// Others User Attributes depend on Application
        /// </summary>
        [HttpPost]
        public IHttpActionResult SavePolicy([FromBody]PolicyViewModel policyViewModel)
        {


            return Ok();
        }
    }
}
