using System.Web.Mvc;

namespace AngularJS.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Contain start-up page (Home/Index) and some static information page
    /// </summary>
    public class HomeController : Controller
    {
        //
        // GET Admin/Home/Index
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET Admin/Home/About
        public ActionResult About()
        {
            ViewBag.Message = "All about this Area!";
            return View();
        }

        //
        // GET Admin/Home/Help
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact information!";
            return View();
        }
    }
}