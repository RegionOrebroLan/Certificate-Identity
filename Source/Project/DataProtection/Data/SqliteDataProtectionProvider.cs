using Microsoft.EntityFrameworkCore;

namespace RegionOrebroLan.CertificateIdentity.DataProtection.Data
{
	public class SqliteDataProtectionProvider : DataProtectionProviderBase<SqliteDataProtection>
	{
		#region Methods

		protected internal override void BuildOptions(string connectionString, DbContextOptionsBuilder options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.UseSqlite(connectionString);
		}

		#endregion
	}
}