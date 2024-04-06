namespace SchoolEats.Web.ViewModels.Dish
{
	public class AllDishesViewModel
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public int Grams { get; set; }

		public int Quantity { get; set; }

		public bool IsAllergenic { get; set; }

		public string ImageUrl { get; set; } = null!;

		public decimal Price { get; set; }

		public string Category { get; set; } = null!;

		public string Owner { get; set; }

		public bool IsActive { get; set; }

    }
}
