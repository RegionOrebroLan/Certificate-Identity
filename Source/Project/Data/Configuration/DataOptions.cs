using RegionOrebroLan.CertificateIdentity.Configuration;

namespace RegionOrebroLan.CertificateIdentity.Data.Configuration
{
	public class DataOptions
	{
		#region Properties

		public virtual string ConnectionStringName { get; set; } = ConfigurationKeys.DefaultConnectionStringName;
		public virtual Type ProviderType { get; set; }

		#endregion
	}
}