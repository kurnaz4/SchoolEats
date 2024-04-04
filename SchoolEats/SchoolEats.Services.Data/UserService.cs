namespace SchoolEats.Services.Data
{
	using System.Diagnostics;
	using Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SchoolEats.Data;
    using SchoolEats.Data.Models;
    using Web.ViewModels.Admin;
    using static Common.GeneralApplicationConstants;
    public class UserService :IUserService
    {
        private readonly SchoolEatsDbContext dbContext;
        private readonly UserManager<SchoolEatsUser> userManager;
        private readonly SignInManager<SchoolEatsUser> signInManager;
		public UserService(SchoolEatsDbContext dbContext, UserManager<SchoolEatsUser> userManager, SignInManager<SchoolEatsUser> signInManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
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

        public async Task<SchoolEatsUser> GetUserAsync(Guid userId)
        {
	        var user = await this.dbContext.Users
		        .FindAsync(userId);

	        return user;
        }

        public async Task<List<AllUsersViewModel>> GetAllUsersAsync()
        {
			var all = await this.dbContext
				.Users
				.Select(x => new AllUsersViewModel()
				{
                    Id = x.Id,
                    Username = x.UserName,
                    IsApproved = x.IsApproved,
                    Email = x.Email
				})
				.ToListAsync();

			return all;
        }

        public async Task<SchoolEatsUser> AddUserToSuperUserRoleAsync(Guid userId)
        {
	        var user = await this.dbContext
		        .Users
		        .FirstAsync(x => x.Id == userId);

	        var currUser = user;

	        this.dbContext.Users.Remove(user);

	        await this.dbContext.SaveChangesAsync();
            user.IsApproved = true;
	        await userManager.CreateAsync(currUser);
			await userManager.AddToRoleAsync(user, SuperUserRoleName);
			await this.dbContext.SaveChangesAsync();

			return currUser;
        }

        public async Task<SchoolEatsUser> RemoveSuperUserRoleFromUserAsync(Guid userId)
        {
	        var user = await this.dbContext
		        .Users
		        .FindAsync(userId);

            var currUser = user;

            this.dbContext.Users.Remove(user);
            await this.dbContext.SaveChangesAsync();
            await userManager.CreateAsync(currUser);
            await userManager.AddToRoleAsync(user, UserRoleName);
            await this.dbContext.SaveChangesAsync();

            return currUser;
        }
    }
}
