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

        public void CreateCategory(Categories category)
        {
            category.Created = DateTime.UtcNow;
            category.CreatedBy = Guid.NewGuid().ToString();

            _context.Categories.Add(category);
            _context.SaveChanges();
        }
    }
}
