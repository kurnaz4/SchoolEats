using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SchoolEats.Web.ViewModels.Dish
{
	using Category;
	using Common;
	using static Common.ValidationConstants.Dish;
	public class DishFormViewModel
	{
		public DishFormViewModel()
		{
			this.Categories = new HashSet<DishSelectCategory>();
		}
		public Guid Id { get; set; }

		[Display(Name = "Име")]
		[Required(AllowEmptyStrings = false, ErrorMessage = ErrorMessages.RequiredField)]
		[StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = ErrorMessages.DishNameLengthError)]
		public string Name { get; set; } = null!;

		[Display(Name = "Описание")]
		[Required(ErrorMessage = ErrorMessages.RequiredField)]
		[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = ErrorMessages.DishDescriptionLengthError)]
		public string Description { get; set; } = null!;


		[Display(Name = "Цена")]
		[Range((double)PriceMinLength, (int)PriceMaxLength,ErrorMessage = ErrorMessages.DishPriceLengthError)]
		public decimal Price { get; set; }

		[Display(Name = "Избери Снимка")]
		[Required(ErrorMessage = ErrorMessages.RequiredField)]
		public IFormFile ProductImage { get; set; } = null!;

		[Display(Name = "Грамаж")]
		[Range(GramsMinLength, GramsMaxLength, ErrorMessage = ErrorMessages.DishGramsLengthError)]
		public int Grams { get; set; }

		[Display(Name = "Количество")]
		[Range(QuantityMinLength, QuantityMaxLength, ErrorMessage = ErrorMessages.DishQuantityLengthError)]
		public int Quantity { get; set; }

		[Required(ErrorMessage = ErrorMessages.RequiredField)]
		public string ImagePath { get; set; } = null!;

		[Display(Name = "Алергени")]
		public bool IsAllergenic { get; set; }

		[Display(Name = "Категории")]
		public int CategoryId { get; set; }

		public Guid UserId { get; set; }
		public IEnumerable<DishSelectCategory> Categories { get; set; }
	}
}
