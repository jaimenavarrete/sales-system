using SalesSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesSystem.Business.Services
{
    public class ProductsService
    {
        public List<Products> GetProducts()
        {
            using(var context = new SalesSystemEntities())
            {
                var products = context.Products
                    .Include("UnitTypes")
                    .Include("Categories")
                    .ToList();

                return products;
            }
        }

        public Products GetProductById(int id)
        {
            using (var context = new SalesSystemEntities())
            {
                var product = context.Products.Find(id);

                return product;
            }
        }

        public void CreateProduct(Products product)
        {
            product.Created = DateTime.UtcNow;
            product.CreatedBy = Guid.NewGuid().ToString();

            using(var context = new SalesSystemEntities())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            using(var context = new SalesSystemEntities())
            {
                var product = new Products() { Id = id };

                context.Products.Attach(product);
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
    }
}
