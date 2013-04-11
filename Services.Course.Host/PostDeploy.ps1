###############################################################################
# Required User Defined Octopus Variables
# =======================================
# Caller
# IISAppPoolIdentity
# OctopusAppRoot
# OctopusPackageDirectoryPath
#
#
# OctopusPackageName (System defined variable)


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