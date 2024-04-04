namespace SchoolEats.Services.Data
{
	using Interfaces;
	using Microsoft.EntityFrameworkCore;
	using SchoolEats.Data;
	using SchoolEats.Data.Models;
	using Web.ViewModels.Dish;
	using Web.ViewModels.Purchase;
	using Web.ViewModels.SuperUser;

	public class PurchaseService : IPurchaseService
	{
		private readonly SchoolEatsDbContext dbContext;
		public PurchaseService(SchoolEatsDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<List<AllPurchasesViewModel>> GetAllPurchasesByUserIdAsync(Guid userId)//For history
		{
			return await this.dbContext
				.Purchases
				.Where(x => x.BuyerId == userId)
				.Select(p => new AllPurchasesViewModel()
				{
					Id = p.Id,
					Name = p.Dish.Name,
					Description = p.Dish.Description,
					Grams = p.Dish.Grams,
					ImageUrl = p.Dish.ImageUrl,
					IsAllergenic = p.Dish.IsAllergenic,
					Price = p.Dish.Price,
					Quantity = p.Dish.Quantity,
					Category = p.Dish.Category.Name,
					Owner = p.Dish.User.UserName,
					PurchasedOn = p.PurchasedOn,
					Code = p.Code,
					PurchaseQuantity = p.PurchasedQuantity
				})
				.ToListAsync();
		}

		public async Task PurchaseDishAsync(Guid dishId, Guid userId, int purchasedQuantity, string code = "с карта")
		{
			var dish = await this.dbContext
				.Dishes
				.FindAsync(dishId);

			var purchase = new Purchase()
			{
				DishId = dishId,
				BuyerId = userId,
				Code = code,
				PurchasedQuantity = purchasedQuantity,
				PurchasedOn = DateTime.Now
			};

			await this.dbContext.Purchases.AddAsync(purchase);
			await this.dbContext.SaveChangesAsync();
		}

		public string GenerateRandomPurchaseCode()
		{
			string code = Guid.NewGuid().ToString("N").Substring(0, 6);
			return code;
		}

		public async Task<List<AllPurchaseForSuperUserViewModel>> GetAllPurchases()
		{
			var all = await this.dbContext
				.Purchases
				.Where(p => !p.IsCompleted)
				.Select(p => new AllPurchaseForSuperUserViewModel()
				{
					PurchaseId = p.Id,
					PurchasedOn = p.PurchasedOn,
					BuyerName = "",
					BuyerEmail = "",
					DishCategory = p.Dish.Category.Name,
					DishGrams = p.Dish.Grams,
					DishImageUrl = p.Dish.ImageUrl,
					DishName = p.Dish.Name,
					DishPrice = p.Dish.Price,
					PurchasedQuantity = p.PurchasedQuantity,
					Code = p.Code,
					BuyerId = p.BuyerId,
				})
				.ToListAsync();
			
			return all;
		}

		public async Task CompletePurchaseAsync(List<Purchase> purchases)
		{
			foreach (var purchase in purchases)
			{
				purchase.IsCompleted = true;
				if (purchase.Code != "с карта")
				{
					purchase.PurchasedOn = DateTime.Now;
				}
			}

			await this.dbContext.SaveChangesAsync();
		}

		public async Task<Purchase> GetPurchaseByPurchaseId(Guid purchaseId)
		{
			var purchase = await this.dbContext
				.Purchases
				.FindAsync(purchaseId);

			return purchase;
		}

		public async Task<List<Purchase>> GetPurchasesByPurchaseCodeAndBuyerIdAsync(string code, Guid buyerId)
		{
			var all = await this.dbContext
				.Purchases
				.Where(x => x.Code == code && x.BuyerId == buyerId)
				.ToListAsync();

			return all;
		}

		public async Task<decimal> GetPriceSumOfPurchaseByCodeAndBuyerIdAsync(string code, Guid buyerId)
		{
			if (code == "с карта")
			{
				return 0;
			}
			var all = await this.dbContext
				.Purchases
				.Where(x => x.Code == code && x.BuyerId == buyerId)
				.Select(x => new AllDishesViewModel()
				{
					Price = x.Dish.Price,
					Quantity = x.PurchasedQuantity
				})
				.ToListAsync();

			return all.Sum(x => x.Price * x.Quantity);
		}

		public async Task<DailyReportViewModel> GetDailyReportAsync()
		{
			var all = await this.dbContext
				.Purchases
				.Where(x => x.IsCompleted && x.PurchasedOn.Date == DateTime.Today)
				.Select(x => new AllDishesViewModel()
				{
					Quantity = x.PurchasedQuantity,
					Price = x.Dish.Price
				})
				.ToListAsync();

			return new DailyReportViewModel()
			{
				TotalPrice = all.Sum(x => x.Price * x.Quantity),
				TotalQuantity = all.Sum(x => x.Quantity)
			};
		}


		public async Task SendDailyReportAsync(DailyReportViewModel report)
		{
			var reportToAdd = new Report()
			{
				TotalPrice = report.TotalPrice,
				TotalQuantity = report.TotalQuantity,
				Time = DateTime.Now
			};

			await this.dbContext.Reports.AddAsync(reportToAdd);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task DeleteAllPurchasesByDateTimeAsync(DateTime date)
		{
			var all = await this.dbContext.Purchases
				.Where(x => x.PurchasedOn.Date == date.Date && !x.IsCompleted)
				.ToListAsync();

			this.dbContext.Purchases.RemoveRange(all);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<bool> IsReportAlreadySend(DateTime time)
		{
			var report = await this.dbContext
				.Reports
				.FirstOrDefaultAsync(x => x.Time.Date == time.Date);
			if (report == null)
			{
				return false;
			}

			return true;
		}
	}
}
