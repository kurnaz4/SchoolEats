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
	    public AdminController(IUserService userService)
	    {
		    this.userService = userService;
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
    }
}
