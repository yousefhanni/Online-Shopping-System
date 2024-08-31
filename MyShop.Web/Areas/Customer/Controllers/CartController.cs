using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Models;
using MyShop.Domain.ViewModels;
using MyShop.Utilities;
using Stripe.Checkout;
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
                var count = (await _unitOfWork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == shoppingcart.ApplicationUserId)).ToList().Count - 1;
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


        //Change This Name To Get Order => 
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            // Retrieve the current user's identity claims
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Initialize a new instance of ShoppingCartVM to hold the shopping cart details and order header information
            ShoppingCartVM = new ShoppingCartVM()
            {
                // Retrieve the list of shopping cart items for the logged-in user, including the related Product information
                CartsList = await _unitOfWork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader() // Initialize a new OrderHeader object
            };

            // Retrieve the ApplicationUser object for the logged-in user
            ShoppingCartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUser.GetItemAsync(x => x.Id == claim.Value);

            // Pre-fill the OrderHeader fields with the user's saved information
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            // Calculate the total price of the order by summing up the price of each item multiplied by its quantity
            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            // Return the Summary view, passing the ShoppingCartVM as the model
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> POSTSummary(ShoppingCartVM ShoppingCartVM)
        {
            // Retrieve the current user's identity claims
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Retrieve the shopping cart items for the logged-in user, including the related Product information
            ShoppingCartVM.CartsList = await _unitOfWork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == claim.Value, includeProperties: "Product");

            // Set the initial status and order details
            ShoppingCartVM.OrderHeader.OrderStatus = AppConstants.Pending;
            ShoppingCartVM.OrderHeader.PaymentStatus = AppConstants.Pending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            // Calculate the total price of the order by summing up the price of each item multiplied by its quantity
            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            // Add the OrderHeader to the database
            await _unitOfWork.OrderHeader.AddAsync(ShoppingCartVM.OrderHeader);
            await _unitOfWork.CompleteAsync();

            // Add each OrderDetail (representing individual items in the order) to the database
            foreach (var item in ShoppingCartVM.CartsList)
            {
                OrderDetails orderDetail = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };

                await _unitOfWork.OrderDetail.AddAsync(orderDetail);
                await _unitOfWork.CompleteAsync();
            }
            
            var domain = "https://localhost:7282/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCartVM.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCartVM.OrderHeader.SessionId = session.Id;
            ShoppingCartVM.OrderHeader.PaymentIntentId = session.PaymentIntentId; // Show This made this 
           await _unitOfWork.CompleteAsync();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetItemAsync(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStatus(id, AppConstants.Approve, AppConstants.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId; //Show This Not
                await _unitOfWork.CompleteAsync();
            }
            List<ShoppingCart> shoppingcarts = (await _unitOfWork._ShoppingCartRepository.GetAllAsync(u => u.ApplicationUserId == orderHeader.ApplicationUserId)).ToList();
            HttpContext.Session.Clear();
            await _unitOfWork._ShoppingCartRepository.RemoveRangeAsync(shoppingcarts);
            await _unitOfWork.CompleteAsync();
            return View(id);
        }



    }
}
