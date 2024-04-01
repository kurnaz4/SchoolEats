using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
	using Services.Data.Interfaces;
	using Web.Infrastructure.Extensions;

	public class PurchaseController : Controller
	{
		private readonly IPurchaseService purchaseService;

		public PurchaseController(IPurchaseService purchaseService)
		{
			this.purchaseService = purchaseService;
		}
        public async Task<IActionResult> All()
        {
	       var all = await this.purchaseService.GetAllPurchasesByUserIdAsync(this.User.GetId());
            return View(all);
        }

        [HttpPost]
        public IActionResult Purchase()
        {
	        return RedirectToAction("All", "Dish");
        }
    }
}
