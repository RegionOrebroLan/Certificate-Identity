using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.CertificateIdentity.Data.SqlClient.Extensions;

namespace RegionOrebroLan.CertificateIdentity.Data
{
	public class SqlServerProvider : DatabaseProviderBase<SqlServerOperational>
	{
		#region Methods

		protected internal override void BuildOptions(string connectionString, OperationalStoreOptions options)
		{
			options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
		}

		protected internal override string ResolveConnectionString(string connectionString, IHostEnvironment hostEnvironment)
		{
			return SqlConnectionStringBuilderExtension.ResolveConnectionString(connectionString, hostEnvironment);
		}

		#endregion
	}
}