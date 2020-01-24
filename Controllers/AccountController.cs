using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CmsShoppingCart.Controllers
{   
    [Authorize]
    public class AccountController : Controller
    {
        //GET/account/register
        [AllowAnonymous]
        public IActionResult Register() => View();

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> passwordHasher)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        //POST /account/register
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if(ModelState.IsValid)
            {
                AppUser appsUser = new AppUser() {UserName = user.UserName, Email = user.Email};

                IdentityResult result = await _userManager.CreateAsync(appsUser, user.Password);
                if(result.Succeeded)
                {
                    return RedirectToAction("Login");

                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(user);


        }

        // GET /account/login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login() {ReturnUrl = returnUrl};

            return View(login);
        }

        //POST /account/login
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser appuser = await _userManager.FindByEmailAsync(login.Email);

                if(appuser!=null)
                {
                    SignInResult result = await _signInManager.PasswordSignInAsync(appuser, login.Password, false, false);

                    if(result.Succeeded)
                    {
                        return Redirect(login.ReturnUrl ?? "/");

                    }
                    else
                    {
                        ModelState.AddModelError("","Login failed, wrong credentials.");
                    }
                }
            }

            return View(login);


        }

        //GET /account/logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        //GET /account/edit
        public async Task<IActionResult> Edit()
        {
            AppUser appuser = await _userManager.FindByNameAsync(User.Identity.Name);

            UserEdit user = new UserEdit(appuser);

            return View(user);
        }

        //POST /account/edit
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEdit user)
        {
            AppUser appuser = await _userManager.FindByNameAsync(User.Identity.Name);

            if(ModelState.IsValid)
            {
                appuser.Email = user.Email;
                if(user.Password!=null) { appuser.PasswordHash = _passwordHasher.HashPassword(appuser, user.Password); }

            }

            IdentityResult result = await _userManager.UpdateAsync(appuser);

            if(result.Succeeded) { TempData["Success"] = "Login data updated"; }
            return View();

        }



    }
}