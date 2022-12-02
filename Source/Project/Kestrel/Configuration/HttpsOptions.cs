namespace RegionOrebroLan.CertificateIdentity.Kestrel.Configuration
{
	public class HttpsOptions
	{
		#region Properties

		public virtual TimeSpan? HandshakeTimeout { get; set; }
		public virtual SslStreamOptions SslStream { get; set; } = new();

		/// <summary>
		/// Default TSL-options.
		/// </summary>
		public virtual TlsOptions Tls { get; set; }

		#endregion
	}
}