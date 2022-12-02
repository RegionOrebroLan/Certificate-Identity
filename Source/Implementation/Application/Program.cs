using System.ComponentModel;
using System.Net;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using RegionOrebroLan.CertificateIdentity.ComponentModel;
using RegionOrebroLan.CertificateIdentity.Configuration;
using RegionOrebroLan.CertificateIdentity.DependencyInjection.Extensions;
using RegionOrebroLan.CertificateIdentity.Kestrel.Configuration;
using RegionOrebroLan.CertificateIdentity.Kestrel.Https;
using RegionOrebroLan.CertificateIdentity.Web.Builder.Extensions;
using RegionOrebroLan.CertificateIdentity.Web.Http.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting host ...");

try
{
	TypeDescriptor.AddAttributes(typeof(IPAddress), new TypeConverterAttribute(typeof(IpAddressTypeConverter)));
	TypeDescriptor.AddAttributes(typeof(IPNetwork), new TypeConverterAttribute(typeof(IpNetworkTypeConverter)));
	TypeDescriptor.AddAttributes(typeof(Type), new TypeConverterAttribute(typeof(TypeTypeConverter)));

	var builder = WebApplication.CreateBuilder(args);

	builder.Host.UseSerilog((hostBuilderContext, serviceProvider, loggerConfiguration) =>
	{
		loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
		loggerConfiguration.ReadFrom.Services(serviceProvider);
	});

	builder.WebHost.ConfigureKestrel(kestrelServerOptions =>
	{
		kestrelServerOptions.ConfigureHttpsDefaults(httpsConnectionAdapterOptions =>
		{
			var clientCertificateValidator = kestrelServerOptions.ApplicationServices.GetService<IClientCertificateValidator>();
			if(clientCertificateValidator != null)
				httpsConnectionAdapterOptions.ClientCertificateValidation = clientCertificateValidator.Validate;

			var httpsOptions = kestrelServerOptions.ApplicationServices.GetRequiredService<IOptions<HttpsOptions>>().Value;
			if(httpsOptions.HandshakeTimeout != null)
				httpsConnectionAdapterOptions.HandshakeTimeout = httpsOptions.HandshakeTimeout.Value;

			var connectionAuthenticator = kestrelServerOptions.ApplicationServices.GetRequiredService<IConnectionAuthenticator>();
			httpsConnectionAdapterOptions.OnAuthenticate = connectionAuthenticator.Authenticate;
		});
	});

	builder.Services.AddAuthentication()
		.AddCertificate(CertificateAuthenticationDefaults.AuthenticationScheme, options =>
		{
			options.EventsType = typeof(RegionOrebroLan.CertificateIdentity.Web.Authentication.Certificate.CertificateAuthenticationEvents);

			builder.Configuration.GetSection(ConfigurationKeys.CertificateAuthenticationPath).Bind(options);
		});
	builder.Services.AddCertificateAuthentication(builder.Configuration);
	builder.Services.AddControllersWithViews();
	builder.Services.AddFeatureManagement();
	builder.Services.AddIdentityServer(builder.Configuration, builder.Environment);
	builder.Services.AddKestrelHttps(builder.Configuration);

	builder.Services.Configure<CertificateForwardingOptions>(builder.Configuration.GetSection(ConfigurationKeys.CertificateForwardingPath));
	builder.Services.ConfigureCookiePolicy(builder.Configuration);
	builder.Services.Configure<ExceptionHandlingOptions>(builder.Configuration.GetSection(ConfigurationKeys.ExceptionHandlingPath));
	builder.Services.ConfigureForwardedHeaders(builder.Configuration);
	builder.Services.ConfigureHsts(builder.Configuration);
	builder.Services.Configure<HttpsRedirectionOptions>(builder.Configuration.GetSection(ConfigurationKeys.HttpsRedirectionPath));
	builder.Services.Configure<SecurityHeadersOptions>(builder.Configuration.GetSection(ConfigurationKeys.SecurityHeadersPath));

	var app = builder.Build();

	var featureManager = app.Services.GetRequiredService<IFeatureManager>();

	var exceptionHandling = app.Services.GetRequiredService<IOptions<ExceptionHandlingOptions>>().Value;
	if(exceptionHandling.DeveloperExceptionPage)
		app.UseDeveloperExceptionPage();
	else
		app.UseExceptionHandler(exceptionHandling.Path);

	if(await featureManager.IsEnabledAsync(ConfigurationKeys.CertificateForwardingPath))
		app.UseCertificateForwarding();

	if(await featureManager.IsEnabledAsync(ConfigurationKeys.ForwardedHeadersPath))
		app.UseForwardedHeaders();

	if(await featureManager.IsEnabledAsync(ConfigurationKeys.HstsPath))
		app.UseHsts();

	if(await featureManager.IsEnabledAsync(ConfigurationKeys.HttpsRedirectionPath))
		app.UseHttpsRedirection();

	if(await featureManager.IsEnabledAsync(ConfigurationKeys.CookiePolicyPath))
		app.UseCookiePolicy();

	app
		.UseSecurityHeaders()
		.UseRouting()
		.UseIdentityServer()
		.UseAuthorization()
		.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapDefaultControllerRoute());

	app.Run();
}
catch(Exception exception)
{
	Log.Fatal(exception, "Host terminated unexpectedly.");
}
finally
{
	Log.Information("Stopping host ...");
	Log.CloseAndFlush();
}