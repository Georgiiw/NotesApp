using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Data.Entities;
using NotesApp.ViewModels.User;

namespace NotesApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            ApplicationUser newUser = new ApplicationUser();

            await _userManager.SetEmailAsync(newUser, model.Email);
            await _userManager.SetUserNameAsync(newUser, model.Email);

            IdentityResult result = await this._userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return this.View(model);
            }
            await _signInManager.SignInAsync(newUser, false);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Login(string? returnUrl)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            LoginFormModel model = new LoginFormModel()
            {
                ReturnUrl = returnUrl,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await this._signInManager
                .PasswordSignInAsync(user.UserName, model.Password, false, false);
            if (!result.Succeeded)
            {
                return this.View(model);
            }
            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();

            return this.RedirectToAction("Index", "Home");
        }
    }
}
