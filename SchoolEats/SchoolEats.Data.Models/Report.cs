namespace SchoolEats.Data.Models
{
	using System.ComponentModel.DataAnnotations;

	public class Report
	{
		[Key]
		public Guid Id { get; set; }

		public DateTime Time { get; set; }

		public decimal TotalPrice { get; set; }

		public int TotalQuantity { get; set; }
	}
}
