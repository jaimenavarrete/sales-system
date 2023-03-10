using SalesSystem.Business.Services;
using SalesSystem.Presentation.Models.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price,
                UnitTypeName = product.UnitTypes.Name,
                CategoryName = product.Categories.Name,
                Created = product.Created,
            };

            return Json(productViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}