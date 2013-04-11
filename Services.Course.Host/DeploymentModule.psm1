$script:ErrorActionPreference = 'Stop'
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
	[array]$regexVariables = Select-String "\`"\`$OctopusVariable\.(?<VariableName>.*?)\`"" $transformFile -AllMatches | Select -Expand Matches
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
				#Write-Host "Unable to locate variable: $octopusVariableName"
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
		# get the transform file content
		[string] $transformFileContent = Get-Content $transformFile
	
		# perform replacement on placeholder values
		foreach($key in $($variableHash.keys))
		{
			$transformFileContent = $transformFileContent -replace "`"\`$OctopusVariable.$key`"", "`"$($variableHash[$key])`""
		}
	
		#Set-Content -Encoding $transformFile $transformFileContent
		[System.IO.File]::WriteAllText($transformFile, $transformFileContent)
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