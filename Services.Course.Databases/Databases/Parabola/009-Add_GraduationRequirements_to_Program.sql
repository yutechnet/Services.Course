
IF EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'GraduationRequirements' and c.TABLE_NAME = 'Program')
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Column (GraduationRequirements) Already Exists on Table (Program)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE BEGIN: Column (GraduationRequirements) being added to Table (Program)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	ALTER TABLE [Program] add [GraduationRequirements] nvarchar(max) NULL;

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE END: Column (GraduationRequirements) being added to Table (Program)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END

