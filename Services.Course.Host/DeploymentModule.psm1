# Deployment Module v0.1.60

$Script:ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$Script:BaseSoftwareDownloadUri = "https://scmsoftware.parabolasolutions.com"
$Script:AuthToken = "c2Ntc29mdHdhcmVpbnN0YWxsOmpFaGF0NGYmU3c="

#region IIS Functions

function ConfigureIIS
{
	$configIIS = $env:OctopusConfigureIIS
	if ($configIIS -and $configIIS -ieq "false") { return $False }
	return $True
}

function Deployment-CheckConfigureIIS
{
	if (-not (ConfigureIIS)) 
	{ 
		Write-Host "***** Skipping IIS configuration because the environment variable `"OctopusConfigureIIS`" has been set to `"false`" on the target deployment machine."
	}
}

function Deployment-RemoveHandlerMapping
{
	param(
		[string]$handlerName = $(Throw 'handler name is required'),
		[string]$websiteName = $(Throw 'website name is required')
	)
	
	if (-not (ConfigureIIS)) {return}

	Import-Module WebAdministration
	# find handler mapping
	Write-Host "Checking existing handler mapping `"$handlerName`""
	$handler = Get-WebConfiguration -PSPath "IIS:\Sites\$websiteName" -Filter "/system.webServer/handlers/add[@name='$handlerName']"	
	if ($handler -ne $null)
	{
		Write-Host "Removing handler mapping `"$handlerName`""
		Remove-WebHandler -Name "$handlerName" -PSPath "IIS:\Sites\$websiteName"
	}
}

function Deployment-SetAppPoolProperty
{
	param(
		[string]$appPoolName = $(Throw 'app pool name is required'),
		[string]$appPoolProperty,
		[object]$appPoolPropertyValue,
		[object]$compareValue,
		[switch]$hide
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	# find app pool
	$appPool = Get-Item -Path "IIS:\AppPools\$appPoolName" -ErrorAction SilentlyContinue
	if ($appPool)
	{
		Write-Host "Checking existing app pool `"$appPoolName`" property `"$appPoolProperty`""
		# get the existing property value
		$existingValue = Get-ItemProperty -Path "IIS:\AppPools\$appPoolName" -Name $appPoolProperty
		$updateValue = $False
		# compare values
		if ($compareValue -and $compareValue -ne $existingValue) { $updateValue = $True }
		if (-not $compareValue -and $existingValue -ne $appPoolPropertyValue) { $updateValue = $True }
		if ($updateValue)
		{
			if ($hide)
			{
				Write-Host "Updating app pool `"$appPoolName`" property `"$appPoolProperty`""
			}
			else
			{
				if ($compareValue)
				{
					Write-Host "Updating app pool `"$appPoolName`" property `"$appPoolProperty`" from `"$existingValue`" to `"$compareValue`""
				}
				else
				{
					Write-Host "Updating app pool `"$appPoolName`" property `"$appPoolProperty`" from `"$existingValue`" to `"$appPoolPropertyValue`""
				}
			}
			# update property value
			Set-ItemProperty -Path "IIS:\AppPools\$appPoolName" -Name $appPoolProperty -Value $appPoolPropertyValue
		}
	}
	else
	{
		$(Throw "An existing app pool with name `"$appPoolName`" was not found")
	}
}

function Deployment-SetWebConfigurationProperty
{
	param(
		[string]$webConfiguration,
		[string]$webConfigurationProperty,
		[object]$webConfigurationPropertyValue,
		[object]$compareValue,
		[switch]$hide
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration

	Write-Host "Checking existing web configuration `"$webConfiguration`" property `"$webConfigurationProperty`""
	# get the existing property value
	$existingValue = Get-WebConfigurationProperty -Filter "$webConfiguration" -Name "$webConfigurationProperty" -ErrorAction SilentlyContinue
	if ($existingValue)
	{
		$updateValue = $False
		# compare values
		if ($compareValue -and $compareValue -ne ($existingValue.Value)) { $updateValue = $True }
		if (-not $compareValue -and ($existingValue.Value) -ne $webConfigurationPropertyValue) { $updateValue = $True }
		if ($updateValue)
		{
			if ($hide)
			{
				Write-Host "Updating web configuration `"$webConfiguration`" property `"$webConfigurationProperty`""
			}
			else
			{
				if ($compareValue)
				{
					Write-Host "Updating web configuration `"$webConfiguration`" property `"$webConfigurationProperty`" from `"$($existingValue.Value)`" to `"$compareValue`""
				}
				else
				{
					Write-Host "Updating web configuration `"$webConfiguration`" property `"$webConfigurationProperty`" from `"$($existingValue.Value)`" to `"$webConfigurationPropertyValue`""
				}
			}
			# update property value
			Set-WebConfigurationProperty -Filter "$webConfiguration" -Name "$webConfigurationProperty" -Value $webConfigurationPropertyValue
		}
	}
	else
	{
		$(Throw "An existing web configuration with filter `"$webConfiguration`" was not found")
	}
}

function Deployment-SetWebsiteBinding
{
	param(
		[string]$websiteName = $(Throw 'website name is required'),
		[string]$protocol,
		[string]$IPAddress, 
		[string]$port,
		[string]$hostheader
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	$bindingString = "$($IPAddress):$($port):$($hostheader)"
	
	# find website
	$website = Get-Item "IIS:\Sites\$websiteName" -ErrorAction SilentlyContinue
	if ($website)
	{
		if ($hostHeader.Contains(";"))
		{
			$hostHeaderArray = $hostHeader.Split(';')
			ForEach($tmpHostHeader in $hostHeaderArray)
			{
				$tmpBindingString = "$($IPAddress):$($port):$($tmpHostHeader)"
				Write-Host "Checking existing website `"$websiteName`" binding"
				# get the existing binding
				$existingBinding = Get-WebBinding -Name $websiteName -Protocol $protocol | Where {$_.BindingInformation -eq $tmpBindingString}
				if (-not $existingBinding )
				{
					Write-Host "Creating website `"$websiteName`" binding `"$protocol $($IPAddress.ToString())`:$port`:$tmpHostHeader`""
					if ($IPAddress)
					{
						New-WebBinding -Name $websiteName -Protocol $protocol -IPAddress $IPAddress -Port $port -HostHeader $tmpHostHeader						
					}
					else
					{
						New-WebBinding -Name $websiteName -Protocol $protocol -Port $port -HostHeader $tmpHostHeader
					}
				}
			}
		}
		else
		{
			Write-Host "Checking existing website `"$websiteName`" binding"
			# get the existing binding
			$existingBindings = Get-WebBinding -Name $websiteName -Protocol $protocol -ErrorAction SilentlyContinue 
			if ($existingBindings -ne $null)
			{
				$existingBinding = $existingBindings | Where {$_.BindingInformation -eq $bindingString}
				if (-not $existingBinding )
				{
					Write-Host "Creating website `"$websiteName`" binding `"$protocol $($IPAddress.ToString())`:$port`:$hostheader`""
					if ($IPAddress)
					{
						New-WebBinding -Name $websiteName -Protocol $protocol -IPAddress $IPAddress -Port $port -HostHeader $hostheader
					}
					else
					{
						New-WebBinding -Name $websiteName -Protocol $protocol -Port $port -HostHeader $hostheader
					}
				}
			}
		}
	}
	else
	{
		$(Throw "An existing website with name `"$websiteName`" was not found")
	}
}

function Deployment-AssociateSSLCertificateWithBinding
{
	param(
		[string]$certThumbprint,
		[string]$certStoreScope,
		[string]$certStoreName,
		[string]$bindingPath
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	$cert = Get-ChildItem cert:\$certStoreScope\$certStoreName | Where-Object {$_.Thumbprint -eq $certThumbprint}
	if ($cert)
	{
		$existingBinding = Get-ChildItem -Path "IIS:\SslBindings\$bindingPath" -ErrorAction SilentlyContinue
		if (-not $existingBinding)
		{
			New-Item -Path "IIS:\SslBindings\$bindingPath" -Value $cert
			Write-Host "SSL binding created for certificate with subject `"$($cert.Subject)`" and thumbprint `"$($cert.Thumbprint)`""
		}
	}
	else
	{
		Throw "Unable to locate certificate with thumbprint `"$certThumbprint`" in `"$certStoreScope\$certStoreName`""
	}
}

