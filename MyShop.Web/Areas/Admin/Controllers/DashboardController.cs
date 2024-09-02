using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Utilities;
using System.Threading.Tasks;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.AdminRole)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Orders = (await _unitOfWork.OrderHeader.GetAllAsync()).Count();
            //ViewBag.ApprovedOrders = (await _unitOfWork.OrderHeader.GetAllAsync(x => x.OrderStatus == AppConstants.Approve)).Count();
            ViewBag.Users = (await _unitOfWork.ApplicationUser.GetAllAsync()).Count();
            ViewBag.Products = (await _unitOfWork.Product.GetAllAsync()).Count();
            ViewBag.Categories = (await _unitOfWork.Category.GetAllAsync()).Count();

            return View();
        }
    }
}
