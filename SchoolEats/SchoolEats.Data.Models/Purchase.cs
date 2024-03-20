namespace SchoolEats.Data.Models
{
	using System.ComponentModel.DataAnnotations.Schema;

	public class Purchase
	{
		public Guid Id { get; set; }

		public Guid BuyerId { get; set; }

		[ForeignKey(nameof(Dish))]
		public Guid DishId { get; set; }

		public Dish Dish { get; set; } = null!;
	}
}
