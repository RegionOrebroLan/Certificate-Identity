using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.CertificateIdentity.Data;

namespace UnitTests.Data
{
	[TestClass]
	public class SqlServerProviderTest
	{
		#region Methods

		[TestMethod]
		public async Task Prerequisite_Windows_Test()
		{
			await Task.CompletedTask;

			if(!OperatingSystem.IsWindows())
			{
				Assert.Inconclusive("Tests exists only for windows-host.");
				return;
			}

			var basePath = @"C:\";
			var path = "Data/IdentityServer.mdf";
			var fullPath = Path.GetFullPath(path, basePath);
			var expectedPath = @"C:\Data\IdentityServer.mdf";
			Assert.AreEqual(expectedPath, fullPath);

			basePath = @"C:\";
			path = @"Data\IdentityServer.mdf";
			fullPath = Path.GetFullPath(path, basePath);
			expectedPath = @"C:\Data\IdentityServer.mdf";
			Assert.AreEqual(expectedPath, fullPath);

			basePath = @"C:\";
			path = "/Data/IdentityServer.mdf";
			fullPath = Path.GetFullPath(path, basePath);
			expectedPath = @"C:\Data\IdentityServer.mdf";
			Assert.AreEqual(expectedPath, fullPath);

			basePath = @"C:\";
			path = @"\Data\IdentityServer.mdf";
			fullPath = Path.GetFullPath(path, basePath);
			expectedPath = @"C:\Data\IdentityServer.mdf";
			Assert.AreEqual(expectedPath, fullPath);

			basePath = @"C:\";
			path = "D:/Data/IdentityServer.mdf";
			fullPath = Path.GetFullPath(path, basePath);
			expectedPath = @"D:\Data\IdentityServer.mdf";
			Assert.AreEqual(expectedPath, fullPath);

			basePath = @"C:\";
			path = @"D:\Data\IdentityServer.mdf";
			fullPath = Path.GetFullPath(path, basePath);
			expectedPath = path;
			Assert.AreEqual(expectedPath, fullPath);
		}

		[TestMethod]
		public async Task ResolveConnectionString_Windows_Test()
		{
			await Task.CompletedTask;

			if(!OperatingSystem.IsWindows())
			{
				Assert.Inconclusive("Tests exists only for windows-host.");
				return;
			}

			var sqlServerProvider = new SqlServerProvider();

			var hostEnvironmentMock = new Mock<IHostEnvironment>();
			hostEnvironmentMock.Setup(hostEnvironment => hostEnvironment.ContentRootPath).Returns(@"C:\Test");
			var hostEnvironment = hostEnvironmentMock.Object;

			const string connectionStringFormat = @"AttachDbFileName={0};Integrated Security=True;Server=(LocalDB)\MSSQLLocalDB";

			var databaseFilePath = "Data/IdentityServer.mdf";
			var connectionString = string.Format(connectionStringFormat, databaseFilePath);
			var expectedConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Test\Data\IdentityServer.mdf;Initial Catalog=C:\Test\Data\IdentityServer.mdf;Integrated Security=True";
			var resolvedConnectionString = sqlServerProvider.ResolveConnectionString(connectionString, hostEnvironment);
			Assert.AreEqual(expectedConnectionString, resolvedConnectionString);

			databaseFilePath = @"Data\IdentityServer.mdf";
			connectionString = string.Format(connectionStringFormat, databaseFilePath);
			resolvedConnectionString = sqlServerProvider.ResolveConnectionString(connectionString, hostEnvironment);
			Assert.AreEqual(expectedConnectionString, resolvedConnectionString);

			databaseFilePath = @"/Data/IdentityServer.mdf";
			connectionString = string.Format(connectionStringFormat, databaseFilePath);
			expectedConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Data\IdentityServer.mdf;Initial Catalog=C:\Data\IdentityServer.mdf;Integrated Security=True";
			resolvedConnectionString = sqlServerProvider.ResolveConnectionString(connectionString, hostEnvironment);
			Assert.AreEqual(expectedConnectionString, resolvedConnectionString);

			databaseFilePath = @"\Data\IdentityServer.mdf";
			connectionString = string.Format(connectionStringFormat, databaseFilePath);
			resolvedConnectionString = sqlServerProvider.ResolveConnectionString(connectionString, hostEnvironment);
			Assert.AreEqual(expectedConnectionString, resolvedConnectionString);
		}

		#endregion
	}
}