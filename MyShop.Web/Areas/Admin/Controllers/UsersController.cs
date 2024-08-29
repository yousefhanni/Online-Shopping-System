using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.DAL.Data;
using MyShop.Utilities;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")] // Specifies that this controller belongs to the "Admin" area
    [Authorize(Roles = AppConstants.AdminRole)]    // This line restricts access to this controller to users with the "AdminRole", but it's currently commented out
    public class UsersController : Controller
    {
        // Reference to the ApplicationDbContext for database access
        private readonly ApplicationDbContext _context;

        // Constructor for the controller, with ApplicationDbContext injected via dependency injection
        public UsersController(ApplicationDbContext context)
        {
            _context = context; // Store the injected ApplicationDbContext into the class-level variable
        }

        // Action method to display all users who have logged into the app, except the current admin user
        public IActionResult Index()
        {
            // Get the identity of the current user from the ClaimsIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // Retrieve the UserId claim
            string userid = claim.Value; // Store the current user's ID

            // Return a view that lists all users except the current user (with UserId = userid)
            return View(_context.ApplicationUsers.Where(x => x.Id != userid).ToList());
        }

        //The method toggles the lock state of a user's account.
        public IActionResult LockUnlock(string? id)
        {
            // Search for the user in the database using their ID
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);

            // If the user is not found, return a "NotFound" result
            if (user == null)
            {
                return NotFound();
            }

            // Check if the user's account is not locked or if the lock period has expired
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                // If the account is not locked or the lock period has expired, lock the account for one year
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                // If the account is currently locked, unlock it by setting the lockout end time to the current time
                user.LockoutEnd = DateTime.Now;
            }

            // Save the changes to the database
            _context.SaveChanges();

            // Redirect the user back to the "Index" action in the "Users" controller within the "Admin" area
            return RedirectToAction("Index", "Users", new { area = "Admin" });
        }

    }
}
