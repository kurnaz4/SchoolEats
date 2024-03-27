namespace SchoolEats.Web.Infrastructure.Extensions
{
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using static Common.GeneralApplicationConstants;
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// This method seeds admin role if it does not exist.
        /// Passed email should be valid email of existing user in the application.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email)
        {
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            UserManager<SchoolEatsUser> userManager =
                serviceProvider.GetRequiredService<UserManager<SchoolEatsUser>>();
            RoleManager<IdentityRole<Guid>> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminRoleName))
                    {
                        return;
                    }

                    IdentityRole<Guid> role =
                        new IdentityRole<Guid>(AdminRoleName);

                    await roleManager.CreateAsync(role);

                    SchoolEatsUser adminUser =
                        await userManager.FindByEmailAsync(email);

                    await userManager.AddToRoleAsync(adminUser, AdminRoleName);
                })
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}