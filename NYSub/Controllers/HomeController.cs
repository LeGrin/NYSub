using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NYSub.Models;

namespace NYSub.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new StationsDataModel();
            return View(model);
        }

     
    }
}