function Deployment-RemoveWebsiteBinding
{
	param(
		[string]$websiteName = $(Throw 'website name is required'),
		[string]$protocol,
		[string]$IPAddress, 
		[string]$port,
		[string]$hostheader
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	$bindingString = "$($IPAddress):$($port):$($hostheader)"
	
	# find website
	$website = Get-Item "IIS:\Sites\$websiteName" -ErrorAction SilentlyContinue
	if ($website)
	{
		Write-Host "Checking existing website binding"
		# get the existing binding
		$existingBinding = Get-WebBinding -Name $websiteName -Protocol $protocol | Where {$_.BindingInformation -eq $bindingString}
		if ($existingBinding)
		{
			Write-Host "Removing website `"$websiteName`" binding `"$protocol $($IPAddress.ToString())`:$port`:$hostheader`""
			Remove-WebBinding -Name $websiteName -Protocol $protocol -IPAddress $IPAddress -Port $port -HostHeader $hostheader -ErrorAction SilentlyContinue
		}
		#Write-Host "All website bindings"
		#Get-WebBinding -Name $websiteName
	}
	else
	{
		$(Throw "An existing website with name `"$websiteName`" was not found")
	}
}

function Deployment-SetupAppPool
{
	param(
		[string]$appPoolName = $(throw 'app pool name is required')
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	Write-Host "Checking for existing app pool `"$appPoolName`""
	# find app pool
	$appPool = Get-Item "IIS:\AppPools\$appPoolName" -ErrorAction SilentlyContinue
	if (-not $appPool)
	{
		# app pool was not found, create the app pool
		Write-Host "Creating new app pool `"$appPoolName`""
		New-WebAppPool -Name $appPoolName -Force | Out-Null
	}
}

function Deployment-SetupIISLogDirectory
{
	param(
		[string]$websiteName = $(throw 'website name is required'),
		[string]$iisLogDirectory = $(throw 'iis log directory is required')
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	
	Write-Host "Checking IIS log path"
	# get existing log directory
	$existingLogDir = (Get-ItemProperty "IIS:\Sites\$websiteName").logFile.directory
	if ($existingLogDir -ne $iisLogDirectory)
	{
		# check to see if the log directory exists
		if (-not (Test-Path $iisLogDirectory))
		{
			Write-Host "Creating directory $iisLogDirectory"
			New-Item -ItemType directory -Path $iisLogDirectory | Out-Null
		}
		
		# app pool was not found, create the app pool
		Write-Host "Updating IIS log path to `"$iisLogDirectory`""
		$loggingProperty = Get-ItemProperty "IIS:\Sites\$websiteName"
		$loggingProperty.logFile.directory = "$iisLogDirectory"
		$loggingProperty | set-item
	}
}

function Deployment-SetupIISLoggingFields
{
	param(
		[string]$websiteName = $(throw 'website name is required'),
		[array]$iisLoggingFields = $(throw 'array of iis log fields is required')
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	
	Write-Host "Checking IIS logging fields"
	
	# build compare string
	$compareFields = ""
	foreach($field in $iisLoggingFields)
	{
		$compareFields += ",$field"
	}
	$compareFields = $compareFields.Substring(1)
	
	# get existing fields
	$existingFields = (Get-ItemProperty "IIS:\Sites\$websiteName").logFile.logExtFileFlags
	
	$testCompare = [string]::Compare($existingFields, $compareFields, $True)
	if ($testCompare -ne 0)
	{
		Write-Host "Updating IIS logging fields"
		$loggingProperty = Get-ItemProperty "IIS:\Sites\$websiteName"
		$loggingProperty.logFile.logExtFileFlags = $compareFields
		$loggingProperty | Set-Item
	}
}

function Deployment-RemoveDefaultWebSite
{
	$websiteName = "Default Web Site"
	
	if (-not (ConfigureIIS)) {return}
	
	#Import-Module ServerManager
	Import-Module WebAdministration
	
	# find the Default Web Site website
	Write-Host "Checking for existing website `"$websiteName`""
	Remove-Item "IIS:\Sites\$websiteName" -Force -Recurse -ErrorAction SilentlyContinue
}

function Deployment-SetupWebsite
{
	param(
		[string]$websiteName = $(throw 'website name is required'),
		[string]$websiteDirectoryPath = $(throw 'website directory path is required'),
		[string]$appPoolName = $(throw 'app pool name is required')
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	
	# make sure that the website directory exists
	if (-not (Test-Path "$websiteDirectoryPath"))
	{
		Write-Host "Creating directory `"$websiteDirectoryPath`""
		New-Item -ItemType Directory -Path "$websiteDirectoryPath" | Out-Null
	}
	
	# check for the app pool
	$appPool = Deployment-SetupAppPool $appPoolName
	Write-Host "Checking for existing website `"$websiteName`""
	
	# find the website
	$website = Get-Item "IIS:\Sites\$websiteName" -ErrorAction SilentlyContinue
	if (-not $website)
	{
		# website not found, create new website
		Write-Host "Creating new website `"$websiteName`""
		$sites = Get-Item "IIS:\Sites"
		if ($sites -eq $null -or ($sites.Count -eq 0))
		{
			# if fist site being created on the server then set the Id = 1
			New-Website -Name "$websiteName" -PhysicalPath "$websiteDirectoryPath" -ApplicationPool "$appPoolName" -Id 1 -Force | Out-Null
		}
		else
		{
			New-Website -Name "$websiteName" -PhysicalPath "$websiteDirectoryPath" -ApplicationPool "$appPoolName" -Force | Out-Null
		}
		Write-Host "A new website `"$websiteName`" has been created"
		$website = Get-Item "IIS:\Sites\$websiteName"
	}
	
	# check the app pool of the website
	if ($website -and $website.ApplicationPool -ne "$appPoolName")
	{
		Write-Host "Updating website `"$websiteName`" app pool from `"$($website.ApplicationPool)`" to `"$appPoolName`"" 
		Set-ItemProperty "IIS:\Sites\$websiteName" ApplicationPool "$appPoolName"
	}
}

function Deployment-SetupWebApplication
{
	param(
		[string]$websiteName = $(throw 'website name is required'),
		[string]$webApplicationName = $(throw 'web application name is required'),
		[string]$webApplicationDirectoryPath = $(throw 'web application directory path is required'),
		[string]$appPoolName
	)
	
	if (-not (ConfigureIIS)) {return}
	
	Import-Module WebAdministration
	
	# make sure that the webapp directory exists
	if (-not (Test-Path "$webApplicationDirectoryPath"))
	{
		Write-Host "Creating directory `"$webApplicationDirectoryPath`""
		New-Item -ItemType Directory -Path "$webApplicationDirectoryPath" | Out-Null
	}
	
	Write-Host "Checking for existing website `"$websiteName`""
	# find the website
	$website = Get-Item "IIS:\Sites\$websiteName" -ErrorAction SilentlyContinue
	if (-not $website)
	{
		Throw "Unable to locate website`"$websiteName`".  An existing website with the name of `"$websiteName`" is required to deploy this web application."
	}
	else
	{
		Write-Host "Checking for existing web application `"$webApplicationName`""
		$existingWebsiteAppPoolName = $website.ApplicationPool
		# find the webapp
		$webApp = Get-Item "IIS:\Sites\$websiteName\$webApplicationName" -ErrorAction SilentlyContinue
		if (-not $webApp)
		{
			if ($appPoolName)
			{
				Deployment-SetupAppPool "$appPoolName"
			}
			else
			{
				$appPoolName = $website.ApplicationPool
			}
			# webapp not found, create new webapp
			Write-Host "Creating new web application `"$webApplicationName`" under website `"$websiteName`""
			$webApp = New-WebApplication -Name "$webApplicationName" -PhysicalPath "$webApplicationDirectoryPath" -Site "$websiteName" -ApplicationPool "$appPoolName" -Force | Out-Null
		}
		else
		{
			if ($appPoolName -and $webApp.ApplicationPool -ne "$appPoolName")
			{
				Deployment-SetupAppPool "$appPoolName"
				Write-Host "Updating web application `"$webApplicationName`" app pool from `"$($webApp.ApplicationPool)`" to `"$appPoolName`"" 
				Set-ItemProperty "IIS:\Sites\$websiteName\$webApplicationName" ApplicationPool "$appPoolName"
			}
		}
	}
}

