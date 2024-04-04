namespace SchoolEats.Web.ViewModels.User
{
	using System.ComponentModel.DataAnnotations;

	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "Полето имейл е задължително!")]
		[RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Въведете валиден имейл адрес!")]
		[EmailAddress(ErrorMessage = "Въведете валиден имейл адрес!")]
		[Display(Name = "Имейл")]
		public string Email { get; set; } = null!;
	}
}
