$cert = New-SelfSignedCertificate -DnsName @("testyarpmaster.dev", "testyarpdetail.dev", "host.docker.internal", "localhost") -CertStoreLocation "cert:\LocalMachine\My"
$rootCertPathPfx = "$env:USERPROFILE/.aspnet/https/testyarpcert.pfx"
$password = ConvertTo-SecureString '123123' -AsPlainText -Force
$cert | Export-PfxCertificate -FilePath $rootCertPathPfx -Password $password
$rootCert = $(Import-PfxCertificate -FilePath $rootCertPathPfx -CertStoreLocation 'Cert:\LocalMachine\Root' -Password $password)