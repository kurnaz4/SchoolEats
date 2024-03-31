using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
	using Services.Data.Interfaces;
	using static Common.NotificationMessagesConstants;
	using static Common.ErrorMessages;
	public class UserController : Controller
	{
		private readonly IUserService userService;
	    public UserController(IUserService userService)
	    {
		    this.userService = userService;
	    }

        [HttpGet]
        public IActionResult RegisterConfirmation(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveUser(Guid userId)
        {
	        try
	        {
				await this.userService.ApproveUserAsync(userId);

				TempData[SuccessMessage] = "Вие успешно удобрихте този потребител!";
	        }
	        catch (Exception e)
	        {
		        TempData[ErrorMessage] = CommonErrorMessage;
	        }

	        return RedirectToAction("PendingUsers", "Admin");
        }
    }
}
