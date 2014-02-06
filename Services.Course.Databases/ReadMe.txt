Project Folder Structure
========================
	DatabaseTool (Directory)
		BpeProducts.Scm.DatabaseUpdater.exe
		<supporting files>
	Databases (Directory)
		ConnectionStrings.config
		ConnectionStrings.Release.config
		Database1 (Directory)
			<deployment scripts (.sql)>
		Database2 (Directory) (Optional)
			<deployment scripts (.sql)>
	Deploy.ps1
	DeploymentModule.psm1
	PostDeploy.ps1
	PreDeploy.ps1
	ReadMe.txt (this file)


Setting Up A New Database Deployment Project
============================================
Open VS2012
Create a new Visual C# - Class Library project
Delete the Class1.cs default class file
Keep the default AssemblyInfo.cs file under the Properties folder
Add the following NuGet package references
	BpeProucts.Scm.DatabaseUpdater
Update configuration for specific database packaging
	Update Deploy.ps1, PostDeploy.ps1, and PreDeploy.ps1 to specific database needs

The target database
	The (empty) target database needs to already exist
	The minimum permissions for the OctopusDeploy SQL user need to be set to db_datareader, db_datawriter, and db_ddladmin on the target database
	The minimum permissions for the svc_products SQL user need to be set to db_datareader and db_datawriter on the target database


Updating The DatabaseTool Inside The Database Deployment Project
================================================================
To update the DatabaseTool, simply update the BpeProucts.Scm.DatabaseUpdater nuget reference.
This will update all of the files under the DatabaseTool directory and the DeploymentModule.psm1 file.
This will NOT update any of the deployment scripts (PreDeploy.ps1, Deploy.ps1, PostDeploy.ps1), nuspec file, or ConnectionStrings.config files.



#############################################
#############################################
##                                         ##
##         Scripts and Other Stuff         ##
##                                         ##
#############################################
#############################################

SQL Statement to manually create SchemaVersions table
=====================================================
CREATE TABLE SchemaVersions (
	[Id] int identity(1,1) NOT NULL constraint PK_SchemaVersions_Id primary key,
	[ScriptName] nvarchar(255) NOT NULL,
	[Release] nvarchar(255) NULL,
	[Applied] datetime NOT NULL
)


SQL Statement to manually seed the SchemaVersions table
=======================================================
INSERT INTO SchemaVersions (ScriptName, Release, Applied) VALUES ('UpdateScriptNameHere','Manually Seeded',GetDate())


SQL Statement to create OctopusDeploy user on server
====================================================
CREATE LOGIN [OctopusDeploy] WITH PASSWORD = 'MyPasswordGoesHere'


SQL Statement to create OctopusDeploy user on database
======================================================
CREATE USER [OctopusDeploy] FOR LOGIN [OctopusDeploy] WITH DEFAULT_SCHEMA=[dbo]


SQL Statement to assign OctopusDeploy user permissions on database
==================================================================
EXEC sp_addrolemember 'db_datareader', 'OctopusDeploy'
EXEC sp_addrolemember 'db_datawriter', 'OctopusDeploy'
EXEC sp_addrolemember 'db_ddladmin',   'OctopusDeploy'


Query Azure database role assignments
=====================================
SELECT a.name AS [Role], b.name AS [User] 
FROM [sys].[sysusers] a
JOIN [sys].[database_role_members] r ON r.role_principal_id = a.uid
JOIN [sys].[sysusers] b ON b.uid = r.member_principal_id


Default Database Roles
======================
db_owner
db_accessadmin
db_securityadmin
db_ddladmin
db_backupoperator
db_datareader
db_datawriter
db_denydatareader
db_denydatawriter



