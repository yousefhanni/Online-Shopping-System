using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Models;
using MyShop.Domain.ViewModels;
using MyShop.Utilities;
using Stripe;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> MyOrders()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var orders = await _unitOfWork.OrderHeader.GetAllAsync(
                o => o.ApplicationUserId == claim.Value,
                includeProperties: "OrderDetails"
            );

            return View(orders);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Retrieve the order to be canceled
            var orderHeader = await _unitOfWork.OrderHeader.GetItemAsync(o => o.Id == orderId && o.ApplicationUserId == claim.Value);

            if (orderHeader == null)
            {
                return NotFound(); // Return a 404 if the order is not found or doesn't belong to the current user
            }

            // Check if the order status allows cancellation
            if (orderHeader.OrderStatus == AppConstants.Pending || orderHeader.OrderStatus == AppConstants.Approve)
            {
                if (orderHeader.PaymentStatus == AppConstants.Approve)
                {
                    var refundOptions = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeader.PaymentIntentId
                    };

                    var refundService = new RefundService();
                    Refund refund = refundService.Create(refundOptions);

                    _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, AppConstants.Cancelled, AppConstants.Refund);
                }
                else
                {
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, AppConstants.Cancelled, AppConstants.Cancelled);
                }
                await _unitOfWork.CompleteAsync();

                TempData["Update"] = "Order has been Cancelled Successfully";
                return RedirectToAction("MyOrders");
            }

            TempData["Error"] = "Order cannot be cancelled at this stage";
            return RedirectToAction("MyOrders");
        }
        [HttpGet]
        public async Task<IActionResult> OrderSummary(int id)
        {
            var order = await _unitOfWork.OrderHeader.GetItemAsync(o => o.Id == id, includeProperties: "OrderDetails,OrderDetails.Product");

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

    }
}
