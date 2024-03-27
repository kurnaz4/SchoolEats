using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
