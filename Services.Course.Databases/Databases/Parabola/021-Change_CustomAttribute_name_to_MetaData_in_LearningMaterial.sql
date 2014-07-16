IF NOT EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'CustomAttribute' and c.TABLE_NAME = 'LearningMaterial')
								  
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Column (CustomAttribute) Does Not Exists on Table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE BEGIN: Column (CustomAttribute) being changed name(MetaData) to Table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	EXEC sp_rename N'dbo.LearningMaterial.CustomAttribute', N'MetaData', N'Column';

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE END: Column (CustomAttribute) being changed name(MetaData) to Table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END

