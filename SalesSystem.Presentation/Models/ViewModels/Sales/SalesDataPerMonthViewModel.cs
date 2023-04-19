namespace SalesSystem.Presentation.Models.ViewModels.Sales
{
    public class SalesDataPerMonthViewModel
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int CompletedSalesCount { get; set; }

        public int PendingSalesCount { get; set; }

        public decimal Income { get; set; }

        public decimal Debt { get; set; }
    }
}