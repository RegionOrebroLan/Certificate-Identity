using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace RegionOrebroLan.CertificateIdentity.Data
{
	public class SqlServerOperational : PersistedGrantDbContext<SqlServerOperational>
	{
		#region Constructors

		public SqlServerOperational(DbContextOptions<SqlServerOperational> options) : base(options) { }

		#endregion
	}
}