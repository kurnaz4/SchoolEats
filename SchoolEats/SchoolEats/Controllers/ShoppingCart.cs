using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
    public class SchoppingCart : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
     
    }
}