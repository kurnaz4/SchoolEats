﻿namespace SchoolEats.Services.Data.Interfaces
{
	using SchoolEats.Data.Models;
	using Web.ViewModels.Admin;

    public interface IUserService
    {
        Task<List<AllPendingUsersViewModel>> GetAllPendingUsersAsync();

        Task ApproveUserAsync(Guid userId);

        Task<bool> IsUserApproved(Guid id);

        Task<SchoolEatsUser> GetUserAsync(Guid userId);
    }
}
