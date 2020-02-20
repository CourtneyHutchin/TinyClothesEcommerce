using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes.Controllers
{
    public class AccountController : Controller
    {

        private readonly StoreContext _context; // Create the field

        private readonly IHttpContextAccessor _http;

        public AccountController(StoreContext context, IHttpContextAccessor http)
        {
            _context = context; // Set the field
            _http = http;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Check if username is not taken
                if(!await AccountDb.IsUsernameAvailable(reg.Username, _context))
                {
                    Account acc = new Account()
                    {
                        Email = reg.Email,
                        FullName = reg.FullName,
                        Password = reg.Password,
                        Username = reg.Username
                    };

                    // Add Account to DB
                    await AccountDb.Register(_context, acc);

                    SessionHelper.CreateUserSession(acc.AccountId, acc.Username, _http);

                    return RedirectToAction("Index", "Home");
                }
                else // If username is taken, add error
                {
                    // Display error with other username error messages
                    ModelState.AddModelError(nameof(Account.Username), "Username is already taken");
                }

            }
            return View(reg);

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                Account acc = await AccountDb.DoesUserMatch(login, _context);

                if (acc != null)
                {
                    SessionHelper.CreateUserSession(acc.AccountId, acc.Username, _http);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid credentials");
                }
            }
            return View(login);
        }

        public IActionResult Logout()
        {
            SessionHelper.DestoryUserSession(_http);
            return RedirectToAction("Index", "Home");
        }
    }
}