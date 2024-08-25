using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Domain.Models;
using MyShop.Domain.ViewModels;


namespace myshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var products = await _unitOfWork.Product.GetAllAsync(includeProperties: "Category");
            return Json(new { data = products });
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // استرجاع قائمة الفئات بشكل غير متزامن
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

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }

                _unitOfWork.Product.Add(productVM.Product);
                await _unitOfWork.CompleteAsync();
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Product.GetItemAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

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

                await _unitOfWork.Product.UpdateAsync(productVM.Product);
                await _unitOfWork.CompleteAsync();
                TempData["Update"] = "Data has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var productInDb = await _unitOfWork.Product.GetItemAsync(x => x.Id == id);
            if (productInDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

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
