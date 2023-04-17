using System;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class SaleDetailViewModel
    {
        public int ProductId { get; set; }

        public int SaleId { get; set; }

        public string ProductName { get; set; }

        public string UnitTypeName { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public decimal Discount { get; set; }

        public string PhotoBase64 { get; set; }

        public decimal NewPrice => Math.Round(Price - Discount, 2);

        public decimal Total => Math.Round(NewPrice * Quantity, 2);
    }
}