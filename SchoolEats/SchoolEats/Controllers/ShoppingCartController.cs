namespace SchoolEats.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCart : Controller
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