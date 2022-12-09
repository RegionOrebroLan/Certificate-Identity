namespace RegionOrebroLan.CertificateIdentity.Configuration
{
	public class ConnectionOptions
	{
		#region Properties

		public virtual string ConnectionStringName { get; set; } = ConfigurationKeys.DefaultConnectionStringName;

		#endregion
	}
}