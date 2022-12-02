using Duende.IdentityServer.Configuration;

namespace RegionOrebroLan.CertificateIdentity.IdentityServer.Configuration.Extensions
{
	public static class DiscoveryOptionsExtension
	{
		#region Methods

		public static void SetDefaults(this DiscoveryOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.ShowApiScopes = false;
			options.ShowClaims = false;
			options.ShowExtensionGrantTypes = false;
			options.ShowGrantTypes = false;
			options.ShowIdentityScopes = false;
			options.ShowResponseModes = false;
			options.ShowResponseTypes = false;
			options.ShowTokenEndpointAuthenticationMethods = false;
		}

		#endregion
	}
}