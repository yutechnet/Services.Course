
IF EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'MaxPoint' and c.TABLE_NAME = 'CourseLearningActivity')
	BEGIN
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'CREATE BEGIN: Column (MaxPoint) of Table (CourseLearningActivity) being updated to Decimal(8,3)' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

		ALTER TABLE dbo.CourseLearningActivity ALTER COLUMN MaxPoint DECIMAL(16,3);

		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'CREATE END: Column (MaxPoint) of Table (CourseLearningActivity) being updated to Decimal(8,3)' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	END
ELSE
	BEGIN
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'NO CHANGE: Column (MaxPoint) Not Exists on Table (CourseLearningActivity)' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	END
GO
