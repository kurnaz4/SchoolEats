namespace SchoolEats.Services.Data
{
	using Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SchoolEats.Data;
	using SchoolEats.Data.Models;
	using Web.ViewModels.Dish;

	public class DishService : IDishService
	{
		private readonly SchoolEatsDbContext dbContext;
        private readonly UserManager<SchoolEatsUser> userManager;
        public DishService(SchoolEatsDbContext dbContext, UserManager<SchoolEatsUser> userManager)
		{
			this.dbContext = dbContext;
			this.userManager = userManager;
        }
		public async Task<List<AllDishesViewModel>> GetAllDishesAsync()
		{
			List<AllDishesViewModel> all = await  this.dbContext
				.Dishes
				.Where(x => x.IsActive && x.Quantity > 0)
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
					Category = d.Category.Name,
					 Owner = d.User.UserName,
					 IsActive = d.IsActive,
				})
				.ToListAsync();
			return all;
		}

		public async Task<List<AllDishesViewModel>> GetAllActiveAndNotActiveDishesAsync()
		{
			List<AllDishesViewModel> all = await this.dbContext
				.Dishes
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
					Category = d.Category.Name,
					Owner = d.User.UserName,
					IsActive = d.IsActive,
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

		public async Task AddDishAsync(DishFormViewModel model)
		{
			Dish dishEntity = new Dish()
			{
				Name = model.Name,
				Description = model.Description,
				IsAllergenic = model.IsAllergenic,
				ImageUrl = model.ImagePath,
				Price = model.Price,
				Quantity = model.Quantity,
				CreatedOn = DateTime.Now,
				Grams = model.Grams,
				IsActive = true,
				UserId = model.UserId,
				CategoryId = model.CategoryId,
			};

			await this.dbContext.AddAsync(dishEntity);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<DishFormViewModel> GetDishForEditAsync(Guid dishId)
		{
			var model = await this.dbContext
				.Dishes
				.Where(x => x.IsActive && x.Id == dishId)
				.Select(d => new DishFormViewModel()
				{
					Id = d.Id,
					Name = d.Name,
					Description = d.Description,
					IsAllergenic = d.IsAllergenic,
					Grams = d.Grams,
					ImagePath = d.ImageUrl,
					Price = d.Price,
					Quantity = d.Quantity,
					CategoryId = d.CategoryId,
					UserId = d.UserId,
				})
				.FirstAsync();

			return model;
		}

		public async Task EditDishAsync(DishFormViewModel model)
		{
			var oldModel = await this.dbContext
				.Dishes
				.Where(x => x.IsActive && x.Id == model.Id).FirstAsync();

			oldModel.Id = model.Id;
			oldModel.Name = model.Name;
			oldModel.Description = model.Description;
			oldModel.Grams = model.Grams;
			oldModel.Price = model.Price;
			oldModel.IsActive = true;
			oldModel.IsAllergenic = model.IsAllergenic;
			oldModel.Quantity = model.Quantity;
			oldModel.CategoryId = model.CategoryId;
			oldModel.UserId = model.UserId;
			oldModel.ImageUrl = model.ImagePath;
			
			await this.dbContext.SaveChangesAsync();
		}

		public async Task DeleteDishAsync(Guid dishId)
		{
			var modelToDelete = await this.dbContext
				.Dishes.FirstAsync(x => x.IsActive && x.Id == dishId);

			if (modelToDelete.IsActive is false)
			{
				return;
			}

			modelToDelete.IsActive = false;

			await this.dbContext.SaveChangesAsync();
		}

		public async Task MakeDishActiveAsync(Guid dishId)
		{
			var dish = await this.dbContext
				.Dishes
				.FindAsync(dishId);
			dish.IsActive = true;
			await this.dbContext.SaveChangesAsync();
		}

		public async Task HardDeleteDishAsync(Guid dishId)
		{
			var dish = await this.dbContext
				.Dishes
				.FindAsync(dishId);

			this.dbContext.Dishes.Remove(dish);

			await this.dbContext.SaveChangesAsync();
		}

		public async Task<bool> IsQuantityEnough(Guid dishId)
		{
			var dish = await this.dbContext
				.Dishes
				.FindAsync(dishId);
			if (dish.Quantity - 1 < 0)
			{
				return false;
			}

			return true;
		}
	}
}