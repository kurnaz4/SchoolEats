namespace SchoolEats.Services.Data.Interfaces
{
	using Web.ViewModels.Category;
	//interface-за да рабори Service
	public interface ICategoryService
	{
		Task<IEnumerable<string>> AllCategoriesNamesAsync();
		Task<IEnumerable<DishSelectCategory>> AllCategoriesAsync();
	}
}
