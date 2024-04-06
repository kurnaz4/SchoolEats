namespace SchoolEats.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Services.Data.Interfaces;
	using Microsoft.AspNetCore.Mvc;
	using Web.Infrastructure.Extensions;
	using Web.Infrastructure.Files;
	using Web.Infrastructure.ImagesCloud;
	using Web.ViewModels.Dish;
	using static Common.NotificationMessagesConstants;
	using static Common.ErrorMessages;
	using static Common.GeneralApplicationConstants;

	[Authorize(Roles = "User,Administrator,SuperUser")]
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
		[Authorize(Roles = SuperUserRoleName)]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            DishFormViewModel model = new DishFormViewModel();
            model.Categories = await this.categoryService.AllCategoriesAsync();
            return View(model);
        }
        [Authorize(Roles = SuperUserRoleName)]
		[HttpPost]
        public async Task<IActionResult> Add(DishFormViewModel model)
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
		        
		        await this.dishService.AddDishAsync(model);
	        }
	        catch (Exception e)
	        {
		        TempData[ErrorMessage] = CommonErrorMessage;
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

        [Authorize(Roles = SuperUserRoleName)] 
		[HttpGet]
        public async Task<IActionResult> Edit(Guid dishId)
        {
			var model = await this.dishService.GetDishForEditAsync(dishId);
			model.Categories = await this.categoryService.AllCategoriesAsync();

			return View(model);
        }

        [Authorize(Roles = SuperUserRoleName)]
		[HttpPost]
        public async Task<IActionResult> Edit(Guid id, DishFormViewModel model)
        {
	        ModelState.Remove(nameof(model.ImagePath));
	        ModelState.Remove(nameof(model.ProductImage));
	        if (!ModelState.IsValid)
	        {
		        model.Categories = await this.categoryService.AllCategoriesAsync();
				return View(model);
	        }

	        model.Id = id;
	        model.UserId = this.User.GetId();
			string fullPath = String.Empty;
			bool isFileCreated = false;
			try
		    { 
			    if (model.ProductImage == null)
		        {
			        var item = await this.dishService.GetDishForDetailsByDishIdAsync(model.Id);
			        model.ImagePath = item.ImageUrl;
		        }
		        else
		        {
					string fileName = model.ProductImage.FileName;
					model.ImagePath = fileName;
					CreateFile.CreateImageFile(model);
					isFileCreated = true;
					fullPath = Path.GetFullPath(fileName);
					await cloudinarySetUp.UploadAsync(fullPath);
					var correctImageUrl = cloudinarySetUp.GenerateImageUrl(fileName);
					model.ImagePath = correctImageUrl;
				}

				await this.dishService.EditDishAsync(model);

				TempData[SuccessMessage] = "Вашият продукт е успешно редактиран!";
	        }
	        catch (Exception e)
	        {
		        TempData[ErrorMessage] = CommonErrorMessage;
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

        [Authorize(Roles = SuperUserRoleName)]
		[HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
	        try
	        {
				await this.dishService.DeleteDishAsync(id);

				TempData[SuccessMessage] = "Вие успешно премахнахте този продукт!";
	        }
	        catch (Exception e)
	        {
				TempData[ErrorMessage] = CommonErrorMessage;
	        }

	        return RedirectToAction("All", "Dish");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateDish(Guid id)
        {
	        try
	        {
				await this.dishService.MakeDishActiveAsync(id);
				TempData[SuccessMessage] = "Успешно активирахте това ястие!";
				TempData[InformationMessage] = "Имайте предвит, че ястието е видно за всички потребители!";
	        }
	        catch (Exception e)
	        {
				TempData[ErrorMessage] = CommonErrorMessage;
	        }

	        if (this.User.IsInRole(SuperUserRoleName))
	        {
				return RedirectToAction("AllDishes", "SuperUser");
	        }

			return RedirectToAction("AllDishes", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateDish(Guid id)
        {
	        try
	        {
		        await this.dishService.DeleteDishAsync(id);
		        TempData[SuccessMessage] = "Успешно деактивирахте това ястие!";
		        TempData[InformationMessage] = "Имайте предвит, че ястието вече не е видно за потребителите!";
	        }
	        catch (Exception e)
	        {
		        TempData[ErrorMessage] = CommonErrorMessage;
	        }

			if (this.User.IsInRole(SuperUserRoleName))
			{
				return RedirectToAction("AllDishes", "SuperUser");
			}

			return RedirectToAction("AllDishes", "Admin");
		}
        [HttpPost]
        public async Task<IActionResult> HardDelete(Guid id)
        {
	        try
	        {
		        await this.dishService.HardDeleteDishAsync(id);
		        TempData[SuccessMessage] = "Успешно изтрихте това ястие!";
		        TempData[InformationMessage] = "Имайте предвит, че ястието вече не съществува в системата!";
	        }
	        catch (Exception e)
	        {
		        TempData[ErrorMessage] = CommonErrorMessage;
	        }

			if (this.User.IsInRole(SuperUserRoleName))
			{
				return RedirectToAction("AllDishes", "SuperUser");
			}

			return RedirectToAction("AllDishes", "Admin");
		}
	}
}