using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegionOrebroLan.CertificateIdentity.Extensions;
using RegionOrebroLan.CertificateIdentity.Kestrel.Configuration;
using RegionOrebroLan.CertificateIdentity.Logging.Extensions;

namespace RegionOrebroLan.CertificateIdentity.Kestrel.Https
{
	public class ConnectionAuthenticator : IConnectionAuthenticator
	{
		#region Constructors

		public ConnectionAuthenticator(ILoggerFactory loggerFactory, IOptions<HttpsOptions> options)
		{
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
			this.Options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }
		protected internal virtual HttpsOptions Options { get; }

		#endregion

		#region Methods

		public virtual void Authenticate(ConnectionContext connectionContext, SslServerAuthenticationOptions sslServerAuthentication)
		{
			if(sslServerAuthentication == null)
				throw new ArgumentNullException(nameof(sslServerAuthentication));

			if(!sslServerAuthentication.ClientCertificateRequired)
			{
				this.Logger.LogDebugIfEnabled("Client-certificate is not required.");

				return;
			}

			if(sslServerAuthentication.ServerCertificate == null)
				throw new ArgumentException("The server-certificate is null.", nameof(sslServerAuthentication));

			if(sslServerAuthentication.ServerCertificate is not X509Certificate2 serverCertificate)
				throw new ArgumentException($"The server-certificate \"{sslServerAuthentication.ServerCertificate.Subject}\" is not a X509Certificate2 certificate.", nameof(sslServerAuthentication));

			var sslStream = this.Options.SslStream;
			var mtls = sslStream != null && sslStream.Mtls.TryGetValue(serverCertificate.Subject, out var value) ? value : null;
			var tls = this.Options.Tls;

			this.SetRevocationMode(mtls, sslServerAuthentication, tls);

			this.SetChainPolicy(mtls, sslServerAuthentication, tls);

			this.SetContext(mtls, serverCertificate, sslServerAuthentication, sslStream);
		}

		protected internal virtual X509Certificate2 CreateCertificate(string path)
		{
			if(path == null)
				throw new ArgumentNullException(nameof(path));

			try
			{
				var certificate = new X509Certificate2(path);

				return certificate;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not create certificate from path {path.ToStringRepresentation()}.", exception);
			}
		}

		protected internal virtual IEnumerable<X509Certificate2> CreateCertificates(ISet<string> paths)
		{
			if(paths == null)
				throw new ArgumentNullException(nameof(paths));

			var certificates = new List<X509Certificate2>();

			foreach(var path in paths)
			{
				try
				{
					var certificate = this.CreateCertificate(path);
					certificates.Add(certificate);
				}
				catch(Exception exception)
				{
					throw new InvalidOperationException($"Could not create certificates from paths: {string.Join(", ", paths.Select(item => item.ToStringRepresentation()))}.", exception);
				}
			}

			return certificates;
		}

		protected internal virtual void PopulateChainPolicy(X509ChainPolicy chainPolicy, ChainPolicyOptions options)
		{
			if(chainPolicy == null)
				throw new ArgumentNullException(nameof(chainPolicy));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(options.CustomTrustStore.Any())
				chainPolicy.CustomTrustStore.AddRange(this.CreateCertificates(options.CustomTrustStore).ToArray());

			if(options.DisableCertificateDownloads != null)
				chainPolicy.DisableCertificateDownloads = options.DisableCertificateDownloads.Value;

			if(options.ExtraStore.Any())
				chainPolicy.ExtraStore.AddRange(this.CreateCertificates(options.ExtraStore).ToArray());

			if(options.RevocationFlag != null)
				chainPolicy.RevocationFlag = options.RevocationFlag.Value;

			if(options.RevocationMode != null)
				chainPolicy.RevocationMode = options.RevocationMode.Value;

			if(options.TrustMode != null)
				chainPolicy.TrustMode = options.TrustMode.Value;

			if(options.UrlRetrievalTimeout != null)
				chainPolicy.UrlRetrievalTimeout = options.UrlRetrievalTimeout.Value;

			if(options.VerificationFlags != null)
				chainPolicy.VerificationFlags = options.VerificationFlags.Value;
		}

