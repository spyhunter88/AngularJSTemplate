using System.Web.Http;
using AngularJS.Service;

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
