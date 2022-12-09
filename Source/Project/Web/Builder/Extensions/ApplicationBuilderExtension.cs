using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.CertificateIdentity.Data;
using RegionOrebroLan.CertificateIdentity.DataProtection;
using RegionOrebroLan.CertificateIdentity.Web.Http;

namespace RegionOrebroLan.CertificateIdentity.Web.Builder.Extensions
{
	public static class ApplicationBuilderExtension
	{
		#region Methods

		public static IApplicationBuilder UseDataProtection(this IApplicationBuilder applicationBuilder)
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			applicationBuilder.ApplicationServices.GetRequiredService<IDataProtectionProvider>().Use(applicationBuilder);

			return applicationBuilder;
		}

		public static IApplicationBuilder UseIdentityServer(this IApplicationBuilder applicationBuilder)
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			applicationBuilder.ApplicationServices.GetRequiredService<IDatabaseProvider>().Use(applicationBuilder);

			applicationBuilder.UseIdentityServer(null);

			return applicationBuilder;
		}

		public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder applicationBuilder)
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			applicationBuilder.UseMiddleware<SecurityHeadersMiddleware>();

			return applicationBuilder;
		}

		#endregion
	}
}