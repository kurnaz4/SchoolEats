namespace SchoolEats.Controllers
{
	using Services.Data.Interfaces;
	using Microsoft.AspNetCore.Mvc;
	using Web.Infrastructure.Extensions;
	using Web.Infrastructure.Files;
	using Web.Infrastructure.ImagesCloud;
	using Web.ViewModels.Dish;
	using static Common.NotificationMessagesConstants;
	public class DishController : Controller
    {
	    private readonly CloudinarySetUp cloudinarySetUp;

        private readonly IDishService dishService;
		private readonly ICategoryService categoryService;
        
	    public DishController(IDishService dishService, ICategoryService categoryService)
	    {
		    this.cloudinarySetUp = new CloudinarySetUp();
		    this.dishService = dishService;
			this.categoryService = categoryService;
	    }
        public async Task<IActionResult> All()
        {
	        var dishes = await this.dishService.GetAllDishesAsync();
            return View(dishes);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddDishViewModel model = new AddDishViewModel();
            model.Categories = await this.categoryService.AllCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddDishViewModel model)
        {
	        ModelState.Remove(nameof(model.ImagePath));
	        if (!ModelState.IsValid)
	        {
		         model.Categories = await this.categoryService.AllCategoriesAsync();
		         return View(model);
            }

	        string fullPath = String.Empty;
	        bool isFileCreated = false;
	        try
	        {
		        model.UserId = this.User.GetId()!;
				string fileName = model.ProductImage.FileName;
		        model.ImagePath = fileName;
		        CreateFile.CreateImageFile(model);
		        isFileCreated = true;
		        fullPath = Path.GetFullPath(fileName);
		        await cloudinarySetUp.UploadAsync(fullPath);
		        var correctImageUrl = cloudinarySetUp.GenerateImageUrl(fileName);
		        model.ImagePath = correctImageUrl;
		        //we should add categories first if we want this method to work
		        await this.dishService.AddDishAsync(model);
	        }
	        catch (Exception e)
	        {
		        TempData[ErrorMessage] = "Възникна неочаквана грешка";
		        return View(model);
	        }
	        finally
	        {
		        if (isFileCreated)
		        {
					System.IO.File.Delete(fullPath);
		        }
	        }
	        TempData[SuccessMessage] = "Успешно добавихте нов продукт!";
			return RedirectToAction("All", "Dish");
        }
    }
}