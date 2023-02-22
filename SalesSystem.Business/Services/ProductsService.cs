using SalesSystem.DataAccess.Data;
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
    }
}
