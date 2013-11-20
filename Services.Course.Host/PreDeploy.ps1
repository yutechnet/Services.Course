###############################################################################
# Required User Defined Octopus Variables
# =======================================
# IdentityIssuerCertThumbprint
# IISAppPoolIdentity
# IISAppPoolIdentityPassword
# IISAppPoolName
# IISHostHeaders
# IISSslCertThumbprint
# IISSslPfxPassword
# IISSslPfxPath
# IISWebApplicationName (Web App)
# IISWebsiteName
# OctopusPackageDirectoryPath
# WindowsServiceName
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

# update config transforms with Octopus variable values
Get-ChildItem $PSScriptRoot -Include "*.Release.config", "*.$OctopusEnvironmentName.config" -Recurse | ForEach-Object {
	Deployment-UpdateConfigurationTransform -TransformFile $_ -Environment $OctopusEnvironmentName
} 

# configure Windows features
$installWindowsFeatures = @(
	# Feature, Description
	("Web-Server", "Web Server"),
	("Web-Common-Http", "Web Server\Common HTTP Features"),
	("Web-Static-Content", "Web Server\Common HTTP Features\Static Content"),
	("Web-Default-Doc", "Web Server\Common HTTP Features\Default Document"),
	("Web-Dir-Browsing", "Web Server\Common HTTP Features\Directory Browsing"),
	("Web-Http-Errors", "Web Server\Common HTTP Features\HTTP Errors"),
	("Web-Http-Redirect", "Web Server\Common HTTP Features\HTTP Redirection"),
	("Web-App-Dev", "Web Server\Application Development"),
	("Web-Asp-Net", "Web Server\Application Development\ASP.Net"),
	("Web-Asp-Net45", "Web Server\Application Development\ASP.Net (4.5)"),
	("Web-Net-Ext", "Web Server\Application Development\.Net Extensibility"),
	("Web-Net-Ext45", "Web Server\Application Development\.Net Extensibility (4.5)"),
	("Web-ISAPI-Ext", "Web Server\Application Development\ISAPI Extensions"),
	("Web-ISAPI-Filter", "Web Server\Application Development\ISAPI Filters"),
	("Web-Health", "Web Server\Health and Diagnostics"),
	("Web-Http-Logging", "Web Server\Health and Diagnostics\HTTP Logging"),
	("Web-Log-Libraries", "Web Server\Health and Diagnostics\Logging Tools"),
	("Web-Request-Monitor", "Web Server\Health and Diagnostics\Request Monitor"),
	("Web-Http-Tracing", "Web Server\Health and Diagnostics\Tracing"),
	("Web-Security", "Web Server\Security"),
	("Web-Basic-Auth", "Web Server\Security\Basic Authentication"),
	("Web-Windows-Auth", "Web Server\Security\Windows Authentication"),
	("Web-Digest-Auth", "Web Server\Security\Digest Authentication"),
	("Web-Client-Auth", "Web Server\Security\Client Certificate Mapping Authentication"),
	("Web-Cert-Auth", "Web Server\Security\IIS Client Certificate Mapping Authentication"),
	("Web-Url-Auth", "Web Server\Security\URL Authorization"),
	("Web-Filtering", "Web Server\Security\Request Filtering"),
	("Web-IP-Security", "Web Server\Security\IP and Domain Restrictions"),
	("Web-Performance", "Web Server\Performance"),
	("Web-Stat-Compression", "Web Server\Performance\Static Content Compression"),
	("Web-Dyn-Compression", "Web Server\Performance\Dynamic Content Compression"),
	("Web-Mgmt-Tools", "Management Tools"),
	("Web-Mgmt-Console", "Management Tools\IIS Console Management"),
	("Web-Scripting-Tools", "Management Tools\IIS Management Scripts and Tools"),
	("Web-Mgmt-Service", "Management Tools\Management Service")
)
Deployment-InstallWindowsFeatures -FeatureArray $installWindowsFeatures

$removeWindowsFeatures = @(
	# Feature, Description
	("Web-ASP", "Web Server\Application Development\ASP"),
	("Web-CGI", "Web Server\Application Development\CGI")
)
Deployment-RemoveWindowsFeatures -FeatureArray $removeWindowsFeatures

##############################
# Deploy Website

# check configure iis
Deployment-CheckConfigureIIS

# required for app pool enum values
Add-Type -Path C:\Windows\System32\inetsrv\Microsoft.Web.Administration.dll

