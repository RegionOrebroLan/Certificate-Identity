using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.CertificateIdentity.Configuration;

namespace RegionOrebroLan.CertificateIdentity.DataProtection
{
	public class DefaultDataProtectionProvider : IDataProtectionProvider
	{
		#region Methods

		public virtual void Add(IConfiguration configuration, ConnectionOptions connectionOptions, IHostEnvironment hostEnvironment, IServiceCollection services)
		{
			// We do nothing. The default data-protection will be used.
		}

		public virtual void Use(IApplicationBuilder applicationBuilder)
		{
			// We do nothing. The default data-protection will be used.
		}

		#endregion
	}
}