using System.Security.Cryptography.X509Certificates;

namespace RegionOrebroLan.CertificateIdentity.Kestrel.Configuration
{
	public class ChainPolicyOptions
	{
		#region Properties

		/// <summary>
		/// Represents a collection of certificates replacing the default certificate trust. A list of *.crt files.
		/// </summary>
		public virtual ISet<string> CustomTrustStore { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		public virtual bool? DisableCertificateDownloads { get; set; }

		/// <summary>
		/// Additional certificates that can be searched by the chaining engine when validating a certificate chain. A list of *.crt files.
		/// </summary>
		public virtual ISet<string> ExtraStore { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		public virtual X509RevocationFlag? RevocationFlag { get; set; }
		public virtual X509RevocationMode? RevocationMode { get; set; }
		public virtual X509ChainTrustMode? TrustMode { get; set; }
		public virtual TimeSpan? UrlRetrievalTimeout { get; set; }
		public virtual X509VerificationFlags? VerificationFlags { get; set; }

		#endregion
	}
}