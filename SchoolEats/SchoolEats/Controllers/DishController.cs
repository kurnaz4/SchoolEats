namespace SchoolEats.Controllers
{
	using Services.Data.Interfaces;
	using Microsoft.AspNetCore.Mvc;
	using Web.Infrastructure.Extensions;
	using Web.Infrastructure.Files;
	using Web.Infrastructure.ImagesCloud;
	using Web.ViewModels.Dish;

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
	        if (!ModelState.IsValid)
	        {
		        return View(model);
            }

	        model.UserId = this.User.GetId()!;

	        try
	        {
		        string fileName = model.ProductImage.FileName;
		        model.ImagePath = fileName;
		        CreateFile.CreateImageFile(model);
		        string fullPath = Path.GetFullPath(fileName);
		        await cloudinarySetUp.UploadAsync(fullPath);
		        System.IO.File.Delete(fullPath);
		        var correctImageUrl = cloudinarySetUp.GenerateImageUrl(fileName);
		        model.ImagePath = correctImageUrl;

		        //we should add categories first if we want to work this method
		        await this.dishService.AddDishAsync(model);
	        }
	        catch (Exception e)
	        {
		        //TempData[WarningMessage] = Un exception occured!
		        return View(model);
	        }

			return RedirectToAction("Index", "Home");
        }
    }
}