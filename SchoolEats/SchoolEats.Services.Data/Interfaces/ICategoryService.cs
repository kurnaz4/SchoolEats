namespace SchoolEats.Services.Data.Interfaces
{
	using Web.ViewModels.Category;

	public interface ICategoryService
	{
		Task<IEnumerable<string>> AllCategoriesNamesAsync();
		Task<IEnumerable<DishSelectCategory>> AllCategoriesAsync();
	}
}
