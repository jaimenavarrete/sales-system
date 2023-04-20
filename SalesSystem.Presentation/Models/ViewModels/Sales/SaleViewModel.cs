using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public bool IsHomeDelivery { get; set; }

        public DateTime? SaleDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsPaymentCompleted { get; set; }

        public decimal Total { get; set; }

        public int SaleDetailsQuantity { get; set; }
    }
}