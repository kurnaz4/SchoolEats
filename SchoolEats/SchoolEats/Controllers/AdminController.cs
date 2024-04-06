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
        private readonly IDishService dishService;
	    public AdminController(IUserService userService, IReportService reportService, IDishService dishService)
	    {
		    this.userService = userService;
            this.reportService = reportService;
            this.dishService = dishService;
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

        [HttpGet]
        public async Task<IActionResult> AllDishes()
        {
	        var all = await this.dishService.GetAllActiveAndNotActiveDishesAsync();
	        return View(all);
        }
	}
}
