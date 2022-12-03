using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer
{
	public static class AddCertificateIdentityExtension
	{
		#region Fields

		private const string _authority = "https://certificate-id.example.local:50000";

		#endregion

		#region Methods

		public static AuthenticationBuilder AddCertificateIdentity(this AuthenticationBuilder authenticationBuilder)
		{
			if(authenticationBuilder == null)
				throw new ArgumentNullException(nameof(authenticationBuilder));

			authenticationBuilder
				.AddCertificateIdentity("SithsHsaCertificate", "Sign in with Siths HSA", "siths-hsa")
				.AddCertificateIdentity("SithsPersonCertificate", "Sign in with Siths Person", "siths-person")
				.AddCertificateIdentity("TestSithsHsaCertificate", "Sign in with Test Siths HSA", "test-siths-hsa")
				.AddCertificateIdentity("TestSithsPersonCertificate", "Sign in with Test Siths Person", "test-siths-person");

			return authenticationBuilder;
		}

		private static AuthenticationBuilder AddCertificateIdentity(this AuthenticationBuilder authenticationBuilder, string authenticationSchemeName, string authenticationSchemeDisplayName, string clientId)
		{
			if(authenticationBuilder == null)
				throw new ArgumentNullException(nameof(authenticationBuilder));

			authenticationBuilder
				.AddOpenIdConnect(authenticationSchemeName, authenticationSchemeDisplayName, options =>
				{
					options.Authority = _authority;
					options.CallbackPath = $"/signin-{clientId}";
					options.ClientId = clientId;
					/*
						Below could also be handled by

							builder.Services.AddAuthentication(options =>
							{
								options.DefaultSignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
							})

					*/
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				});

			return authenticationBuilder;
		}

		#endregion
	}
}