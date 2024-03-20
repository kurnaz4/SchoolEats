using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
