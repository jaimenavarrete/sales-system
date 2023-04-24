using SalesSystem.Business.Exceptions;
using SalesSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using SalesSystem.Business.Enums;
using SalesSystem.Business.Constants;

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

        public List<Sales> GetOnlySales()
        {
            var sales = _context.Sales.ToList();
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

                var saleDetailNewPrice = saleDetail.CurrentPrice - saleDetail.Discount;
                saleDetail.Total = Math.Round(saleDetailNewPrice * saleDetail.Quantity, 2);

                sale.Total += saleDetail.Total * (1 + SaleConstants.Taxes);
            }

            sale.Total = Math.Round(sale.Total, 2);

            sale.SaleDate = DateTime.UtcNow;
            sale.Created = DateTime.UtcNow;
            sale.CreatedBy = Guid.NewGuid().ToString();

            _context.Sales.Add(sale);
            _context.SaveChanges();
        }

        public void EditSale(Sales sale)
        {
            var currentSale = _context.Sales
                .Include("SaleDetails")
                .FirstOrDefault(s => s.Id == sale.Id);

            if (currentSale is null)
            {
                throw new BusinessException($"La venta con código #{sale.Id}, no existe.");
            }

            foreach (var saleDetail in sale.SaleDetails)
            {
                var product = _context.Products.Find(saleDetail.ProductId);

                if (product is null)
                {
                    throw new BusinessException($"El producto con código #{product.Id}, no existe.");
                }

                // Verify if this product exists in the current sale to keep the same price and
                // get the difference in quantities
                var currentSaleDetail = _context.SaleDetails
                    .FirstOrDefault(sd => sd.ProductId == product.Id && sd.SaleId == sale.Id);

                if(currentSaleDetail is object)
                {
                    saleDetail.CurrentPrice = currentSaleDetail.CurrentPrice;

                    if ((saleDetail.Quantity - currentSaleDetail.Quantity) > product.Stock)
                    {
                        throw new BusinessException($"El producto con código #{product.Id}, no tiene las existencias suficientes para la venta.");
                    }
                }
                else
                {
                    saleDetail.CurrentPrice = product.Price;

                    if (product.Stock == 0 || saleDetail.Quantity > product.Stock)
                    {
                        throw new BusinessException($"El producto con código #{product.Id}, no tiene las existencias suficientes para la venta.");
                    }
                }

                // Update product stock
                product.Stock -= saleDetail.Quantity;

                var saleDetailNewPrice = saleDetail.CurrentPrice - saleDetail.Discount;
                saleDetail.Total = Math.Round(saleDetailNewPrice * saleDetail.Quantity, 2);

                sale.Total += saleDetail.Total * (1 + SaleConstants.Taxes);

                saleDetail.SaleId = sale.Id;
            }

            foreach (var saleDetail in currentSale.SaleDetails)
            {
                var product = _context.Products.Find(saleDetail.ProductId);

                product.Stock += saleDetail.Quantity;
            }

            if (sale.HomeDelivery && currentSale.DeliveryStateId is null)
            {
                currentSale.DeliveryStateId = (int)DeliveryState.Ordered;
            }

            currentSale.Total = Math.Round(sale.Total, 2);
            currentSale.ClientId = sale.ClientId;
            currentSale.HomeDelivery = sale.HomeDelivery;
            currentSale.PaymentCompleted = sale.PaymentCompleted;
            currentSale.Observation = sale.Observation;
            currentSale.Modified = DateTime.UtcNow;
            currentSale.ModifiedBy = Guid.NewGuid().ToString();

            _context.SaleDetails.RemoveRange(currentSale.SaleDetails);
            _context.SaleDetails.AddRange(sale.SaleDetails);

            _context.SaveChanges();
        }

        public void DeleteSale(int id)
        {
            var sale = _context.Sales
                .Include("SaleDetails")
                .FirstOrDefault(s => s.Id == id);

            if (sale is null)
            {
                throw new BusinessException("La venta que intenta borrar, no existe.");
            }

            foreach(var saleDetail in sale.SaleDetails)
            {
                var product = _context.Products.Find(saleDetail.ProductId);

                product.Stock += saleDetail.Quantity;
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
