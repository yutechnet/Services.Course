IF EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'ExtensionAssets' and c.TABLE_NAME = 'Course')
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Column (ExtensionAssets) Already Exists on Table (Course)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE BEGIN: Column (ExtensionAssets) being added to Table (Course)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	ALTER TABLE [Course] add [ExtensionAssets] nvarchar(MAX) NULL;

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE END: Column (ExtensionAssets) being added to Table (Course)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END

