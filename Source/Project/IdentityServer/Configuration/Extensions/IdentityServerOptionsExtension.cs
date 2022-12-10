using Duende.IdentityServer.Configuration;

namespace RegionOrebroLan.CertificateIdentity.IdentityServer.Configuration.Extensions
{
	public static class IdentityServerOptionsExtension
	{
		#region Methods

		public static void SetDefaults(this IdentityServerOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.Authentication.SetDefaults();
			options.Discovery.SetDefaults();
			options.Endpoints.SetDefaults();
			options.Events.SetDefaults();
			options.ServerSideSessions.SetDefaults();
			options.UserInteraction.SetDefaults();
		}

		#endregion
	}
}