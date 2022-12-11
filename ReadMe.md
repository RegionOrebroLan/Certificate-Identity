# Certificate-Identity

Library to use when setting up an IdentityServer for interactive client certificate authentication. The idea is that the only connected client will be your "real" IdentityServer. The repository contains an [implementation](/Source/Implementation/Application/) and an [IdentityServer](/Clients/IdentityServer/) acting as client.

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.CertificateIdentity.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.CertificateIdentity)

**License required for production**

- https://duendesoftware.com/products/identityserver#pricing

**NOTE**

SslCertificateTrust with Kestrel on Windows not working yet. The Kestrel launch-profile is disabled.

- [Kestrel-profile disabled in launchSettings.json](/Source/Implementation/Application/Properties/launchSettings.json#L14)

## 1 Technicalities

### 1.1 .NET 7.0

The reason for using net7.0 is the SslCertificateTrust functionality.

## 2 Development

### 2.1 Run in Visual Studio

#### 2.1.1 Docker

Docker Desktop is required.

#### 2.1.2 Host names

Running on Windows requires the following in your C:\Windows\System32\drivers\etc\hosts file:

	127.0.0.1 certificate-id.example.local
	127.0.0.2 siths-hsa-id.example.local
	127.0.0.3 siths-person-id.example.local
	127.0.0.4 test-siths-hsa-id.example.local
	127.0.0.5 test-siths-person-id.example.local

#### 2.1.3 Links

##### 2.1.3.1 Docker

- [https://certificate-id.example.local:50000](https://certificate-id.example.local:50000)
- [https://siths-hsa-id.example.local:50000](https://siths-hsa-id.example.local:50000)
- [https://siths-person-id.example.local:50000](https://siths-person-id.example.local:50000)
- [https://test-siths-hsa-id.example.local:50000](https://test-siths-hsa-id.example.local:50000)
- [https://test-siths-person-id.example.local:50000](https://test-siths-person-id.example.local:50000)

##### 2.1.3.2 Kestrel

- [https://certificate-id.example.local:5000](https://certificate-id.example.local:5000)
- [https://siths-hsa-id.example.local:5000](https://siths-hsa-id.example.local:5000)
- [https://siths-person-id.example.local:5000](https://siths-person-id.example.local:5000)
- [https://test-siths-hsa-id.example.local:5000](https://test-siths-hsa-id.example.local:5000)
- [https://test-siths-person-id.example.local:5000](https://test-siths-person-id.example.local:5000)

### 2.2 Swedish personal identity numbers for testing (test­­personnummer)

Swedish personal identity numbers for testing can be found here:

- [Test­­personnummer](https://www7.skatteverket.se/portal/apier-och-oppna-data/utvecklarportalen/oppetdata/Test%C2%AD%C2%ADpersonnummer)

Used in this solution:

- Woman: 189912289809, 189912309805
- Man: 189912299816, 189912319812

### 2.3 Windows Registry - regedit

If you run the launch-profile "Kestrel" you need to set some keys in the registry correctly:

- HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\ClientAuthTrustMode = 0
- HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\SendTrustedIssuerList = 1

Links:

- https://learn.microsoft.com/en-us/windows-server/security/tls/what-s-new-in-tls-ssl-schannel-ssp-overview

### 2.4 Certificates

If you run the launch-profile "Kestrel" you need to prepare your certificate-store for it:

- [To do before running Kestrel on Windows](/Certificates/ReadMe.md#L49)

Password for p12-files in this solution: **password**

The certificates in this solution are created by using this web-application, [Certificate-Factory](https://github.com/HansKindberg-Lab/Certificate-Factory). It is a web-application you can run in Visual Studio and then upload a json-certificate-file.

The json-certificate-file used for this solution:

	{
		"Defaults": {
			"HashAlgorithm": "Sha256",
			"NotAfter": "2050-01-01"
		},
		"Roots": {
			"https-root": {
				"Certificate": {
					"Subject": "CN=Certificate-Identity Https Root CA"
				},
				"IssuedCertificates": {
					"certificate-id.example.local": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Https certificate-id.example.local",
							"SubjectAlternativeName": {
								"DnsNames": [
									"certificate-id.example.local"
								]
							}
						}
					},
					"siths-hsa-id.example.local": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Https siths-hsa-id.example.local",
							"SubjectAlternativeName": {
								"DnsNames": [
									"siths-hsa-id.example.local"
								]
							}
						}
					},
					"siths-person-id.example.local": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Https siths-person-id.example.local",
							"SubjectAlternativeName": {
								"DnsNames": [
									"siths-person-id.example.local"
								]
							}
						}
					},
					"test-siths-hsa-id.example.local": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Https test-siths-hsa-id.example.local",
							"SubjectAlternativeName": {
								"DnsNames": [
									"test-siths-hsa-id.example.local"
								]
							}
						}
					},
					"test-siths-person-id.example.local": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Https test-siths-person-id.example.local",
							"SubjectAlternativeName": {
								"DnsNames": [
									"test-siths-person-id.example.local"
								]
							}
						}
					}
				},
				"IssuedCertificatesDefaults": {
					"EnhancedKeyUsage": "ServerAuthentication",
					"KeyUsage": "DigitalSignature"
				}
			},
			"siths-root": {
				"Certificate": {
					"Subject": "CN=Certificate-Identity Siths Root CA"
				},
				"IssuedCertificates": {
					"siths-hsa-intermediate": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Siths HSA Intermediate CA"
						},
						"IssuedCertificates": {
							"siths-hsa-alice": {
								"Certificate": {
									"Subject": "C=SE,CN=Alice Smith,E=alice.smith@example.local,G=Alice,L=County,O=Company,SERIALNUMBER=FAKE123456-e001,SN=Smith",
									"SubjectAlternativeName": {
										"EmailAddresses": [
											"alice.smith@example.local"
										],
										"UserPrincipalNames": [
											"alice.smith@example.local"
										]
									}
								}
							},
							"siths-hsa-bob": {
								"Certificate": {
									"Subject": "C=SE,CN=Bob Smith,E=bob.smith@example.local,G=Bob,L=County,O=Company,SERIALNUMBER=FAKE123456-e002,SN=Smith",
									"SubjectAlternativeName": {
										"EmailAddresses": [
											"bob.smith@example.local"
										],
										"UserPrincipalNames": [
											"bob@domain.local"
										]
									}
								}
							}
						},
						"IssuedCertificatesDefaults": {
							"EnhancedKeyUsage": "ClientAuthentication",
							"KeyUsage": "DigitalSignature"
						}
					},
					"siths-person-intermediate": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Siths Person Intermediate CA"
						},
						"IssuedCertificates": {
							"siths-person-alice": {
								"Certificate": {
									"Subject": "C=SE,CN=Alice Smith,G=Alice,L=County,O=Company,SERIALNUMBER=189912289809,SN=Smith"
								}
							},
							"siths-person-bob": {
								"Certificate": {
									"Subject": "C=SE,CN=Bob Smith,G=Bob,L=County,O=Company,SERIALNUMBER=189912299816,SN=Smith"
								}
							}
						},
						"IssuedCertificatesDefaults": {
							"EnhancedKeyUsage": "ClientAuthentication",
							"KeyUsage": "DigitalSignature"
						}
					}
				},
				"IssuedCertificatesDefaults": {
					"CertificateAuthority": {
						"PathLengthConstraint": 0
					},
					"KeyUsage": "KeyCertSign"
				}
			},
			"test-siths-root": {
				"Certificate": {
					"Subject": "CN=Certificate-Identity Test Siths Root CA"
				},
				"IssuedCertificates": {
					"test-siths-hsa-intermediate": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Test Siths HSA Intermediate CA"
						},
						"IssuedCertificates": {
							"test-siths-hsa-alice": {
								"Certificate": {
									"Subject": "C=SE,CN=Alice Smith,E=alice.smith@example.local,G=Alice,L=County,O=Company,SERIALNUMBER=TEST123456-e001,SN=Smith",
									"SubjectAlternativeName": {
										"EmailAddresses": [
											"alice.smith@example.local"
										],
										"UserPrincipalNames": [
											"alice.smith@example.local"
										]
									}
								}
							},
							"test-siths-hsa-bob": {
								"Certificate": {
									"Subject": "C=SE,CN=Bob Smith,E=bob.smith@example.local,G=Bob,L=County,O=Company,SERIALNUMBER=TEST123456-e002,SN=Smith",
									"SubjectAlternativeName": {
										"EmailAddresses": [
											"bob.smith@example.local"
										],
										"UserPrincipalNames": [
											"bob@domain.local"
										]
									}
								}
							}
						},
						"IssuedCertificatesDefaults": {
							"EnhancedKeyUsage": "ClientAuthentication",
							"KeyUsage": "DigitalSignature"
						}
					},
					"test-siths-person-intermediate": {
						"Certificate": {
							"Subject": "CN=Certificate-Identity Test Siths Person Intermediate CA"
						},
						"IssuedCertificates": {
							"test-siths-person-alice": {
								"Certificate": {
									"Subject": "C=SE,CN=Alice Smith,G=Alice,L=County,O=Company,SERIALNUMBER=189912309805,SN=Smith"
								}
							},
							"test-siths-person-bob": {
								"Certificate": {
									"Subject": "C=SE,CN=Bob Smith,G=Bob,L=County,O=Company,SERIALNUMBER=189912319812,SN=Smith"
								}
							}
						},
						"IssuedCertificatesDefaults": {
							"EnhancedKeyUsage": "ClientAuthentication",
							"KeyUsage": "DigitalSignature"
						}
					}
				},
				"IssuedCertificatesDefaults": {
					"CertificateAuthority": {
						"PathLengthConstraint": 0
					},
					"KeyUsage": "KeyCertSign"
				}
			}
		},
		"RootsDefaults": {
			"CertificateAuthority": {
				"PathLengthConstraint": null
			},
			"KeyUsage": "KeyCertSign"
		}
	}

