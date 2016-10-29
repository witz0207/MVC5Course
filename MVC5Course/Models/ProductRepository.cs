using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class ProductRepository : EFRepository<Product>, IProductRepository
	{
        public override IQueryable<Product> All()
        {
            return base.All().OrderByDescending(p => p.ProductId).Take(10);
        }

        public Product Find(int id)
        {
            return All().FirstOrDefault(p => p.ProductId == id);
        }

        public override void Delete(Product product)
        {
            product.IsDeleted = true;
        }
	}

	public  interface IProductRepository : IRepository<Product>
	{

	}
}