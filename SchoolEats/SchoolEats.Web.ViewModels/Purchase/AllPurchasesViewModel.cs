namespace SchoolEats.Web.ViewModels.Purchase
{
	using Dish;

	public class AllPurchasesViewModel : AllDishesViewModel
	{
		public string? Code { get; set; }

		public DateTime PurchasedOn { get; set; }

		public int PurchaseQuantity { get; set; }
	}
}
