namespace SchoolEats.Web.Infrastructure.Files
{
	using ViewModels.Dish;

	public class CreateFile
	{
		//създава копие на снимка в момента
		public static void CreateImageFile(DishFormViewModel model)//here view Model-a
		{
			string fileName = model.ImagePath == null ? model.ProductImage.FileName : model.ImagePath;

			string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
			using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
			{
				model.ProductImage.CopyTo(fileStream);
			}
		}
	}
}
