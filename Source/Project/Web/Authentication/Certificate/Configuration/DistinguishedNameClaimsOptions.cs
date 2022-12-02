namespace RegionOrebroLan.CertificateIdentity.Web.Authentication.Certificate.Configuration
{
	public class DistinguishedNameClaimsOptions
	{
		#region Properties

		/// <summary>
		/// Mapping distinguished-name-component to claim-types.
		/// </summary>
		public virtual IDictionary<string, IEnumerable<string>> Map { get; } = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);

		#endregion
	}
}