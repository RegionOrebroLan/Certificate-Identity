namespace RegionOrebroLan.CertificateIdentity.Kestrel.Configuration
{
	public class MtlsOptions : TlsOptions
	{
		#region Properties

		/// <summary>
		/// Value representing the certificate trust list, CTL, sent in the certificate request during the TLS handshake.
		/// The value is any of the following:
		/// - Linux: One or more paths to *.crt files that should represent the CTL. Each path is separated by a comma.
		/// - Windows: A path to a certificate-store, eg. "CERT:\LocalMachine\My-CTL-Store".
		/// </summary>
		public virtual string Trust { get; set; }

		#endregion
	}
}