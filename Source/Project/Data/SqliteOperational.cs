using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace RegionOrebroLan.CertificateIdentity.Data
{
	public class SqliteOperational : PersistedGrantDbContext<SqliteOperational>
	{
		#region Constructors

		public SqliteOperational(DbContextOptions<SqliteOperational> options) : base(options) { }

		#endregion
	}
}