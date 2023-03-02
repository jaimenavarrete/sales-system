using SalesSystem.Business.Services;
using SalesSystem.Presentation.Models.ViewModels.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoriesService _categoriesService = new CategoriesService();

        // GET: Categories
        public ActionResult Index()
        {
            var categories = _categoriesService.GetCategories();

            var viewModel = categories
                .Select(category => new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Created = category.Created,
                    CreatedBy = category.CreatedBy,
                    Modified = category.Modified,
                    ModifiedBy = category.ModifiedBy
                })
                .ToList();

            return View(viewModel);
        }

        public ActionResult CreateCategory()
        {
            return View();
        }
    }
}