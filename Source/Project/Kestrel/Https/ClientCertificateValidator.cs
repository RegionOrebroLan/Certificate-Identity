using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace RegionOrebroLan.CertificateIdentity.Kestrel.Https
{
	public class ClientCertificateValidator : IClientCertificateValidator
	{
		#region Methods

		public virtual bool Validate(X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return sslPolicyErrors == SslPolicyErrors.None;
		}

		#endregion
	}
}