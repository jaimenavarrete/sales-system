using System;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class SaleDetailViewModel : InvoiceSaleDetailViewModel
    {
        public string PhotoBase64 { get; set; }

        public decimal NewPrice => CurrentPrice - Discount;

        public decimal Total => Math.Round(NewPrice * Quantity, 2);
    }
}