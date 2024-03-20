using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
    public class SuperUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Products()
		{
			return View();
		}
		public IActionResult Settings()
		{
			return View();
		}
	}
}
