using Eshop.Data;
using Eshop.Helpers;
using Eshop.Models;
using Eshop.Repositories;
using Eshop.Extensions;
using Stripe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Product = Eshop.Models.Product;
using Eshop.Filters; //To avoid ambiguity with Eshop.Models.Product and Stripe.Product
using System.Security.Claims;
using Eshop.Services;
using Eshop.Models.DTOs;

namespace Eshop.Controllers
{
    public class StoresController : Controller
    {
        private RepositoryStores repoStores;
        private RepositoryUsers repoUsers;
        private HelperPathProvider helperPath;
        private const string UserKey = "UserId";
        private ServiceEshop service;

        public StoresController(RepositoryStores repoStores, HelperPathProvider helperPath, RepositoryUsers repoUsers, ServiceEshop service) {
            this.repoStores = repoStores;
            this.repoUsers = repoUsers;
            this.helperPath = helperPath;
            this.service = service;
        }

        #region Stores CRUD
        public async Task<IActionResult> Stores() {
            List<StoreDto> stores = await this.service.GetStoresAsync();
            ViewBag.Categories = stores.Select(x => x.Category).Distinct().ToList();
            return View(stores);
        }

        public async Task<IActionResult> StoreDetails(int id) {
            //Find store and add their products loading the ProdCats and Categories
            StoreViewDto storeView = await this.service.GetStoreByIdAsync(id);
            if (storeView == null) {
                return RedirectToAction("Stores");
            }

            return View(storeView);
        }

        [AuthorizeUser]
        public async Task<IActionResult> StoreCreate() {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            StoreDto storeSession = await this.service.FindStoreByUserAsync(userId);

            if (storeSession != null) {
                TempData["Message"] = "You already have a store";
                return RedirectToAction("Profile", "Users");
            }

            return View();
        }

