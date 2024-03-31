using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
	using Data.Models;
	using Microsoft.AspNetCore.Identity;
	using Services.Data.Interfaces;
	using Services.Messaging;
	using static Common.NotificationMessagesConstants;
	using static Common.ErrorMessages;
	using static Common.GeneralApplicationConstants;
	public class UserController : Controller
	{
		private readonly IUserService userService;
		private readonly IEmailSender emailSender;
		private readonly UserManager<SchoolEatsUser> userManager;
	    public UserController(IUserService userService, IEmailSender emailSender, UserManager<SchoolEatsUser> userManager)
	    {
		    this.userService = userService;
			this.emailSender = emailSender;
			this.userManager = userManager;
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

        [HttpPost]
        public async Task<IActionResult> AddUserToSuperUserRole(Guid userId)
        {
	        try
	        {

		        var user = await this.userService.AddUserToSuperUserRoleAsync(userId);
		        
		        await emailSender.SendEmailAsync(user.Email, "Екип готвачи в SchoolEats",
			        "Акаунтът ви е успешно добавен към готвачите и вече имате всички права на един готвач!");

		        TempData[SuccessMessage] = "Вие успешно добавихте този потребител към готвачите!";
			}
	        catch (Exception e)
	        {
				TempData[ErrorMessage] = CommonErrorMessage;
			}

	        return RedirectToAction("AllUsers", "Admin");
		}
    }
}
