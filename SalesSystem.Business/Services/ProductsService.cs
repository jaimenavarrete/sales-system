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

        public void EditProduct(Products product)
        {
            using (var context = new SalesSystemEntities())
            {
                var currentProduct = GetProductById(product.Id);
                context.Products.Attach(currentProduct);

                currentProduct.Name = product.Name;
                currentProduct.Description = product.Description;
                currentProduct.Stock = product.Stock;
                currentProduct.Price = product.Price;
                currentProduct.UnitTypeId = product.UnitTypeId;
                currentProduct.CategoryId = product.CategoryId;
                currentProduct.Modified = DateTime.UtcNow;
                currentProduct.ModifiedBy = Guid.NewGuid().ToString();

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
