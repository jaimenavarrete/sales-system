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
    }
}