using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Presentation.Models.ViewModels.UnitTypes
{
    public class EditUnitTypeViewModel : CreateUnitTypeViewModel
    {
        [Required(ErrorMessage = "Hubo un error con la unidad de medida que está modificando. Por favor, refresque la página")]
        public int Id { get; set; }
    }
}