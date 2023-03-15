using System;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string DeliveryTypeName { get; set; }

        public string StateName { get; set; }

        public DateTime? SaleDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public int ProductsQuantity { get; set; }

        public decimal Total { get; set; }
    }
}