#endregion

#region Certificate Functions

<##
Certificate Store Information

Store Locations: 
	CurrentUser
	LocalMachine
	
Store Names:
	Personal = My
	Trusted Root Certification Authorities = Root
	Intermediate Certification Authorities = CertificationAuthority
	Trusted Publishers = TrustedPublisher
	Untrusted Certificates = Disallowed
	Third-party Root Certification Authorities = AuthRoot
	Trusted People = TrustedPeople
	Other People = AddressBook
##>

function Deployment-ImportCertificateFromPFX
{
	param(
		[string]$pfxPath,
		[string]$pfxPassword,
		[string]$certThumbprint,
		[string]$certStoreLocation,
		[string]$certStoreName
	)
	
	if (-not (ConfigureIIS)) {return}
	
	$importCert = $true
	$cert = $null
	if ($certThumbprint -ne $null)
	{
		$cert = Get-ChildItem cert:\$certStoreLocation\$certStoreName\* | Where-Object { $_.Thumbprint -eq $certThumbprint}
		if ($cert -ne $null)
		{
			$importCert = $false
		}
	}
	
	if ($importCert)
	{
		$pfxCert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
		$pfxCert.Import($pfxPath,$pfxPassword,"Exportable,PersistKeySet,MachineKeySet")
		$store = New-Object System.Security.Cryptography.X509Certificates.X509Store $certStoreName, $certStoreLocation
		$store.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)
		$store.Add($pfxCert)
		$store.Close()
		Write-Host "Certificate imported from `"$pfxPath`" into certificate store `"$certStoreLocation\$certStoreName`""
		Write-Host "Certificate will expire on $($pfxCert.NotAfter)"
		}
	else
	{
		Write-Host "Existing certificate found in `"$certStoreLocation\$certStoreName`" with subject `"$($cert.Subject)`" and thumbprint `"$certThumbprint`""
		Write-Host "Certificate will expire on $($cert.NotAfter)"
	}
}

function Deployment-LocateCertificate
{
	param(
		[string]$certSubject,
		[string]$certThumbprint,
		[string]$certStoreName,
		[string]$certStoreLocation,
		[string]$certPfx,
		[string]$certPfxPassword
	)
	
	if ($certSubject)
	{
		$cert = Deployment-GetCertificate -CertSubject $certSubject -CertStoreName $certStoreName -CertStoreLocation $certStoreLocation -CertPfx $certPfx -CertPfxPassword $certPfxPassword
		if (-not $cert)
		{
			Throw "Unable to locate valid certificate"
		}
	}
	
	if ($certThumbprint)
	{
		$cert = Deployment-GetCertificate -CertThumbprint $certThumbprint -CertStoreName $certStoreName -CertStoreLocation $certStoreLocation -CertPfx $certPfx -CertPfxPassword $certPfxPassword
		if (-not $cert)
		{
			Throw "Unable to locate valid certificate"
		}
	}
}

function Deployment-GetCertificate
{
	param(
		[string]$certSubject,
		[string]$certThumbprint,
		[string]$certStoreName,
		[string]$certStoreLocation,
		[string]$certPfx,
		[string]$certPfxPassword
	)
	
	# default unset store to *
	if( -not $certStoreName )
 	{
 		$certStoreName = '*'
 	}

	# default unset store location to *
	if( -not $certStoreLocation )
 	{
 		$certStoreLocation = '*'
 	}

	$certs = $null
	$cert = $null
	
	# find certificate by thumbprint
	if ($certThumbprint)
	{
		Write-Host "Checking certificate store `"$certStoreName`" located at `"$certStoreLocation`" for certificate with thumbprint `"$certThumbprint`""
		$certs = Get-ChildItem cert:\$certStoreLocation\$certStoreName\* | Where-Object { $_.Thumbprint -eq $certThumbprint }
	}
	
	# find certificate by subject
	if ($certSubject)
	{
		Write-Host "Checking certificate store `"$certStoreName`" located at `"$certStoreLocation`" for certificate with subject `"$certSubject`""
		if (-not $certSubject.StartsWith("CN"))
		{
			$certSubject = "CN=$certSubject"
		}
		$certs = Get-ChildItem cert:\$certStoreLocation\$certStoreName\* | Where-Object { $_.Subject -eq "$certSubject" }
	}
	
	if ($certs)
	{
		if ($certs.GetType().IsArray)
		{
			# find the certificate that is vaid with the longest expiration date
			$cert = $null
			foreach($tmpCert in $certs)
			{
				if ((Get-Date) -gt $tmpCert.NotBefore)
				{
					if ($cert -eq $null)
					{
						$cert = $tmpCert
					}
				
					# keep the cert with the higher expiration date
					if ($tmpCert.NotAfter -gt $cert.NotAfter)
					{
						$cert = $tmpCert
					}
				}
			}
		}
		else
		{
			# if the cert we found is valid then set the variable
			if ((Get-Date) -gt $certs.NotBefore -and (Get-Date) -lt $certs.NotAfter)
			{
				$cert = $certs
			}
		}
	}
	
	if ($cert)
	{
		return $cert
	}
	else
	{
		if ($certPfx -ne $null)
		{
			$pfxFile = Deployment-DownloadCertificate -CertPfx "$certPfx" -DownloadDirectory "C:\Temp\Certificates"
			Deployment-ImportCertificateFromPFX -PfxPath "$pfxFile" -PfxPassword "$certPfxPassword" -CertStoreLocation "$certStoreLocation" -CertStoreName "$certStoreName"
			Remove-Item $pfxFile -Force -ErrorAction SilentlyContinue
			return Deployment-GetCertificate -CertSubject $certSubject -CertThumbprint $CertThumbprint -CertStoreName $certStoreName -certStoreLocation $certStoreLocation
		}
		else
		{
			$errorMsg = "Unable to locate valid non-expired certificate in Store:$certStoreName at Store Location:$certStoreLocation"
			Throw $errorMsg
		}
	}
}


function Deployment-DownloadCertificate
{
	param(
		[string]$certPfx,
		[string]$downloadDirectory
	)
	
	if (-not (Test-Path "$downloadDirectory"))
	{
		New-Item -ItemType directory -Path "$downloadDirectory" | Out-Null
	}
	
	# certificate uir
	$uri = new-Object System.Uri "$($Script:BaseSoftwareDownloadUri)/Certificates/$certPfx"
	Write-Host "Downloading certificate ($uri)"
	
	# get the full download path
	$fileName = $uri.Segments[-1] 
	$downloadFile = Join-Path $downloadDirectory $fileName
	
	# delete the old downloaded file if it exists
	if (Test-Path $downloadFile)
	{
		Remove-Item $downloadFile -Force | Out-Null
	}
	
	# get the content from the software server
	$webClient = New-Object System.Net.WebClient
	$webClient.Headers.Add("Authorization","Basic $($Script:AuthToken)")
	$webClient.DownloadFile($uri, $downloadFile)
	
	return $downloadFile
}

