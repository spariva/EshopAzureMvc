using Microsoft.CodeAnalysis.CSharp.Syntax;
using Eshop.Models;
using Eshop.Models.DTOs;
//using EshopMakiNuget.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Eshop.Services
{
    public class ServiceEshop
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;
        private IHttpContextAccessor contextAccessor;

        public ServiceEshop(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
            this.UrlApi = configuration.GetValue<string>("ApiUrls:Eshop");
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync
            (string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/auth/login";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginDto model = new LoginDto
                {
                    Email = email,
                    Password = password
                };
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content
                        .ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                HttpResponseMessage response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        ////VAMOS A REALIZAR UNA SOBRECARGA DEL METODO
        ////RECIBIENDO EL TOKEN
        private async Task<T> CallApiAsync<T>
            (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> PostApiAsync<T>(string request, object data, string token = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UrlApi);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    }

                    string json = JsonConvert.SerializeObject(data);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(request, content);

                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"Error in API request: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                // Log deserialization error
                Console.WriteLine($"Error deserializing response: {ex.Message}");
                throw;
            }
        }

        private async Task<T> PutApiAsync<T>(string request, object data, string token = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UrlApi);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    }
                    string json = JsonConvert.SerializeObject(data);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(request, content);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"Error in API request: {ex.Message}");
                throw;
            }
        }

        private async Task<bool> DeleteApiAsync(string request, string token = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UrlApi);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    }
                    HttpResponseMessage response = await client.DeleteAsync(request);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"Error in API request: {ex.Message}");
                throw;
            }
        }

        #region Stores

        public async Task<List<StoreDto>> GetStoresAsync()
        {
            string request = "api/stores";
            List<StoreDto> stores = await CallApiAsync<List<StoreDto>>(request);
            return stores;
        }

        public async Task<StoreDto> FindSimpleStoreAsync(int idStore)
        {
            string request = "api/stores/simplestore" + idStore;
            StoreDto store = await CallApiAsync<StoreDto>(request);

            return store;
        }

        public async Task<StoreViewDto> FindStoreAsync(int idStore)
        {
            string request = "api/stores/" + idStore;
            StoreViewDto storeView = await CallApiAsync<StoreViewDto>(request);

            return storeView;
        }

        //public async Task<Store> UpdateStoreAsync(int id, string name, string email, string image, string category)
        //{
        //    Store s = await this.FindSimpleStoreAsync(id);

        //    s.Name = name;
        //    s.Email = email;
        //    s.Image = image;
        //    s.Category = category;
        //    await this.context.SaveChangesAsync();
        //    return s;
        //}

        //public async Task DeleteStoreAsync(int id)
        //{
        //    Store s = await this.FindSimpleStoreAsync(id);
        //    this.context.Stores.Remove(s);
        //    await this.context.SaveChangesAsync();
        //}

        #endregion

        // Store Services
        public async Task<List<Store>> GetAllStoresAsync()
        {
            string request = "api/Stores";
            return await CallApiAsync<List<Store>>(request);
        }

        public async Task<StoreView> GetStoreByIdAsync(int id)
        {
            string request = $"api/Stores/{id}";
            return await CallApiAsync<StoreView>(request);
        }

        public async Task<Store> GetSimpleStoreAsync(int id)
        {
            string request = $"api/Stores/SimpleStore/{id}";
            return await CallApiAsync<Store>(request);
        }

        public async Task<Store> CreateStoreAsync(Store store)
        {
            string request = "api/Stores/Create";
            string token = contextAccessor.HttpContext.User.FindFirst(x => x.Type == "Token").Value;
            return await PostApiAsync<Store>(request, store, token);
        }

        public async Task<bool> OnboardingCompleteAsync(int id)
        {
            string request = $"api/Stores/OnboardingComplete/{id}";
            return await CallApiAsync<bool>(request);
        }

        public async Task<bool> RefreshOnboardingAsync(int id)
        {
            string request = $"api/Stores/Stores/RefreshOnboarding/{id}";
            return await CallApiAsync<bool>(request);
        }

        public async Task<Store> UpdateStoreAsync(int id, Store store)
        {
            string request = $"api/Stores/Update/{id}";
            string token = contextAccessor.HttpContext.User.FindFirst(x => x.Type == "Token").Value;
            return await PutApiAsync<Store>(request, store, token);
        }

        public async Task<bool> DeleteStoreAsync(int id)
        {
            string request = $"api/Stores/delete/{id}";
            return await DeleteApiAsync(request);
        }

        // User Services
        public async Task<ProfileDto> GetUserProfileAsync(int id)
        {
            string request = $"api/Users/Profile/{id}";
            string token = contextAccessor.HttpContext.User.FindFirst(x => x.Type == "Token").Value;
            return await CallApiAsync<ProfileDto>(request, token);
        }

        public async Task<Store> FindStoreByUserAsync(int userId)
        {
            string request = $"api/Users/FindStoreByUser/{userId}";
            return await CallApiAsync<Store>(request);
        }

        public async Task<PurchaseDto> GetPurchaseDetailsAsync(int id)
        {
            string request = $"api/Users/PurchaseDetails/{id}";
            string token = contextAccessor.HttpContext.User.FindFirst(x => x.Type == "Token").Value;
            return await CallApiAsync<PurchaseDto>(request, token);
        }

        #region products 
        // Auth Services
        public async Task<string> LoginAsync(LoginDto loginModel)
        {
            string request = "api/Auth/Login";
            return await PostApiAsync<string>(request, loginModel);
        }

        public async Task<User> RegisterAsync(RegisterDto registerModel)
        {
            string request = "api/Auth/Register";
            return await PostApiAsync<User>(request, registerModel);
        }

        // Product Services
        public async Task<List<Category>> GetCategoriesAsync()
        {
            string request = "api/Products/Categories";
            return await CallApiAsync<List<Category>>(request);
        }

        public async Task<Category> FindOrCreateCategoryAsync(string name)
        {
            string request = $"api/Products/FindorCreateCategory/{name}";
            return await PostApiAsync<Category>(request, null);  // Passing null as no body required
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            string request = "api/Products";
            return await CallApiAsync<List<Product>>(request);
        }

        public async Task<ProductDto> GetProductDetailsAsync(int id)
        {
            string request = $"api/Products/Details/{id}";
            return await CallApiAsync<ProductDto>(request);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            string request = $"api/Products/Create";
            string token = contextAccessor.HttpContext.User.FindFirst(x => x.Type == "Token").Value;
            return await PostApiAsync<Product>(request, product, token);
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            string request = $"api/Products/Update/{id}";
            string token = contextAccessor.HttpContext.User.FindFirst(x => x.Type == "Token").Value;
            return await PutApiAsync<Product>(request, product, token);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            string request = $"api/Products/Delete/{id}";
            string token = contextAccessor.HttpContext.User.FindFirst(x => x.Type == "Token").Value;
            return await DeleteApiAsync(request, token);
        }
        #endregion
    }
}
