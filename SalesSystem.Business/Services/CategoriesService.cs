﻿using SalesSystem.Business.Exceptions;
using SalesSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesSystem.Business.Services
{
    public class CategoriesService
    {
        private readonly SalesSystemEntities _context = new SalesSystemEntities();

        public List<Categories> GetCategories()
        {
            var categories = _context.Categories.ToList();

            return categories;
        }

        public Categories GetCategoryById(int id)
        {
            var category = _context.Categories.Find(id);

            return category;
        }

        public void CreateCategory(Categories category)
        {
            category.Created = DateTime.UtcNow;
            category.CreatedBy = Guid.NewGuid().ToString();

            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void EditCategory(Categories category)
        {
            var currentCategory = _context.Categories.Find(category.Id);

            if(currentCategory is null)
            {
                throw new BusinessException("La categoría que intenta editar, no existe.");
            }

            currentCategory.Name = category.Name;
            currentCategory.Description = category.Description;
            currentCategory.Modified = DateTime.UtcNow;
            currentCategory.ModifiedBy = Guid.NewGuid().ToString();

            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            var hasProducts = _context.Products.Any(product => product.CategoryId == id);

            if (hasProducts)
            {
                throw new BusinessException("La categoría no se puede borrar, porque está siendo utilizada en uno o más productos.");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
