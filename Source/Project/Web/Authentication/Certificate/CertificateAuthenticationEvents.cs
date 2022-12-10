using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegionOrebroLan.CertificateIdentity.Extensions;
using RegionOrebroLan.CertificateIdentity.Logging.Extensions;
using RegionOrebroLan.CertificateIdentity.Web.Authentication.Certificate.Configuration;

namespace RegionOrebroLan.CertificateIdentity.Web.Authentication.Certificate
{
	public class CertificateAuthenticationEvents : Microsoft.AspNetCore.Authentication.Certificate.CertificateAuthenticationEvents
	{
		#region Constructors

		public CertificateAuthenticationEvents(ILoggerFactory loggerFactory, IOptionsMonitor<CertificateAuthenticationEventsOptions> optionsMonitor)
		{
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
			this.OptionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }
		protected internal virtual IOptionsMonitor<CertificateAuthenticationEventsOptions> OptionsMonitor { get; }

		#endregion

		#region Methods

		public override async Task CertificateValidated(CertificateValidatedContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(context.Principal == null)
				throw new ArgumentException("The context-principal can not be null.", nameof(context));

			if(context.Scheme == null)
				throw new ArgumentException("The context-scheme can not be null.", nameof(context));

			var claims = new List<Claim>();
			var claimsPrincipal = context.Principal;
			var issuer = context.ClientCertificate.Issuer;
			var options = this.OptionsMonitor.CurrentValue;
			var scheme = context.Scheme;

			if(!options.DistinguishedNameClaims.TryGetValue(issuer, out var distinguishedNameClaims))
				distinguishedNameClaims = new DistinguishedNameClaimsOptions();

			await this.PopulateClaimsAsync(scheme.Name, claims, claimsPrincipal, options);

			await this.PopulateDistinguishedNameClaimsAsync(claims, claimsPrincipal, distinguishedNameClaims);

			await this.EnsureSubjectClaimAsync(claims, claimsPrincipal, options);

			context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, claimsPrincipal.Identity?.AuthenticationType, options.NameClaimType, options.RoleClaimType));
		}

		protected internal virtual async Task EnsureSubjectClaimAsync(IList<Claim> claims, ClaimsPrincipal claimsPrincipal, CertificateAuthenticationEventsOptions options)
		{
			if(claims == null)
				throw new ArgumentNullException(nameof(claims));

			if(claimsPrincipal == null)
				throw new ArgumentNullException(nameof(claimsPrincipal));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			if(claims.Any(claim => string.Equals(claim.Type, JwtClaimTypes.Subject, StringComparison.OrdinalIgnoreCase)))
				return;

			var fallbackSubjectClaim = claimsPrincipal.FindFirst(options.FallbackSubjectClaimType);

			if(fallbackSubjectClaim == null)
			{
				this.Logger.LogWarningIfEnabled(() => $"The claims-principal does not contain a claim with type {options.FallbackSubjectClaimType.ToStringRepresentation()} to use as {JwtClaimTypes.Subject.ToStringRepresentation()} claim.");

				return;
			}

			claims.Add(new Claim(JwtClaimTypes.Subject, fallbackSubjectClaim.Value));
		}

		protected internal virtual async Task PopulateClaimsAsync(string authenticationScheme, IList<Claim> claims, ClaimsPrincipal claimsPrincipal, CertificateAuthenticationEventsOptions options)
		{
			if(authenticationScheme == null)
				throw new ArgumentNullException(nameof(authenticationScheme));

			if(claims == null)
				throw new ArgumentNullException(nameof(claims));

			if(claimsPrincipal == null)
				throw new ArgumentNullException(nameof(claimsPrincipal));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			claims.Add(new Claim(options.IdentityProviderClaimType, authenticationScheme));

			foreach(var claim in claimsPrincipal.Claims)
			{
				if(!options.ClaimTypeMap.TryGetValue(claim.Type.UrlEncodeColon(), out var claimType))
					claimType = claim.Type;

				claims.Add(new Claim(claimType, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer));
			}
		}

		protected internal virtual async Task PopulateDistinguishedNameClaimsAsync(IList<Claim> claims, ClaimsPrincipal claimsPrincipal, DistinguishedNameClaimsOptions options)
		{
			if(claims == null)
				throw new ArgumentNullException(nameof(claims));

			if(claimsPrincipal == null)
				throw new ArgumentNullException(nameof(claimsPrincipal));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			if(!options.Map.Any())
				return;

			var distinguishedNameClaim = claimsPrincipal.FindFirst(ClaimTypes.X500DistinguishedName);

			if(distinguishedNameClaim == null)
			{
				this.Logger.LogWarningIfEnabled(() => $"The claims-principal does not contain a claim with type {ClaimTypes.X500DistinguishedName.ToStringRepresentation()}.");

				return;
			}

			var distinguishedName = new X500DistinguishedName(distinguishedNameClaim.Value);

			foreach(var component in distinguishedName.EnumerateRelativeDistinguishedNames())
			{
				var name = component.GetSingleElementType().FriendlyName;

				if(name == null)
					continue;

				if(!options.Map.TryGetValue(name, out var claimTypes))
					continue;

				var value = component.GetSingleElementValue();

				if(string.IsNullOrEmpty(value))
					continue;

				foreach(var claimType in claimTypes)
				{
					claims.Add(new Claim(claimType, value));
				}
			}
		}

		#endregion
	}
}