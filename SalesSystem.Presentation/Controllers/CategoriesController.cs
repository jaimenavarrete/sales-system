using SalesSystem.Presentation.Models.ViewModels.Categories;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult Index()
        {
            var viewModel = new List<CategoryViewModel>()
            {
                new CategoryViewModel() { Id = 1, Name = "Categoría 1", Description = "Descripción 1" }
            };

            return View(viewModel);
        }
    }
}