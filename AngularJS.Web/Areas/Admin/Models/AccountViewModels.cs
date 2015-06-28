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
        }


        public class RegisterViewModel
        {
            [Required]
            [Display(Name = "User name")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }

}