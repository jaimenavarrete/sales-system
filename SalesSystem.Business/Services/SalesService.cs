using SalesSystem.Business.Exceptions;
using SalesSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

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
                .Include("SaleDetails")
                .ToList();

            return sales;
        }

        public Sales GetSaleById(int id)
        {
            var sale = _context.Sales
                .Include("Clients")
                .Include("SaleDetails")
                .Include(s => s.SaleDetails.Select(sd => sd.Products))
                .FirstOrDefault(s => s.Id == id);

            return sale;
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

        public void DeleteSale(int id)
        {
            var sale = _context.Sales.Find(id);

            if (sale is null)
            {
                throw new BusinessException("La venta que intenta borrar, no existe.");
            }

            _context.Sales.Remove(sale);
            _context.SaveChanges();
        }
    }
}
