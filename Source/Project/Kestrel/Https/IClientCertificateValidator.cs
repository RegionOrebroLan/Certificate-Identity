using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace RegionOrebroLan.CertificateIdentity.Kestrel.Https
{
	public interface IClientCertificateValidator
	{
		#region Methods

		bool Validate(X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors);

		#endregion
	}
}