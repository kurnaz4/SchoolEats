using Microsoft.AspNetCore.Mvc;

namespace SchoolEats.Controllers
{
	using Data.Models;
	using Microsoft.AspNetCore.Identity;
	using Services.Data.Interfaces;
	using Services.Messaging;
	using Web.ViewModels.User;
	using static Common.NotificationMessagesConstants;
	using static Common.ErrorMessages;
	using static Common.GeneralApplicationConstants;
	using static System.Net.WebRequestMethods;

	public class UserController : Controller
	{
		private readonly IUserService userService;
		private readonly IEmailSender emailSender;
		private readonly UserManager<SchoolEatsUser> userManager;

		public UserController(IUserService userService, IEmailSender emailSender,
			UserManager<SchoolEatsUser> userManager)
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
		public async Task<IActionResult> RemoveUser(Guid userId)
		{
			try
			{
				var user = await this.userService.GetUserAsync(userId);

				await this.userService.RemoveUserAsync(userId);
				await this.emailSender.SendEmailAsync(user.Email, "Отхвърлен акаунт от School Eats",
					"Вашият акаунт е отхвърлен поради опит за повторно влизане на един и същ потребител в системата или поради някаква съмнителна грешка!");

				TempData[SuccessMessage] = "Успешно премахнахте този потребител!";
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

		[HttpPost]
		public async Task<IActionResult> RemoveUserFromSuperUserRole(Guid userId)
		{
			try
			{
				var user = await this.userService.RemoveSuperUserRoleFromUserAsync(userId);

				await emailSender.SendEmailAsync(user.Email, "Екип готвачи в SchoolEats",
					"Акаунтът ви е успешно добавен към готвачите и вече имате всички права на един готвач!");

				TempData[SuccessMessage] = "Вие успешно премахнахте този потребител от готвачите!";
				TempData[InformationMessage] = "Потребителят вече е без права на готвач!";
			}
			catch (Exception e)
			{
				TempData[ErrorMessage] = CommonErrorMessage;
			}

			return RedirectToAction("AllUsers", "Admin");
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			ForgotPasswordViewModel model = new ForgotPasswordViewModel();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			//TODO
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					ModelState.AddModelError("Email", "Такъв имейл не съществува в системата");
					return View(model);
				}

				var token = await userManager.GeneratePasswordResetTokenAsync(user);
				var link = "<a href='" +
				           Url.Action("ResetPassword", "User", new { email = model.Email, code = token }, "https") +
				           "' target='_blank'>Reset Password</a>";

				await this.emailSender.SendEmailAsync(model.Email, "Промени парола в School Eats",
					$"Натиснете {link} за да смените вашата парола");
				TempData[SuccessMessage] = "Успешно заявихте смяна на паролата Ви!";
				TempData[InformationMessage] = "Проверете имейла си!";
			}
			catch (Exception e)
			{
				TempData[ErrorMessage] = CommonErrorMessage;
			}

			return LocalRedirect("/Identity/Account/Login");
		}

		[HttpGet]
		public IActionResult ResetPassword(string email, string code)
		{
			ResetPasswordViewModel model = new ResetPasswordViewModel();
			model.Token = code;
			model.Email = email;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("Password", "Невалидна парола!");
				ModelState.AddModelError("ConfirmPassword", "Невалидна парола!");
				return View(model);
			}
			
			try
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user == null)
					return View(model);
				var resetPassResult = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
				if (!resetPassResult.Succeeded)
				{
					foreach (var error in resetPassResult.Errors)
					{
						ModelState.TryAddModelError(error.Code, error.Description);
					}

					return View(model);
				}

				await this.emailSender.SendEmailAsync(user.Email, "Променена парола в School Eats",
					"Успешно променихте паролата си в нашата система!");
				TempData[SuccessMessage] = "Успешно променихте паролата си!";
			}
			catch (Exception e)
			{
				TempData[ErrorMessage] = CommonErrorMessage;
			}
			
			return LocalRedirect("/Identity/Account/Login");
		}
	}
}