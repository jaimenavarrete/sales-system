using SalesSystem.Business.Services;
using SalesSystem.Presentation.Models.ViewModels;
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
                    Created = product.Created
                })
                .ToList();

            return View(productsViewModel);
        }
    }
}