using Duende.IdentityServer.Configuration;
using IdentityModel;

namespace RegionOrebroLan.CertificateIdentity.IdentityServer.Configuration.Extensions
{
	public static class ServerSideSessionOptionsExtension
	{
		#region Methods

		public static void SetDefaults(this ServerSideSessionOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			// The commented ones are defaults.
			//options.ExpiredSessionsTriggerBackchannelLogout = false;
			//options.RemoveExpiredSessions = true;
			//options.RemoveExpiredSessionsBatchSize = 100;
			//options.RemoveExpiredSessionsFrequency = TimeSpan.FromMinutes(10);
			options.UserDisplayNameClaimType = JwtClaimTypes.Name;
		}

		#endregion
	}
}