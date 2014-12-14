using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AngularJS.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {

        //
        // GET /Customer/Index
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}