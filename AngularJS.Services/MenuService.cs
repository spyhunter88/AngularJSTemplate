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
		List<MenuItemDTO> GetMenuItemByUser(int userId);
		
		/// <summary>
		/// Load menu item and populate as tree view for each User,
		/// userId is prefer, 'roleId' will be taken if 'userId' = 0
		/// </summary>
		List<MenuItemDTO> GetMenuItem(int userId, int roleId);
		
		/// <summary>
		/// Save menu item for each User or Role
		/// </summary>
		void SaveMenuItem(int userId, int roleId, List<MenuItemDTO> menuItems);
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
		
		public List<MenuItemDTO> GetMenuItemByUser(int userId)
		{
			var user = _unitOfWorkAsync.Repository<User>()
                        .Query(x => x.Id == userId)
                        .Include(x => x.MenuItems)
                        .Include(x => x.Roles.Select(r => r.MenuItems))
                        .Select().FirstOrDefault();
			
			List<MenuItem> userMI = user.MenuItems.ToList();

            foreach (Role role in user.Roles)
			{
				
			}

            return AutoMapper.Mapper.Map<List<MenuItem>, List<MenuItemDTO>>(userMI);
		}
		
		public List<MenuItemDTO> GetMenuItem(int userId, int roleId)
		{		
			return null;
		}
		
		public void SaveMenuItem(int userId, int roleId, List<MenuItemDTO> menuItems)
		{
			
		}
	}
}