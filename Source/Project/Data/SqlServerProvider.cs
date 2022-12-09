using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Data.SqlClient;
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
			if(hostEnvironment == null)
				throw new ArgumentNullException(nameof(hostEnvironment));

			SqlConnectionStringBuilder sqlConnectionStringBuilder;

			try
			{
				sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not create a sql-connection-string-builder from connection-string.", exception);
			}

			if(!sqlConnectionStringBuilder.Resolve(hostEnvironment))
				return connectionString;

			return sqlConnectionStringBuilder.ConnectionString;
		}

		#endregion
	}
}