namespace SchoolEats.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using static Common.GeneralApplicationConstants;

    [Authorize(Roles = SuperUserRoleName)]
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
