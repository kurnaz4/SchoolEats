namespace SchoolEats.Common
{
	public static class ValidationConstants
	{
		public static class Dish
		{
			public const int NameMaxLength = 30;
			public const int NameMinLength = 5;
			public const int DescriptionMaxLength = 200;
			public const int DescriptionMinLength = 35;
			public const int ImageUrlMaxLength = 1000;
			public const decimal PriceMinLength = 0.10m;
			public const decimal PriceMaxLength = 100;
		}

		public static class Category
		{
			public const int NameMaxLength = 50;
		}
	}
}
