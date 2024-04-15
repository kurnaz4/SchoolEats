using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SchoolEats.Data.Configurations
{
	using Models;

	public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasData(SeedCategories());
		}

		private Category[] SeedCategories()
		{
			//вкарване на категории като статични данни в база данни
			ICollection<Category> categories = new HashSet<Category>();

			Category category;

			category = new Category()
			{
				Id = 1,
				Name = "Ястия"
			};

			categories.Add(category);

			category = new Category()
			{
				Id = 2,
				Name = "Бърза Храна"
			};
			categories.Add(category);

			category = new Category()
			{
				Id = 3,
				Name = "Снаксове"
			};
			categories.Add(category);

			category = new Category()
			{
				Id = 4,
				Name = "Десерти"
			};
			categories.Add(category);

			category = new Category()
			{
				Id = 5,
				Name = "Напитки"
			};
			categories.Add(category);

			category = new Category()
			{
				Id = 6,
				Name = "Плодове и зеленчуци"
			};
			categories.Add(category);

			return categories.ToArray();
		}
	}
}
