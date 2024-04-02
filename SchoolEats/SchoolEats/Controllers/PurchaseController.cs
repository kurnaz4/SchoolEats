namespace SchoolEats.Controllers
{
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Microsoft.AspNetCore.Mvc;
	using Newtonsoft.Json;
	using Services.Data.Interfaces;
	using Services.Messaging;
	using Stripe.Checkout;
	using Web.Infrastructure.Extensions;
	using Web.ViewModels.Purchase;
	using static Common.NotificationMessagesConstants;
	using static Common.ErrorMessages;
	using JsonSerializer = System.Text.Json.JsonSerializer;

	public class PurchaseController : Controller
	{
		private readonly IPurchaseService purchaseService;
		private readonly IShoppingCartService shoppingCartService;
		private readonly IEmailSender emailSender;

		public PurchaseController(IPurchaseService purchaseService, IShoppingCartService shoppingCartService, IEmailSender emailSender)
		{
			this.purchaseService = purchaseService;
			this.shoppingCartService = shoppingCartService;
			this.emailSender = emailSender;
		}
        public async Task<IActionResult> All()
        {
	       var all = await this.purchaseService.GetAllPurchasesByUserIdAsync(this.User.GetId());
            return View(all);
        }


        [HttpPost]
		public async Task<IActionResult> Purchase()
		{
			var all = await this.shoppingCartService.GetAllByBuyerIdAsync(this.User.GetId());

	        List <SessionLineItemOptions> sessionList = new List<SessionLineItemOptions>();
			foreach (var purchase in all.Dishes)
			{
				var sessionItem = new SessionLineItemOptions()
				{
					PriceData = new SessionLineItemPriceDataOptions()
					{
						UnitAmountDecimal = decimal.Parse(purchase.Price.ToString().Replace(".", "")),
						Currency = "BGN",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = purchase.Name,
							Images = new List<string> { purchase.ImageUrl }
						}
					},
					Quantity = purchase.Quantity,

				};
				sessionList.Add(sessionItem);
			}


			var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");

			var url = "https://";
			url += location.Authority;
			var options = new SessionCreateOptions
			{
				LineItems = sessionList,
				Mode = "payment",
				SuccessUrl = $"{url}/Purchase/Success",
				CancelUrl = $"{url}/Purchase/Failed",
				PaymentMethodTypes = new List<string>
				{
					"card"
				},
			};

			var service = new SessionService();
			Session session = null;
			try
			{
				session = await service.CreateAsync(options);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = CommonErrorMessage;
				return RedirectToAction("All", "Dish");

			}
			return Redirect(session.Url);
		}

		public IActionResult Failed()
		{
			TempData[ErrorMessage] = "Неуспешна транзакция!Опитайте отново!";
			return RedirectToAction("All", "ShoppingCart");
		}
		public async Task<IActionResult> Success()
		{
			try
			{
				var all = await this.shoppingCartService.GetAllByBuyerIdAsync(this.User.GetId());
				foreach (var dish in all.Dishes)
				{
					await this.purchaseService.PurchaseDishAsync(dish.Id, this.User.GetId());
					await this.shoppingCartService.DeleteDishToUserAsync(dish.Id, this.User.GetId());
				}
				await this.emailSender.SendEmailAsync(this.User.GetEmail(), "Успешно заплащане", "Успешно закупихте вашите продукти!");
				TempData[SuccessMessage] = "Успешно транзация!Очакваме ви в стола за да си вземете яденето!";
			}
			catch (Exception e)
			{
				TempData[ErrorMessage] = CommonErrorMessage;
				TempData[InformationMessage] = "Вашата транзакция беше успешна въпреки тази грешка!";
			}
			
			return RedirectToAction("All", "Dish");
		}
	}
}
