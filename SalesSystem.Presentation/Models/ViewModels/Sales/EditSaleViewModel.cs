using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class EditSaleViewModel : CreateSaleViewModel
    {
        [Required(ErrorMessage = "Hubo un error con la venta que está modificando. Por favor, refresque la página")]
        public int Id { get; set; }
    }
}