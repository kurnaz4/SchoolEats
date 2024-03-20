using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
    public class DishController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
