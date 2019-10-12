using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lychee.Scrapper.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetUsers()
        {
            return Json(new List<string> { "Cornelio", "Jona" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Data()
        {
            return View();
        }

        public PartialViewResult ModalData()
        {
            return PartialView("_ModalData");
        }
    }
}