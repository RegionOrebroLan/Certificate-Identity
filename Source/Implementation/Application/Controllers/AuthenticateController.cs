using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using RegionOrebroLan.CertificateIdentity.Extensions;

namespace Application.Controllers
{
	public class AuthenticateController : SiteController
	{
		#region Constructors

		public AuthenticateController(IIdentityServerInteractionService interactionService, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			this.InteractionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
		}

		#endregion

		#region Properties

		protected internal virtual IIdentityServerInteractionService InteractionService { get; }

		#endregion

		#region Methods

		public virtual async Task<IActionResult> Index(string returnUrl)
		{
			if(string.IsNullOrWhiteSpace(returnUrl))
				returnUrl = null;

			if(returnUrl != null && !this.Url.IsLocalUrl(returnUrl) && !this.InteractionService.IsValidReturnUrl(returnUrl))
				throw new InvalidOperationException("Invalid return-url.");

			var authorizationRequest = await this.InteractionService.GetAuthorizationContextAsync(returnUrl);
			var clientCertificate = await this.HttpContext.Connection.GetClientCertificateAsync();

			if(authorizationRequest == null)
			{
				if(this.Logger.IsEnabled(LogLevel.Information))
					this.Logger.LogInformation("Not an authorization request.");
			}
			else
			{
				var client = authorizationRequest.Client;

				if(client == null)
				{
					if(this.Logger.IsEnabled(LogLevel.Information))
						this.Logger.LogInformation("Not a client-authentication request.");
				}
				else
				{
					if(!client.Properties.TryGetValue("AuthenticationOrigin", out var clientAuthenticationOrigin))
						throw new InvalidOperationException($"No authentication-origin registered for client {client.ClientId.ToStringRepresentation()}.");

					if(!client.Properties.TryGetValue("CertificateIssuer", out var requiredClientCertificateIssuer))
						throw new InvalidOperationException($"No certificate-issuer registered for client {client.ClientId.ToStringRepresentation()}.");

					var origin = $"{this.Request.Scheme}{Uri.SchemeDelimiter}{this.Request.Host}";

					if(!string.Equals(clientAuthenticationOrigin, origin, StringComparison.OrdinalIgnoreCase))
						return this.Redirect($"{clientAuthenticationOrigin}{this.Request.Path}{this.Request.QueryString}");

					if(clientCertificate == null)
						throw new InvalidOperationException($"No client-certificate connected for url {this.Request.GetDisplayUrl().ToStringRepresentation()} and client {client.ClientId.ToStringRepresentation()}.");

					if(!string.Equals(clientCertificate.Issuer, requiredClientCertificateIssuer, StringComparison.OrdinalIgnoreCase))
						throw new InvalidOperationException($"Invalid client-certificate issuer for client {client.ClientId.ToStringRepresentation()}.");

					var authenticateResult = await this.HttpContext.AuthenticateAsync(CertificateAuthenticationDefaults.AuthenticationScheme);

					if(!authenticateResult.Succeeded)
						throw new InvalidOperationException("Certificate authentication error.", authenticateResult.Failure);

					await this.HttpContext.SignInAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme, authenticateResult.Principal);
				}
			}

			if(returnUrl != null)
				return this.Redirect(returnUrl);

			return this.Content($"Certificate: {Environment.NewLine}{Environment.NewLine}{clientCertificate}");
		}

		#endregion
	}
}