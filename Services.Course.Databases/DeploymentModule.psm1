$Script:ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

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


function Deployment-UpdateDatabase
{
	param (
		$release = $(Throw 'Release name required')
	)
	
	$pinfo = New-Object System.Diagnostics.ProcessStartInfo
	$pinfo.CreateNoWindow = $true
	$pinfo.UseShellExecute = $false
	$pinfo.FileName = ".\DatabaseTool\BpeProducts.Scm.DatabaseUpdater.exe"
	$pinfo.Arguments = "--deploy --release=`"$release`""
	$pinfo.RedirectStandardOutput = $true
	$pinfo.RedirectStandardError = $true
	Write-Host "$($pinfo.FileName) $($pinfo.Arguments)"
	$p = New-Object System.Diagnostics.Process
	$p.StartInfo = $pinfo
	$p.Start() | Out-Null
	$stdOut = $p.StandardOutput.ReadToEnd()
	$stdOut = Deployment-FilterOutput -Output $stdOut
	$stdErr = $p.StandardError.ReadToEnd()
	$stdErr = Deployment-FilterOutput -Output $stdErr
	$p.WaitForExit()
	$exitCode = $p.ExitCode
	Write-Host "Exit Code: $exitCode"
	Write-Host "----- Output -----"
	Write-Host $stdOut
	Write-Host $stdErr
	Write-Host "----- End Output -----"
	if ($exitCode -ne 0)
	{
		Exit 1
	}
}

function Deployment-FilterOutput
{
	param (
		$output
	)
	
	$filteredOutput = ""
	$previousLine = ""
	$output.Split("`r`n") | ForEach-Object {
		switch ($_.Trim())
		{
			""  { break } # skip empty lines
			"|" { break } # skip lines that only have "|"
			"-" { break } # skip lines that only have "|"
			default {
				# add a separation line for start of new script
				if ($_.Trim().StartsWith("Executing SQL Server script"))
				{
					$filteredOutput += " `r`n"
				}
				# add the line
				$filteredOutput += "$($_.Trim())`r`n"
			}
		}
		$previousLine = $_.Trim()
	}
	
	return $filteredOutput
}