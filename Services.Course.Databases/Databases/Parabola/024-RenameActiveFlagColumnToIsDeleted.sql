

DECLARE @TableName varchar(500), @ColumnName varchar(500),
	@SchemaName varchar(500), @NewColumnName varchar(50),
	@OldColumnName varchar(50)


--Set @OldColumnName = 'IsDeleted'
--Set @NewColumnName = 'ActiveFlag'

Set @OldColumnName = 'ActiveFlag'
Set @NewColumnName = 'IsDeleted'


DECLARE Column_Cursor CURSOR FOR
SELECT Table_Name, Column_Name, Table_Schema
	FROM INFORMATION_SCHEMA.Columns c
	Where c.COLUMN_NAME = @OldColumnName

OPEN Column_Cursor;

FETCH NEXT FROM Column_Cursor
	INTO @TableName, @ColumnName, @SchemaName;
WHILE @@FETCH_STATUS = 0
BEGIN
	
	Select @TableName, @ColumnName, @SchemaName

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Column Begin: Column %s in Table %s is being modified' , 0 , 1, @ColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	
	DECLARE @sRun nvarchar(max)
	DECLARE @DefaultConstraintName varchar(100)

	-- 1. Rename the Column

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 1 BEGIN: Column %s in Table %s RENAME' , 0 , 1, @ColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	Set @sRun = NULL
	Set @sRun = N'EXEC sp_rename ' + '''' + @SchemaName + '.' + @TableName + '.' + @ColumnName + ''',''' + 
		@NewColumnName + ''',''COLUMN'''

	Select @sRun

	If (@sRun IS NOT NULL)
		EXEC sp_executesql @sRun

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 1 END: Column %s in Table %s RENAME' , 0 , 1, @ColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	-- 2. Flip the Data in the Column

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 2 BEGIN: Column %s in Table %s Data Flipped' , 0 , 1, @NewColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	Set @sRun = NULL
	Set @sRun = 'UPDATE [' + @SchemaName + '].[' + @TableName + '] ' + 
		'SET ' + @NewColumnName + ' = CASE ' + @NewColumnName + 
		' WHEN 1 THEN 0 WHEN 0 THEN 1 END' 

	Select @sRun

	If (@sRun IS NOT NULL)
		EXEC sp_executesql @sRun

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 2 END: Column %s in Table %s Data Flipped' , 0 , 1, @NewColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	-- 3. Delete the Default Constraint

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 3 BEGIN: Delete Default Constraint for Column %s in Table %s' , 0 , 1, @NewColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	SELECT
		@DefaultConstraintName = d.name
	FROM sys.default_constraints d INNER JOIN sys.columns c ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id
	INNER JOIN sys.tables t ON t.object_id = c.object_id
	INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
	WHERE t.name = @TableName And c.name = @NewColumnName

	Set @sRun = NULL
	Set @sRun = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ' + 
		'DROP CONSTRAINT [' + @DefaultConstraintName + ']'

	Select @sRun

	If (@sRun IS NOT NULL)
		EXEC sp_executesql @sRun

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 3 END: Delete Default Constraint for Column %s in Table %s' , 0 , 1, @NewColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;


	-- 4. Create the Default Constraint

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 4 BEGIN: Create Default Constraint for Column %s in Table %s' , 0 , 1, @NewColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	Set @sRun = NULL
	Set @sRun = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ' + 
		'ADD CONSTRAINT [DF_' + @TableName + '_' + @NewColumnName + '] DEFAULT ((0)) FOR [' +
		@NewColumnName + ']'
	
	Select @sRun

	If (@sRun IS NOT NULL)
		EXEC sp_executesql @sRun

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 4 END: Create Default Constraint for Column %s in Table %s' , 0 , 1, @NewColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	-- 5. Rename Indexes

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 5 BEGIN: Rename Indexes for Column %s in Table %s' , 0 , 1, @NewColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

	DECLARE @IndexName varchar(500)
	DECLARE @NewIndexName varchar(500)

	DECLARE Index_Cursor CURSOR FOR
		SELECT ind.name as IndexName
		FROM sys.indexes ind 
		INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
		INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
		INNER JOIN  sys.tables t ON ind.object_id = t.object_id 
		WHERE ind.is_primary_key = 0 AND ind.is_unique = 0 AND ind.is_unique_constraint = 0 
			 AND t.is_ms_shipped = 0 AND col.name = @NewColumnName AND t.name = @TableName

	OPEN Index_Cursor;

	FETCH NEXT FROM Index_Cursor
		INTO @IndexName
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		RAISERROR( 'Step 5 Status: Rename Index %s for Column %s in Table %s' , 0 , 1, @IndexName, @NewColumnName, @TableName )WITH NOWAIT;
		RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
		
		Set @NewIndexName = REPLACE(@IndexName, @OldColumnName, @NewColumnName)

		Set @sRun = NULL
		Set @sRun = 'EXEC sp_rename N''' + @SchemaName + '.' + @TableName + '.' + @IndexName + 
			''', N''' + @NewIndexName + ''', N''INDEX'';'

		If (@sRun IS NOT NULL)
			EXEC sp_executesql @sRun

		FETCH NEXT FROM Index_Cursor
		INTO @IndexName

	END;
	CLOSE Index_Cursor;
	DEALLOCATE Index_Cursor;

	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;
	RAISERROR( 'Step 5 END: Rename Indexes for Column %s in Table %s' , 0 , 1, @NewColumnName, @TableName )WITH NOWAIT;
	RAISERROR( '-----------------------------' , 0 , 1 )WITH NOWAIT;

    FETCH NEXT FROM Column_Cursor
	INTO @TableName, @ColumnName, @SchemaName;
END;

CLOSE Column_Cursor;
DEALLOCATE Column_Cursor;