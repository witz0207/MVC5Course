using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    public class ARController : Controller
    {
        private FabricsEntities db = new FabricsEntities();

        // GET: AR
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Notfound()
        {
            return View();
        }

        public ActionResult PartialViewTest()
        {
            return PartialView();
        }

        public ActionResult ContentTest()
        {
            return Content("ContentTest!!", "text/plain", Encoding.GetEncoding("Big5"));
        }

        public ActionResult FileTest()
        {

            var filePath = Server.MapPath("~/Content/view.jpg");

            return File(filePath, "image/jpeg");
        }

        public ActionResult FileTest2()
        {
            var filePath = Server.MapPath("~/Content/view.jpg");

            return File(filePath, "image/jpeg", "view.jpg");
        }

        public ActionResult JasonTest()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var data = db.Product.OrderBy(x => x.ProductId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}