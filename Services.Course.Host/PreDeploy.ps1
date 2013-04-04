###############################################################################
# Required User Defined Octopus Variables
# =======================================
# IISAppPoolIdentity
# IISAppPoolIdentityPassword
# IISAppPoolName
# IISHostHeaders
# IISSslCertThumbprint
# IISSslPfxPassword
# IISSslPfxPath
# IISWebApplicationName
# IISWebsiteName 
# OctopusPackageDirectoryPath
#
#
# OctopusEnvironmentName (System defined variable)


# import deployment module
$PSScriptRoot = $MyInvocation.MyCommand.Path | Split-Path
Import-Module -Name $PSScriptRoot\DeploymentModule.psm1 -Force

# required for enum values
Add-Type -Path C:\Windows\System32\inetsrv\Microsoft.Web.Administration.dll

# update config transforms with Octopus variable values
Get-ChildItem $PSScriptRoot -Include "*.Release.config", "*.$OctopusEnvironmentName.config" -Recurse | ForEach-Object {
	Deployment-UpdateConfigurationTransform -TransformFile $_ -Environment $OctopusEnvironmentName
} 

# check configure iis
Deployment-CheckConfigureIIS

# setup web application
Deployment-SetupWebApplication -WebsiteName "$IISWebsiteName" -WebApplicationName "$IISWebApplicationName" -WebApplicationDirectoryPath "$OctopusPackageDirectoryPath" -AppPoolName "$IISAppPoolName"

# setup app pool
Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'ManagedRuntimeVersion'     -AppPoolPropertyValue 'v4.0'
Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'ManagedPipelineMode'       -AppPoolPropertyValue ([int] [Microsoft.Web.Administration.ManagedPipelineMode]::Identity)          -CompareValue 'Integrated'   #Integrated = 0, Classic = 1
Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'ProcessModel.IdentityType' -AppPoolPropertyValue ([int] [Microsoft.Web.Administration.ProcessModelIdentityType]::SpecificUser) -CompareValue 'SpecificUser' #LocalSystem = 0, LocalService = 1, NetworkService = 2, SpecificUser = 3, ApplicationPoolIdentity = 4
Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'ProcessModel.Username'     -AppPoolPropertyValue $IISAppPoolIdentity
Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'ProcessModel.Password'     -AppPoolPropertyValue $IISAppPoolIdentityPassword -Hide