### 2.5 Migrations

We might want to create/recreate migrations. If we can accept data-loss we can recreate the migrations otherwhise we will have to update them.

Copy all the commands below and run them in the Package Manager Console for the affected database-context.

If you want more migration-information you can add the -Verbose parameter:

	Add-Migration TheMigration -Context TheDatabaseContext -OutputDir Data/Migrations -Project Project -StartupProject Project -Verbose;

#### 2.5.1 DataProtection

##### 2.5.1.1 Create migrations

	Write-Host "Removing migrations...";
	Remove-Migration -Context SqliteDataProtection -Force -Project Project -StartupProject Project;
	Remove-Migration -Context SqlServerDataProtection -Force -Project Project -StartupProject Project;
	Write-Host "Removing current migrations-directory...";
	Remove-Item "Project\DataProtection\Data\Migrations" -ErrorAction Ignore -Force -Recurse;
	Write-Host "Creating migrations...";
	Add-Migration DataProtection -Context SqliteDataProtection -OutputDir DataProtection/Data/Migrations/Sqlite -Project Project -StartupProject Project;
	Add-Migration DataProtection -Context SqlServerDataProtection -OutputDir DataProtection/Data/Migrations/SqlServer -Project Project -StartupProject Project;
	Write-Host "Finnished";

