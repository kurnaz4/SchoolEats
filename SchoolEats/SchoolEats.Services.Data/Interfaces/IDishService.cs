namespace SchoolEats.Services.Data.Interfaces
{
    using Microsoft.AspNetCore.Identity;
    using SchoolEats.Data.Models;
    using Web.ViewModels.Dish;

	public interface IDishService
	{
		Task<List<AllDishesViewModel>> GetAllDishesAsync();

		Task<List<AllDishesViewModel>> GetAllActiveAndNotActiveDishesAsync();
		Task<DishDetailsViewModel> GetDishForDetailsByDishIdAsync(Guid dishId);

		Task AddDishAsync(DishFormViewModel model);

		Task<DishFormViewModel> GetDishForEditAsync(Guid dishId);

		Task EditDishAsync(DishFormViewModel model);

		Task DeleteDishAsync(Guid dishId);

		Task MakeDishActiveAsync(Guid dishId);

		Task HardDeleteDishAsync(Guid dishId);

		Task<bool> IsQuantityEnough(Guid dishId, int quantity);

	}
}