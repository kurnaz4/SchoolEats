namespace SchoolEats.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Services.Data.Interfaces;
    using static Common.GeneralApplicationConstants;

    [Authorize(Roles = AdminRoleName)]
    public class AdminController : Controller
    {
	    private readonly IUserService userService;
        private readonly IReportService reportService;
	    public AdminController(IUserService userService, IReportService reportService)
	    {
		    this.userService = userService;
            this.reportService = reportService;
	    }
		public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PendingUsers()
        {
	        var all = await this.userService.GetAllPendingUsersAsync();
	        return View(all);
        }

        public async Task<IActionResult> AllUsers()
        {
            var all = await this.userService.GetAllUsersAsync();
            return View(all);
        }

        [HttpGet]
        public async Task<IActionResult> AllReports()
        {
            var all = await this.reportService.GetAllReportsAsync();
            return View(all);
        }
    }
}
