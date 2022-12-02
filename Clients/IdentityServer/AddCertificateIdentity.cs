using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer
{
	public static class AddCertificateIdentityExtension
	{
		#region Methods

		public static AuthenticationBuilder AddCertificateIdentity(this AuthenticationBuilder authenticationBuilder)
		{
			if(authenticationBuilder == null)
				throw new ArgumentNullException(nameof(authenticationBuilder));

			const string authority = "https://certificate-id.example.local:50000";

			authenticationBuilder
				.AddOpenIdConnect("SithsHsaCertificate", "Sign in with Siths HSA", options =>
				{
					options.Authority = authority;
					options.CallbackPath = "/signin-siths-hsa";
					options.ClientId = "siths-hsa";
					/*
						Below could also be handled by

							builder.Services.AddAuthentication(options =>
							{
								options.DefaultSignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
							})

					*/
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				})
				.AddOpenIdConnect("SithsPersonCertificate", "Sign in with Siths Person", options =>
				{
					options.Authority = authority;
					options.CallbackPath = "/signin-siths-person";
					options.ClientId = "siths-person";
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				})
				.AddOpenIdConnect("TestSithsHsaCertificate", "Sign in with Test Siths HSA", options =>
				{
					options.Authority = authority;
					options.CallbackPath = "/signin-test-siths-hsa";
					options.ClientId = "test-siths-hsa";
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				})
				.AddOpenIdConnect("TestSithsPersonCertificate", "Sign in with Test Siths Person", options =>
				{
					options.Authority = authority;
					options.CallbackPath = "/signin-test-siths-person";
					options.ClientId = "test-siths-person";
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				});

			return authenticationBuilder;
		}

		#endregion
	}
}