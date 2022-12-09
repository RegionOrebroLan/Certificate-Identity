using RegionOrebroLan.CertificateIdentity.Configuration;

namespace RegionOrebroLan.CertificateIdentity.Data.Configuration
{
	public class DataOptions : ConnectionOptions
	{
		#region Properties

		public virtual Type ProviderType { get; set; }

		#endregion
	}
}