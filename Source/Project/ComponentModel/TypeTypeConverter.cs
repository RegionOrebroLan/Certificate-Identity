using System.ComponentModel;
using System.Globalization;

namespace RegionOrebroLan.CertificateIdentity.ComponentModel
{
	public class TypeTypeConverter : TypeConverter
	{
		#region Properties

		/// <summary>
		/// Used when converting to. If false (default) and the type is string, ConvertTo will return eg: "System.String, System.Private.CoreLib". If true and the type is string, ConvertTo will return eg: "System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e".
		/// </summary>
		public virtual bool UseAssemblyQualifiedName { get; set; }

		#endregion

		#region Methods

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			switch(value)
			{
				case null:
				case string { Length: 0 }:
					return null;
				case string text:
					try
					{
						return Type.GetType(text, true, true);
					}
					catch(Exception exception)
					{
						throw new InvalidOperationException($"Can not convert from {typeof(string)} \"{text}\" to {typeof(Type)}.", exception);
					}
				default:
					return base.ConvertFrom(context, culture, value);
			}
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if(destinationType == null)
				throw new ArgumentNullException(nameof(destinationType));

			// ReSharper disable All
			if(destinationType == typeof(string) && value is Type type)
			{
				if(this.UseAssemblyQualifiedName)
					return type.AssemblyQualifiedName;

				return $"{type.FullName}, {type.Assembly.GetName().Name}";
			}
			// ReSharper restore All

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
}