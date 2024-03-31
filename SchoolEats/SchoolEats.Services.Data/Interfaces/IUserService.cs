namespace SchoolEats.Services.Data.Interfaces
{
    using Web.ViewModels.Admin;

    public interface IUserService
    {
        Task<List<AllPendingUsersViewModel>> GetAllPendingUsersAsync();

        Task ApproveUserAsync(Guid userId);

        Task<bool> IsUserApproved(Guid id);
    }
}