function Deployment-SetupCertificatePermissions
{
	param(
		[string]$certSubject,
		[string]$certThumbprint,
		[string]$certStoreName,
		[string]$certStoreLocation,
		[string]$account,
		[string]$fileSystemRights,
		[string]$accessControlType
	)
	
	if ($certSubject)
	{
		Write-Host "Checking `"$fileSystemRights`" permissions on certificate in `"$certStoreLocation\$certStoreName`" with subject `"$certSubject`" for account `"$account`""
	}
	
	if ($certThumbprint)
	{
		Write-Host "Checking `"$fileSystemRights`" permissions on certificate in `"$certStoreLocation\$certStoreName`" with thumbprint `"$certThumbprint`" for account `"$account`""
	}
	
	# get the cert
	$cert = Deployment-GetCertificate -CertSubject $certSubject -CertThumbprint $certThumbprint -CertStoreName $certStoreName -CertStoreLocation $certStoreLocation
	
	# get the private key
	$certPrivKey = $cert.PrivateKey

	# get the private key file
	$privKeyCertFile = Get-Item -path "$ENV:ProgramData\Microsoft\Crypto\RSA\MachineKeys\*"  | Where {$_.Name -eq $certPrivKey.CspKeyContainerInfo.UniqueKeyContainerName} 
	
	# get the private key file ACL
	$privKeyAcl = (Get-Item -Path $privKeyCertFile.FullName).GetAccessControl("Access")
	
	# try and locate the existing permission in the ACL
	$existingFileSystemAccessRule = $privKeyAcl.GetAccessRules($true, $true, [System.Security.Principal.NTAccount]) | `
		Where { $_.IdentityReference -ieq $account} | `
		Where { $_.AccessControlType -eq $accessControlType} | `
		Where { $_.FileSystemRights -eq $fileSystemRights}
	
	if (-not $existingFileSystemAccessRule)
	{
		# add a new rule to the ACL
		Write-Host "Adding permission `"$accessControlType\$fileSystemRights`" to account `"$account`" on certificate private key"
		$permission = $account, $fileSystemRights, $accessControlType
		$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule $permission 
		$privKeyAcl.AddAccessRule($accessRule) 
		Set-Acl $privKeyCertFile.FullName $privKeyAcl
	}
}

#endregion

#region Transform File Functions

function Deployment-UpdateConfigurationTransform
{
	param (
		$transformFile = $(Throw 'Transform file path required'),
		$environment,
		[switch]$suppressMissingVariableError
	)
	
	if (-not $suppressMissingVariableError)
	{
		$variableError = $env:OctopusVariableError
		if ($variableError -and $variableError -ieq "false")
		{
			Write-Host "***** The environment variable `"OctopusVariableError`" on the target deployment machine has been set to `"false`", so the script WILL NOT fail with missing Octopus variables."
			$suppressMissingVariableError = $True
		}
	}
	
	Write-Host "Updating transform file `"$($transformFile.FullName)`" for environment `"$environment`""
	# variable hashtable
	$variableHash = @{}
	$missingVariables = @()
	
	# find all of the $OctopusVariable.xyz placeholders
	[array]$regexVariables = Select-String "\`$OctopusVariable\.(?<VariableName>.*?)[^a-zA-Z0-9\.\-]" $transformFile -AllMatches | Select -Expand Matches
	if ($regexVariables -ne $null -and $regexVariables.Count -gt 0)
	{
		foreach($regexVariable in $regexVariables)
		{			
			$octopusVariableName = $regexVariable.Groups[1]
			
			# try to match up the replacement variable with an octopus variable
			$octopusVariable = Get-Variable -Name "$octopusVariableName" -ErrorAction SilentlyContinue
			$octopusVariableValue = Get-Variable -Name "$octopusVariableName" -ValueOnly -ErrorAction SilentlyContinue
			if (-not $octopusVariable)
			{
				# error
				if (-not ($missingVariables -contains "$octopusVariableName")) 
				{ 
					$missingVariables += "$octopusVariableName"
				}
			}
			else
			{
				# if this is the first instance of the variable, add it to the hashtable
				if (-not $variableHash.ContainsKey("$octopusVariableName"))
				{
					$variableHash.Add("$octopusVariableName", "$octopusVariableValue")
				}
			}
		}
	}
	
	if ($missingVariables.Count -gt 0)
	{
		if (-not $suppressMissingVariableError)
		{
			$errorMsg = "The following Octopus variables are undefined for the `"$environment`" environment:`r`n"
			foreach($missingVariable in $missingVariables)
			{
				$errorMsg += "    $missingVariable`r`n"
			}
			Throw $errorMsg
		}
		else
		{
			$warnMsg = "The following Octopus variables are undefined for the `"$environment`" environment:`r`n"
			foreach($missingVariable in $missingVariables)
			{
				$warnMsg += "    $missingVariable`r`n"
			}
			Write-Warning $warnMsg
		}
	}
	
	if ($variableHash.Count -gt 0)
	{
		# get the transform file content (keeping line endings)
		$transformFileContent = [string]::Join([Environment]::NewLine, (Get-Content -Path $transformFile))
	
		# perform replacement on placeholder values
		foreach($key in $($variableHash.keys))
		{
			$regex = New-Object System.Text.RegularExpressions.Regex "\`$OctopusVariable\.$key([^a-zA-Z0-9\.\-])"
			$transformFileContent = $regex.Replace($transformFileContent, { param ($m) $variableHash[$key] + $m.Groups[1] })
		}
		# do this to get the existing file encoding
		$sr = New-Object System.IO.StreamReader($transformFile)
  		$encoding = $sr.CurrentEncoding
		$sr.Close()
		# write the updated content back into the file
		[System.IO.File]::WriteAllText($transformFile, $transformFileContent, $encoding)
		Write-Host "Transform file `"$($TransformFile.FullName)`" updated"
	}
	else
	{
		Write-Host "No variables to update in transform file `"$($TransformFile.FullName)`""
	}
}

#endregion

#region Octopus Cleanup Functions

function Deployment-PurgeOldOctopusVersions
{
	param (
		[int]$howManyToKeep
	)
	
	if ($Caller -eq "Octopus" -and $OctopusAppRoot -ne $null) 
	{ 
		$dirMask = Join-Path $OctopusAppRoot ($OctopusPackageName + "\*")
		$dirSortedList = Get-Item $dirMask | Sort-Object LastWriteTime
		if ($dirSortedList -ne $null -and $dirSortedList -is [system.array])
		{
			$dirVersionCount = $dirSortedList.Count
			$diff = $dirVersionCount-$howManyToKeep
			Write-Host "$dirVersionCount old Octopus versions found by $dirMask"
			if ($diff -gt 0) 
			{
				Write-Host "Purging $diff old Octopus versions"
				( $dirSortedList | Select-Object -first $diff) | 
				%{ 
					try {
						Remove-Item $_ -force -recurse 
					} catch { }
				}
			}
		}
		
		$nugetMask = Join-Path $OctopusAppRoot ("..\.Tentacle\Packages\$OctopusPackageName.*_*")
		$nugetSortedList = Get-Item $nugetMask | Sort-Object LastWriteTime
		if ($nugetSortedList -ne $null -and $nugetSortedList -is [system.array])
		{
			$nugetVersionCount = $nugetSortedList.Count
			$diff = $nugetVersionCount-$howManyToKeep
			Write-Host "$nugetVersionCount old Octopus versions found by $nugetMask"
			if ($diff -gt 0) 
			{
				Write-Host "Purging $diff old Octopus nuget versions"
				( $nugetSortedList | Select-Object -first $diff) | 
				%{ 
					try {
						Remove-Item $_ -force 
					} catch { }
				}
			}
		}
	}
}

function Deployment-CleanupDeploymentModules
{
	param (
		[string]$modulePath
	)

	Write-Host "Cleaning up deployment files"
	Get-ChildItem $modulePath -Include DeploymentModule.psm1 -Recurse | 
		ForEach-Object {
			Write-Host "Removing deployment module file $($_.Name)"
			Remove-Item $_.Fullname -Force
		}
		
	if ($Caller -ne "Octopus")
	{
		Get-ChildItem $modulePath -Include PreDeploy.ps1,Deploy.ps1,PostDeploy.ps1 -Recurse | 
			ForEach-Object {
				Write-Host "Removing deployment file $($_.Name)"
				Remove-Item $_.Fullname -Force
			}
	}
}

