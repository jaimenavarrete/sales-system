﻿using SalesSystem.Business.Services;
using SalesSystem.DataAccess.Data;
using SalesSystem.Presentation.Models.ViewModels.Clients;
using SalesSystem.Presentation.Models.ViewModels.Sales;
using SalesSystem.Presentation.Views.Sales.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.Shared;
using SalesSystem.Business.Constants;

namespace SalesSystem.Presentation.Controllers
{
    public class SalesController : Controller
    {
        private readonly SalesService _salesService = new SalesService();
        private readonly ProductsService _productsService = new ProductsService();
        private readonly ClientsService _clientsService = new ClientsService();

        // GET: Sales
        [HttpGet]
        public ActionResult Index()
        {
            var sales = _salesService.GetSales();

            var salesViewModel = sales
                .Select(sale => new SaleViewModel()
                {
                    Id = sale.Id,
                    ClientFirstName = sale.Clients.FirstName,
                    ClientLastName = sale.Clients.LastName,
                    ProductsQuantity = sale.SaleDetails.Count,
                    Total = GetSaleTotal(sale.SaleDetails),
                    IsHomeDelivery = sale.HomeDelivery,
                    SaleDate = sale.SaleDate,
                    DeliveryDate = sale.DeliveryDate,
                    IsCompleted = sale.Completed,
                    IsPaymentCompleted = sale.PaymentCompleted
                })
                .ToList();

            return View(salesViewModel);
        }

        [HttpGet]
        public ActionResult ViewSale(int id)
        {
            var sale = _salesService.GetSaleById(id);

            if (sale is null)
            {
                TempData["error"] = "Error. La venta que ha seleccionado, no existe.";

                return RedirectToAction("Index");
            }

            var viewModel = new ViewSaleViewModel() { };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CreateSale()
        {
            var viewModel = new CreateSaleViewModel()
            {
                ClientsList = GetClients(),
                ProductsList = GetProducts()
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
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

            TempData["success"] = "La venta fue realizada con éxito.";

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteSale(int id = 0)
        {
            _salesService.DeleteSale(id);

            TempData["success"] = "La venta fue borrada con éxito.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateInvoice(int id = 0)
        {
            var sale = _salesService.GetSaleById(id);

            if(sale is null)
            {
                TempData["error"] = "Error. La venta que ha seleccionado para la factura, no existe.";

                return RedirectToAction("Index");
            }

            var invoiceViewModel = new InvoiceViewModel()
            {
                Id = sale.Id,
                Client = MapClientToViewModel(sale.Clients),
                IsHomeDelivery = sale.HomeDelivery,
                Observation = sale.Observation,
                SaleDate = sale.SaleDate ?? DateTime.UtcNow,
                DeliveryDate = sale.DeliveryDate ?? DateTime.UtcNow,
                IsCompleted = sale.Completed,
                IsPaymentCompleted = sale.PaymentCompleted,
                SaleDetails = MapSaleDetailsToInvoiceViewModel(sale.SaleDetails),
            };

            var report = new Invoice();
            report.Load();

            report.Database.Tables["InvoiceViewModel"]
                .SetDataSource(new List<InvoiceViewModel>() { invoiceViewModel });
            report.Database.Tables["ClientViewModel"]
                .SetDataSource(new List<ClientViewModel>() { invoiceViewModel.Client });
            report.Subreports["SaleDetailsSubReport"].Database.Tables["SaleDetailsViewModel"]
                .SetDataSource(invoiceViewModel.SaleDetails);

            var documentStream = report.ExportToStream(ExportFormatType.PortableDocFormat);

            var filename = $"Factura #{id} ({sale.SaleDate?.ToString("dd/MM/yyyy")}).pdf";
            return File(documentStream, "application/pdf");
        }

        private decimal GetSaleTotal(ICollection<SaleDetails> saleDetails)
        {
            var saleSubtotal = (decimal)saleDetails.Sum(sd => (sd.CurrentPrice - sd.Discount) * sd.Quantity);

            var saleTotal = Math.Round(saleSubtotal * (1 + SaleConstants.Taxes), 2);

            return saleTotal;
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

        private ClientViewModel MapClientToViewModel(Clients client)
        {
            var clientViewModel = new ClientViewModel()
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Dui = client.Dui,
                Email = client.Email,
                Phone = client.Phone,
                Address = client.Address,
            };

            return clientViewModel;
        }

        private List<InvoiceSaleDetailViewModel> MapSaleDetailsToInvoiceViewModel(ICollection<SaleDetails> saleDetails)
        {
            var saleDetailsViewModel = saleDetails
                .Select(saleDetail => new InvoiceSaleDetailViewModel()
                {
                    Id = saleDetail.Id,
                    SaleId = saleDetail.SaleId,
                    ProductName = saleDetail.Products.Name,
                    CurrentPrice = saleDetail.CurrentPrice ?? 0,
                    Quantity = saleDetail.Quantity ?? 0,
                    Discount = saleDetail.Discount ?? 0
                })
                .ToList();

            return saleDetailsViewModel;
        }

        private List<SelectListItem> GetProducts()
        {
            var products = _productsService.GetProducts();

            var productsSelectList = products
                .Select(product => new SelectListItem
                {
                    Value = product.Id.ToString(),
                    Text = $"{product.Id}. {product.Name} - {product.UnitTypes.Name} ({product.Stock}) - ${product.Price}"
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
            var clients = _clientsService.GetClients();

            var clientsSelectList = clients
                .Select(client => new SelectListItem
                {
                    Value = client.Id.ToString(),
                    Text = $"{client.Id}. {client.FirstName} {client.LastName} ({client.Dui})"
                })
                .Prepend(new SelectListItem
                {
                    Value = "",
                    Text = "Seleccionar cliente"
                })
                .ToList();

            return clientsSelectList;
        }
    }
}