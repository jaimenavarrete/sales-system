using SalesSystem.Business.Exceptions;
using SalesSystem.DataAccess.Data;
using System;
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

        public void CreateSale(Sales sale)
        {
            sale.SaleDate = DateTime.UtcNow;
            sale.Created = DateTime.UtcNow;
            sale.CreatedBy = Guid.NewGuid().ToString();

            foreach (var saleDetail in sale.SaleDetails)
            {
                var product = _context.Products.Find(saleDetail.ProductId);

                if(product is null)
                {
                    throw new BusinessException("Uno de los productos agregados, no existe.");
                }

                saleDetail.CurrentPrice = product.Price;
            }

            _context.Sales.Add(sale);
            _context.SaveChanges();
        }
    }
}
