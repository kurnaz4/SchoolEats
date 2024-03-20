namespace SchoolEats.Data.Models
{
	using System.ComponentModel.DataAnnotations;
	using static Common.ValidationConstants.Category;
	public class Category
	{
		public Category()
		{
			this.Dishes = new HashSet<Dish>();
		}
		public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		public ICollection<Dish> Dishes { get; set; }
	}
}
