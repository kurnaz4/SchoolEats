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
        private readonly IDishService dishService;
        private readonly CloudinarySetUp cloudinarySetUp;
	    public DishController(IDishService dishService)
	    {
		    this.dishService = dishService;
		    this.cloudinarySetUp = new CloudinarySetUp();
	    }
        public async Task<IActionResult> All()
        {
	        var dishes = await this.dishService.GetAllDishesAsync();
            return View(dishes);
        }
        [HttpGet]
        public IActionResult Add()
        {
            AddDishViewModel model = new AddDishViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddDishViewModel model)
        {
	        ModelState.Remove(nameof(model.ImagePath));
	        if (!ModelState.IsValid)
	        {
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
		        TempData[ErrorMessage] = "Unexpected exception occurred!";
		        return View(model);
	        }
	        finally
	        {
		        if (isFileCreated)
		        {
					System.IO.File.Delete(fullPath);
		        }
	        }

			return RedirectToAction("All", "Dish");
        }
    }
}