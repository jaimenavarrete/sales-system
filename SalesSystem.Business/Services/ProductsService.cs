using SalesSystem.DataAccess.Data;
using SalesSystem.DataAccess.Files;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesSystem.Business.Services
{
    public class ProductsService
    {
        private readonly SalesSystemEntities _context = new SalesSystemEntities();
        private readonly PhotoStorage _photoStorage = new PhotoStorage();

        public List<Products> GetProducts()
        {
            var products = _context.Products
                    .Include("UnitTypes")
                    .Include("Categories")
                    .ToList();

            return products;
        }

        public List<Products> GetProductsWithStock()
        {
            var products = _context.Products
                    .Include("UnitTypes")
                    .Include("Categories")
                    .Where(p => p.Stock > 0)
                    .ToList();

            return products;
        }

        public Products GetProductById(int id)
        {
            var product = _context.Products
                    .Include("UnitTypes")
                    .Include("Categories")
                    .FirstOrDefault(p => p.Id == id);

            return product;
        }

        public byte[] GetProductPhotoBytesByFileName(string photoFileName)
        {
            var photoBytes = _photoStorage.GetPhotoBytesByFileName(photoFileName);

            return photoBytes;
        }

        public void CreateProduct(Products product, byte[] productPhotoBytes)
        {
            var photoFilename = _photoStorage.SavePhotoFromBytes(productPhotoBytes);

            product.PhotoFilename = photoFilename;
            product.Created = DateTime.UtcNow;
            product.CreatedBy = Guid.NewGuid().ToString();

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void EditProduct(Products product, byte[] productPhotoBytes)
        {
            var currentProduct = GetProductById(product.Id);

            if(productPhotoBytes is object)
            {
                _photoStorage.DeletePhotoByFileName(currentProduct.PhotoFilename);

                var photoFileName = _photoStorage.SavePhotoFromBytes(productPhotoBytes);
                currentProduct.PhotoFilename = photoFileName;
            }

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
