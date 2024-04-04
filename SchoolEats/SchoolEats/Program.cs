using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SchoolEats.Common;
using SchoolEats.Data;
using SchoolEats.Data.Models;
using SchoolEats.Services.Data;
using SchoolEats.Services.Data.Interfaces;
using SchoolEats.Services.Messaging;
using SchoolEats.Web.Infrastructure.Extensions;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = "sk_test_51Of7n6Grqe6vOSV1i1suN4aL7YXnvppU0ZgpSifrMGLRvi5Wri21T69V9vovbcwkKc87Hv2436ZP7Ml0APnW3XMD00ggRke19q";

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

builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddTransient<SchoolEatsDbContext>();
builder.Services.AddControllersWithViews();

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
