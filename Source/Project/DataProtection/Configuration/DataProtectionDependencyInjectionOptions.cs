using RegionOrebroLan.CertificateIdentity.Configuration;

namespace RegionOrebroLan.CertificateIdentity.DataProtection.Configuration
{
	public class DataProtectionDependencyInjectionOptions : ConnectionOptions
	{
		#region Properties

		public virtual Type ProviderType { get; set; }

		#endregion
	}
}