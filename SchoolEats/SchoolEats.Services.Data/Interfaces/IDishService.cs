namespace SchoolEats.Services.Data.Interfaces
{
	using Web.ViewModels.Dish;

	public interface IDishService
	{
		Task<List<AllDishesViewModel>> GetAllDishesAsync();

		Task<DishDetailsViewModel> GetDishForDetailsByDishIdAsync(Guid dishId);

		Task AddDishAsync(DishFormViewModel model);

		Task<DishFormViewModel> GetDishForEditAsync(Guid dishId);

		Task EditDishAsync(DishFormViewModel model);
	}
}
