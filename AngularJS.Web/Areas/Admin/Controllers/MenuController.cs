using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AngularJS.Service;
using AngularJS.Services.DTO;

namespace AngularJS.Web.Areas.Admin.Api
{
    public class MenuController : BaseController
    {
        #region Members and Constructor
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            this._menuService = menuService;
        }
        #endregion

        /// <summary>
        /// Get Menu for current user
        /// </summary>
        [HttpGet]
        public IHttpActionResult GetMenu()
        {
            var result = _menuService.GetAllMenu();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetMenu(int userId, int roleId)
        {
            var result = _menuService.GetMenuItem(userId, roleId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult PostMenu(int userId, int roleId, [FromBody] List<int> menus)
        {
            if (menus != null && menus.Count() != 0)
                _menuService.SaveMenuItem(userId, roleId, menus);

            return Ok();
        }
    }
}