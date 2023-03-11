using SalesSystem.Business.Services;
using SalesSystem.Presentation.Models.ViewModels.Products;
using SalesSystem.Presentation.Utilities;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class DataController : Controller
    {
        private readonly ProductsService _productsService = new ProductsService();

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
    }
}