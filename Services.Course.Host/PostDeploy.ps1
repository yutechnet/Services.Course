###############################################################################
# Required User Defined Octopus Variables
# =======================================
# Caller
# OctopusAppRoot
#
#
# OctopusPackageName (System defined variable)


# import deployment module
$PSScriptRoot = $MyInvocation.MyCommand.Path | Split-Path
Import-Module -Name $PSScriptRoot\DeploymentModule.psm1 -Force










# cleanup
Deployment-PurgeOldOctopusVersions 5
Deployment-CleanupDeploymentConfigs $PSScriptRoot
Deployment-CleanupDeploymentModules $PSScriptRoot