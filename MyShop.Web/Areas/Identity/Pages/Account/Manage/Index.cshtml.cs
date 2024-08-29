////// Licensed to the .NET Foundation under one or more agreements.
////// The .NET Foundation licenses this file to you under the MIT license.
////#nullable disable

////using System;
////using System.ComponentModel.DataAnnotations;
////using System.Text.Encodings.Web;
////using System.Threading.Tasks;
////using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.AspNetCore.Mvc.RazorPages;
////using MyShop.Domain.Models;

////namespace MyShop.Web.Areas.Identity.Pages.Account.Manage
////{
////    public class IndexModel : PageModel
////    {
////        private readonly UserManager<ApplicationUser> _userManager;
////        private readonly SignInManager<ApplicationUser> _signInManager;

////        public IndexModel(
////            UserManager<ApplicationUser> userManager,
////            SignInManager<ApplicationUser> signInManager)
////        {
////            _userManager = userManager;
////            _signInManager = signInManager;
////        }

////        /// <summary>
////        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////        ///     directly from your code. This API may change or be removed in future releases.
////        /// </summary>
////        public string Username { get; set; }

////        /// <summary>
////        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////        ///     directly from your code. This API may change or be removed in future releases.
////        /// </summary>
////        [TempData]
////        public string StatusMessage { get; set; }

////        /// <summary>
////        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////        ///     directly from your code. This API may change or be removed in future releases.
////        /// </summary>
////        [BindProperty]
////        public InputModel Input { get; set; }

////        /// <summary>
////        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////        ///     directly from your code. This API may change or be removed in future releases.
////        /// </summary>
////        public class InputModel
////        {
////            /// <summary>
////            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////            ///     directly from your code. This API may change or be removed in future releases.
////            /// </summary>
////            [Phone]
////            [Display(Name = "Phone number")]
////            public string PhoneNumber { get; set; }

////            public string Name { get; set; }

////            public string Address { get; set; }

////            public string City { get; set; }

////            [Display(Name = "Profile Image")]
////            public IFormFile ProfileImage { get; set; }
////        }

////        private async Task LoadAsync(ApplicationUser user)
////        {
////            var userName = await _userManager.GetUserNameAsync(user);
////            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

////            Username = userName;

////            Input = new InputModel
////            {
////                PhoneNumber = phoneNumber,
////                Address = user.Address,
////                Name=user.Name,
////                City = user.City
////            };
////        }

////        public async Task<IActionResult> OnGetAsync()
////        {
////            var user = await _userManager.GetUserAsync(User);
////            if (user == null)
////            {
////                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
////            }

////            await LoadAsync(user);
////            return Page();
////        }

////        public async Task<IActionResult> OnPostAsync()
////        {
////            var user = await _userManager.GetUserAsync(User);
////            if (user == null)
////            {
////                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
////            }

////            if (!ModelState.IsValid)
////            {
////                await LoadAsync(user);
////                return Page();
////            }

////            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
////            var name = user.Name;
////            var address = user.Address;
////            var city = user.City;

////            if (Input.Name != name)
////            {
////                user.Name = Input.Name;
////                await _userManager.UpdateAsync(user);

////            }
////            if (Input.Address != address)
////            {
////                user.Address = Input.Address;
////                await _userManager.UpdateAsync(user);
////            }
////            if (Input.City != city)
////            {
////                user.City = Input.City;
////                await _userManager.UpdateAsync(user);
////            }
////            if (Input.PhoneNumber != phoneNumber)
////            {
////                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
////                if (!setPhoneResult.Succeeded)
////                {
////                    StatusMessage = "Unexpected error when trying to set phone number.";
////                    return RedirectToPage();
////                }
////            }
////            await _signInManager.RefreshSignInAsync(user);
////            StatusMessage = "Your profile has been updated";
////            return RedirectToPage();
////        }
////    }
////}


//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MIT license.
//#nullable disable

//using System;
//using System.ComponentModel.DataAnnotations;
//using System.IO;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using MyShop.Domain.Models;

//namespace MyShop.Web.Areas.Identity.Pages.Account.Manage
//{
//    public class IndexModel : PageModel
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly IWebHostEnvironment _webHostEnvironment;

//        public IndexModel(
//            UserManager<ApplicationUser> userManager,
//            SignInManager<ApplicationUser> signInManager,
//            IWebHostEnvironment webHostEnvironment)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _webHostEnvironment = webHostEnvironment;
//        }

//        public string Username { get; set; }

//        [TempData]
//        public string StatusMessage { get; set; }

//        [BindProperty]
//        public InputModel Input { get; set; }

//        public string ProfileImage { get; set; } // Add this property to store the profile image path


//        public class InputModel
//        {
//            [Phone]
//            [Display(Name = "Phone number")]
//            public string PhoneNumber { get; set; }

//            public string Name { get; set; }

//            public string Address { get; set; }

//            public string City { get; set; }

//            [Display(Name = "Profile Image")]
//            public IFormFile ProfileImage { get; set; } // Property for file upload
//        }


//        private async Task LoadAsync(ApplicationUser user)
//        {
//            var userName = await _userManager.GetUserNameAsync(user);
//            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

//            Username = userName;
//            ProfileImage = user.ProfileImage; // Set the profile image path

//            Input = new InputModel
//            {
//                PhoneNumber = phoneNumber,
//                Address = user.Address,
//                Name = user.Name,
//                City = user.City
//            };
//        }



//        public async Task<IActionResult> OnGetAsync()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
//            }

//            await LoadAsync(user);
//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
//            }

//            if (!ModelState.IsValid)
//            {
//                await LoadAsync(user);
//                return Page();
//            }

//            if (Input.ProfileImage != null)
//            {
//                // Logic to save the uploaded profile image
//                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Input.ProfileImage.FileName)}";
//                var filePath = Path.Combine("wwwroot/images/Users", fileName);
//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await Input.ProfileImage.CopyToAsync(stream);
//                }
//                user.ProfileImage = fileName;
//                await _userManager.UpdateAsync(user);
//            }


//            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
//            var name = user.Name;
//            var address = user.Address;
//            var city = user.City;

//            if (Input.Name != name)
//            {
//                user.Name = Input.Name;
//                await _userManager.UpdateAsync(user);
//            }
//            if (Input.Address != address)
//            {
//                user.Address = Input.Address;
//                await _userManager.UpdateAsync(user);
//            }
//            if (Input.City != city)
//            {
//                user.City = Input.City;
//                await _userManager.UpdateAsync(user);
//            }
//            if (Input.PhoneNumber != phoneNumber)
//            {
//                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
//                if (!setPhoneResult.Succeeded)
//                {
//                    StatusMessage = "Unexpected error when trying to set phone number.";
//                    return RedirectToPage();
//                }
//            }

//            await _signInManager.RefreshSignInAsync(user);
//            StatusMessage = "Your profile has been updated";
//            return RedirectToPage();
//        }

//    }
//}
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyShop.Domain.Models;

namespace MyShop.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }

            public string City { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Address = user.Address,
                Name = user.Name,
                City = user.City
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var name = user.Name;
            var address = user.Address;
            var city = user.City;

            if (Input.Name != name)
            {
                user.Name = Input.Name;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Address != address)
            {
                user.Address = Input.Address;
                await _userManager.UpdateAsync(user);
            }
            if (Input.City != city)
            {
                user.City = Input.City;
                await _userManager.UpdateAsync(user);
            }
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
