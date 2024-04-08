namespace SchoolEats.Services.Data.Interfaces
{
	using SchoolEats.Data.Models;
	using Web.ViewModels.Admin;

    public interface IUserService
    {
        Task<List<AllPendingUsersViewModel>> GetAllPendingUsersAsync();

        Task ApproveUserAsync(Guid userId);
        
        Task RemoveUserAsync(Guid userId);

        Task<bool> IsUserApproved(Guid id);

        Task<SchoolEatsUser> GetUserAsync(Guid userId);

        Task<List<AllUsersViewModel>> GetAllUsersAsync();

        Task<SchoolEatsUser> AddUserToSuperUserRoleAsync(Guid userId);

        Task<SchoolEatsUser> RemoveSuperUserRoleFromUserAsync(Guid userId);

        Task DeleteUser(Guid userId);

    }
}
