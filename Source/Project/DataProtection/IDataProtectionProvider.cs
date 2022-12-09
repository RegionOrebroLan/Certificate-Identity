using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.CertificateIdentity.Configuration;

namespace RegionOrebroLan.CertificateIdentity.DataProtection
{
	public interface IDataProtectionProvider
	{
		#region Methods

		void Add(IConfiguration configuration, ConnectionOptions connectionOptions, IHostEnvironment hostEnvironment, IServiceCollection services);
		void Use(IApplicationBuilder applicationBuilder);

		#endregion
	}
}