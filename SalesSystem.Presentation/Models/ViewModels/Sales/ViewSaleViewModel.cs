﻿using SalesSystem.Business.Enums;
using SalesSystem.Presentation.Models.ViewModels.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class ViewSaleViewModel
    {
        public int Id { get; set; }

        public ClientViewModel Client { get; set; }

        public bool IsHomeDelivery { get; set; }

        public DeliveryState? DeliveryStateId { get; set; }

        public DateTime? SaleDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsPaymentCompleted { get; set; }

        public string Observation { get; set; }

        public List<SaleDetailViewModel> SaleDetails { get; set; }

        public decimal Taxes { get; set; }

        public int SaleDetailsQuantity => SaleDetails.Count;

        public decimal SaleDetailsSubtotal => SaleDetails.Sum(sd => sd.Total);

        public decimal SaleDetailsTaxes => Math.Round(SaleDetailsSubtotal * Taxes, 2);

        public decimal SaleDetailsTotal => SaleDetailsSubtotal + SaleDetailsTaxes;

        public IEnumerable<SelectListItem> DeliveryStatesList { get; set; }
    }
}