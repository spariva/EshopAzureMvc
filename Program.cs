using Eshop.Data;
using Eshop.Helpers;
using Eshop.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Stripe;
using Microsoft.AspNetCore.Authentication.Cookies;
using Eshop.Services;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();

KeyVaultSecret stripeSecret = await secretClient.GetSecretAsync("StripeSecret");
KeyVaultSecret stripePublic = await secretClient.GetSecretAsync("StripePublic");

//StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
StripeConfiguration.ApiKey = stripeSecret.Value;


builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddSingleton<HelperToolkit>();
builder.Services.AddSingleton<HelperCriptography>();

//string connectionString =
//    builder.Configuration.GetConnectionString("SqlClase");
//string connectionString =
//    builder.Configuration.GetConnectionString("SqlCasa");


KeyVaultSecret secretConnectionString = await secretClient.GetSecretAsync("SqlAzure");
string connectionString = secretConnectionString.Value;

builder.Services.AddDbContext<EshopContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<RepositoryUsers>();
builder.Services.AddTransient<RepositoryStores>();
builder.Services.AddTransient<RepositoryPayment>();
builder.Services.AddTransient<ServiceEshop>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddMemoryCache();
//builder.Services.AddDistributedMemoryCache();
builder.Services.AddAntiforgery();


// Add services to the container.
builder.Services.AddControllersWithViews
    (options => options.EnableEndpointRouting = false)
    .AddSessionStateTempDataProvider();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseRouting();

app.UseRequestLocalization();
//app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseSession();


app.UseMvc(
    routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Home}/{id?}"
        );
    });


app.Run();
