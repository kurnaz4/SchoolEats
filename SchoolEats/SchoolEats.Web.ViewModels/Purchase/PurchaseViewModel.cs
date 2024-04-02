namespace SchoolEats.Web.ViewModels.Purchase
{
	using Newtonsoft.Json;

	public class PurchaseViewModel
	{
		public string name { get; set; }

			public string image { get; set; }

			public string id { get; set; }

			public int count { get; set; }

			public int price { get; set; }

			public int basePrice { get; set; }
	}
}
