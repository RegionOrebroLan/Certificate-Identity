using Duende.IdentityServer.Configuration;

namespace RegionOrebroLan.CertificateIdentity.IdentityServer.Configuration.Extensions
{
	public static class EventsOptionsExtension
	{
		#region Methods

		public static void SetDefaults(this EventsOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.RaiseErrorEvents = true;
			options.RaiseFailureEvents = true;
		}

		#endregion
	}
}