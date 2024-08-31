using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Domain.Models;
using MyShop.Domain.ViewModels;
using MyShop.Utilities;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.AdminRole + "," + AppConstants.EditorRole)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor to inject Unit of Work and Web Host Environment for file handling
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // Action to render the main product management page
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // Action to fetch product data in JSON format for displaying in a DataTable
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var products = await _unitOfWork.Product.GetAllAsync(includeProperties: "Category");
            return Json(new { data = products });
        }

        // Action to render the Create Product form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Fetch the list of categories for populating the dropdown
            var categoryList = await _unitOfWork.Category.GetAllAsync();

            ProductViewModel productVM = new ProductViewModel()
            {
                Product = new Product(),
                CategoryList = categoryList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }

        // Action to handle the form submission for creating a new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);

                    // Save the uploaded image file to the server
                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }

                // Add the new product to the database
                await  _unitOfWork.Product.AddAsync(productVM.Product);
                await _unitOfWork.CompleteAsync();
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }

        // Action to render the Edit Product form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the product to edit by its ID
            var product = await _unitOfWork.Product.GetItemAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            // Populate the product view model with the existing data and category list
            ProductViewModel productVM = new ProductViewModel()
            {
                Product = product,
                CategoryList = (await _unitOfWork.Category.GetAllAsync()).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }

        // Action to handle the form submission for editing an existing product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, "Images", "Products");
                    var ext = Path.GetExtension(file.FileName);

                    // If a new image is uploaded, delete the old one and save the new one
                    if (productVM.Product.Img != null)
                    {
                        var oldimg = Path.Combine(RootPath, productVM.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    productVM.Product.Img = Path.Combine("Images", "Products", filename + ext);
                }

                // Update the product in the database
                await _unitOfWork.Product.UpdateAsync(productVM.Product);
                await _unitOfWork.CompleteAsync();
                TempData["Update"] = "Data has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }

        // Action to delete a product by its ID
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var productInDb = await _unitOfWork.Product.GetItemAsync(x => x.Id == id);
            if (productInDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            // Remove the product from the database and delete the associated image file
            _unitOfWork.Product.Remove(productInDb);
            var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, productInDb.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldimg))
            {
                System.IO.File.Delete(oldimg);
            }
            await _unitOfWork.CompleteAsync();
            return Json(new { success = true, message = "Product has been Deleted" });
        }
    }
}
