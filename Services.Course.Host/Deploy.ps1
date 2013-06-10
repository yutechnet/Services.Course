###############################################################################
# Required User Defined Octopus Variables
# =======================================
# OctopusPackageDirectoryPath
# WindowsServiceAccount
# WindowsServiceAccountPassword
# WindowsServiceDescription
# WindowsServiceDisplayName
# WindowsServiceName
# WindowsServiceStartMode
#
# OctopusEnvironmentName (System defined variable)
 
trap [Exception]
{
	Write-Host "########################################################################################################################"
	Write-Host "$($_.InvocationInfo.ScriptName) - Line#: $($_.InvocationInfo.ScriptLineNumber)"
	Write-Host "Invocation: $($_.InvocationInfo.InvocationName)"
	Write-Host $_.Exception.Message
	Write-Host $_.Exception.StackTrace
	Write-Host "########################################################################################################################"
	break
}

# import deployment module
$PSScriptRoot = $MyInvocation.MyCommand.Path | Split-Path
Import-Module -Name $PSScriptRoot\DeploymentModule.psm1 -Force
