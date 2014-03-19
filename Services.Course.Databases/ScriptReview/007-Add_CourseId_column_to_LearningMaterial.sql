
IF EXISTS (Select 1 From INFORMATION_SCHEMA.COLUMNS c 
			Where c.COLUMN_NAME = 'CourseId' and c.TABLE_NAME = 'LearningMaterial')
								  
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Column (CourseId) Already Exists on Table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE BEGIN: Column (CourseId) being added to Table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	ALTER TABLE [LearningMaterial] add [CourseId] UNIQUEIDENTIFIER NULL;

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE END: Column (CourseId) being added to Table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_LearningMaterial_Course') AND parent_object_id = OBJECT_ID(N'LearningMaterial'))
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Foreign key (FK_LearningMaterial_Course) Already Exists on table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE BEGIN: Foreign key (FK_LearningMaterial_Course) being added on table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	ALTER TABLE LearningMaterial  WITH CHECK ADD  CONSTRAINT FK_LearningMaterial_Course FOREIGN KEY(CourseId)
	REFERENCES Course (CourseId)
	ALTER TABLE LearningMaterial CHECK CONSTRAINT FK_LearningMaterial_Course

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE END: Foreign key (FK_LearningMaterial_Course) being added on table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'LearningMaterial') AND name = N'IDX_LearningMaterial_CourseId')
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: NonClustered Index (IDX_LearningMaterial_CourseId) Already Exists on table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE BEGIN: NonClustered Index (IDX_LearningMaterial_CourseId) being added on table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	CREATE NONCLUSTERED INDEX IDX_LearningMaterial_CourseId ON dbo.LearningMaterial
	(
		CourseId ASC
	)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'CREATE END: NonClustered Index (IDX_LearningMaterial_CourseId) being added on table (LearningMaterial)' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
Go


