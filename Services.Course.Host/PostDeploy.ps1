###############################################################################
# Required User Defined Octopus Variables
# =======================================
# Caller
# IISAppPoolIdentity (ACL permissions)
# OctopusAppRoot
# OctopusPackageDirectoryPath (ACL permissions)
# WindowsServiceName
#
#
# OctopusPackageName (System defined variable)

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










# set ACL permissions
# Read
Deployment-SetCustomACLPermissions -Account "$IISAppPoolIdentity" -Path "$OctopusPackageDirectoryPath" -FileSystemRights "ReadAndExecute, Synchronize" -AccessControlType "Allow" -InheritanceFlags "ContainerInherit, ObjectInherit" -PropagationFlags "None"

# cleanup
Deployment-PurgeOldOctopusVersions 5
Deployment-CleanupDeploymentConfigs $PSScriptRoot
Deployment-CleanupDeploymentModules $PSScriptRoot