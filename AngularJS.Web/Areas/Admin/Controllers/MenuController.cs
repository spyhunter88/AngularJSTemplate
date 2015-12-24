using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AngularJS.Service;
using System.Web.Http.OData;

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
        public IHttpActionResult GetMenu(string module)
        {
            if ((module ?? "") == "")
            {
                var result = _menuService.GetAllMenu();
                return Ok(result);
            }
            else
            {
                var userId = GetCurrentUserId();
                if (userId == 0) return Ok();

                var result = _menuService.GetMenuItemByUser(userId, module);
                return Ok(result);
            }
        }

        // [HttpGet]
        // [Route("Menu/UserMenu")]
        //public IHttpActionResult GetUserMenu()
        //{
        //    var userId = GetCurrentUserId();
        //    if (userId == 0) return Ok();

        //    var result = _menuService.GetMenuItemByUser(userId);
        //    return Ok(result);
        //}

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