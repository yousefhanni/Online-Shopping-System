using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.ViewModels;
using MyShop.Utilities;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = await _unitOfWork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
            };

            // حساب الإجمالي الكلي
            decimal totalPrice = 0;
            foreach (var item in ShoppingCartVM.CartsList)
            {
                totalPrice += (item.Count * item.Product.Price);
            }
            ViewBag.TotalPrice = totalPrice;

            return View(ShoppingCartVM);
        }

        public async Task<IActionResult> Plus(int cartid)
        {
            var shoppingcart = await _unitOfWork._ShoppingCartRepository.GetItemAsync(x => x.Id == cartid);
            _unitOfWork._ShoppingCartRepository.IncreaseCount(shoppingcart, 1);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Minus(int cartId)
        {
            var shoppingcart = await _unitOfWork._ShoppingCartRepository.GetItemAsync(x => x.Id == cartId);
            if (shoppingcart.Count <= 1)
            {
                await _unitOfWork._ShoppingCartRepository.Remove(shoppingcart);
                var count = (await _unitOfWork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == shoppingcart.ApplicationUserId)).ToList().Count-1;
                HttpContext.Session.SetInt32(AppConstants.SessionKey, count);

                //return RedirectToAction("Index", "Home");
            }
            else
            {
                _unitOfWork._ShoppingCartRepository.DecreaseCount(shoppingcart, 1);
            }
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int cartId)
        {
            var shoppingcart = await _unitOfWork._ShoppingCartRepository.GetItemAsync(x => x.Id == cartId);
            if (shoppingcart != null)
            {
                await _unitOfWork._ShoppingCartRepository.Remove(shoppingcart);
                await _unitOfWork.CompleteAsync();

                var count = (await _unitOfWork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == shoppingcart.ApplicationUserId)).Count();

                // Update the session with the new count
                string userSessionKey = $"{AppConstants.SessionKey}_{shoppingcart.ApplicationUserId}";
                HttpContext.Session.SetInt32(userSessionKey, count);

                // Optionally, clear the session if the cart is empty
                if (count == 0)
                {
                    HttpContext.Session.Remove(userSessionKey);
                }
            }

            return RedirectToAction("Index");
        }


    }
}
