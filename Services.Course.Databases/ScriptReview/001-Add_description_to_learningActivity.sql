Use Course
go
If exists(select 1 from sys.columns c where c.name = 'Description' and c.object_id = Object_id('CourseLearningActivity'))
begin
print 'column exists already, skip creation';
end
else
begin

ALTER TABLE CourseLearningActivity add [Description] nvarchar(255) NULL;
print 'column added successfully';
end
go