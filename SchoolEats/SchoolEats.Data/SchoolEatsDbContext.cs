namespace SchoolEats.Data
{
	using Configurations;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Models;

	public class SchoolEatsDbContext : IdentityDbContext<SchoolEatsUser,IdentityRole<Guid>, Guid>
	{
		public SchoolEatsDbContext(DbContextOptions<SchoolEatsDbContext> options)
		:base(options)
		{
			
		}
		//Таблици в базата данни
        public DbSet<Dish> Dishes { get; set; } = null!;

		public DbSet<Category> Categories { get; set; } = null!;

		public DbSet<Purchase> Purchases { get; set; } = null!;

        public DbSet<Cart> Carts { get; set; } = null!;

        public DbSet<Report> Reports { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new DishEntityConfiguration());
			builder.ApplyConfiguration(new CategoryEntityConfiguration());
			builder.ApplyConfiguration(new SeedAdminEntityConfiguration());
			base.OnModelCreating(builder);
		}
	}
}
