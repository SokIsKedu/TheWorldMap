using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Contollers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private SignInManager<WorldUser> _signInManager;
        private UserManager<WorldUser> _userManager;

        public AuthController(SignInManager<WorldUser> signInManager, UserManager<WorldUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            
        }



        public IActionResult Login()
        {
             if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.UserMessage = "Unable to login";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.UserMessage = "";
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Trips", "App");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect");
                    ViewBag.UserMessage = "Username or Password is incorrect";
                }

            }
            return View();
        }

        
        public async Task<IActionResult> Logout(LoginViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
          
            return RedirectToAction("Index","App");
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel rvm)
        {
            try
            {
                //if (await _roleManager.RoleExistsAsync("Administrator")) {
                //    IdentityRole role = new IdentityRole("Administrator");
                //}

                var user = new WorldUser()
                {
                    UserName = rvm.Username,
                    Email = rvm.Email
                };
                user.DateJoined = DateTime.Now;
                var result = await _userManager.CreateAsync(user, rvm.Password);
                Debug.WriteLine(result);
                

            }
            catch (Exception ex)
            {

                ViewBag.UserMessage = "Unable to register user. Please try again.  '\n'" + ex.Message;
            }
            return Ok();
        }

    }
}
