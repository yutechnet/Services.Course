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
 

# import deployment module
$PSScriptRoot = $MyInvocation.MyCommand.Path | Split-Path
Import-Module -Name $PSScriptRoot\DeploymentModule.psm1 -Force


##############################
# Deploy Windows Service

# register NServiceBus windows service
Deployment-InstallNServiceBusService -Path "$OctopusPackageDirectoryPath" -Name "$WindowsServiceName" -DisplayName "$WindowsServiceDisplayName" -Description "$WindowsServiceDescription" -Account "$WindowsServiceAccount" -Password "$WindowsServiceAccountPassword" -StartMode "$WindowsServiceStartMode"