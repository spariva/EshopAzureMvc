﻿using Microsoft.AspNetCore.Mvc;
using Eshop.Models;
using Eshop.Repositories;
using Eshop.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Eshop.Filters;
using Eshop.Services;
using Eshop.Models.DTOs;

namespace Eshop.Controllers
{
    public class UsersController: Controller
    {
        private RepositoryStores repoStores;
        private RepositoryUsers repoUsers;
        private RepositoryPayment repoPay;
        private ServiceEshop service;
        private const string UserKey = "UserId";

        public UsersController(RepositoryStores repoStores, RepositoryUsers repoUsers, RepositoryPayment repoPay, ServiceEshop service)
        {
            this.repoStores = repoStores;
            this.repoUsers = repoUsers;
            this.repoPay = repoPay;
            this.service = service;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            User user = await this.repoUsers.LoginAsync(email, password);
            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }
            string token = await this.service.GetTokenAsync(email, password);

            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            Claim claimID = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            Claim claimEmail = new Claim(ClaimTypes.Name, user.Email);
            Claim claimNombre = new Claim("Nombre", user.Name);
            Claim claimToken = new Claim("Token", token);

            identity.AddClaim(claimID);
            identity.AddClaim(claimEmail);
            identity.AddClaim(claimNombre);
            identity.AddClaim(claimToken);

            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

            string controller = TempData["controller"]?.ToString() ?? "Users";
            string action = TempData["action"]?.ToString() ?? "Profile";

            return RedirectToAction(action, controller);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string name, string email, string password, string confirmpassword, string telephone, string address)
        {
            RegisterDto r = new RegisterDto()
            {
                Name = name,
                Email = email,
                Password = password,
                Telephone = telephone,
                Address = address
            };

            User user = await this.service.RegisterAsync(r);
            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            HttpContext.Session.SetObject(UserKey, user.Id);
            return RedirectToAction("Profile", "Users");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Home", "Home");
        }


        [AuthorizeUser]
        public async Task<IActionResult> Profile()
        {
            int userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userId == 0)
            {
                return RedirectToAction("Home", "Home");
            }

            User user = await this.repoUsers.FindUserAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Home", "Home");
            }


            Store store = await this.repoUsers.FindStoreByUserIdAsync(userId);

            ViewBag.Store = store;

            List<Purchase> purchases = await this.repoPay.GetPurchasesByUserIdAsync(userId);

            if (purchases.Count != 0)
            {
                ViewBag.Purchases = purchases;
            }

            return View(user);
        }

        public async Task<IActionResult> PurchaseDetails(int id)
        {
            Purchase purchase = await this.repoPay.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                TempData["Error"] = "Purchase not found";
                return RedirectToAction("Profile", "Users");
            }

            foreach (PurchaseItem item in purchase.PurchaseItems)
            {
                item.Product = await this.repoStores.FindProductAsync(item.ProductId);
            }

            return View(purchase);
        }

    }
}
