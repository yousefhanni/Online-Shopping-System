using Microsoft.AspNetCore.Mvc;
using MyShop.Utilities;
using System.Security.Claims;

namespace MyShop.Web.ViewComponents
{
    // Define a custom ViewComponent to display the cart item count
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitofwork;

        // Constructor to inject the UnitOfWork for database operations
        public ShoppingCartViewComponent(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                string userSessionKey = $"{AppConstants.SessionKey}_{claim.Value}";
                if (HttpContext.Session.GetInt32(userSessionKey) != null)
                {
                    return View(HttpContext.Session.GetInt32(userSessionKey));
                }
                else
                {
                    var cartItems = await _unitofwork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == claim.Value);
                    int cartCount = cartItems.ToList().Count;
                    HttpContext.Session.SetInt32(userSessionKey, cartCount);
                    return View(cartCount);
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }


    }
}
