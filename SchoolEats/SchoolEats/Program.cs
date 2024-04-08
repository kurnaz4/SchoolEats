using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SchoolEats.Common;
using SchoolEats.Data;
using SchoolEats.Data.Models;
using SchoolEats.Services.Data;
using SchoolEats.Services.Data.Interfaces;
using SchoolEats.Services.Messaging;
using SchoolEats.Web.Infrastructure.Extensions;
using SchoolEats.Web.Infrastructure.ModelBinders;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
//key za moqta karta 
StripeConfiguration.ApiKey = "sk_test_51OwLybRo9wWMfFI989Wl9co0guP0vMKV6Cdt6TZ3H0kgFNehUQfubbclBYGx2G8Irk3v14mcRm4MtEihQSmWVvGC00Nj5opGUw";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SchoolEatsDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<SchoolEatsUser>(options =>
	{
		options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;
        options.SignIn.RequireConfirmedAccount = false;
        options.User.AllowedUserNameCharacters = "абвгдежзийклмнопрстуфхцчшщъьюяАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЬЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<SchoolEatsDbContext>();

builder.Services.AddSession();

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddTransient<SchoolEatsDbContext>();

builder.Services.AddControllersWithViews(options =>
{
	options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
});
	

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.SeedAdministrator(GeneralApplicationConstants.DevelopmentAdminEmail);
    app.SeedUser();
    app.SeedSuperUser();    
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
