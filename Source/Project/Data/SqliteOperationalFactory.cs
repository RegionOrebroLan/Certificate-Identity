using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RegionOrebroLan.CertificateIdentity.Data
{
	public class SqliteOperationalFactory : IDesignTimeDbContextFactory<SqliteOperational>
	{
		#region Methods

		public virtual SqliteOperational CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<SqliteOperational>();
			optionsBuilder.UseSqlite("A value that can not be empty just to be able to create/update migrations.");

			return new SqliteOperational(optionsBuilder.Options)
			{
				StoreOptions = new OperationalStoreOptions()
			};
		}

		#endregion
	}
}