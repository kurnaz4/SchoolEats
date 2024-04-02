namespace SchoolEats.Services.Data.Interfaces
{
    using Web.ViewModels.ShoppingCart;

    public interface IShoppingCartService
    {
        Task<ShoppingCartViewModel> GetAllByBuyerIdAsync(Guid buyerId);

        Task AddAsync(Guid dishId, Guid userId);

        Task UpdateDishToUserAsync(Guid dishId, Guid userId, int announceCount);

        Task DeleteDishToUserAsync(Guid dishId, Guid userId);
    }
}
