using Duende.IdentityServer.Configuration;

namespace RegionOrebroLan.CertificateIdentity.IdentityServer.Configuration.Extensions
{
	public static class UserInteractionOptionsExtension
	{
		#region Methods

		public static void SetDefaults(this UserInteractionOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.ErrorUrl = "/Error";
			options.LoginUrl = "/Authenticate";
			options.LogoutUrl = "/Account/SignOut";
			options.LogoutIdParameter = "signOutId";
		}

		#endregion
	}
}