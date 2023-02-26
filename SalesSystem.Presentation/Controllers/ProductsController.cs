﻿using SalesSystem.Business.Services;
using SalesSystem.DataAccess.Data;
using SalesSystem.Presentation.Models.ViewModels.Products;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsService _productsService;

        public ProductsController()
        {
            _productsService = new ProductsService();
        }

        // GET: Products
        [HttpGet]
        public ActionResult Index()
        {
            var products = _productsService.GetProducts();

            var productsViewModel = products
                .Select(product => new ProductViewModel() { 
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Stock = product.Stock,
                    Price = product.Price,
                    UnitTypeName = product.UnitTypes.Name,
                    CategoryName = product.Categories.Name,
                    PhotoUrl = product.PhotoUrl,
                    Created = product.Created,
                    CreatedBy = product.CreatedBy,
                    Modified = product.Modified,
                    ModifiedBy = product.ModifiedBy
                })
                .ToList();

            return View(productsViewModel);
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            var viewModel = new CreateProductViewModel()
            {
                CategoriesList = GetCategories(),
                UnitTypesList = GetUnitTypes()
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateProduct(CreateProductViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.CategoriesList = GetCategories();
                viewModel.UnitTypesList = GetUnitTypes();

                return View(viewModel);
            }

            var product = new Products()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Stock = viewModel.Stock,
                Price = viewModel.Price,
                UnitTypeId = viewModel.UnitTypeId,
                CategoryId = viewModel.CategoryId
            };

            _productsService.CreateProduct(product);

            return RedirectToAction("Index", "Products");
        }

        private List<SelectListItem> GetCategories() => new List<SelectListItem>() {
            new SelectListItem { Text = "Seleccionar categoría", Value = ""},
            new SelectListItem { Text = "Categoría 1", Value = "2" },
            new SelectListItem { Text = "Categoría 2", Value = "3" }
        };

        private List<SelectListItem> GetUnitTypes() => new List<SelectListItem>() {
            new SelectListItem { Text = "Seleccionar tipo de medida", Value = ""},
            new SelectListItem { Text = "Pieza", Value = "2" },
            new SelectListItem { Text = "Litro", Value = "4" }
        };
    }
}