﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AngularJS.Web.Areas.Admin.Models.AngularJS.Web.Models;
using AngularJS.Web.Security.Models;
using Microsoft.AspNet.Identity;

namespace AngularJS.Web.Security.Repository
{
    /// <summary>
    /// Extended current AuthRepository for easy access to UserManager
    /// 
    /// </summary>
    public partial class AuthRepository
    {
        public Task<IdentityResult> RegisterUser(UserViewModel user)
        {
            if (user.Id == 0)
            {
                ApplicationUser _user = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    LockoutEnabled = user.LockOutEnabled,
                    SystemLoginEnabled = user.SystemLoginEnabled,
                    EmailReceiveEnabled = user.EmailReceiveEnabled
                };

                var result = _userManager.CreateAsync(_user, user.NewPassword);
                return result;
            }
            else
            {
                // Update password first
                _userManager.RemovePassword(user.Id);
                _userManager.AddPassword(user.Id, user.NewPassword);

                ApplicationUser _user = _userManager.FindById(user.Id);

                _user.Email = user.Email;
                _user.EmailConfirmed = user.EmailConfirmed;
                _user.EmailReceiveEnabled = user.EmailReceiveEnabled;
                _user.PhoneNumber = user.PhoneNumber;
                _user.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                _user.LockoutEnabled = user.LockOutEnabled;
                _user.SystemLoginEnabled = user.SystemLoginEnabled;

                var result = _userManager.UpdateAsync(_user);
                return result;
            }
        }

        public Task<IdentityResult> DeleteUser(int userId)
        {
            var user = _userManager.FindById(userId);
            var result = _userManager.DeleteAsync(user);
            return result;
        }
    }
}