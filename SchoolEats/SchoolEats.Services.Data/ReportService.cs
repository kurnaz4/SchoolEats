namespace SchoolEats.Services.Data
{
	using Interfaces;
	using Microsoft.EntityFrameworkCore;
	using SchoolEats.Data;
	using Web.ViewModels.SuperUser;

	public class ReportService : IReportService
	{
		private readonly SchoolEatsDbContext dbContext;

		public ReportService(SchoolEatsDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<List<AllReportsViewModel>> GetAllReportsAsync()
		{
			var all = await this.dbContext
				.Reports
				.Select(x => new AllReportsViewModel()
				{
					TotalPrice = x.TotalPrice,
					TotalQuantity = x.TotalQuantity,
					Time = x.Time
				})
				.ToListAsync();

			return all;
		}
	}
}
