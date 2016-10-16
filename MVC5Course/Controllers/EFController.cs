using System.Data.Entity;
using MVC5Course.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class EFController : Controller
    {
        private FabricsEntities db = new FabricsEntities();

        // GET: EF
        public ActionResult Index()
        {
            var product = db.Product.Where(p => p.ProductName.Contains("White")).OrderByDescending(p => p.ProductId);

            return View(product);
        }

        public ActionResult Create()
        {
            var product = new Product()
            {
                ProductName = "White Cat",
                Active = true,
                Price = 199,
                Stock = 5
            };

            db.Product.Add(product);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var product = db.Product.Find(id);

            // delete foreign key for batch remove
            db.OrderLine.RemoveRange(product.OrderLine);

            // delete data
            db.Product.Remove(product);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var product = db.Product.Find(id);
            return View(product);
        }

        public ActionResult Update(int id)
        {
            var product = db.Product.Find(id);
            product.ProductName += "!";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Add20Percent()
        {
            var data = db.Product.Where(p => p.ProductName.Contains("White")).OrderByDescending(p => p.ProductId);

            foreach (var item in data)
            {
                if (item.Price.HasValue)
                {
                    item.Price = item.Price.Value * 1.2m ;
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        #region -- Edit --
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        #endregion -- Edit --

    }
}