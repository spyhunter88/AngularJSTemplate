using System;
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
        public IdentityResult RegisterUser(UserViewModel user)
        {
            // IdentityResult result;

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

                var roles = _roleStore.Roles.Where(x => user.RolesList.Contains(x.Name)).ToList();
                foreach (CustomRole role in roles)
                {
                    CustomUserRole cur = new CustomUserRole();
                    cur.RoleId = role.Id;
                    _user.Roles.Add(cur);
                }

                var result = _userManager.Create(_user, user.NewPassword);
                return result;
                // user.Id = _user.Id;
            }
            else
            {
                // Update password first
                if ((user.NewPassword ?? "") != "")
                {
                    _userManager.RemovePassword(user.Id);
                    _userManager.AddPassword(user.Id, user.NewPassword);
                }

                ApplicationUser _user = _userManager.FindById(user.Id);

                _user.Email = user.Email;
                _user.EmailConfirmed = user.EmailConfirmed;
                _user.EmailReceiveEnabled = user.EmailReceiveEnabled;
                _user.PhoneNumber = user.PhoneNumber;
                _user.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                _user.LockoutEnabled = user.LockOutEnabled;
                _user.SystemLoginEnabled = user.SystemLoginEnabled;

                var roles = _roleStore.Roles.Where(x => user.RolesList.Contains(x.Name)).ToList();
                foreach (CustomRole role in roles)
                {
                    // Check exist roles
                    if (_user.Roles.Where(x => x.RoleId == role.Id).Count() == 0)
                    {
                        CustomUserRole cur = new CustomUserRole();
                        cur.RoleId = role.Id;
                        cur.UserId = _user.Id;
                        _user.Roles.Add(cur);
                    }
                }

                var result = _userManager.Update(_user);
                return result;
            }
        }

        public List<ApplicationUser> GetUsers()
        {
            var users = _userManager.Users
                .Select(x => x)
                .ToList();

            return users;
        }

        public ApplicationUser FindById(int id)
        {
            var user = _userManager.FindById(id);
            return user;
        }


        public Task<IdentityResult> DeleteUser(int userId)
        {
            var user = _userManager.FindById(userId);
            var result = _userManager.DeleteAsync(user);
            return result;
        }

        #region Roles
        public List<CustomRole> GetRoles()
        {
            var roles = _roleStore.Roles.ToList();
            return roles;
        }

        public List<string> GetRoles(int userId)
        {
            var roles = _userManager.GetRoles(userId);
            return roles.ToList();
        }
        #endregion
    }
}