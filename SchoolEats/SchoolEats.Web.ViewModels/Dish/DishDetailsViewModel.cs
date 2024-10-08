﻿namespace SchoolEats.Web.ViewModels.Dish
{
	public class DishDetailsViewModel
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public int Grams { get; set; }

		public DateTime CreatedOn { get; set; }

		public int Quantity { get; set; }

		public bool IsAllergenic { get; set; }

		public string ImageUrl { get; set; } = null!;

		public decimal Price { get; set; }
	}
}
