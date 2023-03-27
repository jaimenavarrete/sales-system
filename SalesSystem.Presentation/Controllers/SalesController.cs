using SalesSystem.Business.Services;
using SalesSystem.DataAccess.Data;
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
        private readonly ProductsService _productsService = new ProductsService();

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

        public ActionResult CreateSale()
        {
            var viewModel = new CreateSaleViewModel()
            {
                ClientsList = GetClients(),
                ProductsList = GetProducts()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateSale(CreateSaleViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.ClientsList = GetClients();
                viewModel.ProductsList = GetProducts();

                TempData["error"] = "Error. Por favor, revise que todos los campos sean válidos.";

                return View(viewModel);
            }

            var saleDetails = ConvertToSaleDetails(viewModel.SaleProductsList);

            var sale = new Sales()
            {
                ClientId = viewModel.ClientId,
                HomeDelivery = viewModel.IsHomeDelivery,
                PaymentCompleted = viewModel.IsPaymentCompleted,
                Observation = viewModel.Observation,
                SaleDetails = saleDetails
            };

            _salesService.CreateSale(sale);

            TempData["success"] = "La venta fue realizada con éxito";

            return RedirectToAction("Index");
        }

        private List<SaleDetails> ConvertToSaleDetails(SaleProductsListViewModel saleProductsList)
        {
            var saleDetails = new List<SaleDetails>();

            for (int index = 0; index < saleProductsList.ProductId.Length; index++)
            {
                if(saleProductsList.ProductId[index] == 0 || saleProductsList.Quantity[index] == 0)
                {
                    continue;
                }

                var saleDetail = new SaleDetails()
                {
                    ProductId = saleProductsList.ProductId[index],
                    Discount = saleProductsList.Discount[index],
                    Quantity = saleProductsList.Quantity[index]
                };

                saleDetails.Add(saleDetail);
            }

            return saleDetails;
        }

        private List<SelectListItem> GetProducts()
        {
            var products = _productsService.GetProducts();

            var productsSelectList = products
                .Select(product => new SelectListItem
                {
                    Value = product.Id.ToString(),
                    Text = $"{product.Id}. {product.UnitTypes.Name} - {product.Name}"
                })
                .Prepend(new SelectListItem
                {
                    Value = "",
                    Text = "Seleccionar producto"
                })
                .ToList();

            return productsSelectList;
        }

        private List<SelectListItem> GetClients()
        {
            var clients = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "1", Text = "Cliente 1"},
                new SelectListItem() { Value = "3", Text = "Cliente 2"},
            };

            return clients;
        }
    }
}