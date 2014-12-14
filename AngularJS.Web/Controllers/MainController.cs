using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AngularJS.Web.Controllers
{
    /// <summary>
    /// Contain some View for general App: Login, DashBoard, Logout ...
    /// </summary>
    public class MainController : Controller
    {
        //
        // GET Main/DashBoard
        public ActionResult DashBoard()
        {
            return PartialView();
        }

        //
        // GET/Main/Login
        public ActionResult Login()
        {
            return PartialView();
        }

        //
        // GET/Main/UnAuthorize
        public ActionResult UnAuthorize()
        {
            return PartialView();
        }
    }
}