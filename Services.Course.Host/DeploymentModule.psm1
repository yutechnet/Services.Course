﻿$script:ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

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
	
		Get-WebConfiguration -PSPath "IIS:\Sites\$websiteName" -Filter "/system.webServer/handlers/add[@name='$handlerName']" `
    		| % { Clear-WebConfiguration -PSPath $_.PSPath -Filter $_.ItemXPath -Location $_.Location }
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
	$appPool = Get-Item IIS:\AppPools\$appPoolName -ErrorAction SilentlyContinue
	if ($appPool)
	{
		Write-Host "Checking existing app pool property `"$appPoolProperty`""
		# get the existing property value
		$existingValue = Get-ItemProperty ("IIS:\AppPools\" + $appPoolName) -Name $appPoolProperty
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
			Set-ItemProperty ("IIS:\AppPools\" + $appPoolName) -Name $appPoolProperty -Value $appPoolPropertyValue
		}
	}
	else
	{
		$(Throw "An existing app pool with name `"$appPoolName`" was not found")
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
	$website = Get-Item IIS:\Sites\$websiteName -ErrorAction SilentlyContinue
	if ($website)
	{
		if($hostHeader)
		{
			if ($hostHeader.Contains(";"))
			{
				$hostHeaderArray = $hostHeader.Split(';')
				ForEach($tmpHostHeader in $hostHeaderArray)
				{
					$tmpBindingString = "$($IPAddress):$($port):$($tmpHostHeader)"
					Write-Host "Checking existing website binding"
					# get the existing binding
					$existingBinding = Get-WebBinding -Name $websiteName -Protocol $protocol | Where {$_.BindingInformation -eq $tmpBindingString}
					if (-not $existingBinding )
					{
						Write-Host "Creating website `"$websiteName`" binding `"$protocol $($IPAddress.ToString())`:$port`:$tmpHostHeader`""
						New-WebBinding -Name $websiteName -Protocol $protocol -IPAddress $IPAddress -Port $port -HostHeader $tmpHostHeader
					}
				}
			}
			else
			{
				Write-Host "Checking existing website binding"
				# get the existing binding
				$existingBinding = Get-WebBinding -Name $websiteName -Protocol $protocol | Where {$_.BindingInformation -eq $bindingString}
				if (-not $existingBinding )
				{
					Write-Host "Creating website `"$websiteName`" binding `"$protocol $($IPAddress.ToString())`:$port`:$hostheader`""
					New-WebBinding -Name $websiteName -Protocol $protocol -IPAddress $IPAddress -Port $port -HostHeader $hostheader
				}
			}
		}
		else
		{
			Write-Host "Checking existing website binding"
			# get the existing binding
			$existingBinding = Get-WebBinding -Name $websiteName -Protocol $protocol | Where {$_.BindingInformation -eq $bindingString}
			if (-not $existingBinding )
			{
				Write-Host "Creating website `"$websiteName`" binding `"$protocol $($IPAddress.ToString())`:$port`:$hostheader`""
				New-WebBinding -Name $websiteName -Protocol $protocol -IPAddress $IPAddress -Port $port -HostHeader $hostheader
			}
		}
		
		Write-Host "All website bindings"
		Get-WebBinding -Name $websiteName
	}
	else
	{
		$(Throw "An existing website with name [$websiteName] was not found")
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
		else
		{
			Write-Host "Existing SSL binding found `"$bindingPath`""
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
	$website = Get-Item IIS:\Sites\$websiteName -ErrorAction SilentlyContinue
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
		Write-Host "All website bindings"
		Get-WebBinding -Name $websiteName
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
	Write-Host "Checking for existing app pool"
	# find app pool
	$appPool = Get-Item IIS:\AppPools\$appPoolName -ErrorAction SilentlyContinue
	if (-not $appPool)
	{
		# app pool was not found, create the app pool
		Write-Host "Creating new app pool `"$appPoolName`""
		New-WebAppPool -Name $appPoolName -Force | Out-Null
	}
	else
	{
		Write-Host "An existing app pool with name `"$appPoolName`" was found"
	}
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
	# check for the app pool
	$appPool = Deployment-SetupAppPool $appPoolName
	Write-Host "Checking for existing website"
	# find the website
	$website = Get-Item IIS:\Sites\$websiteName -ErrorAction SilentlyContinue
	if (-not $website)
	{
		# website not found, create new website
		Write-Host "Creating new website `"$websiteName`""
		$sites = Get-Item IIS:\Sites
		if ($sites -eq $null -or ($sites.Count -eq 0))
		{
			# if fist site being created on the server then set the Id = 1
			New-Website -Name $websiteName -PhysicalPath $websiteDirectoryPath -ApplicationPool $appPoolName -Id 1 -Force | Out-Null
		}
		else
		{
			New-Website -Name $websiteName -PhysicalPath $websiteDirectoryPath -ApplicationPool $appPoolName -Force | Out-Null
		}
		Write-Warning "A new website `"$websiteName`" has been created"
		$website = Get-Item IIS:\Sites\$websiteName
	}
	else
	{
		Write-Host "An existing website with name `"$websiteName`" was found"
	}
	
	# check the app pool of the website
	if ($website.ApplicationPool -ne $appPoolName)
	{
		Write-Host "Updating website `"$websiteName`" app pool from `"$($website.ApplicationPool)`" to `"$appPoolName`"" 
		Set-ItemProperty "IIS:\Sites\$websiteName" ApplicationPool $appPoolName
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
	Write-Host "Checking for existing website"
	# find the website
	$website = Get-Item IIS:\Sites\$websiteName -ErrorAction SilentlyContinue
	if (-not $website)
	{
		Throw "Unable to locate website`"$websiteName`".  An existing website with the name of `"$websiteName`" is required to deploy this web application."
	}
	else
	{
		Write-Host "An existing website with name `"$websiteName`" was found"
		Write-Host "Checking for existing web application"
		$existingWebsiteAppPoolName = $website.ApplicationPool
		# find the webapp
		$webApp = Get-Item IIS:\Sites\$websiteName\$webApplicationName -ErrorAction SilentlyContinue
		if (-not $webApp)
		{
			if ($appPoolName)
			{
				Deployment-SetupAppPool $appPoolName
			}
			else
			{
				$appPoolName = $website.ApplicationPool
			}
			# webapp not found, create new webapp
			Write-Host "Creating new web application `"$webApplicationName`" under website `"$websiteName`""
			$webApp = New-WebApplication -Name $webApplicationName -PhysicalPath $webApplicationDirectoryPath -Site $websiteName -ApplicationPool $appPoolName -Force | Out-Null
		}
		else
		{
			Write-Host "An existing web application with name `"$webApplicationName`" was found under website `"$websiteName`""
			if ($appPoolName -and $webApp.ApplicationPool -ne $appPoolName)
			{
				Deployment-SetupAppPool $appPoolName
				Write-Host "Updating web application `"$webApplicationName`" app pool from `"$($webApp.ApplicationPool)`" to `"$appPoolName`"" 
				Set-ItemProperty "IIS:\Sites\$websiteName\$webApplicationName" ApplicationPool $appPoolName
			}
		}
	}
}

