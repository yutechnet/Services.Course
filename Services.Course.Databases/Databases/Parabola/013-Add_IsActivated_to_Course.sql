
IF EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'IsActivated' and c.TABLE_NAME = 'Course')
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Column (IsActivated) Already Exists on Table (Course)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE BEGIN: Column (IsActivated) being added to Table (Course)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	ALTER TABLE [Course] add IsActivated nvarchar(max) NULL;

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE END: Column (IsActivated) being added to Table (Course)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END

