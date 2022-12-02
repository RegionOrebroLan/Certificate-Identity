namespace RegionOrebroLan.CertificateIdentity.Web.Http.Configuration
{
	public class SecurityHeadersOptions
	{
		#region Fields

		private const string _contentSecurityPolicy = "default-src 'none'";

		#endregion

		#region Properties

		public virtual bool Enabled { get; set; } = true;

		public virtual ISet<string> ExcludePaths { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"/.well-known/",
			"/connect/"
		};

		public virtual IDictionary<string, string> Headers { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
			{ "X-Content-Type-Options", "nosniff" },
			// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
			{ "X-Frame-Options", "DENY" },
			// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
			{ "Content-Security-Policy", _contentSecurityPolicy },
			{ "X-Content-Security-Policy", _contentSecurityPolicy },
			// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
			{ "Referrer-Policy", "no-referrer" }
		};

		#endregion
	}
}