function Deployment-CleanupDeploymentConfigs
{
	param (
		[string]$modulePath
	)

	Write-Host "Cleaning up deployment configs"
	Get-ChildItem $modulePath -Include *.debug.config,*.release.config,*.local.config,*.dev.config,*.qa.config,*.stg.config,*.test.config,*.test2.config,*.prod.config,packages.config,*.nuspec -Recurse | 
		ForEach-Object {
			Write-Host "Removing config file $($_.Name)"
			Remove-Item $_.Fullname -Force
		}
}

#endregion

#region ACL Functions

function Deployment-SetCustomACLPermissions
{
	param(
		$account,
		$path,
		$fileSystemRights,
		$accessControlType,
		$inheritanceFlags,
		$propagationFlags
	)
	# check existing acl
	Write-Host "Checking ACL permissions for `"$account`" on `"$path`""
	$existingAccessControlList = Get-Acl -Path $path
	$existingFileSystemAccessRule = $existingAccessControlList.GetAccessRules($true, $true, [System.Security.Principal.NTAccount]) | `
		Where { $_.IdentityReference -ieq $account} | `
		Where { $_.AccessControlType -eq $accessControlType} | `
		Where { $_.FileSystemRights -eq $fileSystemRights} | `
		Where { $_.InheritanceFlags -eq $inheritanceFlags} | `
		Where { $_.PropagationFlags -eq $propagationFlags}
	
	if ($existingFileSystemAccessRule -eq $null)
	{
		Write-Host "Updating permissions ($fileSystemRights) for `"$account`" on `"$path`""
		$accessControlList = Get-Acl -Path $path
		$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($account, $fileSystemRights, $inheritanceFlags, $propagationFlags, $accessControlType)
		$accessControlList.SetAccessRule($accessRule)
		Set-Acl -Path $path -AclObject $accessControlList
	}
}

#endregion

#region Windows Service Functions

function Deployment-StartWindowsService
{
	param(
		$name,
		[switch]$warnOnError
	)
	
	# check to see if the windows service can be found
	$existingService = Get-Service | Where-Object {$_.Name -eq "$name"}
	if ($existingService -ne $null)
	{
		if ($existingService.Status -ine "running")
		{
			Write-Host "Starting Windows service `"$name`""
			Start-Service "$name"
		}
		else
		{
			Write-Host "Windows service `"$name`" is already running"
		}
	}
	else
	{
		# windows service was not found
		if($warnOnError)
		{
			Write-Warning "Unable to locate Windows service named `"$name`""
		}
		else
		{
			Throw "Unable to locate Windows service named `"$name`""
		}
	}
}

function Deployment-StopWindowsService
{
	param(
		$name,
		[switch]$warnOnError
	)
	
	# check to see if the windows service can be found
	$existingService = Get-Service | Where-Object {$_.Name -eq "$name"}
	if ($existingService -ne $null)
	{
		if ($existingService.Status -ine "stopped")
		{
			Write-Host "Stopping Windows service `"$name`""
			Stop-Service "$name" -ErrorAction SilentlyContinue
			
			# check to make sure service is stopped
			$existingService = Get-Service | Where-Object {$_.Name -eq "$name"}
			do {
				Start-Sleep -s 5
				$existingService = Get-Service | Where-Object {$_.Name -eq "$name"}
			}
			while ($existingService.Status -ine "stopped")
		}
		else
		{
			Write-Host "Windows service `"$name`" is already stopped"
		}
	}
	else
	{
		# windows service was not found
		if ($warnOnError)
		{
			Write-Warning "Unable to locate Windows service named `"$name`""
		}
		else
		{
			Throw "Unable to locate Windows service named `"$name`""
		}
	}
}

#endregion

#region Database Functions

#endregion

#region Windows Features Functions

function Deployment-InstallWindowsFeatures
{
	param(
		$featureArray = $(throw 'An array with Windows feature names and descriptions is required')
	)
	
	$serverManagerAvailable = Get-Module -ListAvailable | Where {$_.Name -eq "ServerManager"}
	if (-not $serverManagerAvailable)
	{
		Write-Host "Skip checking Windows features for installation (module ServerManager not supported)"
		return
	}
	Import-Module ServerManager
	
	Write-Host "Checking Windows features for installation"
	# get all of the Windows Features
	$features = Get-WindowsFeature
	
	for($index = 0; $index -lt $featureArray.Count; $index++) {
		$testFeature = $features | Where {$_.Name -eq $featureArray[$index][0]}
		if ($testFeature -ne $null -and -not $($testFeature.Installed))
		{
			Deployment-InstallWindowsFeature -FeatureName $featureArray[$index][0] -Description $featureArray[$index][1]
		}
	}
}

function Deployment-RemoveWindowsFeatures
{
	param(
		$featureArray = $(throw 'An array with Windows feature names and descriptions is required')
	)
	
	$serverManagerAvailable = Get-Module -ListAvailable | Where {$_.Name -eq "ServerManager"}
	if (-not $serverManagerAvailable)
	{
		Write-Host "Skip checking Windows features for removal (module ServerManager not supported)"
		return
	}
	Import-Module ServerManager
	
	Write-Host "Checking Windows features for removal"
	# get all of the Windows Features
	$features = Get-WindowsFeature
	
	for($index = 0; $index -lt $featureArray.Count; $index++) {
		$testFeature = $features | Where {$_.Name -eq $featureArray[$index][0]}
		if ($testFeature -ne $null -and $($testFeature.Installed))
		{
			Deployment-RemoveWindowsFeature -FeatureName $featureArray[$index][0] -Description $featureArray[$index][1]
		}
	}
}

function Deployment-InstallWindowsFeature
{
	param(
		[string]$featureName = $(throw 'Windows feature name is required'),
		[string]$description,
		[string]$index,
		[string]$total
	)
	
	$serverManagerAvailable = Get-Module -ListAvailable | Where {$_.Name -eq "ServerManager"}
	if (-not $serverManagerAvailable)
	{
		Write-Host "Skip checking Windows feature [$index/$total] $featureName ($description) for installation (module ServerManager not supported)"
		return
	}
	Import-Module ServerManager
	
	$osVersion = Deployment-GetOSVersion
	
	if ($index -and $total)
	{
		Write-Host "Checking Windows feature [$index/$total] $featureName ($description) for installation"
	}
	else
	{
		Write-Host "Checking Windows feature $featureName ($description) for installation"
	}
	$feature = Get-WindowsFeature -Name $featureName
	
	if ($feature -ne $null)
	{
		if (-not ($feature.Installed))
		{
			Write-Host "Installing Windows feature $featureName ($description)"
			
			switch ($osVersion) 
			{ 
				6.1 {  # Windows 2008 R2
						Add-WindowsFeature -Name $featureName | Out-Null
					}
				6.2 { # Windows 2012
						Install-WindowsFeature -Name $featureName | Out-Null
					}
				default {
					Throw "Windws OS version is $osVersion, undefined method for Windows Feature management"
					}
			}
		}
	}
}

function Deployment-RemoveWindowsFeature
{
	param(
		[string]$featureName = $(throw 'Windows feature name is required'),
		[string]$description,
		[string]$index,
		[string]$total
	)
	
	$serverManagerAvailable = Get-Module -ListAvailable | Where {$_.Name -eq "ServerManager"}
	if (-not $serverManagerAvailable)
	{
		Write-Host "Skip checking Windows feature [$index/$total] $featureName ($description) for removal (module ServerManager not supported)"
		return
	}
	Import-Module ServerManager
	
	if ($index -and $total)
	{
		Write-Host "Checking Windows feature [$index/$total] $featureName ($description) for removal"
	}
	else
	{
		Write-Host "Checking Windows feature $featureName ($description) for removal"
	}
	$feature = Get-WindowsFeature -Name $featureName
	
	if ($feature.Installed)
	{
		Write-Host "Removing Windows feature $featureName ($description)"
		Remove-WindowsFeature -Name $featureName | Out-Null
	}
}

#endregion

#region .Net Framework Functions

