namespace SchoolEats.Web.ViewModels.Admin
{
	public class AllUsersViewModel
	{
		public Guid Id { get; set; }
		public string Username { get; set; } = null;

		public bool IsApproved { get; set; }

		public string Email { get; set; } = null!;

	}
}