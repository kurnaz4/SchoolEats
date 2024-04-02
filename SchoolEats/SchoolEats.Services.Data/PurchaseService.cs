namespace SchoolEats.Services.Data
{
	using Interfaces;
	using Microsoft.EntityFrameworkCore;
	using SchoolEats.Data;
	using SchoolEats.Data.Models;
	using Web.ViewModels.Dish;

	public class PurchaseService : IPurchaseService
	{
		private readonly SchoolEatsDbContext dbContext;
		public PurchaseService(SchoolEatsDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<List<AllDishesViewModel>> GetAllPurchasesByUserIdAsync(Guid userId)//For history
		{
			return await this.dbContext
				.Purchases
				.Where(x => x.BuyerId == userId)
				.Select(p => new AllDishesViewModel()
				{
					Id = p.Id,
					Name = p.Dish.Name,
					Description = p.Dish.Description,
					Grams = p.Dish.Grams,
					ImageUrl = p.Dish.ImageUrl,
					IsAllergenic = p.Dish.IsAllergenic,
					Price = p.Dish.Price,
					Quantity = p.Dish.Quantity,
					Category = p.Dish.Category.Name,
					Owner = p.Dish.User.UserName,
					PurchasedOn = DateTime.Now,
				})
				.ToListAsync();
		}

		public async Task PurchaseDishAsync(Guid dishId, Guid userId)
		{
			var dish = await this.dbContext
				.Dishes
				.FindAsync(dishId);

			var purchase = new Purchase()
			{
				DishId = dishId,
				BuyerId = userId,
			};

			await this.dbContext.Purchases.AddAsync(purchase);
			await this.dbContext.SaveChangesAsync();
		}
	}
}
