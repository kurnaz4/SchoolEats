namespace SchoolEats.Web.ViewModels.User
{
	using System.ComponentModel.DataAnnotations;

	public class ResetPasswordViewModel
	{
		[Display(Name = "Парола")]
		public string Password { get; set; }

		[Display(Name = "Потвърди парола")]
		public string ConfirmPassword { get; set; }

		public string Token { get; set; }

		public string Email { get; set; }
	}
}
