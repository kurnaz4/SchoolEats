// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SchoolEats.Data.Models;
using static SchoolEats.Common.ValidationConstants.User;
using static SchoolEats.Common.GeneralApplicationConstants;
namespace SchoolEats.Areas.Identity.Pages.Account
{
    using System.ComponentModel;
    using Services.Data.Interfaces;
    using Web.Infrastructure.Extensions;

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<SchoolEatsUser> _signInManager;
        private readonly UserManager<SchoolEatsUser> _userManager;
        private readonly IUserStore<SchoolEatsUser> _userStore;
        private readonly IUserEmailStore<SchoolEatsUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly Services.Messaging.IEmailSender _emailSender;
        private readonly IDishService _dishService;

        public RegisterModel(
            UserManager<SchoolEatsUser> userManager,
            IUserStore<SchoolEatsUser> userStore,
            SignInManager<SchoolEatsUser> signInManager,
            ILogger<RegisterModel> logger,
            Services.Messaging.IEmailSender emailSender,
            IDishService dishService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _dishService = dishService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Display(Name = "Име")]
            [Required(ErrorMessage = "Полето име е задължително!")]
            [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Полето {0} трябва да бъде от {2} до {1} символа")]
            public string Name { get; set; }
            [Display(Name= "Фамилия")]
            [Required(ErrorMessage = "Полето фамилия е задължително!")]
            [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Полето {0} трябва да бъде от {2} до {1} символа")]
            public string Surname { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Полето имейл е задължително!")]
            [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Въведете валиден имейл адрес!")]
            [EmailAddress(ErrorMessage = "Въведете валиден имейл адрес!")]
            [Display(Name = "Имейл")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Паролата е задължителна!")]
            [StringLength(30, ErrorMessage = "{0} трябва да бъде поне {2} и максимум {1} символа дълга.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Повтори парола")]
            [Compare("Password", ErrorMessage = "Паролата не съвпада с повторената парола!")]
            public string ConfirmPassword { get; set; }
        }


        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
	        if (User.Identity.IsAuthenticated)
	        {
		        return RedirectToAction("Index", "Home");
	        }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                SchoolEatsUser userEmailCheck = _userManager.FindByEmailAsync(Input.Email).Result;
                if (userEmailCheck != null)
                {
                    ModelState.AddModelError(String.Empty, "Потребител с такъв имейл вече съществува!");
                }
                string fullName = Input.Name + Input.Surname;
                SchoolEatsUser userUsernameCheck = _userManager.FindByNameAsync(fullName).Result;
                if (userUsernameCheck != null)
                {
                    ModelState.AddModelError(String.Empty, "Потребител с такова име и фамилия вече съществува!");
                }

                bool isNameCorrect = this.ValidateUserUsername(Input.Name);
                bool isSurnameCorrect = this.ValidateUserUsername(Input.Surname);

                if (!isNameCorrect)
                {
                    ModelState.AddModelError("Input.Name", "Името трябва да е по-голямо от 2 символа, първата буква да бъде главна и всички други малки, трябва да бъде само на кирилица!");
                }

                if (!isSurnameCorrect)
                {
                    ModelState.AddModelError("Input.Surname", "Фамилията трябва да е по-голяма от 2 символа, първата буква да бъде главна и всички други малки, трябва да бъде само на кирилица!");
                }

                if (userEmailCheck != null || userUsernameCheck != null || !isNameCorrect || !isSurnameCorrect)
                {
                    return Page();
                }

                var user = CreateUser();
                await _userStore.SetUserNameAsync(user, fullName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                user.IsApproved = false;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
	                    await _emailSender.SendEmailAsync(user.Email, "Регистрация в School Eats",
		                    "Благодаря за твоята регистрация в SchoolEats. Твоите данни ще бъдат разгледани. При упешно регистриране ще ви пратим имейл.");

						await _userManager.AddToRoleAsync(user, UserRoleName);
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("RegisterConfirmation", "User",
                            new {returnUrl = ""});
                    } 
                }
				else
				{
					ModelState.AddModelError("Input.Password", "Неправилнa парола");
					ModelState.AddModelError("Input.ConfirmPassword", "Неправилнa парола");
					return Page();
				}


			}

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private SchoolEatsUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<SchoolEatsUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(SchoolEatsUser)}'. " +
                    $"Ensure that '{nameof(SchoolEatsUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<SchoolEatsUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<SchoolEatsUser>)_userStore;
        }

        private bool ValidateUserUsername(string username)
        {
            if (username == null)
            {
                return false;
            }

            if (username.Length == 0)
            {
                return false;
            }

            if (username.Length < 2)
            {
                return false;
            }

            string firstLetter = username[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                return false;
            }

            var name = username.Skip(1);
            var fullname = "";
            foreach (var c in name)
            {
                fullname += c;
            }
            if (fullname != fullname.ToLower())
            {
                return false;
            }
            if (Regex.IsMatch(username, @"\P{IsCyrillic}"))
            {
                return false;
            }
            return true;
        }
    }
}
