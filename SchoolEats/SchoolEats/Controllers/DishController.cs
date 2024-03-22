namespace SchoolEats.Controllers
{
	using Services.Data.Interfaces;
	using Microsoft.AspNetCore.Mvc;



	public class DishController : Controller
    {
        private readonly IDishService dishService;
	    public DishController(IDishService dishService)
	    {
		    this.dishService = dishService;
	    }
        public async Task<IActionResult> All()
        {
	        var dishes = await this.dishService.GetAllDishesAsync();
            return View(dishes);
        }
    }
}