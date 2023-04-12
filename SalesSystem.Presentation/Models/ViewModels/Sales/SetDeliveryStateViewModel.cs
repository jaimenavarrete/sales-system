using SalesSystem.Business.Enums;

namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class SetDeliveryStateViewModel
    {
        public int Id { get; set; }

        public DeliveryState NewDeliveryState { get; set; }
    }
}