using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Presentation.Models.ViewModels.Categories
{
    public class EditCategoryViewModel : CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Hubo un error con la cateegoría que está modificando. Por favor, refresque la página")]
        public int Id { get; set; }
    }
}