function Deployment-EnsureInstalledFrameworkVersion
{
	param(
		[string]$netFrameworkVersion
	)
	
	Write-Host "Checking for installed .Net framework v$netFrameworkVersion"
	if (-not (Deployment-CheckForInstalledFrameworkVersion -NetFrameworkVersion $netFrameWorkVersion))
	{
		Throw "The .Net framework version v$netFrameworkVersion has not been installed"
	}
}

function Deployment-CheckForInstalledFrameworkVersion
{
	param(
		[string]$netFrameworkVersion
	)
	
	$installedVersions = Deployment-GetInstalledFrameworkVersions
	return ($installedVersions -Contains $netFrameworkVersion)
}

function Deployment-GetInstalledFrameworkVersions
{
	$installedFrameworks = @()
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\.NETFramework\Policy\v1.0" -Key "3705") { $installedFrameworks += "1.0" }
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v1.1.4322" -Key "Install") { $installedFrameworks += "1.1" }
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v2.0.50727" -Key "Install") { $installedFrameworks += "2.0" }
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v3.0\Setup" -Key "InstallSuccess") { $installedFrameworks += "3.0" }
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v3.5" -Key "Install") { $installedFrameworks += "3.5" }
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v4\Client" -Key "Install") { $installedFrameworks += "4.0c" }
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v4\Full" -Key "Install") { $installedFrameworks += "4.0" }
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v4\Full" -Key "Release") { $installedFrameworks += "4.5" }
	if(TestRegistryKey -Path "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v4\Full" -Key "Release" -Value "378758") { $installedFrameworks += "4.5.1" }
	
	return $installedFrameworks
}

function TestRegistryKey
{
	param(
		[string]$path, 
		[string]$key,
		$value = $null
	)
	
	if(-not (Test-Path $path))
	{
		return $false
	}
	
	if ((Get-ItemProperty $path).$key -eq $null) 
	{
		return $false
	}
	else
	{
		if ($value -ne $null)
		{
			if (-not ((Get-ItemProperty $path).$key -eq $value))
			{
				return $false
			}
			else
			{
				return $true
			}
		}
		else
		{
			return $true
		}
	}
}

#endregion


function Deployment-GetOSVersion
{
	$version = Get-WmiObject Win32_OperatingSystem
	$version = $version.Version.substring(0,3)
	return $version
}

function Deployment-UpdateDeploymentModuleWithVersion
{
	param(
		[string]$version
	)
	# append comment to top of file
	$content = "# Deployment Module v$version`r`n`r`n"
	$deploymentModuleFile = $script:MyInvocation.MyCommand.Path
	$content += [System.IO.File]::ReadAllText($deploymentModuleFile)
	# save updated file
	$content | Set-Content -Path $deploymentModuleFile
}

#region Event Log Source

function Deployment-SetEventLogSource
{
	param(
		$eventLogName,
		$eventSource
	)
	
	Write-Host "Checking for Event Source $eventSource in Windows Event Log $eventLogName"
	$existingEventLog = Get-EventLog -List | Where {$_.Log -eq $eventLogName}
	if ($existingEventLog -eq $null) 
	{
		Write-Host "Creating Windows Event Log $eventLogName and Event Source $eventSource"
		New-EventLog -LogName "$eventLogName" -Source "$eventSource" -ErrorAction SilentlyContinue
		Write-EventLog -LogName "$eventLogName" -Source "$eventSource" -Message "Log and Source created" -EventId 0 -EntryType information
	}
	else
	{
		$existingSource = Get-EventLog -LogName "$eventLogName" | Select-Object Source -Unique | Where {$_.Source -eq "$eventSource"}
		if ($existingSource -eq $null) 
		{
			Write-Host "Creating Event Source $eventSource in Windows Event Log $eventLogName"
			New-EventLog -LogName "$eventLogName" -Source "$eventSource" -ErrorAction SilentlyContinue
			Write-EventLog -LogName "$eventLogName" -Source "$eventSource" -Message "Source created" -EventId 0 -EntryType information
		}
	}
}

#endregion

#region NServiceBus

function Deployment-DeployNServiceBusService
{
	param(
		$serviceName,
		$displayName,
		$description,
		$startMode,
		$account,
		$password,
		[switch]$forceReinstall
	)

	# check to see if the service has already been registered
	Write-Host "Checking for existing NServiceBus Windows service named `"$serviceName`""
	$existingService = Get-WmiObject win32_service -Filter "name='$serviceName'"
	$reinstallService = $False
  
	# windows service exists
	if ($existingService -ne $null)
	{
		# check display name
		if ($displayName -and $existingService.DisplayName -ine $displayName)
		{
			Write-Host "Existing service has a different display name"
			$reinstallService = $True
		}
		# check description
		if ($description -and $existingService.Description -ine $description)
		{
			Write-Host "Existing service has a different service description"
			$reinstallService = $True
		}
		# check start mode
		if ($startMode -and $existingService.StartMode -ine $startMode)
		{
			Write-Host "Existing service has a different start mode"
			$reinstallService = $True
		}
		# check account
		if ($account -and $existingService.StartName -ine $account)
		{
			Write-Host "Existing service is set to run under a different service account"
			$reinstallService = $True
		}
		
		# uninstall service
		if ($reinstallService -or $forceReinstall)
		{
			Deployment-UninstallNServiceBusService -ServiceName $serviceName
		}
	}
  
	# install new windows service
	if ( $reinstallService -or $forceReinstall -or $existingService -eq $null )
	{
		if ($account -ne $null)
		{
			Deployment-InstallNServiceBusService -ServiceName $serviceName -DisplayName $displayName -Description $description -StartMode $startMode -Account $account -Password $password
		}
		else
		{
			Deployment-InstallNServiceBusService -ServiceName $serviceName -DisplayName $displayName -Description $description -StartMode $startMode
		}
	}  
}

function Deployment-UninstallNServiceBusService
{
	param(
		$serviceName
	)
	Write-Host "Uninstalling NServiceBus Windows service named `"$serviceName`""
	$pinfo = New-Object System.Diagnostics.ProcessStartInfo
	$pinfo.CreateNoWindow = $true
	$pinfo.UseShellExecute = $false
	$pinfo.FileName = ".\NServiceBus.Host.exe"
	$pinfo.Arguments = "-uninstall -serviceName=`"$serviceName`""
	$pinfo.RedirectStandardOutput = $true
	$pinfo.RedirectStandardError = $true
	if($password)
	{
		Write-Host "$($pinfo.FileName) $($($pinfo.Arguments).Replace("$password", "********"))"
	}
	else
	{
		Write-Host "$($pinfo.FileName) $($pinfo.Arguments)"
	}
	$p = New-Object System.Diagnostics.Process
	$p.StartInfo = $pinfo
	$p.Start() | Out-Null
	$stdOut = $p.StandardOutput.ReadToEnd()
	$stdErr = $p.StandardError.ReadToEnd()
	$p.WaitForExit()
	$exitCode = $p.ExitCode
	Write-Host "Uninstall Exit Code: $exitCode"
	Write-Host "----- Uninstall Output -----"
	Write-Host $stdOut
	Write-Host "----- End Uninstall Output -----"
	if ($exitCode -ne 0)
	{
		Write-Host "----- Uninstall Error -----"
		Write-Host $stdErr
		Write-Host "----- End Uninstall Error -----"
	}
}

function Deployment-InstallNServiceBusService
{
	param(
		$serviceName,
		$displayName,
		$description,
		$startMode,
		$account,
		$password
	)
	Write-Host "Installing NServiceBus Windows service named `"$serviceName`""
	
	$pinfo = New-Object System.Diagnostics.ProcessStartInfo
	$pinfo.CreateNoWindow = $true
	$pinfo.UseShellExecute = $false
	$pinfo.FileName = ".\NServiceBus.Host.exe"
	$pinfo.Arguments = "-install"
	$pinfo.Arguments += " -serviceName=`"$serviceName`""
	if ($displayName) { $pinfo.Arguments += " -displayName=`"$displayName`"" }
	if ($description) { $pinfo.Arguments += " -description=`"$description`"" }
	if ($account -and $password) { $pinfo.Arguments += " -username=`"$account`" -password=`"$password`"" }
	if ($startMode) { if ($startMode -ine "auto") { $pinfo.Arguments += " -startManually" } }
	$pinfo.RedirectStandardOutput = $true
	$pinfo.RedirectStandardError = $true
	if ($password)
	{
		Write-Host "$($pinfo.FileName) $($($pinfo.Arguments).Replace("$password", "********"))"
	}
	else
	{
		Write-Host "$($pinfo.FileName) $($pinfo.Arguments)"
	}
	$p = New-Object System.Diagnostics.Process
	$p.StartInfo = $pinfo
	$p.Start() | Out-Null
	$stdOut = $p.StandardOutput.ReadToEnd()
	$stdErr = $p.StandardError.ReadToEnd()
	$p.WaitForExit()
	$exitCode = $p.ExitCode
	Write-Host "Install Exit Code: $exitCode"
	Write-Host "----- Install Output -----"
	Write-Host $stdOut
	Write-Host "----- End Install Output -----"
	if ($exitCode -ne 0)
	{
		Write-Host "----- Install Error -----"
		Write-Host $stdErr
		Write-Host "----- End Install Error -----"
	}
}

