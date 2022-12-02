namespace RegionOrebroLan.CertificateIdentity.Kestrel.Configuration
{
	public class SslStreamOptions
	{
		#region Properties

		/// <summary>
		/// Supplementary certificates used to build the certificate chain. A list of *.crt files.
		/// </summary>
		public virtual ISet<string> AdditionalCertificates { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Mapping of server-certificate to mTLS settings.
		/// The key is a server-certificate subject.
		/// </summary>
		public virtual IDictionary<string, MtlsOptions> Mtls { get; } = new Dictionary<string, MtlsOptions>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// False: to indicate that the missing certificates can be downloaded from the network. True: to indicate that only available X509Certificate stores should be searched for missing certificates.
		/// </summary>
		public virtual bool Offline { get; set; }

		#endregion
	}
}