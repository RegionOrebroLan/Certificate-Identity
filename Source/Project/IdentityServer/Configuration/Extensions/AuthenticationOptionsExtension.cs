using Duende.IdentityServer.Configuration;
using Microsoft.AspNetCore.Http;

namespace RegionOrebroLan.CertificateIdentity.IdentityServer.Configuration.Extensions
{
	public static class AuthenticationOptionsExtension
	{
		#region Methods

		public static void SetDefaults(this AuthenticationOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.CheckSessionCookieSameSiteMode = SameSiteMode.Strict;
			options.CookieLifetime = TimeSpan.FromSeconds(15);
			options.CookieSameSiteMode = SameSiteMode.Lax;
			options.CoordinateClientLifetimesWithUserSession = true;
		}

		#endregion
	}
}