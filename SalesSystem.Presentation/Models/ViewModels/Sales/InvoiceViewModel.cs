using SalesSystem.Presentation.Models.ViewModels.Clients;
using System;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }

        public ClientViewModel Client { get; set; }

        public int ProductsQuantity { get; set; }

        public decimal Total { get; set; }

        public bool IsHomeDelivery { get; set; }

        public DateTime SaleDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsPaymentCompleted { get; set; }
    }
}