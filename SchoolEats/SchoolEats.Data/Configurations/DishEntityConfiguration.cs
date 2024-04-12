namespace SchoolEats.Data.Configurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Models;

	public class DishEntityConfiguration : IEntityTypeConfiguration<Dish>
	{
		public void Configure(EntityTypeBuilder<Dish> builder)
        {
			//задаване начални стойности на данните и създаваме връзките между таблиците
            builder
                .Property(x => x.Id)
                .IsUnicode();
            builder
                .Property(x => x.CreatedOn)
				//дата за създадени ястия
                .HasDefaultValueSql("GETDATE()");

            builder
				.Property(x => x.IsActive)
				.HasDefaultValue(true);

			builder
				.Property(x => x.IsAllergenic)
				.HasDefaultValue(false);
			//връзки между таблиците 
			builder.HasOne(x => x.User)
                .WithMany(x => x.Dishes)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict);
			
			builder.HasOne(x => x.Category)
				.WithMany(x => x.Dishes)
				.HasForeignKey(x => x.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
