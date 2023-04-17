namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class InvoiceSaleDetailViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public decimal Discount { get; set; }
    }
}