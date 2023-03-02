using SalesSystem.Business.Exceptions;
using SalesSystem.Business.Services;
using SalesSystem.DataAccess.Data;
using SalesSystem.Presentation.Models.ViewModels.Categories;
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

        [HttpPost]
        public ActionResult CreateCategory(CreateCategoryViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                TempData["error"] = "Error. Por favor, revise que todos los campos sean válidos.";

                return View(viewModel);
            }

            var category = new Categories()
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };

            _categoriesService.CreateCategory(category);

            TempData["success"] = "La categoría ha sido creada exitosamente.";

            return RedirectToAction("Index");
        }

        public ActionResult EditCategory(int id = 0)
        {
            var category = _categoriesService.GetCategoryById(id);

            if(category is null)
            {
                TempData["error"] = "La categoría que ha seleccionado no existe.";
                return RedirectToAction("Index");
            }

            var viewModel = new EditCategoryViewModel()
            {
                Id = id,
                Name = category.Name,
                Description = category.Description,
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                _categoriesService.DeleteCategory(id);

                TempData["success"] = "La categoría ha sido borrada exitosamente.";
            }
            catch(BusinessException exception)
            {
                TempData["error"] = exception.Message;
            }

            return RedirectToAction("Index");
        }
    }
}