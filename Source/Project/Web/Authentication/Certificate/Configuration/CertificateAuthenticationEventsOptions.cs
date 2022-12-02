using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using RegionOrebroLan.CertificateIdentity.Extensions;

namespace RegionOrebroLan.CertificateIdentity.Web.Authentication.Certificate.Configuration
{
	public class CertificateAuthenticationEventsOptions
	{
		#region Fields

		private static IDictionary<string, string> _internalClaimTypeMap;
		private static readonly object _internalClaimTypeMapLock = new();

		#endregion

		#region Properties

		/// <summary>
		/// Mapping Microsoft-claims to Jwt-claims.
		/// </summary>
		public virtual IDictionary<string, string> ClaimTypeMap { get; } = new Dictionary<string, string>(InternalClaimTypeMap, StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Mapping certificate-issuer to options.
		/// </summary>
		public virtual IDictionary<string, DistinguishedNameClaimsOptions> DistinguishedNameClaims { get; } = new Dictionary<string, DistinguishedNameClaimsOptions>(StringComparer.OrdinalIgnoreCase);

		public virtual string FallbackSubjectClaimType { get; set; } = ClaimTypes.Thumbprint;
		public virtual string IdentityProviderClaimType { get; set; } = JwtClaimTypes.IdentityProvider;

		private static IDictionary<string, string> InternalClaimTypeMap
		{
			get
			{
				// ReSharper disable InvertIf
				if(_internalClaimTypeMap == null)
				{
					lock(_internalClaimTypeMapLock)
					{
						if(_internalClaimTypeMap == null)
						{
							var temporaryCorrectedClaimTypeMap = new Dictionary<string, string>(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap, StringComparer.OrdinalIgnoreCase)
							{
								[ClaimTypes.Name] = JwtClaimTypes.Name,
								[ClaimTypes.NameIdentifier] = JwtClaimTypes.Subject
							};

							temporaryCorrectedClaimTypeMap.Add(ClaimTypes.Dns, "dns");
							temporaryCorrectedClaimTypeMap.Add(ClaimTypes.X500DistinguishedName, "x500distinguishedname");

							var correctedClaimTypeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

							foreach(var (key, value) in temporaryCorrectedClaimTypeMap)
							{
								correctedClaimTypeMap.Add(key.UrlEncodeColon(), value);
							}

							_internalClaimTypeMap = correctedClaimTypeMap;
						}
					}
				}
				// ReSharper restore InvertIf

				return _internalClaimTypeMap;
			}
		}

		public virtual string NameClaimType { get; set; } = JwtClaimTypes.Name;
		public virtual string RoleClaimType { get; set; } = JwtClaimTypes.Role;

		#endregion
	}
}