using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models.ViewModels;

namespace MVC5Course.Controllers
{
    public class MVController : Controller
    {
        // GET: MV
        public ActionResult Index()
        {
            ViewData["Temp1"] = "test : ViewData";

            var b = new ClientLoginViewModel()
            {
                FirstName = "Witz",
                LastName = "Yu"
            };

            ViewData["Temp2"] = b;

            ViewBag.Temp3 = b;

            return View();
        }

        public ActionResult MyForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MyForm(ClientLoginViewModel c)
        {
            if (ModelState.IsValid)
            {
                TempData["MyFormData"] = c;
                return RedirectToAction("MyFormResult");
            }
            return View();
        }

        public ActionResult MyFormResult()
        {
            return View();
        }
    }
}