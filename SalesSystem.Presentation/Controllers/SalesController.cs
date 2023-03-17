using SalesSystem.Business.Services;
using SalesSystem.Presentation.Models.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class SalesController : Controller
    {
        private readonly SalesService _salesService = new SalesService();

        // GET: Sales
        public ActionResult Index()
        {
            var sales = _salesService.GetSales();

            var salesViewModel = sales
                .Select(sale => new SaleViewModel()
                {
                    Id = sale.Id,
                    ClientFirstName = sale.Clients.FirstName,
                    ClientLastName = sale.Clients.LastName,
                    IsHomeDelivery = sale.HomeDelivery,
                    SaleDate = sale.SaleDate,
                    DeliveryDate = sale.DeliveryDate,
                    IsCompleted = sale.Completed,
                    IsPaymentCompleted = sale.PaymentCompleted
                })
                .ToList();

            return View(salesViewModel);
        }
    }
}