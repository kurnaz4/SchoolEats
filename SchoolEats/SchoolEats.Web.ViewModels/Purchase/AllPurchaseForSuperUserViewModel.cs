namespace SchoolEats.Web.ViewModels.Purchase
{
	public class AllPurchaseForSuperUserViewModel
	{
		public Guid PurchaseId { get; set; }

		public Guid BuyerId { get; set; }
		public string BuyerName { get; set; } = null!;

		public string BuyerEmail { get; set; } = null;
		public DateTime PurchasedOn { get; set; }

		public string DishName { get; set; } = null!;

		public string DishImageUrl { get; set; } = null!;

		public int PurchasedQuantity { get; set; }

		public int DishGrams { get; set; }

		public decimal DishPrice { get; set; }

		public string DishCategory { get; set; } = null!;

		public string Code { get; set; } = null!;
	}
}
