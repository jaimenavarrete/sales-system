using SalesSystem.Business.Services;
using SalesSystem.Presentation.Models.ViewModels.Products;
using SalesSystem.Presentation.Models.ViewModels.Sales;
using SalesSystem.Presentation.Utilities;
using System.Linq;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class DataController : Controller
    {
        private readonly ProductsService _productsService = new ProductsService();
        private readonly SalesService _salesService = new SalesService();

        public JsonResult GetProductById(int id)
        {
            var product = _productsService.GetProductById(id);

            if(product is null)
            {
                return null;
            }

            var productPhotoBytes = _productsService.GetProductPhotoBytesByFileName(product.PhotoUrl);

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price,
                UnitTypeName = product.UnitTypes.Name,
                CategoryName = product.Categories.Name,
                PhotoBase64 = PhotoUtilities.ConvertBytesToBase64(productPhotoBytes),
                Created = product.Created
            };

            return Json(productViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesDataPerMonth()
        {
            // Group sales by month and year
            var sales = _salesService.GetOnlySales()
                .GroupBy(sale => new {
                    sale.Created.Date.Year,
                    sale.Created.Date.Month
                })
                .Select(group => new SalesDataPerMonthViewModel()
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    CompletedSalesCount = group.Where(s => s.Completed).Count(),
                    PendingSalesCount = group.Where(s => !s.Completed).Count(),
                    Income = group.Where(s => s.PaymentCompleted).Sum(s => s.Total),
                    Debt = group.Where(s => !s.PaymentCompleted).Sum(s => s.Total)
                })
                .ToList();

            return Json(sales, JsonRequestBehavior.AllowGet);
        }
    }
}