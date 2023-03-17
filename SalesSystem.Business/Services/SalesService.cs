using SalesSystem.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;

namespace SalesSystem.Business.Services
{
    public class SalesService
    {
        private readonly SalesSystemEntities _context = new SalesSystemEntities();

        public List<Sales> GetSales()
        {
            var sales = _context.Sales
                .Include("Clients")
                .Include("DeliveryStates")
                .ToList();

            return sales;
        }
    }
}
