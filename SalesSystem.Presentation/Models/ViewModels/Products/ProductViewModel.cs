using System;

namespace SalesSystem.Presentation.Models.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Stock { get; set; }

        public decimal Price { get; set; }

        public decimal Total => Math.Round(Stock * Price, 2);

        public string UnitTypeName { get; set; }

        public string CategoryName { get; set; }

        public string PhotoUrl { get; set; }

        public string PhotoBase64 { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public string ModifiedBy { get; set; }
    }
}