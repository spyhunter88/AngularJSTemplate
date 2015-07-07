using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJS.Entities.Models;
using System.Web.Http.Description;
using AngularJS.Service;
using AngularJS.Services.DTO;

namespace AngularJS.Web.Api
{
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            this._menuService = menuService;
        }

        [HttpGet]
        // [ResponseType(typeof(MenuItemDTO))]
        public IHttpActionResult GetMenu()
        {
            int userId = (int)GetCurrentUserId();
            if (userId == 0) return Ok();
            var menus = _menuService.GetMenuItemByUser(userId);

            return Ok(menus);
        }
    }
}
