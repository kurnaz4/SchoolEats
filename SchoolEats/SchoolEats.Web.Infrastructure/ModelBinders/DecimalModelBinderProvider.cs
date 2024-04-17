namespace SchoolEats.Web.Infrastructure.ModelBinders
{
	using Microsoft.AspNetCore.Mvc.ModelBinding;

	public class DecimalModelBinderProvider : IModelBinderProvider
	{
		//чрез провайдар използваме DecimalModelBinder
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			if (context.Metadata.ModelType == typeof(decimal) ||
			    context.Metadata.ModelType == typeof(decimal?))
			{
				return new DecimalModelBinder();
			}

			return null!;
		}
	}
}
