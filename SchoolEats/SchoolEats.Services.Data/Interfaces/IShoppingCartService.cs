namespace SchoolEats.Services.Data.Interfaces
{
	using SchoolEats.Data.Models;
	using Web.ViewModels.ShoppingCart;

    public interface IShoppingCartService
    {
        Task<ShoppingCartViewModel> GetAllByBuyerIdAsync(Guid buyerId);

        Task AddAsync(Guid dishId, Guid userId);

        Task UpdateDishToUserAsync(Guid dishId, Guid userId, int announceCount);

        Task DeleteDishToUserAsync(Guid dishId, Guid userId);
        
        Task<Cart> GetDishFromCartAsync(Guid dishId, Guid userId);
    }
}
