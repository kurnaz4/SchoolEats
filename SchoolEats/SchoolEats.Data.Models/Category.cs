namespace SchoolEats.Data.Models
{
	using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;
    using static Common.ValidationConstants.Category;
	public class Category
	{
		public Category()
		{
			this.Dishes = new HashSet<Dish>();
		}
		[Unicode(true)]
		public int Id { get; set; }

		[Required]
		[Unicode(true)]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; }

		public ICollection<Dish> Dishes { get; set; }
	}
}