##### 2.5.1.2 Update migrations

	Write-Host "Updating migrations...";
	Add-Migration DataProtection -Context SqliteDataProtection -OutputDir DataProtection/Data/Migrations/Sqlite -Project Project -StartupProject Project;
	Add-Migration DataProtection -Context SqlServerDataProtection -OutputDir DataProtection/Data/Migrations/SqlServer -Project Project -StartupProject Project;
	Write-Host "Finnished";

#### 2.5.2 Operational (PersistedGrant)

##### 2.5.2.1 Create migrations

	Write-Host "Removing migrations...";
	Remove-Migration -Context SqliteOperational -Force -Project Project -StartupProject Project;
	Remove-Migration -Context SqlServerOperational -Force -Project Project -StartupProject Project;
	Write-Host "Removing current migrations-directory...";
	Remove-Item "Project\Data\Migrations\Operational" -ErrorAction Ignore -Force -Recurse;
	Write-Host "Creating migrations...";
	Add-Migration Operational -Context SqliteOperational -OutputDir Data/Migrations/Operational/Sqlite -Project Project -StartupProject Project;
	Add-Migration Operational -Context SqlServerOperational -OutputDir Data/Migrations/Operational/SqlServer -Project Project -StartupProject Project;
	Write-Host "Finnished";

##### 2.5.2.2 Update migrations

	Write-Host "Updating migrations...";
	Add-Migration Operational -Context SqliteOperational -OutputDir Data/Migrations/Operational/Sqlite -Project Project -StartupProject Project;
	Add-Migration Operational -Context SqlServerOperational -OutputDir Data/Migrations/Operational/SqlServer -Project Project -StartupProject Project;
	Write-Host "Finnished";

### 2.6 Versioning

The version should follow the Duende.IdentityServer version. The Duende.IdentityServer version with a fourth revision number at the end. So the version have the format *major.minor.build.revision*.

**Important**: The revision-number can not be zero (0), it must be greater than zero. If the revision number is zero it is not included.

#### 2.6.1 Example 1

Example without prereleases.

1. Duende.IdentityServer 6.2.0 is released
2. We create a new version 6.2.0.1
3. If we need a fix we create version 6.2.0.2
4. Next fix will be 6.2.0.3 etc.
5. Duende.IdentityServer 6.2.1 is released
6. We create a new version 6.2.1.1 etc.

#### 2.6.2 Example 2

Example with prereleases.

1. Duende.IdentityServer 6.2.0 is released
2. We create a new version 6.2.0.1-alpha
3. If we need a fix we create version 6.2.0.1-alpha.2
4. Next fix will be 6.2.0.1-alpha.3 etc.
5. When 6.2.0.1 is stable we create release 6.2.0.1
6. Duende.IdentityServer 6.2.1 is released
7. We create a new version 6.2.1.1-alpha etc.