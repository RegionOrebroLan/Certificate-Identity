using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.CertificateIdentity.ComponentModel;

namespace UnitTests.ComponentModel
{
	[TestClass]
	public class TypeTypeConverterTest
	{
		#region Methods

		[TestMethod]
		public async Task CanConvertFrom_IfTheSourceTypeParameterIsString_ShouldReturnTrue()
		{
			await Task.CompletedTask;

			Assert.IsTrue(new TypeTypeConverter().CanConvertFrom(typeof(string)));
		}

		[TestMethod]
		public async Task CanConvertTo_IfTheDestinationTypeParameterIsString_ShouldReturnTrue()
		{
			await Task.CompletedTask;

			Assert.IsTrue(new TypeTypeConverter().CanConvertTo(typeof(string)));
		}

		[TestMethod]
		public async Task ConvertFrom_IfTheValueParameterIsAnEmptyString_ShouldReturnNull()
		{
			await Task.CompletedTask;

			Assert.IsNull(new TypeTypeConverter().ConvertFrom(string.Empty));
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public async Task ConvertFrom_IfTheValueParameterIsNotAString_ShouldThrowANotSupportedException()
		{
			await Task.CompletedTask;

			new TypeTypeConverter().ConvertFrom(5);
		}

		[TestMethod]
		public async Task ConvertFrom_IfTheValueParameterIsNull_ShouldReturnNull()
		{
			await Task.CompletedTask;

			Assert.IsNull(new TypeTypeConverter().ConvertFrom(null));
		}

		[TestMethod]
		public async Task ConvertFrom_Test()
		{
			await Task.CompletedTask;

			var typeConverter = new TypeTypeConverter();

			// String (primitive type)
			var type = typeof(string);
			var text = type.FullName;
			Assert.AreEqual(type, typeConverter.ConvertFrom(text));

			text = $"{type.FullName}, {type.Assembly.GetName().Name}";
			Assert.AreEqual(type, typeConverter.ConvertFrom(text));

			text = type.AssemblyQualifiedName;
			Assert.AreEqual(type, typeConverter.ConvertFrom(text));

			// This type. Full name without assembly name will not work.
			type = this.GetType();

			text = $"{type.FullName}, {type.Assembly.GetName().Name}";
			Assert.AreEqual(type, typeConverter.ConvertFrom(text));

			text = type.AssemblyQualifiedName;
			Assert.AreEqual(type, typeConverter.ConvertFrom(text));
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public async Task ConvertTo_IfTheDestinationTypeParameterIsNotString_ShouldThrowANotSupportedException()
		{
			await Task.CompletedTask;

			new TypeTypeConverter().ConvertTo(typeof(string), typeof(object));
		}

		[TestMethod]
		public async Task ConvertTo_String_Test()
		{
			await Task.CompletedTask;

			var stringType = typeof(string);

			Assert.AreEqual($"{stringType.FullName}, {stringType.Assembly.GetName().Name}", new TypeTypeConverter().ConvertTo(typeof(string), typeof(string)));

			Assert.AreEqual(stringType.AssemblyQualifiedName, new TypeTypeConverter { UseAssemblyQualifiedName = true }.ConvertTo(typeof(string), typeof(string)));
		}

		[TestMethod]
		public async Task UseAssemblyQualifiedName_ShouldReturnFalseByDefault()
		{
			await Task.CompletedTask;

			Assert.IsFalse(new TypeTypeConverter().UseAssemblyQualifiedName);
		}

		#endregion
	}
}