using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.CertificateIdentity.Configuration;
using RegionOrebroLan.CertificateIdentity.Data.Configuration;

namespace RegionOrebroLan.CertificateIdentity.Data
{
	public abstract class DatabaseProviderBase<TOperational> : IDatabaseProvider where TOperational : DbContext, IPersistedGrantDbContext
	{
		#region Methods

		public virtual void Add(IConfiguration configuration, DataOptions dataOptions, IHostEnvironment hostEnvironment, IServiceCollection services)
		{
			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if(dataOptions == null)
				throw new ArgumentNullException(nameof(dataOptions));

			if(hostEnvironment == null)
				throw new ArgumentNullException(nameof(hostEnvironment));

			if(services == null)
				throw new ArgumentNullException(nameof(services));

			var identityServerBuilder = this.CreateIdentityServerBuilder(services);

			var connectionString = configuration.GetConnectionString(dataOptions.ConnectionStringName);

			connectionString = this.ResolveConnectionString(connectionString, hostEnvironment);

			identityServerBuilder.AddOperationalStore<TOperational>(options =>
			{
				this.BuildOptions(connectionString, options);

				configuration.GetSection(ConfigurationKeys.OperationalStorePath).Bind(options);
			});
		}

		protected internal abstract void BuildOptions(string connectionString, OperationalStoreOptions options);

		protected internal virtual IIdentityServerBuilder CreateIdentityServerBuilder(IServiceCollection services)
		{
			return new IdentityServerBuilder(services);
		}

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
				((DbContext)serviceScope.ServiceProvider.GetRequiredService<IPersistedGrantDbContext>()).Database.Migrate();
			}

			using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				try
				{
					var operationalContext = serviceScope.ServiceProvider.GetRequiredService<IPersistedGrantDbContext>();

					// Just to trigger an exception if there were no migrations.
					var _ = operationalContext.ServerSideSessions.Count();
				}
				catch(Exception exception)
				{
					throw new InvalidOperationException("Error when testing the database. There are no tables. Probably migrations are missing.", exception);
				}
			}
			// ReSharper restore ConvertToUsingDeclaration
		}

		#endregion
	}
}