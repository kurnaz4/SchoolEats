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

        public ShoppingCartController(IShoppingCartService shoppingCart)
        {
            this.shoppingCart = shoppingCart;
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
                await this.shoppingCart.AddAsync(dishId, this.User.GetId());

                TempData[SuccessMessage] = "Успешно добавихте този продукт в количката!";
            }
            catch (Exception e)
            {
                TempData[ErrorMessage] = CommonErrorMessage;
            }

            return RedirectToAction("All", "Dish");
        }
    }
}
