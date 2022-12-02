using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.CertificateIdentity.Data.Configuration;

namespace RegionOrebroLan.CertificateIdentity.Data
{
	public interface IDatabaseProvider
	{
		#region Methods

		void Add(IConfiguration configuration, DataOptions dataOptions, IHostEnvironment hostEnvironment, IServiceCollection services);
		void Use(IApplicationBuilder applicationBuilder);

		#endregion
	}
}