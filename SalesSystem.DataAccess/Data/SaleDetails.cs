//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SalesSystem.DataAccess.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class SaleDetails
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public Nullable<decimal> CurrentPrice { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Discount { get; set; }
    
        public virtual Products Products { get; set; }
        public virtual Sales Sales { get; set; }
    }
}
