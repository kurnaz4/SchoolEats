namespace SchoolEats.Common
{
	public class ErrorMessages
	{
		//Dish
		public const string DishNameLengthError = "Името трябва да бъде от {2} до {1} символа!";
		public const string RequiredField = "Полето {0} е задължително!";
		public const string DishDescriptionLengthError = "Описанието трябва да бъде от {2} до {1} символа!";
		public const string InvalidValue = "Стойността {0} е невалидна!";
		public const string DishQuantityLengthError = "Количеството трябва да бъде от {1} до {2}";
		public const string DishPriceLengthError = "Цената трябва да бъде от {1} до {2}";
		public const string DishGramsLengthError = "Грамажът трябва да бъде от {1} до {2}";

		//Common unexpected exception message
		public const string CommonErrorMessage = "Възникна неочаквана грешка! Моля опитайте отново!";
	}
}
