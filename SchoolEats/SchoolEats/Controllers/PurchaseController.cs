namespace SchoolEats.Controllers
{
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Microsoft.AspNetCore.Mvc;
	using Newtonsoft.Json;
	using Services.Data.Interfaces;
	using Stripe.Checkout;
	using Web.Infrastructure.Extensions;
	using Web.ViewModels.Purchase;
	using static Common.NotificationMessagesConstants;
	using static Common.ErrorMessages;
	using JsonSerializer = System.Text.Json.JsonSerializer;

	public class PurchaseController : Controller
	{
		private readonly IPurchaseService purchaseService;

		public PurchaseController(IPurchaseService purchaseService)
		{
			this.purchaseService = purchaseService;
		}
        public async Task<IActionResult> All()
        {
	       var all = await this.purchaseService.GetAllPurchasesByUserIdAsync(this.User.GetId());
            return View(all);
        }

        public IActionResult AddToShoppingCart()
        {
	        TempData[SuccessMessage] = "Вие успешно добавихте този продукт в количката";

	        return RedirectToAction("All", "Dish");
        }

        [HttpPost]
		public async Task<IActionResult> Purchase(string key)
		{
			string newKey = key;

			JsonSerializerOptions optionsTest = new JsonSerializerOptions();
			optionsTest.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			List<PurchaseViewModel>? myDeserializedClass =  JsonSerializer.Deserialize<List<PurchaseViewModel>>(newKey, optionsTest);

	        List <SessionLineItemOptions> sessionList = new List<SessionLineItemOptions>();
			foreach (var purchase in myDeserializedClass)
			{
				var sessionItem = new SessionLineItemOptions()
				{
					PriceData = new SessionLineItemPriceDataOptions()
					{
						UnitAmountDecimal = decimal.Parse(purchase.price.ToString().Replace(".", "")),
						Currency = "BGN",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = purchase.name,
							Images = new List<string> { purchase.image }
						}
					},
					Quantity = purchase.count,

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
			return View();
		}
		public IActionResult Success()
		{
			return View();
		}
	}
}
