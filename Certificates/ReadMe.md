# Certificates

Certificates used in this solution.

**IMPORTANT**

All these certificates are not "real" certificates. They should only be used when running this solution in Visual Studio, not when deploying. If you do not want to trust them in you Windows certificate store thats fine. But then you have to create your own certificates.

## 1 Client certificates

Import them to *CERT:\\CurrentUser\\My* on your development-machine if you want to authenticate.

- **siths-hsa-alice.p12**
- **siths-hsa-bob.p12**
- **siths-person-alice.p12**
- **siths-person-bob.p12**
- **test-siths-hsa-alice.p12**
- **test-siths-hsa-bob.p12**
- **test-siths-person-alice.p12**
- **test-siths-person-bob.p12**

## 2 Https certificates

Mount in the Docker-container and use in appsettings.json to configure the https-certificates.

- **certificate-id.example.local.crt**
- **certificate-id.example.local.key**
- **siths-hsa-id.example.local.crt**
- **siths-hsa-id.example.local.key**
- **siths-person-id.example.local.crt**
- **siths-person-id.example.local.key**
- **test-siths-hsa-id.example.local.crt**
- **test-siths-hsa-id.example.local.key**
- **test-siths-person-id.example.local.crt**
- **test-siths-person-id.example.local.key**

## 3 Root and intermediate certificates

Trust in the Docker-container.

- **https-root.crt** - if you want to avoid a certificate trust warning in the browser you can import it to *CERT:\\CurrentUser\\Root* or *CERT:\\LocalMachine\\Root* on your development-machine
- **siths-hsa-intermediate.crt**
- **siths-person-intermediate.crt**
- **siths-root.crt**
- **test-siths-hsa-intermediate.crt**
- **test-siths-person-intermediate.crt**
- **test-siths-root.crt**

### 3.1 Kestrel-Development, Kestrel on Windows

If you are running Kestrel-Development, Kestrel on Windows, you need further imports to the certificate-store on your development-machine.

Imports to *CERT:\\CurrentUser\\Root* or *CERT:\\LocalMachine\\Root*:

- siths-root.crt
- test-siths-root.crt

Imports to *CERT:\\CurrentUser\\CA* or *CERT:\\LocalMachine\\CA* (StoreName.CertificateAuthority):

- siths-hsa-intermediate.crt
- siths-person-intermediate.crt
- test-siths-hsa-intermediate.crt
- test-siths-person-intermediate.crt

#### 3.1.1 Create custom certificate-stores for your trust-lists

You create custom certificate stores on Windows by using Powershell, you need to run Powershell as administrator, run as administrator.

#### 3.1.1.1 Create the store "Certificate-Identity-Siths-HSA-Trust"

	New-Item -Path "CERT:\LocalMachine\Certificate-Identity-Siths-HSA-Trust"

Import the following certificate to this store:

- siths-hsa-intermediate.crt

#### 3.1.1.2 Create the store "Certificate-Identity-Siths-Person-Trust"

	New-Item -Path "CERT:\LocalMachine\Certificate-Identity-Siths-Person-Trust"

Import the following certificate to this store:

- siths-person-intermediate.crt

#### 3.1.1.3 Create the store "Certificate-Identity-Test-Siths-HSA-Trust"

	New-Item -Path "CERT:\LocalMachine\Certificate-Identity-Test-Siths-HSA-Trust"

Import the following certificate to this store:

- test-siths-hsa-intermediate.crt

#### 3.1.1.4 Create the store "Certificate-Identity-Test-Siths-Person-Trust"

	New-Item -Path "CERT:\LocalMachine\Certificate-Identity-Test-Siths-Person-Trust"

Import the following certificate to this store:

- test-siths-person-intermediate.crt