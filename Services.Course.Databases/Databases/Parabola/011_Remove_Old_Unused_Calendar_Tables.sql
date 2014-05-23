IF EXISTS (Select 1 From INFORMATION_SCHEMA.TABLES t 
			Where t.TABLE_NAME = 'Task'
			And t.TABLE_SCHEMA = 'dbo')
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'DROP BEGIN: Dropping Task Table' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	DROP TABLE [dbo].[Task]

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'DROP END: Task Table Dropped' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Task Table does not Exist' , 16 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

END

IF EXISTS (Select 1 From INFORMATION_SCHEMA.TABLES t 
			Where t.TABLE_NAME = 'CalendarRelationship'
			And t.TABLE_SCHEMA = 'dbo')
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'DROP BEGIN: Dropping [CalendarRelationship] Table' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	DROP TABLE [dbo].[CalendarRelationship]

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'DROP END: [CalendarRelationship] Table Dropped' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: [CalendarRelationship] Table does not Exist' , 16 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

END

IF EXISTS (Select 1 From INFORMATION_SCHEMA.TABLES t 
			Where t.TABLE_NAME = 'Meeting'
			And t.TABLE_SCHEMA = 'dbo')
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'DROP BEGIN: Dropping [[Meeting]] Table' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	DROP TABLE [dbo].[Meeting]

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'DROP END: [[Meeting]] Table Dropped' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: [[Meeting]] Table does not Exist' , 16 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

END

IF EXISTS (Select 1 From INFORMATION_SCHEMA.TABLES t 
			Where t.TABLE_NAME = 'Calendar'
			And t.TABLE_SCHEMA = 'dbo')
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'DROP BEGIN: Dropping Calendar Table' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	
	DROP TABLE [dbo].[Calendar]

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'DROP END: Calendar Table Dropped' , 0 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
END
ELSE
BEGIN

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'NO CHANGE: Calendar Table does not Exist' , 16 , 1 )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

END