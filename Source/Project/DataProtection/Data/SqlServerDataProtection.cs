using Microsoft.EntityFrameworkCore;

namespace RegionOrebroLan.CertificateIdentity.DataProtection.Data
{
	public class SqlServerDataProtection : DataProtectionContext<SqlServerDataProtection>
	{
		#region Constructors

		public SqlServerDataProtection(DbContextOptions<SqlServerDataProtection> options) : base(options) { }

		#endregion
	}
}