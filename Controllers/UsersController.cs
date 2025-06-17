using Microsoft.AspNetCore.Mvc;
using Eshop.Models;
using Eshop.Repositories;
using Eshop.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Eshop.Filters;
using Eshop.Services;
using Eshop.Models.DTOs;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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
            //User user = await this.repoUsers.LoginAsync(email, password);

            string token = await this.service.GetTokenAsync(email, password);

            if (!string.IsNullOrEmpty(token))
            {
                // Create a handler to read the JWT token
                var handler = new JwtSecurityTokenHandler();

                // Read the JWT token
                var jwtToken = handler.ReadJwtToken(token);

                // Extract the UserData claim from the JWT token
                var userDataClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserData")?.Value;

                if (!string.IsNullOrEmpty(userDataClaim))
                {
                    // Deserialize the JSON string back to a User object
                    User user = JsonConvert.DeserializeObject<User>(userDataClaim);

                    // Now you can use the user object
                    // For example: var userId = user.Id;
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
                else
                {
                    ViewBag.Error = "Invalid credentials";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }
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

            string token = HttpContext.User.FindFirst(x => x.Type == "Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Users");
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userDataClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserData")?.Value;

            if (string.IsNullOrEmpty(userDataClaim))
            {
                return RedirectToAction("Login", "Users");
            }

            User user = JsonConvert.DeserializeObject<User>(userDataClaim);

            StoreDto store = await this.service.FindStoreByUserAsync(user.Id);

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
            //Purchase purchase = await this.repoPay.GetPurchaseByIdAsync(id);
            //if (purchase == null)
            //{
            //    TempData["Error"] = "Purchase not found";
            //    return RedirectToAction("Profile", "Users");
            //}

            //foreach (PurchaseItem item in purchase.PurchaseItems)
            //{
            //    item.Product = await this.repoStores.FindProductAsync(item.ProductId);
            //}
            PurchaseDto purchase = await this.service.GetPurchaseDetailsAsync(id);

            return View(purchase);
        }

    }
}
