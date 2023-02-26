using SalesSystem.Business.Services;
using SalesSystem.Presentation.Models.ViewModels.Products;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsService _productsService;

        public ProductsController()
        {
            _productsService = new ProductsService();
        }

        // GET: Products
        [HttpGet]
        public ActionResult Index()
        {
            var products = _productsService.GetProducts();

            var productsViewModel = products
                .Select(product => new ProductViewModel() { 
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Stock = product.Stock,
                    Price = product.Price,
                    UnitTypeName = product.UnitTypes.Name,
                    CategoryName = product.Categories.Name,
                    PhotoUrl = product.PhotoUrl,
                    Created = product.Created,
                    CreatedBy = product.CreatedBy,
                    Modified = product.Modified,
                    ModifiedBy = product.ModifiedBy
                })
                .ToList();

            return View(productsViewModel);
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            var categoriesList = new List<SelectListItem>() {
                new SelectListItem { Text = "Categoría 1", Value = "2" },
                new SelectListItem { Text = "Categoría 2", Value = "3" }
            };
            var unitTypesList = new List<SelectListItem>() {
                new SelectListItem { Text = "Pieza", Value = "2" },
                new SelectListItem { Text = "Litro", Value = "4" }
            };

            var model = new CreateProductViewModel()
            {
                CategoriesList = categoriesList,
                UnitTypesList = unitTypesList
            };

            return View(model);
        }
    }
}