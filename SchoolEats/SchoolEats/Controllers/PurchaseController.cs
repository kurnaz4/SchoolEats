using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
    public class PurchaseController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
