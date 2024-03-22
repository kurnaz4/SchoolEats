namespace SchoolEats.Services.Data.Interfaces
{
	using Web.ViewModels.Dish;

	public interface IPurchaseService
	{
		Task<List<AllDishesViewModel>> GetAllPurchasesByUserIdAsync(Guid userId);
	}
}
