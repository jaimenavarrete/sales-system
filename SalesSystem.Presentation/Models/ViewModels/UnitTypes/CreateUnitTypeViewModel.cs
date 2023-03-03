using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Presentation.Models.ViewModels.UnitTypes
{
    public class CreateUnitTypeViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El nombre debe contener entre 1 y 50 caracteres. Se han colocado {0} caracteres.")]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres. Se han colocado {0} caracteres.")]
        [DisplayName("Descripción")]
        public string Description { get; set; }
    }
}