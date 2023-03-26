namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class SaleProductsListViewModel
    {
        public int[] ProductId { get; set; }

        public decimal[] Quantity { get; set; }

        public decimal[] Discount { get; set; }
    }
}