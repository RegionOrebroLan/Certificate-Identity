using System.Web;

namespace RegionOrebroLan.CertificateIdentity.Extensions
{
	public static class StringExtension
	{
		#region Fields

		private const string _colon = ":";
		private static readonly string _urlEncodedColon = HttpUtility.UrlEncode(_colon).ToLowerInvariant();

		#endregion

		#region Methods

		public static string UrlDecodeColon(this string value)
		{
			const StringComparison comparison = StringComparison.OrdinalIgnoreCase;

			if(value != null && value.Contains(_urlEncodedColon, comparison))
				value = value.Replace(_urlEncodedColon, _colon, comparison);

			return value;
		}

		public static string UrlEncodeColon(this string value)
		{
			const StringComparison comparison = StringComparison.Ordinal;

			if(value != null && value.Contains(_colon, comparison))
				value = value.Replace(_colon, _urlEncodedColon, comparison);

			return value;
		}

		#endregion
	}
}