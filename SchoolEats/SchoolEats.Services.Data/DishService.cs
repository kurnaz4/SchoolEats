namespace SchoolEats.Services.Data
{
	using Interfaces;
	using Microsoft.EntityFrameworkCore;
	using SchoolEats.Data;
	using Web.ViewModels.Dish;

	public class DishService : IDishService
	{
		private readonly SchoolEatsDbContext dbContext;

		public DishService(SchoolEatsDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<List<AllDishesViewModel>> GetAllDishesAsync()
		{
			List<AllDishesViewModel> all = await  this.dbContext
				.Dishes
				.Where(x => x.IsActive)
				.Select(d => new AllDishesViewModel()
				{
					Id = d.Id,
					Name = d.Name,
					Description = d.Description,
					Grams = d.Grams,
					ImageUrl = d.ImageUrl,
					IsAllergenic = d.IsAllergenic,
					Price = d.Price,
					Quantity = d.Quantity,
				})
				.ToListAsync();
			return all;
		}
	}
}
