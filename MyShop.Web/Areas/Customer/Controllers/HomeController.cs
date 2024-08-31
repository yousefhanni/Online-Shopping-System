using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Models;
using MyShop.Domain.ViewModels;
using MyShop.Utilities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly ILogger<HomeController> _logger;

        // Constructor to inject the Unit of Work for database operations and logger
        public HomeController(IUnitOfWork unitofwork, ILogger<HomeController> logger)
        {
            _unitofwork = unitofwork;
            _logger = logger;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            _logger.LogInformation("User is adding a product to the cart.");
            _logger.LogInformation($"ProductId: {shoppingCart.ProductId}, UserId: {shoppingCart.ApplicationUserId}");

            ShoppingCart shoppingCartObj = await _unitofwork._ShoppingCartRepository.GetItemAsync(
                s => s.ApplicationUserId == claim.Value && s.ProductId == shoppingCart.ProductId);

            if (shoppingCartObj == null)
            {
                await _unitofwork._ShoppingCartRepository.AddAsync(shoppingCart);
                await _unitofwork.CompleteAsync();
                string userSessionKey = $"{AppConstants.SessionKey}_{claim.Value}";
                HttpContext.Session.SetInt32(userSessionKey, _unitofwork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == claim.Value).Result.Count());
            }
            else
            {
                _unitofwork._ShoppingCartRepository.IncreaseCount(shoppingCartObj, shoppingCart.Count);
                await _unitofwork.CompleteAsync();
            }

            _logger.LogInformation("Redirecting to the Index page after adding product to cart.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int ProductId)
        {
            _logger.LogInformation($"Fetching details for ProductId: {ProductId}");

            if (ProductId == 0)
            {
                _logger.LogWarning("ProductId is 0, redirecting to Index.");
                return RedirectToAction("Index");
            }

            var product = await _unitofwork.Product.GetItemAsync(v => v.Id == ProductId, includeProperties: "Category");

            if (product == null)
            {
                _logger.LogWarning($"Product with ProductId: {ProductId} not found.");
                return NotFound(); // Return a 404 page if the product is not found
            }

            var relatedProducts = await _unitofwork.Product.GetAllAsync(
                p => p.CategoryId == product.CategoryId && p.Id != product.Id, includeProperties: "Category");

            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = ProductId,
                Product = product,
                Count = 1,
                RelatedProducts = relatedProducts.ToList()
            };

            // Store the ProductId in session to use after login
            HttpContext.Session.SetInt32("CurrentProductId", ProductId);
            _logger.LogInformation($"Stored ProductId {ProductId} in session.");

            return View(obj);
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
