namespace SchoolEats.Services.Data.Interfaces
{
	using Web.ViewModels.Purchase;

	public interface IPurchaseService
	{
		Task<List<AllPurchasesViewModel>> GetAllPurchasesByUserIdAsync(Guid userId);

		Task PurchaseDishAsync(Guid dishId, Guid userId,int purchasedQuantity, string code = "с карта");

		string GenerateRandomPurchaseCode();
	}
}
