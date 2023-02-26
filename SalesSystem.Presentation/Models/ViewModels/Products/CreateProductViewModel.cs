﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Models.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required]
        [DisplayName("Nombre")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El nombre debe contener entre 1 y 50 caracteres. Se han colocado {0} caracteres.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres. Se han colocado {0} caracteres.")]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Existencias")]
        public decimal Stock { get; set; } = 0m;

        [Required]
        [DisplayName("Precio")]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Unidad de medida")]
        public string UnitTypeId { get; set; }

        public IEnumerable<SelectListItem> UnitTypesList { get; set; }

        [Required]
        [DisplayName("Categoría")]
        public string CategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoriesList { get; set; }

        [DisplayName("Foto")]
        public string Photo { get; set; }
    }
}