using SalesSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesSystem.Business.Services
{
    public class ProductsService
    {
        private readonly SalesSystemEntities _context = new SalesSystemEntities();

        public List<Products> GetProducts()
        {
            var products = _context.Products
                    .Include("UnitTypes")
                    .Include("Categories")
                    .ToList();

            return products;
        }

        public Products GetProductById(int id)
        {
            var product = _context.Products.Find(id);

            return product;
        }

        public void CreateProduct(Products product)
        {
            product.Created = DateTime.UtcNow;
            product.CreatedBy = Guid.NewGuid().ToString();

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void EditProduct(Products product)
        {
            var currentProduct = GetProductById(product.Id);

            currentProduct.Name = product.Name;
            currentProduct.Description = product.Description;
            currentProduct.Stock = product.Stock;
            currentProduct.Price = product.Price;
            currentProduct.UnitTypeId = product.UnitTypeId;
            currentProduct.CategoryId = product.CategoryId;
            currentProduct.Modified = DateTime.UtcNow;
            currentProduct.ModifiedBy = Guid.NewGuid().ToString();

            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);

            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
