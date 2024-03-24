namespace SchoolEats.Services.Data
{
	using Interfaces;
	using Microsoft.EntityFrameworkCore;
	using SchoolEats.Data;
	using SchoolEats.Data.Models;
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

		public async Task<DishDetailsViewModel> GetDishForDetailsByDishIdAsync(Guid dishId)
		{
			var dish = await this.dbContext
				.Dishes
				.Where(x => x.IsActive && x.Id == dishId)
				.Select(d => new DishDetailsViewModel()
				{
					Id = d.Id,
					Name = d.Name,
					Description = d.Description,
					CreatedOn = d.CreatedOn,
					Grams = d.Grams,
					ImageUrl = d.ImageUrl,
					IsAllergenic = d.IsAllergenic,
					Price = d.Price,
					Quantity = d.Quantity,
				})
				.FirstAsync();

			return dish;
		}

		public async Task AddDishAsync(AddDishViewModel model)
		{
			Dish dishEntity = new Dish()
			{
				Name = model.Name,
				Description = model.Description,
				IsAllergenic = model.IsAllergenic,
				ImageUrl = model.ImagePath,
				Price = model.Price,
				Quantity = model.Quantity,
				CreatedOn = DateTime.Now.Date,
				Grams = model.Grams,
				IsActive = true,
				UserId = model.UserId,
				CategoryId = model.CategoryId,
			};

			await this.dbContext.AddAsync(dishEntity);
			await this.dbContext.SaveChangesAsync();
		}
	}
}
