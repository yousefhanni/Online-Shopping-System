using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.ViewModels;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        // Constructor to inject the Unit of Work for database operations
        public HomeController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        // Action to display the list of products, with optional filtering by search query and category
        public async Task<IActionResult> Index(string query, string category)
        {
            var products = await _unitofwork.Product.GetAllAsync(null, "Category");

            // Filter products by search query if provided
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter products by category if provided, ensuring the category is not null
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category != null &&
                                                p.Category.Name.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(products);
        }

        // Action to display the details of a specific product, including related products in the same category
        public async Task<IActionResult> Details(int ProductId)
        {
            // Fetch the main product based on the provided ID
            var product = await _unitofwork.Product.GetItemAsync(p => p.Id == ProductId, includeProperties: "Category");

            if (product == null)
            {
                return NotFound();
            }

            // Fetch related products from the same category, excluding the current product
            var relatedProducts = await _unitofwork.Product.GetAllAsync(
                p => p.CategoryId == product.CategoryId && p.Id != product.Id,
                includeProperties: "Category"
            );

            // Create the view model to pass to the view, including the main product and related products
            ShoppingCartViewModel cartViewModel = new ShoppingCartViewModel()
            {
                Product = product,
                Count = 1,
                RelatedProducts = relatedProducts.ToList()
            };

            return View(cartViewModel);
        }

        // Action to provide search suggestions via AJAX for the autocomplete feature
        [HttpGet]
        public async Task<JsonResult> GetProductSuggestions(string term)
        {
            var products = await _unitofwork.Product.GetAllAsync();
            var suggestions = products
                              .Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                              .Select(p => p.Name)
                              .ToList();

            return Json(suggestions);
        }
    }
}
