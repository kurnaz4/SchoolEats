using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
	using Services.Data.Interfaces;
	using Services.Messaging;
	using static Common.NotificationMessagesConstants;
	using static Common.ErrorMessages;
	public class UserController : Controller
	{
		private readonly IUserService userService;
		private readonly IEmailSender emailSender;
	    public UserController(IUserService userService, IEmailSender emailSender)
	    {
		    this.userService = userService;
			this.emailSender = emailSender;
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

				var user = await this.userService.GetUserAsync(userId);

				await emailSender.SendEmailAsync(user.Email, "Удобрена регистрация в School Eats",
					"Благодарим ви за вашата регистрация в SchoolEats. Акаунтът ви е успешно удобрен!");

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
