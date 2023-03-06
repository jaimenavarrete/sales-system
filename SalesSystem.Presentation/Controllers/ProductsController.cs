using SalesSystem.Business.Services;
using SalesSystem.DataAccess.Data;
using SalesSystem.Presentation.Models.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesSystem.Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsService _productsService = new ProductsService();
        private readonly CategoriesService _categoriesService = new CategoriesService();
        private readonly UnitTypesService _unitTypesService = new UnitTypesService();

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

                TempData["error"] = "Error. Por favor, revise que todos los campos sean válidos.";

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

            var productPhotoBytes = ConvertPhotoToJpegBytes(viewModel.Photo);

            _productsService.CreateProduct(product, productPhotoBytes);

            TempData["success"] = "El producto ha sido creado exitosamente.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditProduct(int id = 0)
        {
            var product = _productsService.GetProductById(id);

            if(product is null)
            {
                TempData["error"] = "El producto que ha seleccionado no existe.";
                return RedirectToAction("Index");
            }

            var viewModel = new EditProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price,
                UnitTypeId = product.UnitTypeId,
                CategoryId = product.CategoryId,

                CategoriesList = GetCategories(),
                UnitTypesList = GetUnitTypes()
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditProduct(EditProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.CategoriesList = GetCategories();
                viewModel.UnitTypesList = GetUnitTypes();

                TempData["error"] = "Error. Por favor, revise que todos los campos sean válidos.";

                return View(viewModel);
            }

            var product = new Products()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Stock = viewModel.Stock,
                Price = viewModel.Price,
                UnitTypeId = viewModel.UnitTypeId,
                CategoryId = viewModel.CategoryId
            };

            _productsService.EditProduct(product);

            TempData["success"] = "El producto ha sido editado exitosamente.";

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {
            _productsService.DeleteProduct(id);

            TempData["success"] = "El producto ha sido borrado exitosamente.";

            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetCategories()
        {
            var categories = _categoriesService.GetCategories();

            var categoriesSelectList = categories
                .Select(category => new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                })
                .Prepend(new SelectListItem
                {
                    Value = "",
                    Text = "Seleccionar categoria"
                })
                .ToList();

            return categoriesSelectList;
        }

        private List<SelectListItem> GetUnitTypes()
        {
            var unitTypes = _unitTypesService.GetUnitTypes();

            var unitTypesSelectList = unitTypes
                .Select(unitType => new SelectListItem
                {
                    Value = unitType.Id.ToString(),
                    Text = unitType.Name
                })
                .Prepend(new SelectListItem
                {
                    Value = "",
                    Text = "Seleccionar unidad de medida"
                })
                .ToList();

            return unitTypesSelectList;
        }

        private byte[] ConvertPhotoToJpegBytes(HttpPostedFileBase photo)
        {
            var oldPhoto = new MemoryStream();
            photo.InputStream.CopyTo(oldPhoto);

            var newPhoto = new MemoryStream();

            var imageStream = Image.FromStream(oldPhoto);
            imageStream.Save(newPhoto, ImageFormat.Jpeg);

            return newPhoto.ToArray();
        }

        private byte[] ConvertPhotoToBytes(HttpPostedFileBase photo)
        {
            if(photo is null)
            {
                return null;
            }

            BinaryReader reader = new BinaryReader(photo.InputStream);
            byte[] photoBytes = reader.ReadBytes((int)photo.ContentLength);

            return photoBytes;
        }

        private string ConvertBytesToBase64(byte[] photoBytes)
        {
            if (photoBytes is null)
            {
                return null;
            }

            var base64String = Convert.ToBase64String(photoBytes);
            var photoBase64 = "data:image/jpeg;base64," + base64String;

            return photoBase64;
        }
    }
}