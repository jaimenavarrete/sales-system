namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class InvoiceSaleDetailViewModel
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public string ProductName { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal Quantity { get; set; }

        public decimal Discount { get; set; }
    }
}