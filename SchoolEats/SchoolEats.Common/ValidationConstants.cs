namespace SchoolEats.Common
{
	public static class ValidationConstants
	{
        public static class User
        {
            public const int NameMaxLength = 30;
            public const int NameMinLength = 2;
        }
        public static class Dish
		{
			public const int NameMaxLength = 30;
			public const int NameMinLength = 2;
			public const int DescriptionMaxLength = 200;
			public const int DescriptionMinLength = 10;
			public const int ImageUrlMaxLength = 1000;
			public const decimal PriceMinLength = 0.5m;
			public const decimal PriceMaxLength = 100;
			public const int QuantityMaxLength = 1000;
			public const int QuantityMinLength = 1;
			public const int GramsMaxLength = 5000;
			public const int GramsMinLength = 1;

		}

		public static class Category
		{
			public const int NameMaxLength = 50;
		}
	}
}
