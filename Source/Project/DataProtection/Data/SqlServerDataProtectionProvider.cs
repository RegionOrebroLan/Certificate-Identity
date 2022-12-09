using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.CertificateIdentity.Data.SqlClient.Extensions;

namespace RegionOrebroLan.CertificateIdentity.DataProtection.Data
{
	public class SqlServerDataProtectionProvider : DataProtectionProviderBase<SqlServerDataProtection>
	{
		#region Methods

		protected internal override void BuildOptions(string connectionString, DbContextOptionsBuilder options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.UseSqlServer(connectionString);
		}

		protected internal override string ResolveConnectionString(string connectionString, IHostEnvironment hostEnvironment)
		{
			return SqlConnectionStringBuilderExtension.ResolveConnectionString(connectionString, hostEnvironment);
		}

		#endregion
	}
}