# setup website
Deployment-RemoveDefaultWebSite
Deployment-SetupWebsite -WebsiteName "$IISWebsiteName" -WebsiteDirectoryPath "$IISWebsitePath" -AppPoolName "$IISWebsiteAppPoolName"
Deployment-SetWebsiteBinding -WebsiteName "$IISWebsiteName" -Protocol "http" -IPAddress "*" -Port "80" -HostHeader "$IISHostHeaders"
if ($IISHostHeaders -ne $null)
{
	Deployment-RemoveWebsiteBinding -WebsiteName "$IISWebsiteName" -Protocol "http" -IPAddress "*" -Port "80" -HostHeader ""
}

# setup app pool
Deployment-SetAppPoolProperty -AppPoolName "$IISWebsiteAppPoolName" -AppPoolProperty 'managedRuntimeVersion'     -AppPoolPropertyValue 'v4.0'
Deployment-SetAppPoolProperty -AppPoolName "$IISWebsiteAppPoolName" -AppPoolProperty 'managedPipelineMode'       -AppPoolPropertyValue ([int] [Microsoft.Web.Administration.ManagedPipelineMode]::Identity)          -CompareValue 'Integrated'   #Integrated = 0, Classic = 1
if ($IISAppPoolIdentity -ne $null)
{
	Deployment-SetAppPoolProperty -AppPoolName "$IISWebsiteAppPoolName" -AppPoolProperty 'processModel.identityType' -AppPoolPropertyValue ([int] [Microsoft.Web.Administration.ProcessModelIdentityType]::SpecificUser) -CompareValue 'SpecificUser' #LocalSystem = 0, LocalService = 1, NetworkService = 2, SpecificUser = 3, ApplicationPoolIdentity = 4
	Deployment-SetAppPoolProperty -AppPoolName "$IISWebsiteAppPoolName" -AppPoolProperty 'processModel.userName'     -AppPoolPropertyValue $IISWebsiteAppPoolIdentity
	Deployment-SetAppPoolProperty -AppPoolName "$IISWebsiteAppPoolName" -AppPoolProperty 'processModel.password'     -AppPoolPropertyValue $IISWebsiteAppPoolIdentityPassword -Hide
}

# Removing default IIS setting OPTIONVebHandler
Deployment-RemoveHandlerMapping -HandlerName "OPTIONSVerbHandler" -WebsiteName "$IISWebsiteName"

# setup web application
Deployment-SetupWebApplication -WebsiteName "$IISWebsiteName" -WebApplicationName "$IISWebApplicationName" -WebApplicationDirectoryPath "$OctopusPackageDirectoryPath" -AppPoolName "$IISAppPoolName"

# setup app pool
Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'managedRuntimeVersion'     -AppPoolPropertyValue 'v4.0'
Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'managedPipelineMode'       -AppPoolPropertyValue ([int] [Microsoft.Web.Administration.ManagedPipelineMode]::Identity)          -CompareValue 'Integrated'   #Integrated = 0, Classic = 1
if ($IISAppPoolIdentity -ne $null)
{
	Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'processModel.identityType' -AppPoolPropertyValue ([int] [Microsoft.Web.Administration.ProcessModelIdentityType]::SpecificUser) -CompareValue 'SpecificUser' #LocalSystem = 0, LocalService = 1, NetworkService = 2, SpecificUser = 3, ApplicationPoolIdentity = 4
	Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'processModel.userName'     -AppPoolPropertyValue $IISAppPoolIdentity
	Deployment-SetAppPoolProperty -AppPoolName "$IISAppPoolName" -AppPoolProperty 'processModel.password'     -AppPoolPropertyValue $IISAppPoolIdentityPassword -Hide
}

# verify identity signing certificate is installed
Deployment-LocateCertificate -CertThumbprint "$IdentityIssuerCertThumbprint" -CertStoreName "My" -CertStoreLocation "LocalMachine"  -CertPfx $IdentityIssuerCertPfx -CertPfxPassword $IdentityIssuerCertPfxPassword 

# set eventlog source
if($EventLogSource -ne $null)
{
	Deployment-SetEventLogSource -EventLogName "Application" -EventSource "$EventLogSource"
}

# create logging directory
if($Log4netLogFile -ne $null)
{
	Deployment-CreateParentDirectory -File "$Log4netLogFile"
}