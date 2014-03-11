USE Parabola
GO

IF EXISTS(SELECT 1 FROM sys.columns c WHERE c.name = 'MaxPoint' AND c.object_id = Object_id('CourseLearningActivity'))
	BEGIN
		ALTER TABLE dbo.CourseLearningActivity ALTER COLUMN MaxPoint DECIMAL(8,3);
		PRINT 'Column MaxPoint successfully changed to DECIMAL(8,3) in table CourseLearningActivity';
	END
ELSE
	BEGIN
		PRINT 'Column MaxPoint not exists in table CourseLearningActivity, skip update';
	END
GO