        #region Stripe
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public async Task<IActionResult> StoreCreate(string name, string email, IFormFile image, string category) {
            //Create route and save image
            string fileName = image.FileName;

            string path = this.helperPath.MapPath(fileName, Folder.Stores);

            using (Stream stream = new FileStream(path, FileMode.Create)) {
                await image.CopyToAsync(stream);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Create Stripe connected account
            try {
                var service = new AccountService();

                var options = new AccountCreateOptions
                {
                    Controller = new AccountControllerOptions
                    {
                        StripeDashboard = new AccountControllerStripeDashboardOptions
                        {
                            Type = "express"
                        },
                        Fees = new AccountControllerFeesOptions
                        {
                            Payer = "application"
                        },
                        Losses = new AccountControllerLossesOptions
                        {
                            Payments = "application"
                        },
                    },
                };

                Account account = service.Create(options);

                //Insert store
                Store s = new Store
                {
                    Name = name,
                    Email = email,
                    Image = fileName,
                    Category = category,
                    UserId = userId,
                    StripeId = account.Id
                };

                Store store = await this.service.CreateStoreAsync(s);

                // Create directly account link for onboarding
                var accountLinkService = new AccountLinkService();
                var accountLink = accountLinkService.Create(new AccountLinkCreateOptions
                {
                    Account = account.Id,
                    ReturnUrl = Url.Action("OnboardingComplete", "Stores", new { id = store.Id }, Request.Scheme),
                    RefreshUrl = Url.Action("RefreshOnboarding", "Stores", new { id = store.Id }, Request.Scheme),
                    Type = "account_onboarding",
                });

                // Redirect to Stripe onboarding
                return Redirect(accountLink.Url);
            }
            catch (Exception ex) {
                Console.Write("An error occurred when calling the Stripe API to create an account:  " + ex.Message);
                Response.StatusCode = 500;
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet("Stores/OnboardingComplete/{id}")]
        public async Task<IActionResult> OnboardingComplete(int id) {
            var store = await this.service.FindSimpleStoreAsync(id);
            if (store == null) {
                return NotFound();
            }

            // Verify this user owns the store
            if (store.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Forbid();
            }

            // Show success page or redirect to store dashboard
            return RedirectToAction("StoreDetails", new { id = store.Id });
        }

        [HttpGet("Stores/RefreshOnboarding/{id}")]
        public async Task<IActionResult> RefreshOnboarding(int id) {
            Store store = await this.service.GetSimpleStoreAsync(id);
            if (store == null) {
                return NotFound();
            }

            // Verify this user owns the store
            if (store.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Forbid();
            }

            // Create a new account link
            var accountLinkService = new AccountLinkService();
            var accountLink = accountLinkService.Create(new AccountLinkCreateOptions
            {
                Account = store.StripeId,
                ReturnUrl = Url.Action("OnboardingComplete", "Stores", new { id = store.Id }, Request.Scheme),
                RefreshUrl = Url.Action("RefreshOnboarding", "Stores", new { id = store.Id }, Request.Scheme),
                Type = "account_onboarding",
            });

            return Redirect(accountLink.Url);
        }

        [AuthorizeUser]
        public async Task<IActionResult> StripeDashboard(int id) {

            Store store = await this.service.GetSimpleStoreAsync(id);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (userId != store.UserId) {
                return RedirectToAction("Home", "Home");
            }

            var service = new AccountLoginLinkService();
            var dashboardLink = service.Create(store.StripeId);
            return Redirect(dashboardLink.Url);
        }



        #endregion


        [AuthorizeUser]
        public async Task<IActionResult> StoreEdit(int id) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            StoreDto storeSession = await this.service.FindStoreByUserAsync(userId);

            if (storeSession == null || id != storeSession.Id) {
                TempData["Message"] = "That was not your store to Edit";
                return RedirectToAction("Profile", "Users");
            }

            return View(storeSession);
        }

        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreEdit(int id, string name, string email, IFormFile image, string oldimage, string category) {
            try {
                //Update image
                if (image != null) {
                    string fileName = image.FileName;
                    string path = this.helperPath.MapPath(fileName, Folder.Stores);
                    using (Stream stream = new FileStream(path, FileMode.Create)) {
                        await image.CopyToAsync(stream);
                    }

                    StoreDto storeDto = new StoreDto()
                    {
                        Id = id,
                        Name = name,
                        Email = email,
                        Image = fileName,
                        Category = category.ToUpper()
                    };
                    await this.service.UpdateStoreAsync(id, storeDto);

                }
                else {

                    StoreDto storeDto = new StoreDto()
                    {
                        Id = id,
                        Name = name,
                        Email = email,
                        Image = oldimage,
                        Category = category.ToUpper()
                    };
                    await this.service.UpdateStoreAsync(id, storeDto);

                }


                return RedirectToAction("StoreDetails", new { id = id });
            }
            catch (Exception ex) {
                // Log the exception (you can use any logging framework)
                Console.WriteLine($"Error updating store: {ex.Message}");
                // Optionally, add a user-friendly error message to the view
                ModelState.AddModelError(string.Empty, "An error occurred while updating the store. Please try again.");
                // Return the view with the current model to display the error
                Store store = await this.service.GetSimpleStoreAsync(id);
                return View(store);
            }
        }

        [AuthorizeUser]
        public async Task<IActionResult> StoreDelete(int id) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            StoreDto storeSession = await this.service.FindStoreByUserAsync(userId);

            if (storeSession == null || id != storeSession.Id) {
                TempData["Message"] = "That was not your store to Delete!";
                return RedirectToAction("Profile", "Users");
            }

            await this.service.DeleteStoreAsync(id);
            return RedirectToAction("Stores");
        }


        #endregion

