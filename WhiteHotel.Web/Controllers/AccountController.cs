using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Application.Common.Utility;
using WhiteHotel.Domain.Entities;
using WhiteHotel.Web.ViewModels;

namespace WhiteHotel.Web.Controllers
{
    public class AccountController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        
        public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };
            return View(loginVM);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            createRole(SD.Role_Admin);
            createRole(SD.Role_Customer);

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                RedirectUrl = returnUrl 
            };

            return View(registerVM);

            void createRole(string role)
            {
                if (!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                    _roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    else
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                        return RedirectToAction(nameof(Index), "Home");
                    else
                        return LocalRedirect(registerVM.RedirectUrl);
                    }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                } 

            }
            registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });

            return View(registerVM);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);
                    
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))                   
                        return RedirectToAction("Index", "Dashboard");                    
                    else
                    {
                        if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                        return RedirectToAction(nameof(Index), "Home");
                        else
                            return LocalRedirect(loginVM.RedirectUrl);                        
                    }
                }
                else
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");              
            }
            return View(loginVM);
        }
    }
}
