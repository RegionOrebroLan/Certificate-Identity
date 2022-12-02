using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace RegionOrebroLan.CertificateIdentity.Data
{
	public class SqliteProvider : DatabaseProviderBase<SqliteOperational>
	{
		#region Methods

		protected internal override void BuildOptions(string connectionString, OperationalStoreOptions options)
		{
			options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
		}

		#endregion
	}
}