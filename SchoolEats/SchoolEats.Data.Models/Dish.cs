namespace SchoolEats.Data.Models
{
	using System.ComponentModel.DataAnnotations;
	using static Common.ValidationConstants.Dish;
	public class Dish
	{
        public Dish()
		{
			this.Id = Guid.NewGuid();
		}

		[Key]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[MaxLength(DescriptionMaxLength)]
		public string Description { get; set; } = null!;

		[Required]
		public int Grams { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		public bool IsAllergenic { get; set; }

		[MaxLength(ImageUrlMaxLength)]
		public string ImageUrl { get; set; }
		
		public bool IsActive { get; set; }

		public DateTime CreatedOn { get; set; }

		[Required]
		public decimal Price { get; set; }

		public Guid UserId { get; set; }

		public SchoolEatsUser User { get; set; } = null!;

		public int CategoryId { get; set; }

		public Category Category { get; set; }

	}
}