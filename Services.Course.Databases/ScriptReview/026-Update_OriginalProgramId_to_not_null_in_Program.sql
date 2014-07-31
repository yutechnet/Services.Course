IF EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'OriginalProgramId' 
			and c.TABLE_NAME = 'Program'
			and c.IS_NULLABLE = 'No')
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Column (OriginalProgramId) Already Non-Null on Table (Program)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE BEGIN: Column (OriginalProgramId) being updated on Table (Program)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	UPDATE Program SET OriginalProgramId = ProgramId WHERE OriginalProgramId IS NULL;

	RAISERROR( 'CREATE STATUS: Column (OriginalProgramId) Set to True (1) for all NULL Rows' , 0 , 1 )WITH NOWAIT;
    
	ALTER TABLE Program ALTER COLUMN OriginalProgramId UNIQUEIDENTIFIER NOT NULL  

	RAISERROR( 'CREATE STATUS: Column (OriginalProgramId) set to Non-Null' , 0 , 1 )WITH NOWAIT;

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE END: Column (OriginalProgramId) being added to Table (Program)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
