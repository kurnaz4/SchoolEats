namespace SchoolEats.Services.Data.Interfaces
{
	using Web.ViewModels.SuperUser;

	public interface IReportService
	{
		Task<List<AllReportsViewModel>> GetAllReportsAsync();
	}
}
