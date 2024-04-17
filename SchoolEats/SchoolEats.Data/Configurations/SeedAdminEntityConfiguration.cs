namespace SchoolEats.Data.Configurations
{
	using Microsoft.EntityFrameworkCore;
	using SchoolEats.Data.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using static Common.GeneralApplicationConstants;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class SeedAdminEntityConfiguration : IEntityTypeConfiguration<SchoolEatsUser>
	{
		public void Configure(EntityTypeBuilder<SchoolEatsUser> builder)
		{
			builder.HasData(SeedAdmin());
		}

		private SchoolEatsUser SeedAdmin()
		{
			var user = new SchoolEatsUser()
			{
				Id = Guid.Parse("809EC051-8E26-4200-9248-9C4815FF3AF2"),
				Email = DevelopmentAdminEmail,
				IsApproved = true,
				AccessFailedCount = 0,
				LockoutEnabled = true,
				TwoFactorEnabled = false,
				PhoneNumber = null,
				PhoneNumberConfirmed = false,
				LockoutEnd = null,
				ConcurrencyStamp = "4eff5749-8f73-470d-b2b7-fb3c6fd7e6f0",
				SecurityStamp = "6SMIWUAQOBJRLP7BOERO2MQNLDTVGMPA",
				PasswordHash = "AQAAAAEAACcQAAAAEElb00msN19oCULHAmkvl/UaLyHRRVFXXg8PH7/fWIkzXaH2/vaFqyfpIxqiXcRcXg==",
				EmailConfirmed = false,
				NormalizedEmail = "ADMINISTRATOR@GMAIL.COM",
				NormalizedUserName = "АДМИНАДМИНОВ",
				UserName = "АдминАдминов",
			};
			return user;
		}
	}
}
