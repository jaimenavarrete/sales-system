using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Presentation.Models.ViewModels.Products
{
    public class EditProductViewModel : CreateProductViewModel
    {
        [Required(ErrorMessage = "Hubo un error con el producto que está modificando. Por favor, refresque la página")]
        public int Id { get; set; }
    }
}