using SalesSystem.Business.Exceptions;
using SalesSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using SalesSystem.Business.Enums;

namespace SalesSystem.Business.Services
{
    public class SalesService
    {
        private readonly SalesSystemEntities _context = new SalesSystemEntities();

        public List<Sales> GetSales()
        {
            var sales = _context.Sales
                .Include("Clients")
                .Include("SaleDetails")
                .ToList();

            return sales;
        }

        public List<Sales> GetSalesCreationDates()
        {
            var sales = _context.Sales
                .Select(s => new { s.Created })
                .ToList()
                .Select(s => new Sales() {
                    Created = s.Created
                })
                .ToList();

            return sales;
        }

        public List<DeliveryStates> GetDeliveryStates()
        {
            var deliveryStates = _context.DeliveryStates.ToList();

            return deliveryStates;
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

            if(sale.HomeDelivery)
            {
                sale.DeliveryStateId = (int)DeliveryState.Ordered;
            }

            foreach (var saleDetail in sale.SaleDetails)
            {
                var product = _context.Products.Find(saleDetail.ProductId);

                if(product is null)
                {
                    throw new BusinessException($"El producto con código #{product.Id}, no existe.");
                }

                if(product.Stock == 0 || saleDetail.Quantity > product.Stock)
                {
                    throw new BusinessException($"El producto con código #{product.Id}, no tiene las existencias suficientes para la venta.");
                }

                // Update product stock
                product.Stock -= saleDetail.Quantity;

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

        public void SetDeliveryState(int id, DeliveryState newDeliveryState)
        {
            var sale = _context.Sales.Find(id);

            if (sale is null)
            {
                throw new BusinessException("La venta que ha seleccionado, no existe.");
            }

            var existDeliveryState = Enum.IsDefined(typeof(DeliveryState), newDeliveryState);

            if (!existDeliveryState)
            {
                throw new BusinessException("El estado de envío que ha seleccionado, no existe.");
            }

            sale.Completed = newDeliveryState == DeliveryState.Delivered;

            if (sale.Completed)
            {
                sale.DeliveryDate = DateTime.UtcNow;
            }
            else
            {
                sale.DeliveryDate = null;
            }

            sale.DeliveryStateId = (int)newDeliveryState;
            _context.SaveChanges();
        }

        public void ChangeSaleState(int id)
        {
            var sale = _context.Sales.Find(id);

            if (sale is null)
            {
                throw new BusinessException("La venta que ha seleccionado, no existe.");
            }

            if(sale.HomeDelivery)
            {
                throw new BusinessException("La venta que ha seleccionado, es a domicilio, por lo que no se puede cambiar el estado directamente.");
            }

            sale.Completed = !sale.Completed;

            if(sale.Completed)
            {
                sale.DeliveryDate = DateTime.UtcNow;
                _context.SaveChanges();
                return;
            }

            sale.DeliveryDate = null;
            _context.SaveChanges();
        }
    }
}
