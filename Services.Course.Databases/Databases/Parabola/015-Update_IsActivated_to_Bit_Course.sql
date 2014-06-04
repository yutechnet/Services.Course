IF EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'IsActivated' and c.TABLE_NAME = 'Course' AND IS_NULLABLE='Yes')
	BEGIN
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'CREATE BEGIN: Column (IsActivated) of Table (Course) being updated to null' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

		ALTER TABLE Course ALTER COLUMN IsActivated bit NULL;

		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'CREATE END: Column (IsActivated) of Table (Course) being updated to null' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	END
ELSE
	BEGIN
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'NO CHANGE: Column (IsActivated) of Table (Course) being updated to null' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	END
GO