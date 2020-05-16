using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuoteQuiz.Models;
using FamousQuoteQuiz.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "main");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email, Name = model.Name, JoinedDate = DateTime.Now };
                var result = await userManager.CreateAsync(user, model.Password);

                if(result.Succeeded)
                {
                   await signInManager.SignInAsync(user, false);
                    return RedirectToAction("index", "main");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string id)
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "main");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "main");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
           

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                var isLockedOut = await userManager.IsLockedOutAsync(user);

                if (isLockedOut)
                {
                    ModelState.AddModelError("", "Account Disabled");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "main");
                }

                ModelState.AddModelError("", "Wrong username or password");
            }

          

            return View(model);
        }
    }
}