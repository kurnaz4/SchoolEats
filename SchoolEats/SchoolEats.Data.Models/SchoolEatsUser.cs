namespace SchoolEats.Data.Models
{
	using Microsoft.AspNetCore.Identity;
	public class SchoolEatsUser : IdentityUser<Guid>
	{
		public SchoolEatsUser()
		{
			Id = Guid.NewGuid();
			Dishes = new HashSet<Dish>();
		}

		public ICollection<Dish> Dishes { get; set; }
	}
}