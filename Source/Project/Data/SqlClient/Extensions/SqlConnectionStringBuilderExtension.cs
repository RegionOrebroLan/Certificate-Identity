using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;

namespace RegionOrebroLan.CertificateIdentity.Data.SqlClient.Extensions
{
	public static class SqlConnectionStringBuilderExtension
	{
		#region Fields

		public const string LocalDatabasePrefix = "(LocalDb)";

		#endregion

		#region Methods

		public static bool IsLocalDatabaseConnectionString(this SqlConnectionStringBuilder sqlConnectionStringBuilder)
		{
			if(sqlConnectionStringBuilder == null)
				throw new ArgumentNullException(nameof(sqlConnectionStringBuilder));

			return sqlConnectionStringBuilder.DataSource.StartsWith(LocalDatabasePrefix, StringComparison.OrdinalIgnoreCase);
		}

		public static bool Resolve(this SqlConnectionStringBuilder sqlConnectionStringBuilder, IHostEnvironment hostEnvironment)
		{
			if(sqlConnectionStringBuilder == null)
				throw new ArgumentNullException(nameof(sqlConnectionStringBuilder));

			if(hostEnvironment == null)
				throw new ArgumentNullException(nameof(hostEnvironment));

			if(!sqlConnectionStringBuilder.IsLocalDatabaseConnectionString())
				return false;

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