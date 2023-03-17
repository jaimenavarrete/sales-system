using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class CreateSaleViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido")]
        [DisplayName("Cliente")]
        public int ClientId { get; set; }

        [DisplayName("¿Es entrega a domicilio?")]
        public bool IsHomeDelivery { get; set; }

        [DisplayName("Observación")]
        public string Observation { get; set; }

        [DisplayName("¿La venta ha sido pagada?")]
        public bool IsPaymentCompleted { get; set; }

        public IEnumerable<SelectListItem> ClientsList { get; set; }
    }
}