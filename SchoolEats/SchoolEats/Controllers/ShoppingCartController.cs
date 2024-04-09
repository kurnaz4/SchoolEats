namespace SchoolEats.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using Web.Infrastructure.Extensions;
    using static Common.ErrorMessages;
    using static Common.NotificationMessagesConstants;
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCart;
        private readonly IDishService dishService;

        public ShoppingCartController(IShoppingCartService shoppingCart, IDishService dishService)
        {
            this.shoppingCart = shoppingCart;
            this.dishService = dishService;
        }
        public async Task<IActionResult> All()
        {
            var all = await this.shoppingCart.GetAllByBuyerIdAsync(this.User.GetId());
            return View(all);
        }

        public async Task<IActionResult> Add(Guid dishId)
        {
            try
            {
	            var neededQuantity = 0;
	            var all = await this.shoppingCart.GetAllByBuyerIdAsync(this.User.GetId());
	            var neededDish = all.Dishes.FirstOrDefault(x => x.Id == dishId);
	            if (neededDish == null)
	            {
		            neededQuantity = 1;
	            }
	            if (neededDish != null)
	            {
                    neededQuantity = neededDish.Quantity + 1;
	            }
	            bool isQuantityEnough = await this.dishService.IsQuantityEnough(dishId, neededQuantity);
	            if (isQuantityEnough)
	            {
					await this.shoppingCart.AddAsync(dishId, this.User.GetId());
					TempData[SuccessMessage] = "Успешно добавихте този продукт в количката!";
	            }
	            else
	            {
		            TempData[ErrorMessage] = "Няма налични бройки!";
	            }
            }
            catch (Exception e)
            {
                TempData[ErrorMessage] = CommonErrorMessage;
            }

            return RedirectToAction("All", "Dish");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid dishId)
        {
	        try
	        {
				await this.shoppingCart.DeleteDishToUserAsync(dishId, this.User.GetId());
				TempData[SuccessMessage] = "Успешно премахнахте ястието от количката си!";
	        }
	        catch (Exception e)
	        {
                TempData[ErrorMessage] = CommonErrorMessage;
	        }
	        

            return RedirectToAction("All", "ShoppingCart");
        }
    }
}