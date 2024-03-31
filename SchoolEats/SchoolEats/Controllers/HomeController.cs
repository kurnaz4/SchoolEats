using Microsoft.AspNetCore.Mvc;
using SchoolEats.Areas.Identity.Pages.Account;
using SchoolEats.Models;
using System.Diagnostics;

namespace SchoolEats.Controllers
{
	using Services.Data.Interfaces;
	using Web.Infrastructure.Extensions;
    using static Common.GeneralApplicationConstants;
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;
        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
	        if (User.Identity.IsAuthenticated)
			{
				bool isUserApproved = await this.userService.IsUserApproved(this.User.GetId());
				if (!isUserApproved && User.IsInRole(UserRoleName))
				{
					return RedirectToAction("RegisterConfirmation", "User");
				}
				
			}

            if (!this.User.Identity.IsAuthenticated)
	        {
				return LocalRedirect("/Identity/Account/Login");
	        }

            return View();
        }
      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
