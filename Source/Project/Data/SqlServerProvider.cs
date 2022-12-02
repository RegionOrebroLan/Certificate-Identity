using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace RegionOrebroLan.CertificateIdentity.Data
{
	public class SqlServerProvider : DatabaseProviderBase<SqlServerOperational>
	{
		#region Fields

		private const string _localDatabasePrefix = "(LocalDb)";

		#endregion

		#region Properties

		protected internal virtual string LocalDatabasePrefix => _localDatabasePrefix;

		#endregion

		#region Methods

		protected internal override void BuildOptions(string connectionString, OperationalStoreOptions options)
		{
			options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
		}

		protected internal virtual bool IsLocalDatabaseConnectionString(SqlConnectionStringBuilder sqlConnectionStringBuilder)
		{
			if(sqlConnectionStringBuilder == null)
				throw new ArgumentNullException(nameof(sqlConnectionStringBuilder));

			return sqlConnectionStringBuilder.DataSource.StartsWith(this.LocalDatabasePrefix, StringComparison.OrdinalIgnoreCase);
		}

		protected internal override string ResolveConnectionString(string connectionString, IHostEnvironment hostEnvironment)
		{
			if(hostEnvironment == null)
				throw new ArgumentNullException(nameof(hostEnvironment));

			var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

			if(!this.IsLocalDatabaseConnectionString(sqlConnectionStringBuilder))
				return connectionString;

			if(!this.ResolveConnectionStringBuilder(hostEnvironment, sqlConnectionStringBuilder))
				return connectionString;

			return sqlConnectionStringBuilder.ConnectionString;
		}

		protected internal virtual bool ResolveConnectionStringBuilder(IHostEnvironment hostEnvironment, SqlConnectionStringBuilder sqlConnectionStringBuilder)
		{
			if(hostEnvironment == null)
				throw new ArgumentNullException(nameof(hostEnvironment));

			if(sqlConnectionStringBuilder == null)
				throw new ArgumentNullException(nameof(sqlConnectionStringBuilder));

			var attachDbFilename = sqlConnectionStringBuilder.AttachDBFilename;
			var fullAttachDbFilename = Path.GetFullPath(attachDbFilename, hostEnvironment.ContentRootPath);

			if(fullAttachDbFilename.Length == attachDbFilename.Length)
				return false;

			sqlConnectionStringBuilder.AttachDBFilename = fullAttachDbFilename;

			if(string.IsNullOrEmpty(sqlConnectionStringBuilder.InitialCatalog))
				sqlConnectionStringBuilder.InitialCatalog = fullAttachDbFilename;

			return true;
		}

		#endregion
	}
}