		protected internal virtual SslCertificateTrust ResolveSslCertificateTrust(MtlsOptions mtls)
		{
			if(mtls == null || string.IsNullOrWhiteSpace(mtls.Trust))
				return null;

			try
			{
				if(OperatingSystem.IsWindows())
				{
					StoreLocation? storeLocation = null;
					string storeName = null;

					try
					{
						var parts = mtls.Trust.Split(Path.DirectorySeparatorChar);

						if(parts.Length != 3)
							throw new InvalidOperationException($"The path {mtls.Trust.ToStringRepresentation()} is invalid. The path split with {Path.DirectorySeparatorChar.ToStringRepresentation()} must contain 3 parts. The path {mtls.Trust.ToStringRepresentation()} contains {parts.Length} parts.");

						const string firstPart = "CERT:";

						if(!parts[0].Equals(firstPart, StringComparison.OrdinalIgnoreCase))
							throw new InvalidOperationException($"The path {mtls.Trust.ToStringRepresentation()} is invalid. The path must start with {firstPart.ToStringRepresentation()}.");

						if(!Enum.TryParse<StoreLocation>(parts[1], true, out var location))
							throw new InvalidOperationException();
						//throw new InvalidOperationException($"The path {mtls.Trust.ToStringRepresentation()} is invalid. The second part mpath must start with {firstPart.ToStringRepresentation()}.");

						storeLocation = location;
						storeName = parts[2];
					}
					catch(Exception exception)
					{
						throw new InvalidOperationException($"Could not resolve certificate-store from path {mtls.Trust.ToStringRepresentation()}.", exception);
					}

					return SslCertificateTrust.CreateForX509Store(new X509Store(storeName, storeLocation.Value));
				}

				var paths = mtls.Trust.Split(',').Select(path => path.Trim()).Where(path => !string.IsNullOrEmpty(path)).ToHashSet(StringComparer.OrdinalIgnoreCase);

				var certificates = this.CreateCertificates(paths);

				var certificateCollection = new X509Certificate2Collection(certificates.ToArray());

				return SslCertificateTrust.CreateForX509Collection(certificateCollection, true);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not resolve ssl-certificate-trust from value {mtls.Trust.ToStringRepresentation()}.", exception);
			}
		}

		protected internal virtual void SetChainPolicy(MtlsOptions mtls, SslServerAuthenticationOptions sslServerAuthentication, TlsOptions tls)
		{
			if(sslServerAuthentication == null)
				throw new ArgumentNullException(nameof(sslServerAuthentication));

			var mtlsChainPolicy = mtls?.ChainPolicy;
			var tlsChainPolicy = tls?.ChainPolicy;

			if(mtlsChainPolicy == null && tlsChainPolicy == null)
				return;

			var chainPolicy = new X509ChainPolicy();

			if(tlsChainPolicy != null)
				this.PopulateChainPolicy(chainPolicy, tlsChainPolicy);

			if(mtlsChainPolicy != null)
				this.PopulateChainPolicy(chainPolicy, mtlsChainPolicy);

			sslServerAuthentication.CertificateChainPolicy = chainPolicy;
		}

		protected internal virtual void SetContext(MtlsOptions mtls, X509Certificate2 serverCertificate, SslServerAuthenticationOptions sslServerAuthentication, SslStreamOptions sslStream)
		{
			if(serverCertificate == null)
				throw new ArgumentNullException(nameof(serverCertificate));

			if(sslServerAuthentication == null)
				throw new ArgumentNullException(nameof(sslServerAuthentication));

			X509Certificate2Collection additionalCertificates = null;

			if(sslStream != null && sslStream.AdditionalCertificates.Any())
				additionalCertificates = new X509Certificate2Collection(this.CreateCertificates(sslStream.AdditionalCertificates).ToArray());

			var offline = sslStream?.Offline ?? false;

			var sslCertificateTrust = this.ResolveSslCertificateTrust(mtls);

			sslServerAuthentication.ServerCertificateContext = SslStreamCertificateContext.Create(serverCertificate, additionalCertificates, offline, sslCertificateTrust);
		}

		protected internal virtual void SetRevocationMode(MtlsOptions mtls, SslServerAuthenticationOptions sslServerAuthentication, TlsOptions tls)
		{
			if(sslServerAuthentication == null)
				throw new ArgumentNullException(nameof(sslServerAuthentication));

			if(tls?.RevocationMode != null)
				sslServerAuthentication.CertificateRevocationCheckMode = tls.RevocationMode.Value;

			if(mtls?.RevocationMode != null)
				sslServerAuthentication.CertificateRevocationCheckMode = mtls.RevocationMode.Value;
		}

		#endregion
	}
}