namespace RegionOrebroLan.CertificateIdentity.Configuration
{
	public class ConfigurationKeys
	{
		#region Fields

		public const string AuthenticationPath = "Authentication";
		public const string CertificateAuthenticationEventsPath = $"{AuthenticationPath}:CertificateEvents";
		public const string CertificateAuthenticationPath = $"{AuthenticationPath}:Certificate";
		public const string CertificateForwardingPath = "CertificateForwarding";
		public const string ClientsPath = "Clients";
		public const string CookiePolicyPath = "CookiePolicy";
		public const string DataPath = "Data";
		public const string DataProtectionPath = "DataProtection";
		public const string DefaultConnectionStringName = "IdentityServer";
		public const string ExceptionHandlingPath = "ExceptionHandling";
		public const string FeatureManagementPath = "FeatureManagement";
		public const string ForwardedHeadersPath = "ForwardedHeaders";
		public const string HstsPath = "Hsts";
		public const string HttpsRedirectionPath = "HttpsRedirection";
		public const string IdentityResourcesPath = "IdentityResources";
		public const string IdentityServerPath = "IdentityServer";
		public const string KestrelHttpsPath = $"{KestrelPath}:Https";
		public const string KestrelPath = "Kestrel";
		public const string OperationalStorePath = $"{IdentityServerPath}:OperationalStore";
		public const string SecurityHeadersPath = "SecurityHeaders";

		#endregion
	}
}