#endregion

#region Certificate Functions

function Deployment-ImportCertificateFromPFX
{
	param(
		[string]$pfxPath,
		[string]$pfxPassword,
		[string]$certThumbprint,
		[string]$certStoreScope,
		[string]$certStoreName
	)
	
	if (-not (ConfigureIIS)) {return}
			
	$cert = Get-ChildItem cert:\$certStoreScope\$certStoreName | Where-Object {$_.Thumbprint -eq $certThumbprint}
	if (-not $cert)
	{
		$pfxCert = new-object system.security.cryptography.x509certificates.x509certificate2
		$pfxCert.Import($pfxPath,$pfxPassword,"Exportable,PersistKeySet")
		$store = New-Object system.security.cryptography.X509Certificates.X509Store $certStoreName, $certStoreScope
		$store.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)
		$store.Add($pfxCert)
		$store.Close()
		Write-Host "Certificate imported from `"$pfxPath`" into certificate store `"$certStoreScope\$certStoreName`""
		Write-Host "Certificate will expire on $($pfxCert.NotAfter)"
		}
	else
	{
		Write-Host "Existing certificate found in `"$certStoreScope\$certStoreName`" with subject `"$($cert.Subject)`" and thumbprint `"$certThumbprint`""
		Write-Host "Certificate will expire on $($cert.NotAfter)"
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
			$octopusVariableValue = Get-Variable -Name "$octopusVariableName" -ValueOnly -ErrorAction SilentlyContinue
			if (-not $octopusVariableValue)
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
	
	if ($Caller -eq "Octopus") 
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
	if ($Caller -eq "Octopus") 
	{ 
		Write-Host "Cleaning up deployment modules"
		Get-ChildItem $modulePath\*.psm1 | ForEach ($_) {
			Write-Host "Removing deployment module file $($_.Name)"
			Remove-Item $_.Fullname -Force
		}
	}
}