#endregion

#region Generic Functions
function Deployment-CreateParentDirectory
{
	param(
		[string]$file
	)
	$fileInfo = ([System.IO.FileInfo]$file)
	$folderPath =  Split-Path $fileInfo.fullname -Parent
	if (-not (Test-Path $folderPath))
	{
		Write-Host "Creating directory $folderPath"
		New-Item -ItemType directory -Path $folderPath | Out-Null
	}
}

function Deployment-BackupFile
{
	param(
		[string]$file,
		[string]$backupExtension
	)
	
	# set the default backup extension
	if(-not $backupExtension){$backupExtension = "backup"}
	
	# get the full path to the file
	if (-not ([System.IO.Path]::IsPathRooted($file)))
	{
		$file = Resolve-Path $file
	}
	
	# set the backed up file name
	$backedUpFile = "$file.$backupExtension"
	
	# delete the target file if it exists
	if (Test-Path $backedUpFile)
	{
		Write-Host "Deleting file $backedUpFile"
		Remove-Item $backedUpFile -Force | Out-Null
 	}
	
	# backup the file
	Write-Host "Backing up file $file to $backedUpFile" 
	Copy-Item -Path $file -Destination $backedUpFile | Out-Null
}

function Deployment-RestoreFile
{
	param(
		[string]$file,
		[string]$backupExtension,
		[switch]$deleteBackup
	)
	
	# set the default backup extension
	if(-not $backupExtension){$backupExtension = "backup"}
	
	# get the full path to the file
	if (-not ([System.IO.Path]::IsPathRooted($file)))
	{
		$file = Resolve-Path $file
	}
	
	# set the backed up file name
	$backedUpFile = "$file.$backupExtension"
	
	if (-not (Test-Path $backedUpFile))
	{
		# we cant find the backed up file to restore
		#error
 	}
	else
	{
		# delete the target file if it exists
		if (Test-Path $file)
		{
			Remove-Item $file -Force | Out-Null
		}
	}
	
	# restore file
	Write-Host "Restoring file $file from $backedUpFile" 
	Copy-Item -Path $backedUpFile -Destination $file | Out-Null
	
	# delete backup
	if ($deleteBackup)
	{
		if (Test-Path $backedUpFile)
		{
			Write-Host "Deleting file $backedUpFile"
			Remove-Item $backedUpFile -Force | Out-Null
		}
	}
}

function Deployment-UpdateFile
{
	param(
		[string]$file,
		[string]$regex,
		[string]$value,
		[string]$message,
		[switch]$htmlEncode
	)
	
	# we might need to encode the value to make it xml safe
	# encode the value if the htmlEncode switch has been set
	if ($htmlEncode)
	{
		Add-Type -AssemblyName System.Web
		$value = [System.Web.HttpUtility]::HtmlEncode($value)
	}
	
	if (-not $message)
	{
		Write-Host "Updating file $file"
	}
	else
	{
		Write-Host "Updating file $file - $message"
	}
	
	# update the file using the regex
	(Get-Content $file) | 
		Foreach-Object {$_ -replace $regex, $value} | 
		Set-Content $file
}

function Is64Bit  
{
	[IntPtr]::Size -eq 8
}
 
function Deployment-InstallMsi
{
	param(
		$productList,
		[string]$applicationProductName,
		[string]$applicationDownloadUri,
		[string]$downloadDirectory
	)
	
	Write-Host "Checking to see if $applicationProductName has been installed"
	
	# if the product list is empty populate it (this can be slow)
	if ($productList -eq $null)
	{
		$productList = Get-WmiObject -Class Win32_Product
	}
	
	# search the product list for the application
	$product = $productList | Where-Object {$_.Name -like "*$applicationProductName*"}
	if ($product -eq $null)
	{
		if (-not (Test-Path "$downloadDirectory"))
		{
			New-Item -ItemType directory -Path "$downloadDirectory" | Out-Null
		}
		
		# get the 32 or 64 bit url
		$uri = $null
		if (Is64Bit)
		{
			$uri = new-Object System.Uri "$($Script:BaseSoftwareDownloadUri)/$($applicationDownloadUri)_x64.msi"
		}
		else
		{
			$uri = new-Object System.Uri "$($Script:BaseSoftwareDownloadUri)/$($applicationDownloadUri)_x86.msi"
		}
		
		Write-Host "Downloading $applicationProductName ($uri)"
		
		# get the full download path
		$fileName = $uri.Segments[-1] 
		$downloadFile = Join-Path $downloadDirectory $fileName
		
		# delete the old downloaded file if it exists
		if (Test-Path $downloadFile)
		{
			Remove-Item $downloadFile -Force | Out-Null
		}
		
		# get the content from the GitHub
		$webClient = New-Object System.Net.WebClient
		$webClient.Headers.Add("Authorization","Basic $($Script:AuthToken)")
		$webClient.DownloadFile($uri, $downloadFile)
		
		# install the application
		Write-Host "Installing $applicationProductName (msiexec.exe /i `"$downloadFile`" /passive)"
		$args = @()
		$args += "/i"
		$args += "`"$downloadFile`""
		$args += "/passive"
		$process = (Start-Process -FilePath "msiexec.exe" -ArgumentList $args -PassThru -Wait)
		Write-Host "Install $applicationProductName Exit Code: $($process.ExitCode)"
		
		Start-Sleep -s 30
	}
}

