using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJS.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;

    namespace AngularJS.Web.Models
    {
        public class UserViewModel
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
            public string Email { get; set; }
            public bool EmailConfirmed { get; set; }
            public string PhoneNumber { get; set; }
            public bool PhoneNumberConfirmed { get; set; }
            public bool LockOutEnabled { get; set; }
            public Boolean SystemLoginEnabled { get; set; }
            public Boolean EmailReceiveEnabled { get; set; }

            // public List<RoleViewModel> Roles { get; set; }
            public List<string> RolesList { get; set; }
        }


        public class RoleViewModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }

}