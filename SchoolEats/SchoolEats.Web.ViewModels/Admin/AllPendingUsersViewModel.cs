namespace SchoolEats.Web.ViewModels.Admin
{
    public class AllPendingUsersViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public bool IsApproved { get; set; }
    }
}
