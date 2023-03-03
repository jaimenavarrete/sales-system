using SalesSystem.Business.Services;
using SalesSystem.DataAccess.Data;
using SalesSystem.Presentation.Models.ViewModels.UnitTypes;
using System.Linq;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class UnitTypesController : Controller
    {
        private readonly UnitTypesService _unitTypesService = new UnitTypesService();

        // GET: Measure
        public ActionResult Index()
        {
            var unitTypes = _unitTypesService.GetUnitTypes();

            var viewModel = unitTypes
            .Select(unitType => new UnitTypeViewModel() {
                Id = unitType.Id,
                Name = unitType.Name,
                Description = unitType.Description,
                Created = unitType.Created,
                CreatedBy = unitType.CreatedBy,
                Modified = unitType.Modified,
                ModifiedBy = unitType.ModifiedBy
            })
            .ToList();

            return View(viewModel);
        }

        public ActionResult CreateUnitType()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateUnitType(CreateUnitTypeViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                TempData["error"] = "Error. Por favor, revise que todos los campos sean válidos.";

                return View(viewModel);
            }

            var unitType = new UnitTypes()
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };

            _unitTypesService.CreateUnitType(unitType);

            TempData["success"] = "La unidad de medida fue creada exitosamente.";

            return RedirectToAction("Index");
        }

        public ActionResult EditUnitType(int id = 0)
        {
            var unitType = _unitTypesService.GetUnitTypeById(id);

            if(unitType is null)
            {
                TempData["error"] = "La unidad de medida que ha seleccionado no existe.";
                return RedirectToAction("Index");
            }

            var viewModel = new EditUnitTypeViewModel()
            {
                Id = id,
                Name = unitType.Name,
                Description = unitType.Description
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditUnitType(EditUnitTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Error. Por favor, revise que todos los campos sean válidos.";

                return View(viewModel);
            }

            var unitType = new UnitTypes()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description
            };

            _unitTypesService.EditUnitType(unitType);

            TempData["success"] = "La unidad de medida fue editada exitosamente.";

            return RedirectToAction("Index");
        }
    }
}