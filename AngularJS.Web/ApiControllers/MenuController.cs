using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJS.Entities.Models;
using AngularJS.Web.Models;
using System.Web.Http.Description;

namespace AngularJS.Web.Api
{
    public class MenuController : ApiController
    {

        [HttpGet]
        [ResponseType(typeof(MenuItemViewModels))]
        public IHttpActionResult Menu()
        {
            MenuItemViewModels res = new MenuItemViewModels();

            res.menuItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Href = "/home",
                        Title = "Home"
                    },
                    new MenuItem
                    {
                        Href = "/customer",
                        Title = "Customers"
                    },
                    new MenuItem
                    {
                        Href = "/claim",
                        Title = "Claims"
                    }
                };

            return Ok(res);
        }
    }
}
