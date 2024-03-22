namespace SchoolEats.Services.Data
{
	using Interfaces;
	using Microsoft.EntityFrameworkCore;
	using SchoolEats.Data;
	using Web.ViewModels.Category;

	public class CategoryService : ICategoryService
	{
		private readonly SchoolEatsDbContext dbContext;
		public CategoryService(SchoolEatsDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<IEnumerable<string>> AllCategoriesNamesAsync()
		{
			string[] allCategories = await this.dbContext
				.Categories
				.Select(x => x.Name)
				.ToArrayAsync();

			return allCategories;
		}

		public async Task<IEnumerable<DishSelectCategory>> AllCategoriesAsync()
		{
			IEnumerable<DishSelectCategory> allCategories = await dbContext
				.Categories
				.AsNoTracking()
				.Select(c => new DishSelectCategory()
				{
					Id = c.Id,
					Name = c.Name,
				})
				.ToArrayAsync();

			return allCategories;
		}
	}
}