function Deployment-CleanupDeploymentConfigs
{
	param (
		[string]$modulePath
	)
	
	if ($Caller -eq "Octopus") 
	{ 
		Write-Host "Cleaning up deployment configs"
		Get-ChildItem $modulePath\*.*.config -Include *.debug.config,*.release.config,*.local.config,*.dev.config,*.qa.config,*.stg.config,*.test.config,*.test2.config,*.prod.config | ForEach ($_) {
			Write-Host "Removing config file $($_.Name)"
			Remove-Item $_.Fullname -Force
		}
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
	
	if ($existingFileSystemAccessRule -ne $null)
	{
		# we found a matching acl
		Write-Host "Existing permissions ($fileSystemRights) for `"$account`" on `"$path`" have been found"
	}
	else
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
		$name
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
		Write-Warning "Unable to locate Windows service named `"$name`""
	}
}

function Deployment-StopWindowsService
{
	param(
		$name
	)
	
	# check to see if the windows service can be found
	$existingService = Get-Service | Where-Object {$_.Name -eq "$name"}
	if ($existingService -ne $null)
	{
		if ($existingService.Status -ine "stopped")
		{
			Write-Host "Stopping Windows service `"$name`""
			Stop-Service "$name" -ErrorAction SilentlyContinue
		}
		else
		{
			Write-Host "Windows service `"$name`" is already stopped"
		}
	}
	else
	{
		# windows service was not found
		Write-Warning "Unable to locate Windows service named `"$name`""
	}
}

#endregion

#region NServiceBus Functions