        #region Products CRUD
        public async Task<IActionResult> ProductList() {
            List<ProductDto> products = await this.service.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> ProductDetails(int id) {
            ProductDto product = await this.service.GetProductDetailsAsync(id);
            Store store = await this.service.GetSimpleStoreAsync(product.StoreId);
            ViewBag.Store = store;

            return View(product);
        }

        [AuthorizeUser]
        public async Task<IActionResult> ProductCreate() {
            List<Category> categories = await this.service.GetCategoriesAsync();
            ViewBag.Productcategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View();
        }

        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(string name, string description, IFormFile image, decimal price, int stockQuantity, List<int> selectedCategories, string newCategories) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Store store = await this.service.GetSimpleStoreAsync(userId);

            if (store == null) {
                TempData["Message"] = "Create a store before!";
                return RedirectToAction("Profile", "Users");
            }

            int storeId = store.Id;

            if (ModelState.IsValid) {
                // Save the image 
                string fileName = image.FileName;
                string path = this.helperPath.MapPath(fileName, Folder.Products);
                using (Stream stream = new FileStream(path, FileMode.Create)) {
                    await image.CopyToAsync(stream);
                }

                // Handle new categories
                if (!string.IsNullOrEmpty(newCategories)) {
                    var newCategoryNames = newCategories.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToUpper()).ToList();
                    foreach (var categoryName in newCategoryNames) {
                        var category = await this.service.FindOrCreateCategoryAsync(categoryName);
                        selectedCategories.Add(category.Id);
                    }
                }

                // Insert the product
                Product p = new Product
                {
                    Name = name,
                    Description = description,
                    Image = fileName,
                    Price = price,
                    StockQuantity = stockQuantity,
                    StoreId = storeId
                };

                //var product = await this.service.CreateProductAsync(p);
                var product = await this.repoStores.CreateProductAsync(name, storeId, description, fileName, price, stockQuantity, selectedCategories);

                return RedirectToAction("ProductDetails", new { id = product.Id });
            }


            // If we got this far, something failed; re-populate the categories
            var categories = await this.service.GetCategoriesAsync();
            ViewBag.Productcategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList();
            ViewBag.Mensaje = "Error en el formulario model state is not valid";

            return View();
        }

        [AuthorizeUser]
        public async Task<IActionResult> ProductEdit(int id) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            StoreDto store = await this.service.FindStoreByUserAsync(userId);

            if (store == null) {
                TempData["Message"] = "Create a store before!";
                return RedirectToAction("Profile", "Users");
            }

            //ProductDto product = await this.service.GetProductDetailsAsync(id);
            Product product = await this.repoStores.FindProductAsync(id);

            if (product.StoreId != store.Id) {
                TempData["Message"] = "That was not your product to Edit";
                return RedirectToAction("Profile", "Users");
            }

            List<Category> categories = await this.service.GetCategoriesAsync();
            ViewBag.Productcategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View(product);
        }

        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(int id, string name, string description, IFormFile image, string oldimage, decimal price, int stockQuantity, List<int> selectedCategories, string newCategories) {
            if (!string.IsNullOrEmpty(newCategories)) {
                var newCategoryNames = newCategories.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToUpper()).ToList();
                foreach (var categoryName in newCategoryNames) {
                    var category = await this.service.FindOrCreateCategoryAsync(categoryName);
                    selectedCategories.Add(category.Id);
                }
            }

            if (image != null) {
                string fileName = image.FileName;
                string path = this.helperPath.MapPath(fileName, Folder.Products);
                using (Stream stream = new FileStream(path, FileMode.Create)) {
                    await image.CopyToAsync(stream);
                }

                Product p = new Product()
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Image = fileName,
                    Price = price,
                    StockQuantity = stockQuantity,
                    StoreId = id
                };
                //await this.service.UpdateProductAsync(id, p);
                var product = await this.repoStores.UpdateProductAsync(id, name, description, fileName, price, stockQuantity, selectedCategories);

            }
            else {
                Product p = new Product()
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Image = oldimage,
                    Price = price,
                    StockQuantity = stockQuantity,
                    StoreId = id
                };
                //await this.service.UpdateProductAsync(id, p);
                var product = await this.repoStores.UpdateProductAsync(id, name, description, oldimage, price, stockQuantity, selectedCategories);

            }
            return RedirectToAction("ProductDetails", new { id = id });


            // If we got this far, something failed; re-populate the categories? TODO
        }

        //First I find the product to get the id, so I pass the Product to not call twice the database
        [AuthorizeUser]
        public async Task<IActionResult> ProductDelete(int id) {
            ProductDto p = await this.service.GetProductDetailsAsync(id);
            int storeId = p.StoreId;

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            StoreDto store = await this.service.FindStoreByUserAsync(userId);

            if (store == null) {
                TempData["Message"] = "Create a store before!";
                return RedirectToAction("Profile", "User");
            }

            if (p.StoreId != store.Id) {
                TempData["Message"] = "That was not your product to Delete";
                return RedirectToAction("Profile", "Users");
            }

            await this.service.DeleteProductAsync(p.Id);
            return RedirectToAction("StoreDetails", storeId);
        }



        #endregion


    }
}
