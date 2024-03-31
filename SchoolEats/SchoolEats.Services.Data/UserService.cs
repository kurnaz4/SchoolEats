namespace SchoolEats.Services.Data
{
	using System.Diagnostics;
	using Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SchoolEats.Data;
    using SchoolEats.Data.Models;
    using Web.ViewModels.Admin;

    public class UserService :IUserService
    {
        private readonly SchoolEatsDbContext dbContext;
        private readonly UserManager<SchoolEatsUser> userManager;

        public UserService(SchoolEatsDbContext dbContext, UserManager<SchoolEatsUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<List<AllPendingUsersViewModel>> GetAllPendingUsersAsync()
        {
            var allUsers = await this.dbContext
                .Users
                .Where(x => x.IsApproved == false)
                .Select(x => new AllPendingUsersViewModel()
                {
                    Id = x.Id,
                    Username = x.UserName,
                    IsApproved = x.IsApproved,
                })
                .ToListAsync();

            return allUsers;
        }

        public async Task ApproveUserAsync(Guid userId)
        {
	        var user = await this.dbContext
		        .Users.FindAsync(userId);

	        user.IsApproved = true;
	        this.dbContext.Users.Update(user);
            await this.dbContext.SaveChangesAsync();
	        ;
        }

        public async Task<bool> IsUserApproved(Guid id)
        {
	        var user = await this.dbContext
		        .Users
		        .FindAsync(id);

	        if (user.IsApproved)
	        {
		        return true;
	        }

            return false;
        }
    }
}
