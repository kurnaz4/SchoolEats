using Microsoft.AspNetCore.Mvc;
using SchoolEats.Areas.Identity.Pages.Account;
using SchoolEats.Models;
using System.Diagnostics;

namespace SchoolEats.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
