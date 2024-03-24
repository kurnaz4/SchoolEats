namespace SchoolEats.Web.Infrastructure.Extensions
{
	using System.Security.Claims;
	public static class ClaimsPrincipalExtensions
	{
		public static Guid GetId(this ClaimsPrincipal user)
		{
			return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
		}
		public static string? GetEmail(this ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.Email);
		}
		public static bool IsAdmin(this ClaimsPrincipal user)
		{
			return user.IsInRole("AdminRoleName");//TODO:replace with admin roleName
		}
	}
}
