using System.Net.Security;
using Microsoft.AspNetCore.Connections;

namespace RegionOrebroLan.CertificateIdentity.Kestrel.Https
{
	public interface IConnectionAuthenticator
	{
		#region Methods

		void Authenticate(ConnectionContext connectionContext, SslServerAuthenticationOptions sslServerAuthentication);

		#endregion
	}
}