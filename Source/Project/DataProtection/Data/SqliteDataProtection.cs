using Microsoft.EntityFrameworkCore;

namespace RegionOrebroLan.CertificateIdentity.DataProtection.Data
{
	public class SqliteDataProtection : DataProtectionContext<SqliteDataProtection>
	{
		#region Constructors

		public SqliteDataProtection(DbContextOptions<SqliteDataProtection> options) : base(options) { }

		#endregion
	}
}