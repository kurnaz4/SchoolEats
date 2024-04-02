namespace SchoolEats.Services.Data
{
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using SchoolEats.Data;
    using SchoolEats.Data.Models;
    using Web.ViewModels.Dish;
    using Web.ViewModels.ShoppingCart;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly SchoolEatsDbContext dbContext;

        public ShoppingCartService(SchoolEatsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ShoppingCartViewModel> GetAllByBuyerIdAsync(Guid buyerId)
        {
            var all = await this.dbContext
                .Carts
                .Where(x => x.BuyerId == buyerId)
                .Select(x => new AllDishesViewModel()
                {
                    Id = x.DishId,
                    Price = x.Dish.Price,
                    Description = x.Dish.Description,
                    ImageUrl = x.Dish.ImageUrl,
                    Name = x.Dish.Name,
                    Quantity = x.Quantity,
                    IsAllergenic = x.Dish.IsAllergenic,
                    Category = x.Dish.Category.Name,
                    Grams = x.Dish.Grams,
                    Owner = x.Dish.User.UserName,
                })
                .ToListAsync();

            return new ShoppingCartViewModel()
            {
                Dishes = all
            };
        }

        public async Task AddAsync(Guid dishId, Guid userId)
        {
            var dish = await this.dbContext
                .Dishes
                .FindAsync(dishId);

            var cart = new Cart()
            {
                DishId = dishId,
                BuyerId = userId,
                Quantity = 1,
            };

            var all = await this.GetAllByBuyerIdAsync(userId);
            foreach (var currDish in all.Dishes)
            {
                if (currDish.Id == dishId)
                {
                    cart.Quantity = currDish.Quantity + 1;
                    await this.UpdateDishToUserAsync(dishId, userId, cart.Quantity);
                    return;
                }
            }
            await this.dbContext.Carts.AddAsync(cart);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateDishToUserAsync(Guid dishId, Guid userId, int announceCount)
        {
            var cart = await this.dbContext
                .Carts
                .FirstAsync(x => x.DishId == dishId && x.BuyerId == userId);
            cart.Quantity = announceCount;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteDishToUserAsync(Guid dishId, Guid userId)
        {
            var cartItem = await this.dbContext
                .Carts
                .FirstAsync(x => x.DishId == dishId && x.BuyerId == userId);

            this.dbContext.Remove(cartItem);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
