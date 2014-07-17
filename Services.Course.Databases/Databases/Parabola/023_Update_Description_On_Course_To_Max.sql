RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
RAISERROR( 'START: Column (Description) being updated on Table (Course)' , 0 , 1 )WITH NOWAIT;
RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

IF EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'Description' and c.TABLE_NAME = 'Course'
			And CHARACTER_MAXIMUM_LENGTH = -1)  -- -1 = max
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Column (Description) exists on Table (Course) and is set to MAX' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

END
ELSE
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'UPDATE BEGIN: Column (Description) being updated on Table (Course)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	ALTER TABLE [Course] ALTER COLUMN [Description] nvarchar(max) NULL;

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'UPDATE END: Column (Description) being updated on Table (Course)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

END


RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
RAISERROR( 'FINISH: Column (Description) being updated on Table (Course)' , 0 , 1 )WITH NOWAIT;
RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;