using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.CertificateIdentity.Configuration;

namespace RegionOrebroLan.CertificateIdentity.DataProtection.Data
{
	public abstract class DataProtectionProviderBase<TContext> : IDataProtectionProvider where TContext : DbContext, IDataProtectionKeyContext
	{
		#region Methods

		public virtual void Add(IConfiguration configuration, ConnectionOptions connectionOptions, IHostEnvironment hostEnvironment, IServiceCollection services)
		{
			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if(connectionOptions == null)
				throw new ArgumentNullException(nameof(connectionOptions));

			if(hostEnvironment == null)
				throw new ArgumentNullException(nameof(hostEnvironment));

			if(services == null)
				throw new ArgumentNullException(nameof(services));

			var connectionString = configuration.GetConnectionString(connectionOptions.ConnectionStringName);

			connectionString = this.ResolveConnectionString(connectionString, hostEnvironment);

			services.AddDbContext<IDataProtectionKeyContext, TContext>(options => { this.BuildOptions(connectionString, options); });

			var configurationSection = configuration.GetSection(ConfigurationKeys.DataProtectionPath);

			services.AddDataProtection(configurationSection.Bind).PersistKeysToDbContext<TContext>();
		}

		protected internal abstract void BuildOptions(string connectionString, DbContextOptionsBuilder options);

		protected internal virtual string ResolveConnectionString(string connectionString, IHostEnvironment hostEnvironment)
		{
			return connectionString;
		}

		public virtual void Use(IApplicationBuilder applicationBuilder)
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			// ReSharper disable ConvertToUsingDeclaration
			using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				((DbContext)serviceScope.ServiceProvider.GetRequiredService<IDataProtectionKeyContext>()).Database.Migrate();
			}

			using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				try
				{
					var context = serviceScope.ServiceProvider.GetRequiredService<IDataProtectionKeyContext>();

					// Just to trigger an exception if there were no migrations.
					var _ = context.DataProtectionKeys.Count();
				}
				catch(Exception exception)
				{
					throw new InvalidOperationException("Error when querying the database. Probably migrations are missing.", exception);
				}
			}
			// ReSharper restore ConvertToUsingDeclaration
		}

		#endregion
	}
}