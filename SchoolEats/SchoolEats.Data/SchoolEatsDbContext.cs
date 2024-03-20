namespace SchoolEats.Data
{
	using Configurations;
	using Microsoft.AspNet.Identity.EntityFramework;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Models;
	using System.Data.Entity;

	public class SchoolEatsDbContext : IdentityDbContext<SchoolEatsUser,IdentityRole<Guid>, Guid>
	{
		public SchoolEatsDbContext(DbContextOptions<SchoolEatsDbContext> options)
		:base(options)
		{
			
		}

		public DbSet<Dish> Dishes { get; set; } = null!;

		public DbSet<Category> Categories { get; set; } = null!;

		public DbSet<Purchase> Purchases { get; set; } = null!;
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new DishEntityConfiguration());
			base.OnModelCreating(builder);
		}
	}
}
