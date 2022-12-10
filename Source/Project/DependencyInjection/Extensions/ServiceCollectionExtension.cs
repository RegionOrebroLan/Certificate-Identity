using Duende.IdentityServer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RegionOrebroLan.CertificateIdentity.Configuration;
using RegionOrebroLan.CertificateIdentity.Data;
using RegionOrebroLan.CertificateIdentity.Data.Configuration;
using RegionOrebroLan.CertificateIdentity.DataProtection;
using RegionOrebroLan.CertificateIdentity.DataProtection.Configuration;
using RegionOrebroLan.CertificateIdentity.Extensions;
using RegionOrebroLan.CertificateIdentity.IdentityServer.Configuration.Extensions;
using RegionOrebroLan.CertificateIdentity.Kestrel.Configuration;
using RegionOrebroLan.CertificateIdentity.Kestrel.Https;
using RegionOrebroLan.CertificateIdentity.Web.Authentication.Certificate;
using RegionOrebroLan.CertificateIdentity.Web.Authentication.Certificate.Configuration;

namespace RegionOrebroLan.CertificateIdentity.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddCertificateAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.Configure<CertificateAuthenticationEventsOptions>(configuration.GetSection(ConfigurationKeys.CertificateAuthenticationEventsPath));
			services.TryAddTransient<CertificateAuthenticationEvents>();

			return services;
		}

		public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if(webHostEnvironment == null)
				throw new ArgumentNullException(nameof(webHostEnvironment));

			var dataProtectionDependencyInjectionOptions = new DataProtectionDependencyInjectionOptions();
			configuration.GetSection(ConfigurationKeys.DataProtectionPath).Bind(dataProtectionDependencyInjectionOptions);

			IDataProtectionProvider dataProtectionProvider;

			if(dataProtectionDependencyInjectionOptions.ProviderType == null)
			{
				dataProtectionProvider = new DefaultDataProtectionProvider();
			}
			else
			{
				dataProtectionProvider = (IDataProtectionProvider)Activator.CreateInstance(dataProtectionDependencyInjectionOptions.ProviderType);

				if(dataProtectionProvider == null)
					throw new InvalidOperationException("The data-protection-provider is null.");
			}

			services.TryAddSingleton(dataProtectionProvider);

			dataProtectionProvider.Add(configuration, dataProtectionDependencyInjectionOptions, webHostEnvironment, services);

			return services;
		}

		public static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if(webHostEnvironment == null)
				throw new ArgumentNullException(nameof(webHostEnvironment));

			var identityServerBuilder = services.AddIdentityServer(options =>
				{
					options.SetDefaults();

					configuration.GetSection(ConfigurationKeys.IdentityServerPath).Bind(options);
				})
				.AddInMemoryClients(configuration.GetSection(ConfigurationKeys.ClientsPath))
				.AddInMemoryIdentityResources(configuration.GetSection(ConfigurationKeys.IdentityResourcesPath));

			if(configuration.GetValue($"{ConfigurationKeys.FeatureManagementPath}:{nameof(IdentityServerOptions.ServerSideSessions)}", false))
				identityServerBuilder.AddServerSideSessions();

			var dataDependencyInjectionOptions = new DataDependencyInjectionOptions();
			configuration.GetSection(ConfigurationKeys.DataPath).Bind(dataDependencyInjectionOptions);

			if(dataDependencyInjectionOptions.ProviderType == null)
				throw new InvalidOperationException($"The data-provider-type is null. You need to configure it, {"Data:ProviderType".ToStringRepresentation()}.");

			var databaseProvider = (IDatabaseProvider)Activator.CreateInstance(dataDependencyInjectionOptions.ProviderType);

			if(databaseProvider == null)
				throw new InvalidOperationException("The database-provider is null.");

			services.TryAddSingleton(databaseProvider);

			databaseProvider.Add(configuration, dataDependencyInjectionOptions, webHostEnvironment, services);

			return services;
		}

		public static IServiceCollection AddKestrelHttps(this IServiceCollection services, IConfiguration configuration)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.Configure<HttpsOptions>(configuration.GetSection(ConfigurationKeys.KestrelHttpsPath));

			// If you want custom client-certificate validation you can implement a class like RegionOrebroLan.CertificateIdentity.Kestrel.Https.ClientCertificateValidator and register it. It will then be used during client-certificate validation.
			// services.TryAddSingleton<IClientCertificateValidator, MyClientCertificateValidator>();
			services.TryAddSingleton<IConnectionAuthenticator, ConnectionAuthenticator>();

			return services;
		}

		/// <summary>
		/// From https://github.com/DuendeSoftware/IdentityServer/blob/main/hosts/main/Extensions/SameSiteHandlingExtensions.cs -> https://devblogs.microsoft.com/aspnet/upcoming-samesite-cookie-changes-in-asp-net-and-asp-net-core/
		/// </summary>
		private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
		{
			if(httpContext == null)
				throw new ArgumentNullException(nameof(httpContext));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(options.SameSite != SameSiteMode.None)
				return;

			var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

			if(!httpContext.Request.IsHttps || DisallowsSameSiteNone(userAgent))
				options.SameSite = SameSiteMode.Unspecified;
		}

		public static IServiceCollection ConfigureCookiePolicy(this IServiceCollection services, IConfiguration configuration)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
				options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
				options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
			});

			services.Configure<CookiePolicyOptions>(configuration.GetSection(ConfigurationKeys.CookiePolicyPath));

			return services;
		}

		public static IServiceCollection ConfigureForwardedHeaders(this IServiceCollection services, IConfiguration configuration)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.AllowedHosts.Clear();
				options.KnownNetworks.Clear();
				options.KnownProxies.Clear();
			});

			services.Configure<ForwardedHeadersOptions>(configuration.GetSection(ConfigurationKeys.ForwardedHeadersPath));

			return services;
		}

		public static IServiceCollection ConfigureHsts(this IServiceCollection services, IConfiguration configuration)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.Configure<HstsOptions>(options =>
			{
				options.ExcludedHosts.Clear();
				options.IncludeSubDomains = false;
				options.MaxAge = TimeSpan.FromDays(365);
				options.Preload = true;
			});

			services.Configure<HstsOptions>(configuration.GetSection(ConfigurationKeys.HstsPath));

			return services;
		}

		/// <summary>
		/// From https://github.com/DuendeSoftware/IdentityServer/blob/main/hosts/main/Extensions/SameSiteHandlingExtensions.cs -> https://devblogs.microsoft.com/aspnet/upcoming-samesite-cookie-changes-in-asp-net-and-asp-net-core/
		/// </summary>
		private static bool DisallowsSameSiteNone(string userAgent)
		{
			if(userAgent == null)
				throw new ArgumentNullException(nameof(userAgent));

			if(userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
				return true;

			if(userAgent.Contains("Macintosh; Intel Mac OS X 10_14") && userAgent.Contains("Version/") && userAgent.Contains("Safari"))
				return true;

			// ReSharper disable ConvertIfStatementToReturnStatement
			if(userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
				return true;
			// ReSharper restore ConvertIfStatementToReturnStatement

			return false;
		}

		#endregion
	}
}