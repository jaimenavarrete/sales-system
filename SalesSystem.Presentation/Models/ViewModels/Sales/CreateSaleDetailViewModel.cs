namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class CreateSaleDetailViewModel
    {
        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal Discount { get; set; }
    }
}