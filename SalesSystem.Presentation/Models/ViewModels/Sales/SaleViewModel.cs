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

        public List<SaleDetailViewModel> SaleDetails { private get; set; }

        public decimal Taxes { get; set; }

        public int SaleDetailsQuantity => SaleDetails.Count;

        public decimal SaleDetailsTotal => SaleDetailsSubtotal + SaleDetailsTaxes;


        private decimal SaleDetailsSubtotal => SaleDetails.Sum(sd => sd.Total);

        private decimal SaleDetailsTaxes => Math.Round(SaleDetailsSubtotal * Taxes, 2);
    }
}