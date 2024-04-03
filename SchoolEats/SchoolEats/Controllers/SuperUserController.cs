namespace SchoolEats.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Services.Data.Interfaces;
    using static Common.GeneralApplicationConstants;

    [Authorize(Roles = SuperUserRoleName)]
    public class SuperUserController : Controller
    {
	    private readonly IPurchaseService purchaseService;
        private readonly IUserService userService;
	    public SuperUserController(IPurchaseService purchaseService, IUserService userService)
	    {
		    this.purchaseService = purchaseService;
            this.userService = userService;
	    }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Orders()
        {
	        var all = await this.purchaseService.GetAllPurchases();

	        foreach (var purchase in all)
	        {
		        purchase.BuyerName = this.userService.GetUserAsync(purchase.BuyerId).Result.UserName;
		        purchase.BuyerEmail = this.userService.GetUserAsync(purchase.BuyerId).Result.Email;
	        }
	        return this.View(all);
        }
    }
}
