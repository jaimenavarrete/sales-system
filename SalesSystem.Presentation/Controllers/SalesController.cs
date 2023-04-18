using CrystalDecisions.Shared;
using SalesSystem.Business.Constants;
using SalesSystem.Business.Enums;
using SalesSystem.Business.Exceptions;
using SalesSystem.Business.Services;
using SalesSystem.DataAccess.Data;
using SalesSystem.Presentation.Models.ViewModels.Clients;
using SalesSystem.Presentation.Models.ViewModels.Sales;
using SalesSystem.Presentation.Utilities;
using SalesSystem.Presentation.Views.Sales.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
                    IsHomeDelivery = sale.HomeDelivery,
                    SaleDate = sale.SaleDate,
                    DeliveryDate = sale.DeliveryDate,
                    IsCompleted = sale.Completed,
                    IsPaymentCompleted = sale.PaymentCompleted,
                    SaleDetails = MapSaleDetailsToSaleViewModel(sale.SaleDetails),
                    Taxes = SaleConstants.Taxes
                })
                .ToList();

            return View(salesViewModel);
        }

        [HttpGet]
        public ActionResult ViewSale(int id = 0)
        {
            var sale = _salesService.GetSaleById(id);

            if (sale is null)
            {
                TempData["error"] = "Error. La venta que ha seleccionado, no existe.";

                return RedirectToAction("Index");
            }

            var viewModel = new ViewSaleViewModel() {
                Id = sale.Id,
                IsCompleted = sale.Completed,
                IsHomeDelivery = sale.HomeDelivery,
                DeliveryStateId = (DeliveryState?)sale.DeliveryStateId,
                IsPaymentCompleted = sale.PaymentCompleted,
                SaleDate = sale.SaleDate,
                DeliveryDate = sale.DeliveryDate,
                Observation = sale.Observation,
                SaleDetails = MapSaleDetailsToSaleViewModelWithPhotos(sale.SaleDetails),
                Client = MapClientToViewModel(sale.Clients),
                Taxes = SaleConstants.Taxes,
                DeliveryStatesList = GetDeliveryStates()
            };

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
            try
            {
                if (!ModelState.IsValid)
                {
                    viewModel.ClientsList = GetClients();
                    viewModel.ProductsList = GetProducts();

                    TempData["error"] = "Error. Por favor, revise que todos los campos sean válidos.";

                    return View(viewModel);
                }

                var sale = new Sales()
                {
                    ClientId = viewModel.ClientId,
                    HomeDelivery = viewModel.IsHomeDelivery,
                    PaymentCompleted = viewModel.IsPaymentCompleted,
                    Observation = viewModel.Observation,
                    SaleDetails = MapViewModelToSaleDetails(viewModel.SaleDetails)
                };

                _salesService.CreateSale(sale);

                TempData["success"] = "La venta fue realizada con éxito.";

                return RedirectToAction("Index");
            }
            catch(BusinessException exception)
            {
                TempData["error"] = exception.Message;

                return RedirectToAction("CreateSale");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteSale(int id = 0)
        {
            try
            {
                _salesService.DeleteSale(id);

                TempData["success"] = "La venta fue borrada con éxito.";
            }
            catch (BusinessException exception)
            {
                TempData["error"] = exception.Message;
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SetDeliveryState(SetDeliveryStateViewModel viewModel)
        {
            try
            {
                _salesService.SetDeliveryState(viewModel.Id, viewModel.NewDeliveryState);

                TempData["success"] = "El estado de envío de la venta se ha cambiado con éxito.";
            }
            catch (BusinessException exception)
            {
                TempData["error"] = exception.Message;
            }

            return RedirectToAction("ViewSale", new { id = viewModel.Id });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangeSaleState(int id = 0)
        {
            try
            {
                _salesService.ChangeSaleState(id);

                TempData["success"] = "El estado de de la venta se ha cambiado con éxito.";
            }
            catch (BusinessException exception)
            {
                TempData["error"] = exception.Message;
            }

            return RedirectToAction("ViewSale", new { id });
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
            report.Subreports["SaleDetailsSubReport"].Database.Tables["InvoiceSaleDetailViewModel"]
                .SetDataSource(invoiceViewModel.SaleDetails);

            var documentStream = report.ExportToStream(ExportFormatType.PortableDocFormat);

            var filename = $"Factura #{id} ({sale.SaleDate?.ToString("dd/MM/yyyy")}).pdf";
            return File(documentStream, "application/pdf");
        }

        private List<SaleDetails> MapViewModelToSaleDetails(IEnumerable<SaleDetailViewModel> saleDetailsViewModel)
        {
            var saleDetails = saleDetailsViewModel
                .Select(saleDetailViewModel => new SaleDetails()
                {
                    ProductId = saleDetailViewModel.ProductId,
                    Discount = saleDetailViewModel.Discount,
                    Quantity = saleDetailViewModel.Quantity
                })
                .ToList();

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

        private List<SaleDetailViewModel> MapSaleDetailsToSaleViewModelWithPhotos(ICollection<SaleDetails> saleDetails)
        {
            var saleDetailsViewModel = MapSaleDetailsToSaleViewModel(saleDetails);

            foreach(var saleDetail in saleDetailsViewModel)
            {
                var productPhotoBytes = _productsService.GetProductPhotoBytesByFileName(saleDetail.PhotoBase64);
                var photoBase64 = PhotoUtilities.ConvertBytesToBase64(productPhotoBytes);

                saleDetail.PhotoBase64 = photoBase64;
            }

            return saleDetailsViewModel;
        }

        private List<SaleDetailViewModel> MapSaleDetailsToSaleViewModel(ICollection<SaleDetails> saleDetails)
        {
            var saleDetailsViewModel = saleDetails
                .Select(saleDetail => {
                    var saleDetailViewModel = new SaleDetailViewModel()
                    {
                        ProductId = saleDetail.ProductId,
                        SaleId = saleDetail.SaleId,
                        ProductName = saleDetail.Products.Name,
                        Price = saleDetail.CurrentPrice,
                        Quantity = saleDetail.Quantity,
                        Discount = saleDetail.Discount,
                        // Save the photo filename in this property to get photo base 64 in another method and avoid creating a new property
                        PhotoBase64 = saleDetail.Products.PhotoUrl
                    };

                    return saleDetailViewModel;
                })
                .ToList();

            return saleDetailsViewModel;
        }

        private List<InvoiceSaleDetailViewModel> MapSaleDetailsToInvoiceViewModel(ICollection<SaleDetails> saleDetails)
        {
            var saleDetailsViewModel = saleDetails
                .Select(saleDetail => new InvoiceSaleDetailViewModel()
                {
                    ProductId = saleDetail.ProductId,
                    ProductName = saleDetail.Products.Name,
                    Price = Math.Round(saleDetail.CurrentPrice, 2),
                    Quantity = Math.Round(saleDetail.Quantity, 2),
                    Discount = Math.Round(saleDetail.Discount, 2)
                })
                .ToList();

            return saleDetailsViewModel;
        }

        private List<SelectListItem> GetProducts()
        {
            var products = _productsService.GetProductsWithStock();

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

        private List<SelectListItem> GetDeliveryStates()
        {
            var deliveryStates = _salesService.GetDeliveryStates();

            var deliveryStatesSelectList = deliveryStates
                .Select(deliveryState => new SelectListItem
                {
                    Value = deliveryState.Id.ToString(),
                    Text = deliveryState.Name
                })
                .ToList();

            return deliveryStatesSelectList;
        }
    }
}