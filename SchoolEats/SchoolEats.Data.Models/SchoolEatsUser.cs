namespace SchoolEats.Data.Models
{
	using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class SchoolEatsUser : IdentityUser<Guid>
	{
        //класове за приемане на потребител
		public SchoolEatsUser()
		{
			Id = Guid.NewGuid();
            Dishes = new HashSet<Dish>();
        }
		[Unicode(true)]
        public override Guid Id { get; set; }

        public bool IsApproved { get; set; }

        public ICollection<Dish> Dishes { get; set; }
    }
}