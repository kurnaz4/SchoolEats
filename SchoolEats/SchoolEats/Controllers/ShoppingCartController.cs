namespace SchoolEats.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using Web.Infrastructure.Extensions;
    using static Common.ErrorMessages;
    using static Common.NotificationMessagesConstants;
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IDishService dishService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IDishService dishService)
        {
            this.shoppingCartService = shoppingCartService;
            this.dishService = dishService;
        }
        public async Task<IActionResult> All()
        {
            var all = await this.shoppingCartService.GetAllByBuyerIdAsync(this.User.GetId());
            return View(all);
        }

        public async Task<IActionResult> Add(Guid dishId)
        {
            try
            {
	            var neededQuantity = 0;
	            var all = await this.shoppingCartService.GetAllByBuyerIdAsync(this.User.GetId());
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
					await this.shoppingCartService.AddAsync(dishId, this.User.GetId());
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
				await this.shoppingCartService.DeleteDishToUserAsync(dishId, this.User.GetId());
				TempData[SuccessMessage] = "Успешно премахнахте ястието от количката си!";
	        }
	        catch (Exception e)
	        {
                TempData[ErrorMessage] = CommonErrorMessage;
	        }
	        

            return RedirectToAction("All", "ShoppingCart");
        }

        [HttpGet]
        public async Task<IActionResult> IncreaseCount(Guid dishId)
        {
	        try
	        {
				var neededQuantity = 0;
				var all = await this.shoppingCartService.GetAllByBuyerIdAsync(this.User.GetId());
				var neededDish = all.Dishes.FirstOrDefault(x => x.Id == dishId);
				neededQuantity = neededDish.Quantity + 1;
		        bool isQuantityEnough = await this.dishService.IsQuantityEnough(dishId, neededQuantity);
				if (isQuantityEnough)
				{
					await this.shoppingCartService.AddAsync(dishId, this.User.GetId());
					TempData[SuccessMessage] = "Успешно увеличихте количеството на този продукт в количката!";
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
	        
			return RedirectToAction("All", "ShoppingCart");
        }

        [HttpGet]
        public async Task<IActionResult> DecreaseCount(Guid dishId)
        {
	        try
	        {
				var all = await this.shoppingCartService.GetAllByBuyerIdAsync(this.User.GetId());
				var neededDish = all.Dishes.FirstOrDefault(x => x.Id == dishId);
				neededDish.Quantity -= 1;
		        if (neededDish.Quantity <= 0)
		        {
			        TempData[ErrorMessage] = "Минималният брой е 1!";
		        }
		        else
		        {
			        await this.shoppingCartService.UpdateDishToUserAsync(dishId, this.User.GetId(), neededDish.Quantity);
			        TempData[SuccessMessage] = "Успешно увеличихте количеството на продукта!";
		        }
			}
	        catch (Exception e)
	        {
				TempData[ErrorMessage] = CommonErrorMessage;
	        }
	        
	        return RedirectToAction("All", "ShoppingCart");
        }
	}
}