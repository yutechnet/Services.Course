IF EXISTS (Select * From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'CourseSegmentId' and c.TABLE_NAME = 'LearningMaterial' AND IS_NULLABLE='NO')
	BEGIN
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'CREATE BEGIN: Column (CourseSegmentId) of Table (LearningMaterial) being updated to null' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

		ALTER TABLE LearningMaterial ALTER COLUMN CourseSegmentId UNIQUEIDENTIFIER NULL;

		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'CREATE END: Column (CourseSegmentId) of Table (LearningMaterial) being updated to null' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	END
ELSE
	BEGIN
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'NO CHANGE: Column (CourseSegmentId) of Table (LearningMaterial) being updated to null' , 0 , 1 )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	END
GO
