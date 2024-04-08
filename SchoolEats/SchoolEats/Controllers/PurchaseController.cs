namespace SchoolEats.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Services.Data.Interfaces;
	using Services.Messaging;
	using Stripe.Checkout;
	using Web.Infrastructure.Extensions;
	using Web.ViewModels.ShoppingCart;
	using static Common.NotificationMessagesConstants;
	using static Common.ErrorMessages;

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

	        List<SessionLineItemOptions> sessionList = new List<SessionLineItemOptions>();
			foreach (var purchase in all.Dishes)
			{
				var stringPrice = purchase.Price.ToString();
				if (stringPrice.Contains("."))
				{
                    stringPrice = stringPrice.Replace(".", "");
				}
				else if (stringPrice.Contains(","))
				{
					stringPrice = stringPrice.Replace(",", "");
				}
                var price = decimal.Parse(stringPrice);

                var sessionItem = new SessionLineItemOptions()
				{
					PriceData = new SessionLineItemPriceDataOptions()
					{
						UnitAmountDecimal = price,
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

			//tursi hosta na komputara
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
				CustomerEmail = this.User.GetEmail(),
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
			ShoppingCartViewModel all = null;
			try
			{
				List<string> names = new List<string>();
				all = await this.shoppingCartService.GetAllByBuyerIdAsync(this.User.GetId());
				foreach (var dish in all.Dishes) 
				{
					names.Add(dish.Name);
					await this.purchaseService.PurchaseDishAsync(dish.Id, this.User.GetId(), dish.Quantity);
					await this.shoppingCartService.DeleteDishToUserAsync(dish.Id, this.User.GetId());
				}
				await this.emailSender.SendEmailAsync(this.User.GetEmail(), "Успешно заплащане", $"Успешно закупихте вашите продукти: {String.Join(", ", names)}!");

				TempData[SuccessMessage] = "Успешно транзация!Очакваме ви в стола за да си вземете яденето!";
			}
			catch (Exception e)
			{
				if (all != null)
				{
					foreach (var dish in all.Dishes)
					{
						await this.purchaseService.PurchaseDishAsync(dish.Id, this.User.GetId(), dish.Quantity);
						await this.shoppingCartService.DeleteDishToUserAsync(dish.Id, this.User.GetId());
					}
				}
				TempData[ErrorMessage] = CommonErrorMessage;
				TempData[InformationMessage] = "Вашата транзакция беше успешна въпреки тази грешка!";
			}
			
			return RedirectToAction("All", "Dish");
		}

		[HttpPost]
		public async Task<IActionResult> PurchaseWithCode()
		{
			try
			{
				var all = await this.shoppingCartService.GetAllByBuyerIdAsync(this.User.GetId());
				List<string> names = new List<string>();
				//Generate Template
				string code = this.purchaseService.GenerateRandomPurchaseCode();
				foreach (var dish in all.Dishes)
				{
					names.Add(dish.Name);
					await this.purchaseService.PurchaseDishAsync(dish.Id, this.User.GetId(), dish.Quantity, code);
					await this.shoppingCartService.DeleteDishToUserAsync(dish.Id, this.User.GetId());
				}

				await this.emailSender.SendEmailAsync(this.User.GetEmail(), "Благодарим за поръчаната храна!", $"Благодарим ви за поръчката на продуктите: {String.Join(", ", names)}. Кодът на вашата поръчка е: {code} . Очакваме ви в стола за да вземете поръчката си!");

				TempData[SuccessMessage] = "Успешно заявихте вашите продукти!";
			}
			catch (Exception e)
			{
				TempData[ErrorMessage] = CommonErrorMessage;
			}
			
			return RedirectToAction("All", "Dish");
		}

		[HttpPost]
		public async Task<IActionResult> CompleteOrder(Guid purchaseId)
		{
			try
			{
				var purchase = await this.purchaseService.GetPurchaseByPurchaseId(purchaseId);
				var allByCode = await this.purchaseService.GetPurchasesByPurchaseCodeAndBuyerIdAsync(purchase.Code, purchase.BuyerId);
				await this.purchaseService.CompletePurchaseAsync(allByCode);
				var priceSum = await this.purchaseService.GetPriceSumOfPurchaseByCodeAndBuyerIdAsync(purchase.Code, purchase.BuyerId);

				TempData[SuccessMessage] = $"Успешно завършена поръчка на стойност {priceSum} лв.";
			}
			catch (Exception e)
			{
				TempData[ErrorMessage] = CommonErrorMessage;
			}
			

			return RedirectToAction("Orders", "SuperUser");
		}
	}
}