function Deployment-InstallNServiceBusService
{
	param(
		$path,
		$name,
		$displayName,
		$description,
		$startMode,
		$account,
		$password
	)
	
	# check to see if the service has already been registered
	#$existingService = Get-Service | Where-Object {$_.Name -eq "$name"}
	$existingService = Get-WmiObject win32_service -Filter "name='$name'"
	$reinstallService = $False
	if ($existingService -ne $null)
	{
		# windows service exists
		Write-Host "Existing Windows service named `"$name`" was found"
		# check display name
		if ($displayName -and $existingService.DisplayName -ine $displayName)
		{
			Write-Host "Existing service has a different display name `"$($existingService.DisplayName)`" vs `"$displayName`""
			$reinstallService = $True
		}
		# check description
		if ($description -and $existingService.Description -ine $description)
		{
			Write-Host "Existing service has a different service description `"$($existingService.Description)`" vs `"$description`""
			$reinstallService = $True
		}
		# check start mode
		if ($startMode -and $existingService.StartMode -ine $startMode)
		{
			Write-Host "Existing service has a different start mode `"$($existingService.StartMode)`" vs `"$startMode`""
			$reinstallService = $True
		}
		# check account
		if ($account -and $existingService.StartName -ine $account)
		{
			Write-Host "Existing service is set to run under a different service account `"$($existingService.StartName)`" vs `"$account`""
			$reinstallService = $True
		}
		
		if ($reinstallService)
		{
			# uninstall existing service
			$nserviceBusHostExe = Join-Path $path "NServiceBus.Host.exe"
			$uninstallArguments = @()
			$uninstallArguments += "/uninstall"
			if ($name) { $uninstallArguments += "/serviceName:`"$name`"" }
			Write-Host "Uninstalling Windows service named `"$name`""
			Set-Alias nservicebushost $nserviceBusHostExe
			Start-Process -NoNewWindow -Wait -FilePath nservicebushost -ArgumentList $uninstallArguments
		}
	}
	
	if ( $reinstallService -or $existingService -eq $null )
	{
		# install new windows service
		$nserviceBusHostExe = Join-Path $path "NServiceBus.Host.exe"
		$installArguments = @()
		$installArguments += "/install"
		# set service name
		if ($name) { $installArguments += "/serviceName:`"$name`"" }
		# set service display name 
		if ($displayName) { $installArguments += "/displayName:`"$displayName`"" }
		# set service description
		if ($description) { $installArguments += "/description:`"$description`"" }
		# set start mode
		if ($startMode) { if ($startMode -ine "automatic") { $installArguments += "/startManually" } }
		# set service credentials
		if ($account -and $password) 
		{ 
			$installArguments += "/username:`"$account`""
			$installArguments += "/password:`"$password`""
		}
		# install the service
		Write-Host "Installing Windows service named `"$name`""
		Set-Alias nservicebushost $nserviceBusHostExe
		Start-Process -NoNewWindow -Wait -FilePath nservicebushost -ArgumentList $installArguments
	}	
}

#endregion

#region Database Functions

function Deployment-SyncDatabaseSchemaToScript
{
	param(
		$connectionString = $(Throw 'Connection string required'),
		$compareParametersFile
	)
	# check to make sure that the SqlCompareHome environment variable has been set
	$sqlCompareHome = $env:SqlCompareHome
	if ($sqlCompareHome -eq $null -or $sqlCompareHome -eq "") 
	{
		Throw "The environment variable `"SqlCompareHome`" must be set on the server to run SQLCompare.exe"
	}
	# update Schema.xml file
	$schemaFile = New-Object System.IO.FileInfo $compareParametersFile
	$schemaDirectory = $schemaFile.DirectoryName
	Write-Host "Sync database to script directory for database `"$($schemaFile.Directory.Name)`""
	$databasesDirectory = New-Object System.IO.DirectoryInfo("$schemaDirectory\..\")
	$builder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder $connectionString
	[xml]$xml = Get-Content $compareParametersFile
	
	# add source server (server1)
	$server1 = $xml.CreateElement("server1")
	$server1.AppendChild($xml.CreateTextNode($builder.DataSource)) | Out-Null
	$xml.commandline.AppendChild($server1) | Out-Null
	# add source database (database1)
	$database1 = $xml.CreateElement("database1")
	$database1.AppendChild($xml.CreateTextNode($builder.InitialCatalog)) | Out-Null
	$xml.commandline.AppendChild($database1) | Out-Null
	# add source username (username1)
	$username1 = $xml.CreateElement("username1")
	$username1.AppendChild($xml.CreateTextNode($builder.UserID)) | Out-Null
	$xml.commandline.AppendChild($username1) | Out-Null
	# add source password (password1)
	$password1 = $xml.CreateElement("password1")
	$password1.AppendChild($xml.CreateTextNode($builder.Password)) | Out-Null
	$xml.commandline.AppendChild($password1) | Out-Null
	# add target script directory (scripts2)
	$scripts2 = $xml.CreateElement("scripts2")
	$scripts2.AppendChild($xml.CreateTextNode($schemaDirectory)) | Out-Null
	$xml.commandline.AppendChild($scripts2) | Out-Null
	# add output logging
	$outputlog = $xml.CreateElement("out")
	$outputlog.AppendChild($xml.CreateTextNode("$($databasesDirectory.FullName)SqlCompare.log")) | Out-Null
	$xml.commandline.AppendChild($outputlog) | Out-Null
	# save config file
	$xml.Save($compareParametersFile+".database-to-script")	
	
	# define command line arguments
	$sqlCompareExe = Join-Path $sqlCompareHome "SQLCompare.exe"
	$arguments = @()
	$arguments += "/argfile:`"$compareParametersFile.database-to-script`""

	# define process
	$process = New-Object System.Diagnostics.Process
	$processStartInfo = New-Object System.Diagnostics.ProcessStartInfo
	$processStartInfo.CreateNoWindow = $true
	$processStartInfo.UseShellExecute = $false
	$processStartInfo.FileName = $sqlCompareExe	
	$processStartInfo.Arguments = $arguments
	$process.StartInfo = $processStartInfo
	
	# execute process
	Write-Host "Executing SQLCompare [$sqlCompareExe /argfile:`"$compareParametersFile.database-to-script`"]"
	$process.Start() | Out-Null
	$process.WaitForExit()
	Write-Host "SQLCompare exit code: $($process.ExitCode)"
	$logContent = [string]::Join([Environment]::NewLine, (Get-Content -Path "$($databasesDirectory.FullName)SqlCompare.log"))
	Write-Host $logContent
	if ($process.ExitCode -gt 0)
	{
		Throw "SQLCompare has failed with exit code `"$($process.ExitCode)`""
	}
	
	# clean up
	# remove temp db to script argfile
	if (Test-Path "$compareParametersFile.database-to-script") { Remove-Item "$compareParametersFile.database-to-script" -Force }	
	# remove SqlCompare.log
	if (Test-Path "$($databasesDirectory.FullName)SqlCompare.log") { Remove-Item "$($databasesDirectory.FullName)SqlCompare.log" -Force }	
}

function Deployment-SyncDatabaseDataToScript
{
	param(
		$connectionString = $(Throw 'Connection string required'),
		$compareParametersFile
	)
	# check to make sure that the SqlDataCompareHome environment variable has been set
	$sqlDataCompareHome = $env:SqlDataCompareHome
	if ($sqlDataCompareHome -eq $null -or $sqlDataCompareHome -eq "") 
	{
		Throw "The environment variable `"SqlDataCompareHome`" must be set on the server to run SQLDataCompare.exe"
	}
	# update Data.xml file
	$dataFile = New-Object System.IO.FileInfo $compareParametersFile
	$dataDirectory = $dataFile.DirectoryName
	Write-Host "Sync database data to script directory for database `"$($dataFile.Directory.Name)`""
	$databasesDirectory = New-Object System.IO.DirectoryInfo("$dataDirectory\..\")
	$builder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder $connectionString
	[xml]$xml = Get-Content $compareParametersFile
	
	# add source server (server1)
	$server1 = $xml.CreateElement("server1")
	$server1.AppendChild($xml.CreateTextNode($builder.DataSource)) | Out-Null
	$xml.commandline.AppendChild($server1) | Out-Null
	# add source database (database1)
	$database1 = $xml.CreateElement("database1")
	$database1.AppendChild($xml.CreateTextNode($builder.InitialCatalog)) | Out-Null
	$xml.commandline.AppendChild($database1) | Out-Null
	# add source username (username1)
	$username1 = $xml.CreateElement("username1")
	$username1.AppendChild($xml.CreateTextNode($builder.UserID)) | Out-Null
	$xml.commandline.AppendChild($username1) | Out-Null
	# add source password (password1)
	$password1 = $xml.CreateElement("password1")
	$password1.AppendChild($xml.CreateTextNode($builder.Password)) | Out-Null
	$xml.commandline.AppendChild($password1) | Out-Null
	# add target script directory (scripts2)
	$scripts2 = $xml.CreateElement("scripts2")
	$scripts2.AppendChild($xml.CreateTextNode($dataDirectory)) | Out-Null
	$xml.commandline.AppendChild($scripts2) | Out-Null
	# add output logging
	$outputlog = $xml.CreateElement("out")
	$outputlog.AppendChild($xml.CreateTextNode("$($databasesDirectory.FullName)SqlDataCompare.log")) | Out-Null
	$xml.commandline.AppendChild($outputlog) | Out-Null
	# save config file
	$xml.Save($compareParametersFile+".data-to-script")	
	
	# define command line arguments
	$sqlDataCompareExe = Join-Path $sqlDataCompareHome "SQLDataCompare.exe"
	$arguments = @()
	$arguments += "/argfile:`"$compareParametersFile.data-to-script`""

	# define process
	$process = New-Object System.Diagnostics.Process
	$processStartInfo = New-Object System.Diagnostics.ProcessStartInfo
	$processStartInfo.CreateNoWindow = $true
	$processStartInfo.UseShellExecute = $false
	$processStartInfo.FileName = $sqlDataCompareExe	
	$processStartInfo.Arguments = $arguments
	$process.StartInfo = $processStartInfo
	
	# execute process
	Write-Host "Executing SQLDataCompare [$sqlDataCompareExe /argfile:`"$compareParametersFile.data-to-script`"]"
	$process.Start() | Out-Null
	$process.WaitForExit()
	Write-Host "SQLDataCompare exit code: $($process.ExitCode)"
	$logContent = [string]::Join([Environment]::NewLine, (Get-Content -Path "$($databasesDirectory.FullName)SqlDataCompare.log"))
	Write-Host $logContent
	if ($process.ExitCode -gt 0)
	{
		Throw "SQLDataCompare has failed with exit code `"$($process.ExitCode)`""
	}
	
	# clean up
	# remove temp db to script argfile
	if (Test-Path "$compareParametersFile.data-to-script") { Remove-Item "$compareParametersFile.data-to-script" -Force }	
	# remove SqlCompare.log
	if (Test-Path "$($databasesDirectory.FullName)SqlDataCompare.log") { Remove-Item "$($databasesDirectory.FullName)SqlDataCompare.log" -Force }	
}

function Deployment-SyncDatabaseScriptToSchema
{
	param(
		$connectionString = $(Throw 'Connection string required'),
		$compareParametersFile
	)
	# check to make sure that the SqlCompareHome environment variable has been set
	$sqlCompareHome = $env:SqlCompareHome
	if ($sqlCompareHome -eq $null -or $sqlCompareHome -eq "") 
	{
		Throw "The environment variable `"SqlCompareHome`" must be set on the server to run SQLCompare.exe"
	}
	# update Schema.xml file
	$schemaFile = New-Object System.IO.FileInfo $compareParametersFile
	$schemaDirectory = $schemaFile.DirectoryName
	Write-Host "Sync script directory to database for database `"$($schemaFile.Directory.Name)`""
	$databasesDirectory = New-Object System.IO.DirectoryInfo("$schemaDirectory\..\")
	$builder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder $connectionString
	[xml]$xml = Get-Content $compareParametersFile
	
	# add source server (server1)
	$server2 = $xml.CreateElement("server2")
	$server2.AppendChild($xml.CreateTextNode($builder.DataSource)) | Out-Null
	$xml.commandline.AppendChild($server2) | Out-Null
	# add source database (database2)
	$database2 = $xml.CreateElement("database2")
	$database2.AppendChild($xml.CreateTextNode($builder.InitialCatalog)) | Out-Null
	$xml.commandline.AppendChild($database2) | Out-Null
	# add source username (username2)
	$username2 = $xml.CreateElement("username2")
	$username2.AppendChild($xml.CreateTextNode($builder.UserID)) | Out-Null
	$xml.commandline.AppendChild($username2) | Out-Null
	# add source password (password2)
	$password2 = $xml.CreateElement("password2")
	$password2.AppendChild($xml.CreateTextNode($builder.Password)) | Out-Null
	$xml.commandline.AppendChild($password2) | Out-Null
	# add target script directory (scripts1)
	$scripts1 = $xml.CreateElement("scripts1")
	$scripts1.AppendChild($xml.CreateTextNode($schemaDirectory)) | Out-Null
	$xml.commandline.AppendChild($scripts1) | Out-Null
	# add output logging
	$outputlog = $xml.CreateElement("out")
	$outputlog.AppendChild($xml.CreateTextNode("$($databasesDirectory.FullName)SqlCompare.log")) | Out-Null
	$xml.commandline.AppendChild($outputlog) | Out-Null
	# save config file
	$xml.Save($compareParametersFile+".script-to-database")	
	
	Deployment-CreateDatabase -ConnectionString $connectionString
	
	# define command line arguments
	$sqlCompareExe = Join-Path $sqlCompareHome "SQLCompare.exe"
	$arguments = @()
	$arguments += "/argfile:`"$compareParametersFile.script-to-database`""

	# define process
	$process = New-Object System.Diagnostics.Process
	$processStartInfo = New-Object System.Diagnostics.ProcessStartInfo
	$processStartInfo.CreateNoWindow = $true
	$processStartInfo.UseShellExecute = $false
	$processStartInfo.FileName = $sqlCompareExe	
	$processStartInfo.Arguments = $arguments
	$process.StartInfo = $processStartInfo
	
	# execute process
	Write-Host "Executing SQLCompare [$sqlCompareExe /argfile:`"$compareParametersFile.script-to-database`"]"
	$process.Start() | Out-Null
	$process.WaitForExit()
	Write-Host "SQLCompare exit code: $($process.ExitCode)"
	$logContent = [string]::Join([Environment]::NewLine, (Get-Content -Path "$($databasesDirectory.FullName)SqlCompare.log"))
	Write-Host $logContent
	if ($process.ExitCode -gt 0)
	{
		Throw "SQLCompare has failed with exit code `"$($process.ExitCode)`""
	}
	
	Deployment-InsertDatabaseSyncTrackingRecord -ConnectionString $connectionString -RecordType "Schema Sync" -Version $OctopusPackageNameAndVersion -Log $logContent

	# clean up
	# remove temp script to db argfile
	if (Test-Path "$compareParametersFile.script-to-database") { Remove-Item "$compareParametersFile.script-to-database" -Force }	
	# remove SqlCompare.log
	if (Test-Path "$($databasesDirectory.FullName)SqlCompare.log") { Remove-Item "$($databasesDirectory.FullName)SqlCompare.log" -Force }	
}

function Deployment-SyncDatabaseScriptToData
{
	param(
		$connectionString = $(Throw 'Connection string required'),
		$compareParametersFile
	)
	# check to make sure that the SqlDataCompareHome environment variable has been set
	$sqlDataCompareHome = $env:SqlDataCompareHome
	if ($sqlDataCompareHome -eq $null -or $sqlDataCompareHome -eq "") 
	{
		Throw "The environment variable `"SqlDataCompareHome`" must be set on the server to run SQLDataCompare.exe"
	}
	# update Data.xml file
	$dataFile = New-Object System.IO.FileInfo $compareParametersFile
	$dataDirectory = $dataFile.DirectoryName
	Write-Host "Sync script directory to database data for database `"$($dataFile.Directory.Name)`""
	$databasesDirectory = New-Object System.IO.DirectoryInfo("$dataDirectory\..\")
	$builder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder $connectionString
	[xml]$xml = Get-Content $compareParametersFile
	
	# add target server (server2)
	$server2 = $xml.CreateElement("server2")
	$server2.AppendChild($xml.CreateTextNode($builder.DataSource)) | Out-Null
	$xml.commandline.AppendChild($server2) | Out-Null
	# add target database (database2)
	$database2 = $xml.CreateElement("database2")
	$database2.AppendChild($xml.CreateTextNode($builder.InitialCatalog)) | Out-Null
	$xml.commandline.AppendChild($database2) | Out-Null
	# add target username (username2)
	$username2 = $xml.CreateElement("username2")
	$username2.AppendChild($xml.CreateTextNode($builder.UserID)) | Out-Null
	$xml.commandline.AppendChild($username2) | Out-Null
	# add target password (password2)
	$password2 = $xml.CreateElement("password2")
	$password2.AppendChild($xml.CreateTextNode($builder.Password)) | Out-Null
	$xml.commandline.AppendChild($password2) | Out-Null
	# add source script directory (scripts1)
	$scripts1 = $xml.CreateElement("scripts1")
	$scripts1.AppendChild($xml.CreateTextNode($dataDirectory)) | Out-Null
	$xml.commandline.AppendChild($scripts1) | Out-Null
	# add output logging
	$outputlog = $xml.CreateElement("out")
	$outputlog.AppendChild($xml.CreateTextNode("$($databasesDirectory.FullName)SqlDataCompare.log")) | Out-Null
	$xml.commandline.AppendChild($outputlog) | Out-Null
	# save config file
	$xml.Save($compareParametersFile+".script-to-data")	
		
	# define command line arguments
	$sqlDataCompareExe = Join-Path $sqlDataCompareHome "SQLDataCompare.exe"
	$arguments = @()
	$arguments += "/argfile:`"$compareParametersFile.script-to-data`""

	# define process
	$process = New-Object System.Diagnostics.Process
	$processStartInfo = New-Object System.Diagnostics.ProcessStartInfo
	$processStartInfo.CreateNoWindow = $true
	$processStartInfo.UseShellExecute = $false
	$processStartInfo.FileName = $sqlDataCompareExe	
	$processStartInfo.Arguments = $arguments
	$process.StartInfo = $processStartInfo
	
	# execute process
	Write-Host "Executing SQLDataCompare [$sqlDataCompareExe /argfile:`"$compareParametersFile.script-to-data`"]"
	$process.Start() | Out-Null
	$process.WaitForExit()
	Write-Host "SQLDataCompare exit code: $($process.ExitCode)"
	$logContent = [string]::Join([Environment]::NewLine, (Get-Content -Path "$($databasesDirectory.FullName)SqlDataCompare.log"))
	Write-Host $logContent
	if ($process.ExitCode -gt 0)
	{
		Throw "SQLDataCompare has failed with exit code `"$($process.ExitCode)`""
	}
	
	Deployment-InsertDatabaseSyncTrackingRecord -ConnectionString $connectionString -RecordType "Data Sync" -Version $OctopusPackageNameAndVersion -Log $logContent
	
	# clean up
	# remove temp script to db argfile
	if (Test-Path "$compareParametersFile.script-to-data") { Remove-Item "$compareParametersFile.script-to-data" -Force }	
	# remove SqlCompare.log
	if (Test-Path "$($databasesDirectory.FullName)SqlDataCompare.log") { Remove-Item "$($databasesDirectory.FullName)SqlDataCompare.log" -Force }	
}

function Deployment-CreateDatabase
{
	param(
		$connectionString,
		$server,
		$database,
		$username,
		$password
	)
	# Create and open a database connection
	$sqlConnection = $null
	if ($connectionString)
	{
		$sqlConnection = New-Object System.Data.SqlClient.SqlConnection $connectionString
	}
	else
	{
		$sqlConnection = New-Object System.Data.SqlClient.SqlConnection "server=$server;initial catalog=$database;user id=$username;password=$password"	
	}
	
	# set the connection to the master database
	$builder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder $sqlConnection.ConnectionString	
	$originalDatabase = $builder["Initial Catalog"]
	$builder["Initial Catalog"] = "master"	
	$sqlConnection = New-Object System.Data.SqlClient.SqlConnection $builder.ConnectionString
	try
	{
		$sqlConnection.Open()	
		$sqlCommand = $sqlConnection.CreateCommand()
		$sqlCommand.CommandText = "SELECT COUNT(*) FROM sys.databases WHERE name = '$originalDatabase'"
        [int]$dbCount = $sqlCommand.ExecuteScalar()
        if ($dbCount -lt 1)
        {
			# create the new database
			Write-Host "Creating new database `"$originalDatabase`""		
			$sqlCommand.CommandText = "CREATE DATABASE [$originalDatabase]"
			$sqlCommand.ExecuteNonQuery() | Out-Null			
		}
		else
		{
			Write-Host "Existing database `"$originalDatabase`" was found"
		}
		Deployment-CreateTrackingTable -ConnectionString $connectionString
	}
	catch [System.Exception]
	{
  		Write-Error "Error creating new database `"$originalDatabase`""
	}
	finally
	{
		# Close the database connection
		$sqlConnection.Close()
	}
}

function Deployment-CreateTrackingTable
{
	param(
		$connectionString
	)
	# Create and open a database connection
	$sqlConnection = New-Object System.Data.SqlClient.SqlConnection $connectionString	
	try
	{
		$sqlConnection.Open()	
		$sqlCommand = $sqlConnection.CreateCommand()
		$sqlCommand.CommandText = "SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '_Tracking' AND TABLE_TYPE = 'BASE TABLE'"
        [int]$tableCount = $sqlCommand.ExecuteScalar()
        if ($tableCount -lt 1)
        {
			# create the new _tracking table
			Write-Host "Creating new `"_Tracking`" table"		
			$sqlCommand.CommandText = "CREATE TABLE [dbo].[_Tracking]([Id] [int] IDENTITY(1,1) NOT NULL, [Date] [datetime] NULL, [Type] [varchar](50) NULL, [Version] [varchar](50) NULL, [Log] [nvarchar](max) NULL, CONSTRAINT [PK__Tracking] PRIMARY KEY CLUSTERED ([Id] ASC))"
			$sqlCommand.ExecuteNonQuery() | Out-Null
		}
		else
		{
			Write-Host "Existing `"_Tracking`" table found"
		}
	}
	catch [System.Exception] 
	{
  		Write-Error "Error creating new `"_Tracking`" table"
	}
	finally
	{
		# Close the database connection
		$sqlConnection.Close()
	}	
}

function Deployment-InsertDatabaseSyncTrackingRecord
{
	param(
		$connectionString,
		$recordType,
		$version,
		$log
	)
	# Create and open a database connection
	$sqlConnection = $null
	$sqlConnection = New-Object System.Data.SqlClient.SqlConnection $connectionString
	
	try
	{
		$sqlConnection.Open()	
		$sqlCommand = $sqlConnection.CreateCommand()
		$sqlCommand.CommandText = "INSERT INTO [_Tracking] (Date, Type, Version, Log) VALUES (GETDATE(), @Type, @Version, @Log)"

		# type
		if ($recordType)
		{
			$sqlCommand.Parameters.Add("@Type", $recordType) | Out-Null		
		}
		else
		{
			$sqlCommand.Parameters.Add("@Type", [DBNull]::Value) | Out-Null
		}
		
		# version
		if ($version)
	{
			$sqlCommand.Parameters.Add("@Version", $version) | Out-Null		
	}	
		else
		{
			$sqlCommand.Parameters.Add("@Version", [DBNull]::Value) | Out-Null
}

		# log
		if ($log)
		{
			$sqlCommand.Parameters.Add("@Log", $log) | Out-Null		
		}
		else
		{
			$sqlCommand.Parameters.Add("@Log", [DBNull]::Value) | Out-Null
		}

		$sqlCommand.ExecuteNonQuery() | Out-Null
	}
	catch [System.Exception] 
	{
  		Write-Error "Error inserting new database sync tracking record"		
	}
	finally
	{
		# Close the database connection
		$sqlConnection.Close()
	}
}

#endregion