namespace SchoolEats.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Services.Data.Interfaces;
    using Services.Messaging;
    using static Common.GeneralApplicationConstants;
    using static Common.ErrorMessages;
    using static Common.NotificationMessagesConstants;
    [Authorize(Roles = SuperUserRoleName)]
    public class SuperUserController : Controller
    {
	    private readonly IPurchaseService purchaseService;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;
	    public SuperUserController(IPurchaseService purchaseService, IUserService userService, IEmailSender emailSender)
	    {
		    this.purchaseService = purchaseService;
            this.userService = userService;
            this.emailSender = emailSender;
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

        [HttpGet]
        public async Task<IActionResult> DailyReport()
        {
	        var report = await this.purchaseService.GetDailyReportAsync();
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> SendReport()
        {
	        try
	        {
		        var report = await this.purchaseService.GetDailyReportAsync();
		        await this.purchaseService.SendDailyReportAsync(report);
		        await this.purchaseService.DeleteAllPurchasesByDateTimeAsync(DateTime.Now);
		        await this.emailSender.SendEmailAsync(DevelopmentAdminEmail, $"{DateTime.Now.Date} report",
			        $"На днешната дата {DateTime.Now.Date} продаденото количество храна е {report.TotalQuantity} .Изкараната сума пари за деня е: {report.TotalPrice} . ");

		        TempData[SuccessMessage] = "Успешно запаметихте отчета!";
	        }
	        catch (Exception e)
	        {
		        TempData[ErrorMessage] = CommonErrorMessage;
	        }
	        
	        return RedirectToAction("DailyReport", "SuperUser");
        }
    }
}
