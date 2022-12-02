using Duende.IdentityServer.Configuration;

namespace RegionOrebroLan.CertificateIdentity.IdentityServer.Configuration.Extensions
{
	public static class EndpointsOptionsExtension
	{
		#region Methods

		public static void SetDefaults(this EndpointsOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.EnableBackchannelAuthenticationEndpoint = false;
			options.EnableCheckSessionEndpoint = false;
			options.EnableDeviceAuthorizationEndpoint = false;
			options.EnableEndSessionEndpoint = false;
			options.EnableIntrospectionEndpoint = false;
			options.EnableJwtRequestUri = false;
			options.EnableTokenEndpoint = false;
			options.EnableTokenRevocationEndpoint = false;
			options.EnableUserInfoEndpoint = false;
		}

		#endregion
	}
}