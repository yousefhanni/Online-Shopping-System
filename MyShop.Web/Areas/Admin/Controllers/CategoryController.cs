using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Models;
using MyShop.Utilities;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.AdminRole + "," + AppConstants.EditorRole)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Category/Index
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();
            return View(categories);
        }

        // GET: Category/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                 await _unitOfWork.Category.AddAsync(category);
                await _unitOfWork.CompleteAsync();
                TempData["Create"] = "Item has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _unitOfWork.Category.GetItemAsync(c => c.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Category/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.UpdateAsync(category);
                await _unitOfWork.CompleteAsync();
                TempData["Update"] = "Item has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        // GET: Category/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Category.GetItemAsync(c => c.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Category/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _unitOfWork.Category.GetItemAsync(c => c.Id == id);
            if (category == null)
                return NotFound();

            _unitOfWork.Category.Remove(category);
            await _unitOfWork.CompleteAsync();
            TempData["Delete"] = "Item has been deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
