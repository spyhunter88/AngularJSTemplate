using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AngularJS.Web.Controllers
{
    public class ClaimController : Controller
    {
        //
        // GET: /Claim/
        public ActionResult Index()
        {
            return PartialView();
        }

        //
        // GET: /Claim/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        //
        // GET: /Claim/Update
        public ActionResult Update()
        {
            return PartialView();
        }
	}
}