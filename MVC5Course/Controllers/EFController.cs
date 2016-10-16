using MVC5Course.Models;
using MVC5Course.Models.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityErrors in ex.EntityValidationErrors)
                {
                    foreach (var vErrors in entityErrors.ValidationErrors)
                    {
                        throw new DbEntityValidationException(vErrors.PropertyName + " 發生錯誤：" + vErrors.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Add20Percent()
        {
            // use sp method
            string str = "%White%";
            db.Database.ExecuteSqlCommand("UPDATE dbo.Product SET Price=Price*1.2 WHERE ProductName LIKE @p0", str);

            // use lambda
            //var data = db.Product.Where(p => p.ProductName.Contains("White")).OrderByDescending(p => p.ProductId);

            //foreach (var item in data)
            //{
            //    if (item.Price.HasValue)
            //    {
            //        item.Price = item.Price.Value * 1.2m ;
            //    }
            //}

            //db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ClientContribution()
        {
            var data = db.vw_ClientContribution.Take(10);
            return View(data);
        }

        public ActionResult ClientContribution2(string keyword = "Mary")
        {
            var data = db.Database.SqlQuery<ClientContributionViewModel>(@"
 	        SELECT
                 c.ClientId,
 		         c.FirstName,
 		         c.LastName,
 		         (SELECT SUM(o.OrderTotal)
 		          FROM [dbo].[Order] o
 		          WHERE o.ClientId = c.ClientId) as OrderTotal
 	        FROM
 		        [dbo].[Client] as c
             WHERE
                 c.FirstName LIKE @p0", "%" + keyword + "%");

            return View(data);
        }

        /// <summary>
        /// use ?keyword=Mary
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult ClientContribution3(string keyword)
        {
            var data = db.usp_GetClientContribution(keyword);
            return View(data);
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