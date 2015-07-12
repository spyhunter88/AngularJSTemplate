using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AngularJS.Entities.Models;
using AngularJS.Entities;
using AngularJS.Services.DTO;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System.IO;
using Omu.ValueInjecter;
using AngularJS.Services.InjectConfig;
using AngularJS.Services.Utility;

namespace AngularJS.Service
{
	public interface IMenuService
	{
		/// <summary>
		/// Load MenuItems for each User, include his roles
		/// to generate Menu
		/// </summary>
		List<MenuItemDTO> GetMenuItemByUser(int userId, string module = "");
		
		/// <summary>
		/// Load menu item and populate as tree view for each User,
		/// userId is prefer, 'roleId' will be taken if 'userId' = 0
		/// </summary>
		List<int> GetMenuItem(int userId, int roleId);
		
		/// <summary>
		/// Save menu item for each User or Role
		/// </summary>
		void SaveMenuItem(int userId, int roleId, List<int> menuIds);

        /// <summary>
        /// Get all menu for config
        /// </summary>
        List<MenuItemDTO> GetAllMenu();
	}
	
	
	public class MenuService : IMenuService
	{
		#region Unity injector
		private readonly IUnitOfWorkAsync _unitOfWorkAsync;
		
		public MenuService(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }
		#endregion
		
		public List<MenuItemDTO> GetMenuItemByUser(int userId, string module = "")
		{
			var user = _unitOfWorkAsync.Repository<User>()
                        .Query(x => x.Id == userId)
                        .Include(x => x.MenuItems)
                        .Include(x => x.Roles.Select(r => r.MenuItems))
                        .Select().FirstOrDefault();

            List<MenuItem> userMI = user.MenuItems.Where(x => (x.Module ?? "") == module).ToList();

            //foreach (Role role in user.Roles)
            //{
				
            //}

            return AutoMapper.Mapper.Map<List<MenuItem>, List<MenuItemDTO>>(userMI);
		}
		
		public List<int> GetMenuItem(int userId, int roleId)
		{
            if (userId != 0)
            {
                var user = _unitOfWorkAsync.Repository<User>()
                        .Query(x => x.Id == userId)
                        .Include(x => x.MenuItems)
                        .Select().FirstOrDefault();

                if (user == null) return null;

                return user.MenuItems.Select(x => x.ID).ToList();
            }
            else if (roleId != 0)
            {
                var role = _unitOfWorkAsync.Repository<Role>()
                        .Query(x => x.Id == roleId)
                        .Include(x => x.MenuItems)
                        .Select().FirstOrDefault();

                if (role == null) return null;
                return role.MenuItems.Select(x => x.ID).ToList();
            }

            return null;
		}
		
		public void SaveMenuItem(int userId, int roleId, List<int> menuIds)
		{
            var menuItems = _unitOfWorkAsync.Repository<MenuItem>()
                            .Query(x => menuIds.Contains(x.ID))
                            .Select().ToList();

		    if (userId != 0)
            {
                var user = _unitOfWorkAsync.Repository<User>()
                        .Query(x => x.Id == userId)
                        .Include(x => x.MenuItems)
                        .Select().FirstOrDefault();

                if (user == null) return;

                // remove old
                foreach (MenuItem mi in user.MenuItems)
                {
                    mi.ObjectState = ObjectState.Deleted;
                }

                // add new
                foreach (MenuItem mi in menuItems)
                {
                    user.MenuItems.Add(mi);
                }
                user.ObjectState = ObjectState.Modified;

                _unitOfWorkAsync.SaveChanges();
            }
            else if (roleId != 0)
            {

            }
		}

        public List<MenuItemDTO> GetAllMenu()
        {
            var menus = _unitOfWorkAsync.Repository<MenuItem>().Queryable().Select(x => x).ToList();
            return AutoMapper.Mapper.Map<List<MenuItem>, List<MenuItemDTO>>(menus);
        }
	}
}