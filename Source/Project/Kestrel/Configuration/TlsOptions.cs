using System.Security.Cryptography.X509Certificates;

namespace RegionOrebroLan.CertificateIdentity.Kestrel.Configuration
{
	public class TlsOptions
	{
		#region Properties

		public virtual ChainPolicyOptions ChainPolicy { get; set; }
		public virtual X509RevocationMode? RevocationMode { get; set; }

		#endregion
	}
}