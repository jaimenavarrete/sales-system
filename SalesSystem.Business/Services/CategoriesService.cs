using SalesSystem.DataAccess.Data;
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
    }
}