function Deployment-InstallDotNetFramework45
{
	param(
		[string]$downloadDirectory
	)
	
	Write-Host "Checking to see if .Net Framework 4.5 has been installed"
	[array] $versions = Deployment-GetInstalledFrameworkVersions
	$frameworkInstalled = $versions -contains "4.5"
	if (-not $frameworkInstalled)
	{
		if (-not (Test-Path "$downloadDirectory"))
		{
			New-Item -ItemType directory -Path "$downloadDirectory" | Out-Null
		}
		
		# get the MS .Net 4.5 executable
		$uri = new-Object System.Uri "$($Script:BaseSoftwareDownloadUri)/DotNetFramework-4.5/dotnetfx45_full_x86_x64.exe"
		Write-Host "Downloading Microsoft .Net Framework 4.5 ($uri)"
		
		# get the full download path
		$fileName = $uri.Segments[-1] 
		$downloadFile = Join-Path $downloadDirectory $fileName
		
		# delete the old downloaded file if it exists
		if (Test-Path $downloadFile)
		{
			Remove-Item $downloadFile -Force | Out-Null
		}
		
		# get the content from the software server
		$webClient = New-Object System.Net.WebClient
		$webClient.Headers.Add("Authorization","Basic $($Script:AuthToken)")
		$webClient.DownloadFile($uri, $downloadFile)
		
		Write-Host "Installing Microsoft .Net Framework 4.5 (`"$downloadFile`" /q /norestart)"
		$args = @()
		$args += "/q"
		$args += "/norestart"
		$process = (Start-Process -FilePath "$downloadFile" -ArgumentList $args -PassThru -Wait)
		Write-Host "Install Microsoft .Net Framework 4.5 Exit Code: $($process.ExitCode)"
		if ($process.ExitCode -eq 1641)
		{
			Write-Host "The install Microsoft .Net Framework 4.5 has returned exit code $($process.ExitCode) indicating that a reboot is required to complete installation"
		}
		if ($process.ExitCode -eq 3010)
		{
			Write-Host "The install Microsoft .Net Framework 4.5 has returned exit code $($process.ExitCode) indicating that a reboot is required to complete installation"
		}
	}
}

function Deployment-InstallDotNetFramework451
{
	param(
		[string]$downloadDirectory
	)
	
	Write-Host "Checking to see if .Net Framework 4.5.1 has been installed"
	[array] $versions = Deployment-GetInstalledFrameworkVersions
	$frameworkInstalled = $versions -contains "4.5.1"
	if (-not $frameworkInstalled)
	{
		if (-not (Test-Path "$downloadDirectory"))
		{
			New-Item -ItemType directory -Path "$downloadDirectory" | Out-Null
		}
		
		# get the MS .Net 4.5.1 executable
		$uri = new-Object System.Uri "$($Script:BaseSoftwareDownloadUri)/DotNetFramework-4.5.1/NDP451-KB2858728-x86-x64-AllOS-ENU.exe"
		Write-Host "Downloading Microsoft .Net Framework 4.5.1 ($uri)"
		
		# get the full download path
		$fileName = $uri.Segments[-1] 
		$downloadFile = Join-Path $downloadDirectory $fileName
		
		# delete the old downloaded file if it exists
		if (Test-Path $downloadFile)
		{
			Remove-Item $downloadFile -Force | Out-Null
		}
		
		# get the content from the software server
		$webClient = New-Object System.Net.WebClient
		$webClient.Headers.Add("Authorization","Basic $($Script:AuthToken)")
		$webClient.DownloadFile($uri, $downloadFile)
		
		Write-Host "Installing Microsoft .Net Framework 4.5.1 (`"$downloadFile`" /q /norestart)"
		$args = @()
		$args += "/q"
		$args += "/norestart"
		$process = (Start-Process -FilePath "$downloadFile" -ArgumentList $args -PassThru -Wait)
		Write-Host "Install Microsoft .Net Framework 4.5.1 Exit Code: $($process.ExitCode)"
		if ($process.ExitCode -eq 1641)
		{
			Write-Host "The install Microsoft .Net Framework 4.5.1 has returned exit code $($process.ExitCode) indicating that a reboot is required to complete installation"
		}
		if ($process.ExitCode -eq 3010)
		{
			Write-Host "The install Microsoft .Net Framework 4.5.1 has returned exit code $($process.ExitCode) indicating that a reboot is required to complete installation"
		}
	}
}

function Deployment-InstallPhalanger
{
	param(
		$productList,
		[string]$downloadDirectory
	)
	
	Write-Host "Checking to see if Phalanger 3.0 has been installed"
	
	# if the product list is empty populate it (this can be slow)
	if ($productList -eq $null)
	{
		$productList = Get-WmiObject -Class Win32_Product
	}
	
	if (-not (Test-Path "$downloadDirectory"))
	{
		New-Item -ItemType directory -Path "$downloadDirectory" | Out-Null
	}
	
	# MS VC++ Redistributable
	$product = $productList | Where-Object {$_.Name -like "*Microsoft Visual C++ 2010  x86 Redistributable*"}
	if ($product -eq $null)
	{
		# get the MS VC++ 2010 x86 Redistributable file
		$uri = new-Object System.Uri "$($Script:BaseSoftwareDownloadUri)/Phalanger-3.0.0.4072/en_visual_c++_2010_sp1_redistributable_package_x86_651767.exe"
		Write-Host "Downloading Microsoft Visual C++ 2010 x86 Redistributable ($uri)"
		
		# get the full download path
		$fileName = $uri.Segments[-1] 
		$downloadFile = Join-Path $downloadDirectory $fileName
		
		# delete the old downloaded file if it exists
		if (Test-Path $downloadFile)
		{
			Remove-Item $downloadFile -Force | Out-Null
		}
		
		# get the content from the GitHub
		$webClient = New-Object System.Net.WebClient
		$webClient.Headers.Add("Authorization","Basic $($Script:AuthToken)")
		$webClient.DownloadFile($uri, $downloadFile)
		
		Write-Host "Installing Microsoft Visual C++ 2010 x86 Redistributable (`"$downloadFile`" /q /norestart)"
		$args = @()
		$args += "/q"
		$args += "/norestart"
		$process = (Start-Process -FilePath "$downloadFile" -ArgumentList $args -PassThru -Wait)
		Write-Host "Install Microsoft Visual C++ 2010 x86 Redistributable Exit Code: $($process.ExitCode)"
		Start-Sleep -s 30
	}
	
	# MS SDK files
	if (-not (Test-Path -Path "C:\Program Files (x86)\Microsoft SDKs"))
	{
		# get the MS SDK files
		$uri = new-Object System.Uri "$($Script:BaseSoftwareDownloadUri)/Phalanger-3.0.0.4072/MicrosoftSDKs.zip"
		Write-Host "Downloading MS SDK files ($uri)"
		
		# get the full download path
		$fileName = $uri.Segments[-1] 
		$downloadFile = Join-Path $downloadDirectory $fileName
		
		# delete the old downloaded file if it exists
		if (Test-Path $downloadFile)
		{
			Remove-Item $downloadFile -Force | Out-Null
		}
		$copySdkDir = Join-Path $downloadDirectory "MicrosoftSDKs"
		if (Test-Path $copySdkDir)
		{
			Remove-Item $copySdkDir -Recurse -Force | Out-Null
		}
		
		# get the content from the GitHub
		$webClient = New-Object System.Net.WebClient
		$webClient.Headers.Add("Authorization","Basic $($Script:AuthToken)")
		$webClient.DownloadFile($uri, $downloadFile)
		
		# unzip the file
		Write-Host "Unzipping MS SDK files ($downloadFile -> $copySdkDir)"
		[System.Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem") | Out-Null 
		[System.IO.Compression.ZipFile]::ExtractToDirectory("$downloadFile", "$copySdkDir")
		
		# copy files
		Write-Host "Copying MS SDK files ($copySdkDir -> C:\Program Files (x86)\Microsoft SDKs)"
		Copy-Item "$copySdkDir" "C:\Program Files (x86)\Microsoft SDKs" -Recurse | Out-Null
	}
	
	# Phalanger
	$product = $productList | Where-Object {$_.Name -like "*Phalanger 3.0*"}
	if ($product -eq $null)
	{
		# get the Phalanger file
		$uri = new-Object System.Uri "$($Script:BaseSoftwareDownloadUri)/Phalanger-3.0.0.4072/Phalanger.msi"
		Write-Host "Downloading Phalanger ($uri)"
		
		# get the full download path
		$fileName = $uri.Segments[-1] 
		$downloadFile = Join-Path $downloadDirectory $fileName
		
		# delete the old downloaded file if it exists
		if (Test-Path $downloadFile)
		{
			Remove-Item $downloadFile -Force | Out-Null
		}
		
		# get the content from the GitHub
		$webClient = New-Object System.Net.WebClient
		$webClient.Headers.Add("Authorization","Basic $($Script:AuthToken)")
		$webClient.DownloadFile($uri, $downloadFile)
		
		Write-Host "Installing Phalanger (msiexec.exe ALLUSERS=1 /i `"$downloadFile`" /passive)"
		$args = @()
		$args += "ALLUSERS=1"
		$args += "/i"
		$args += "`"$downloadFile`""
		$args += "/passive"
		$process = (Start-Process -FilePath "msiexec.exe" -ArgumentList $args -PassThru -Wait)
		Write-Host "Install Phalanger Exit Code: $($process.ExitCode)"
		Start-Sleep -s 30
	}
}
  
#endregion
