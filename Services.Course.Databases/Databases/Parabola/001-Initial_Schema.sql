/**
Note: All SQL scripts MUST be Azure SQL compliant

To generate this initial SQL script

Open SQL Server Management Studio
Right click on database to script
Select "Tasks"
Select "Generate Scripts..."
Introduction
	Click Next

Choose Objects
	Select "Select specific database objects"
	Select all objects except "Users"
	Click Next

Set Scripting Options
	Output Type
		Select "Save scripts to a specific location"
	Select "Save to file"
	Click "Advanced"
		General
			Check for object existence
				Select "True"
			Script for the database engine type
				Select "SQL Azure Database"
		Table/View Options
			Script Indexes
				Select "True"
		Click OK
	Set output script file location
	Check "Overwite existing file"
	Save as
		Select "Unicode text"
	Click Next

Summary
	Click Next

Save or Publish Scripts
	Click Finish
**/


/****** Object:  Table [dbo].[Calendar]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Calendar]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Calendar](
	[CalendarId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[Name] [nvarchar](250) NOT NULL,
	[CalendarSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
	[OwnerId] [uniqueidentifier] NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Calendar] PRIMARY KEY NONCLUSTERED 
(
	[CalendarId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[CalendarRelationship]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CalendarRelationship](
	[CalendarRelationshipId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[CalendarId] [uniqueidentifier] NOT NULL,
	[RelationshipId] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[CalendarRelationshipSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
 CONSTRAINT [PK_CalendarRelationship] PRIMARY KEY NONCLUSTERED 
(
	[CalendarRelationshipId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[Commits]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Commits]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Commits](
	[StreamId] [uniqueidentifier] NOT NULL,
	[StreamRevision] [int] NOT NULL,
	[Items] [tinyint] NOT NULL,
	[CommitId] [uniqueidentifier] NOT NULL,
	[CommitSequence] [int] NOT NULL,
	[CommitStamp] [datetime] NOT NULL,
	[Dispatched] [bit] NOT NULL,
	[Headers] [varbinary](max) NULL,
	[Payload] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Commits] PRIMARY KEY CLUSTERED 
(
	[StreamId] ASC,
	[CommitSequence] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[Course]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Course](
	[CourseId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](1000) NULL,
	[ParentCourseId] [uniqueidentifier] NULL,
	[OriginalCourseId] [uniqueidentifier] NOT NULL,
	[VersionNumber] [nvarchar](50) NULL,
	[PublishNote] [nvarchar](250) NULL,
	[IsPublished] [bit] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[TemplateCourseId] [uniqueidentifier] NULL,
	[CourseType] [nvarchar](50) NULL,
	[IsTemplate] [bit] NOT NULL,
	[Credit] [decimal](18, 2) NULL,
	[CourseSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
	[PublishDate] [datetime] NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY NONCLUSTERED 
(
	[CourseId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[CourseLearningActivity]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CourseLearningActivity](
	[CourseLearningActivityId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Type] [nvarchar](50) NULL,
	[IsGradeable] [bit] NOT NULL,
	[IsExtraCredit] [bit] NOT NULL,
	[MaxPoint] [int] NULL,
	[Weight] [int] NULL,
	[ObjectId] [uniqueidentifier] NULL,
	[CourseSegmentId] [uniqueidentifier] NULL,
	[ActiveDate] [int] NULL,
	[InactiveDate] [int] NULL,
	[DueDate] [int] NULL,
	[CustomAttribute] [nvarchar](250) NULL,
	[CourseLearningActivitySequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
	[AssessmentId] [uniqueidentifier] NULL,
	[AssessmentType] [nvarchar](50) NULL,
 CONSTRAINT [PK_CourseLearningActivity] PRIMARY KEY NONCLUSTERED 
(
	[CourseLearningActivityId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[CoursePrerequisite]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CoursePrerequisite]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CoursePrerequisite](
	[CourseId] [uniqueidentifier] NOT NULL,
	[PrerequisiteCourseId] [uniqueidentifier] NOT NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_CoursePrerequisite] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC,
	[PrerequisiteCourseId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[CourseProgram]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CourseProgram]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CourseProgram](
	[CourseId] [uniqueidentifier] NOT NULL,
	[ProgramId] [uniqueidentifier] NOT NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_CourseProgram] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC,
	[ProgramId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[CourseSegment]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CourseSegment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CourseSegment](
	[CourseSegmentId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[CourseId] [uniqueidentifier] NOT NULL,
	[ParentSegmentId] [uniqueidentifier] NULL,
	[SerializedData] [nvarchar](max) NULL,
	[Name] [nvarchar](250) NULL,
	[Type] [nvarchar](50) NULL,
	[Description] [nvarchar](1000) NULL,
	[DisplayOrder] [int] NULL,
	[ActiveDate] [int] NULL,
	[InactiveDate] [int] NULL,
	[CourseSegmentSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
 CONSTRAINT [PK_CourseSegment] PRIMARY KEY NONCLUSTERED 
(
	[CourseSegmentId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[Discussion]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Discussion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Discussion](
	[DiscussionId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[Instructions] [varchar](max) NULL,
	[Topic] [varchar](max) NULL,
	[Points] [nvarchar](10) NULL,
	[ParentDiscussionId] [uniqueidentifier] NULL,
	[OriginalDiscussionId] [uniqueidentifier] NOT NULL,
	[VersionNumber] [nvarchar](50) NULL,
	[SomePointerToTheForum] [nvarchar](500) NULL,
	[PublishNote] [nvarchar](250) NULL,
	[IsPublished] [bit] NOT NULL,
	[AttributeXML] [nvarchar](max) NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[DiscussionSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
 CONSTRAINT [PK_Discussion] PRIMARY KEY NONCLUSTERED 
(
	[DiscussionId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[EntityLearningOutcome]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntityLearningOutcome]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntityLearningOutcome](
	[EntityId] [uniqueidentifier] NOT NULL,
	[LearningOutcomeId] [uniqueidentifier] NOT NULL,
	[TenantId] [int] NULL,
	[EntityType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EntityLearningOutcome] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC,
	[LearningOutcomeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[LearningLearningOutcome]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LearningLearningOutcome]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LearningLearningOutcome](
	[LearningOutcomeId] [uniqueidentifier] NOT NULL,
	[SupportingId] [uniqueidentifier] NOT NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_LearningLearningOutcome] PRIMARY KEY CLUSTERED 
(
	[LearningOutcomeId] ASC,
	[SupportingId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[LearningMaterial]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LearningMaterial]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LearningMaterial](
	[LearningMaterialId] [uniqueidentifier] NOT NULL,
	[LearningMaterialSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[Instruction] [nvarchar](1000) NULL,
	[IsRequired] [bit] NOT NULL,
	[AssetId] [uniqueidentifier] NOT NULL,
	[CourseSegmentId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_LearningMaterial] PRIMARY KEY NONCLUSTERED 
(
	[LearningMaterialId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[LearningOutcome]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LearningOutcome]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LearningOutcome](
	[LearningOutcomeId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[Description] [nvarchar](max) NULL,
	[ParentLearningOutcomeId] [uniqueidentifier] NULL,
	[VersionNumber] [nvarchar](50) NULL,
	[OriginalLearningOutcomeId] [uniqueidentifier] NOT NULL,
	[IsPublished] [bit] NOT NULL,
	[PublishNote] [nvarchar](250) NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[LearningOutcomeSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
 CONSTRAINT [PK_LearningOutcome] PRIMARY KEY NONCLUSTERED 
(
	[LearningOutcomeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[Meeting]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Meeting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Meeting](
	[MeetingId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[Description] [nvarchar](1000) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CalendarId] [uniqueidentifier] NOT NULL,
	[MeetingSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
 CONSTRAINT [PK_Meeting] PRIMARY KEY NONCLUSTERED 
(
	[MeetingId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[Program]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Program]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Program](
	[ProgramId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[OrganizationId] [uniqueidentifier] NULL,
	[ProgramType] [nvarchar](50) NOT NULL,
	[ProgramSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
 CONSTRAINT [PK_Program] PRIMARY KEY NONCLUSTERED 
(
	[ProgramId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[Snapshots]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Snapshots]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Snapshots](
	[StreamId] [uniqueidentifier] NOT NULL,
	[StreamRevision] [int] NOT NULL,
	[Payload] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Snapshots] PRIMARY KEY CLUSTERED 
(
	[StreamId] ASC,
	[StreamRevision] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[Task]    Script Date: 2/6/2014 10:46:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Task](
	[TaskId] [uniqueidentifier] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[AddedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[Description] [nvarchar](1000) NULL,
	[Assignee] [uniqueidentifier] NOT NULL,
	[DueDate] [datetime] NULL,
	[CalendarId] [uniqueidentifier] NOT NULL,
	[IsComplete] [bit] NOT NULL,
	[ObjectId] [uniqueidentifier] NULL,
	[TaskSequenceId] [bigint] IDENTITY(1000,1) NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY NONCLUSTERED 
(
	[TaskId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Index [IDX_Calendar_CalendarSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Calendar]') AND name = N'IDX_Calendar_CalendarSequenceId')
CREATE CLUSTERED INDEX [IDX_Calendar_CalendarSequenceId] ON [dbo].[Calendar]
(
	[CalendarSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CalendarRelationship_CalendarRelationshipSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]') AND name = N'IDX_CalendarRelationship_CalendarRelationshipSequenceId')
CREATE CLUSTERED INDEX [IDX_CalendarRelationship_CalendarRelationshipSequenceId] ON [dbo].[CalendarRelationship]
(
	[CalendarRelationshipSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Course_CourseSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = N'IDX_Course_CourseSequenceId')
CREATE CLUSTERED INDEX [IDX_Course_CourseSequenceId] ON [dbo].[Course]
(
	[CourseSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseLearningActivity_CourseLearningActivitySequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]') AND name = N'IDX_CourseLearningActivity_CourseLearningActivitySequenceId')
CREATE CLUSTERED INDEX [IDX_CourseLearningActivity_CourseLearningActivitySequenceId] ON [dbo].[CourseLearningActivity]
(
	[CourseLearningActivitySequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseSegment_CourseSegmentSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseSegment]') AND name = N'IDX_CourseSegment_CourseSegmentSequenceId')
CREATE CLUSTERED INDEX [IDX_CourseSegment_CourseSegmentSequenceId] ON [dbo].[CourseSegment]
(
	[CourseSegmentSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Discussion_DiscussionSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Discussion]') AND name = N'IDX_Discussion_DiscussionSequenceId')
CREATE CLUSTERED INDEX [IDX_Discussion_DiscussionSequenceId] ON [dbo].[Discussion]
(
	[DiscussionSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningMaterial_LearningMaterialSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningMaterial]') AND name = N'IDX_LearningMaterial_LearningMaterialSequenceId')
CREATE CLUSTERED INDEX [IDX_LearningMaterial_LearningMaterialSequenceId] ON [dbo].[LearningMaterial]
(
	[LearningMaterialSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningOutcome_LearningOutcomeSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningOutcome]') AND name = N'IDX_LearningOutcome_LearningOutcomeSequenceId')
CREATE CLUSTERED INDEX [IDX_LearningOutcome_LearningOutcomeSequenceId] ON [dbo].[LearningOutcome]
(
	[LearningOutcomeSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Meeting_MeetingSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Meeting]') AND name = N'IDX_Meeting_MeetingSequenceId')
CREATE CLUSTERED INDEX [IDX_Meeting_MeetingSequenceId] ON [dbo].[Meeting]
(
	[MeetingSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Program_ProgramSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Program]') AND name = N'IDX_Program_ProgramSequenceId')
CREATE CLUSTERED INDEX [IDX_Program_ProgramSequenceId] ON [dbo].[Program]
(
	[ProgramSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Task_TaskSequenceId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = N'IDX_Task_TaskSequenceId')
CREATE CLUSTERED INDEX [IDX_Task_TaskSequenceId] ON [dbo].[Task]
(
	[TaskSequenceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Calendar_CalendarId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Calendar]') AND name = N'IDX_Calendar_CalendarId')
CREATE NONCLUSTERED INDEX [IDX_Calendar_CalendarId] ON [dbo].[Calendar]
(
	[CalendarId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Calendar_Owner]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Calendar]') AND name = N'IDX_Calendar_Owner')
CREATE NONCLUSTERED INDEX [IDX_Calendar_Owner] ON [dbo].[Calendar]
(
	[OwnerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Calendar_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Calendar]') AND name = N'IDX_Calendar_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_Calendar_TenantId_ActiveFlag] ON [dbo].[Calendar]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CalendarRelationship_CalendarRelationshipId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]') AND name = N'IDX_CalendarRelationship_CalendarRelationshipId')
CREATE NONCLUSTERED INDEX [IDX_CalendarRelationship_CalendarRelationshipId] ON [dbo].[CalendarRelationship]
(
	[CalendarRelationshipId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CalendarRelationship_RelationshipId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]') AND name = N'IDX_CalendarRelationship_RelationshipId')
CREATE NONCLUSTERED INDEX [IDX_CalendarRelationship_RelationshipId] ON [dbo].[CalendarRelationship]
(
	[RelationshipId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CalendarRelationship_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]') AND name = N'IDX_CalendarRelationship_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_CalendarRelationship_TenantId_ActiveFlag] ON [dbo].[CalendarRelationship]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IX_Commits]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Commits]') AND name = N'IX_Commits')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Commits] ON [dbo].[Commits]
(
	[StreamId] ASC,
	[CommitId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IX_Commits_Dispatched]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Commits]') AND name = N'IX_Commits_Dispatched')
CREATE NONCLUSTERED INDEX [IX_Commits_Dispatched] ON [dbo].[Commits]
(
	[Dispatched] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IX_Commits_Revisions]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Commits]') AND name = N'IX_Commits_Revisions')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Commits_Revisions] ON [dbo].[Commits]
(
	[StreamId] ASC,
	[StreamRevision] ASC,
	[Items] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IX_Commits_Stamp]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Commits]') AND name = N'IX_Commits_Stamp')
CREATE NONCLUSTERED INDEX [IX_Commits_Stamp] ON [dbo].[Commits]
(
	[CommitStamp] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Course_CourseId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = N'IDX_Course_CourseId')
CREATE NONCLUSTERED INDEX [IDX_Course_CourseId] ON [dbo].[Course]
(
	[CourseId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Course_DateUpdated]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = N'IDX_Course_DateUpdated')
CREATE NONCLUSTERED INDEX [IDX_Course_DateUpdated] ON [dbo].[Course]
(
	[DateUpdated] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_Course_Name]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = N'IDX_Course_Name')
CREATE NONCLUSTERED INDEX [IDX_Course_Name] ON [dbo].[Course]
(
	[Name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Course_OriginalCourseId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = N'IDX_Course_OriginalCourseId')
CREATE NONCLUSTERED INDEX [IDX_Course_OriginalCourseId] ON [dbo].[Course]
(
	[OriginalCourseId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Course_ParentCourseId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = N'IDX_Course_ParentCourseId')
CREATE NONCLUSTERED INDEX [IDX_Course_ParentCourseId] ON [dbo].[Course]
(
	[ParentCourseId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Course_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = N'IDX_Course_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_Course_TenantId_ActiveFlag] ON [dbo].[Course]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseLearningActivity_CourseLearningActivityId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]') AND name = N'IDX_CourseLearningActivity_CourseLearningActivityId')
CREATE NONCLUSTERED INDEX [IDX_CourseLearningActivity_CourseLearningActivityId] ON [dbo].[CourseLearningActivity]
(
	[CourseLearningActivityId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseLearningActivity_CourseSegmentId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]') AND name = N'IDX_CourseLearningActivity_CourseSegmentId')
CREATE NONCLUSTERED INDEX [IDX_CourseLearningActivity_CourseSegmentId] ON [dbo].[CourseLearningActivity]
(
	[CourseSegmentId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseLearningActivity_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]') AND name = N'IDX_CourseLearningActivity_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_CourseLearningActivity_TenantId_ActiveFlag] ON [dbo].[CourseLearningActivity]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseProgram_ProgramId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseProgram]') AND name = N'IDX_CourseProgram_ProgramId')
CREATE NONCLUSTERED INDEX [IDX_CourseProgram_ProgramId] ON [dbo].[CourseProgram]
(
	[ProgramId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseSegment_CourseId_DisplayOrder]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseSegment]') AND name = N'IDX_CourseSegment_CourseId_DisplayOrder')
CREATE NONCLUSTERED INDEX [IDX_CourseSegment_CourseId_DisplayOrder] ON [dbo].[CourseSegment]
(
	[CourseId] ASC,
	[DisplayOrder] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseSegment_CourseSegmentId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseSegment]') AND name = N'IDX_CourseSegment_CourseSegmentId')
CREATE NONCLUSTERED INDEX [IDX_CourseSegment_CourseSegmentId] ON [dbo].[CourseSegment]
(
	[CourseSegmentId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_CourseSegment_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CourseSegment]') AND name = N'IDX_CourseSegment_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_CourseSegment_TenantId_ActiveFlag] ON [dbo].[CourseSegment]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_EntityLearningOutcome_LearningOutcomeId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[EntityLearningOutcome]') AND name = N'IDX_EntityLearningOutcome_LearningOutcomeId')
CREATE NONCLUSTERED INDEX [IDX_EntityLearningOutcome_LearningOutcomeId] ON [dbo].[EntityLearningOutcome]
(
	[LearningOutcomeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningLearningOutcome_SupportingId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningLearningOutcome]') AND name = N'IDX_LearningLearningOutcome_SupportingId')
CREATE NONCLUSTERED INDEX [IDX_LearningLearningOutcome_SupportingId] ON [dbo].[LearningLearningOutcome]
(
	[SupportingId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningMaterial_CourseSegmentId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningMaterial]') AND name = N'IDX_LearningMaterial_CourseSegmentId')
CREATE NONCLUSTERED INDEX [IDX_LearningMaterial_CourseSegmentId] ON [dbo].[LearningMaterial]
(
	[CourseSegmentId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningMaterial_LearningMaterialId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningMaterial]') AND name = N'IDX_LearningMaterial_LearningMaterialId')
CREATE NONCLUSTERED INDEX [IDX_LearningMaterial_LearningMaterialId] ON [dbo].[LearningMaterial]
(
	[LearningMaterialId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningMaterial_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningMaterial]') AND name = N'IDX_LearningMaterial_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_LearningMaterial_TenantId_ActiveFlag] ON [dbo].[LearningMaterial]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningOutcome_LearningOutcomeId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningOutcome]') AND name = N'IDX_LearningOutcome_LearningOutcomeId')
CREATE NONCLUSTERED INDEX [IDX_LearningOutcome_LearningOutcomeId] ON [dbo].[LearningOutcome]
(
	[LearningOutcomeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningOutcome_OrganizationId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningOutcome]') AND name = N'IDX_LearningOutcome_OrganizationId')
CREATE NONCLUSTERED INDEX [IDX_LearningOutcome_OrganizationId] ON [dbo].[LearningOutcome]
(
	[OrganizationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningOutcome_ParentLearningOutcomeId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningOutcome]') AND name = N'IDX_LearningOutcome_ParentLearningOutcomeId')
CREATE NONCLUSTERED INDEX [IDX_LearningOutcome_ParentLearningOutcomeId] ON [dbo].[LearningOutcome]
(
	[ParentLearningOutcomeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_LearningOutcome_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LearningOutcome]') AND name = N'IDX_LearningOutcome_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_LearningOutcome_TenantId_ActiveFlag] ON [dbo].[LearningOutcome]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Meeting_CalendarId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Meeting]') AND name = N'IDX_Meeting_CalendarId')
CREATE NONCLUSTERED INDEX [IDX_Meeting_CalendarId] ON [dbo].[Meeting]
(
	[CalendarId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Meeting_MeetingId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Meeting]') AND name = N'IDX_Meeting_MeetingId')
CREATE NONCLUSTERED INDEX [IDX_Meeting_MeetingId] ON [dbo].[Meeting]
(
	[MeetingId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Meeting_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Meeting]') AND name = N'IDX_Meeting_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_Meeting_TenantId_ActiveFlag] ON [dbo].[Meeting]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Program_OrganizationId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Program]') AND name = N'IDX_Program_OrganizationId')
CREATE NONCLUSTERED INDEX [IDX_Program_OrganizationId] ON [dbo].[Program]
(
	[OrganizationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Program_ProgramId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Program]') AND name = N'IDX_Program_ProgramId')
CREATE NONCLUSTERED INDEX [IDX_Program_ProgramId] ON [dbo].[Program]
(
	[ProgramId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Program_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Program]') AND name = N'IDX_Program_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_Program_TenantId_ActiveFlag] ON [dbo].[Program]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Task_Assignee]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = N'IDX_Task_Assignee')
CREATE NONCLUSTERED INDEX [IDX_Task_Assignee] ON [dbo].[Task]
(
	[Assignee] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Task_CalendarId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = N'IDX_Task_CalendarId')
CREATE NONCLUSTERED INDEX [IDX_Task_CalendarId] ON [dbo].[Task]
(
	[CalendarId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Task_TaskId]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = N'IDX_Task_TaskId')
CREATE NONCLUSTERED INDEX [IDX_Task_TaskId] ON [dbo].[Task]
(
	[TaskId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Index [IDX_Task_TenantId_ActiveFlag]    Script Date: 2/6/2014 10:46:32 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = N'IDX_Task_TenantId_ActiveFlag')
CREATE NONCLUSTERED INDEX [IDX_Task_TenantId_ActiveFlag] ON [dbo].[Task]
(
	[TenantId] ASC,
	[ActiveFlag] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Calendar_CalendarId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF_Calendar_CalendarId]  DEFAULT (NEWID()) FOR [CalendarId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Calendar_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF_Calendar_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Calendar_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF_Calendar_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Calendar_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF_Calendar_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Calendar_OrganizationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF_Calendar_OrganizationId]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [OrganizationId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CalendarRelationship_CalendarRelationshipId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CalendarRelationship] ADD  CONSTRAINT [DF_CalendarRelationship_CalendarRelationshipId]  DEFAULT (NEWID()) FOR [CalendarRelationshipId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CalendarRelationship_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CalendarRelationship] ADD  CONSTRAINT [DF_CalendarRelationship_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CalendarRelationship_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CalendarRelationship] ADD  CONSTRAINT [DF_CalendarRelationship_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CalendarRelationship_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CalendarRelationship] ADD  CONSTRAINT [DF_CalendarRelationship_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Commits__Dispatc__30C33EC3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Commits] ADD  DEFAULT ((0)) FOR [Dispatched]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Course_CourseId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Course] ADD  CONSTRAINT [DF_Course_CourseId]  DEFAULT (NEWID()) FOR [CourseId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Course_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Course] ADD  CONSTRAINT [DF_Course_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Course_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Course] ADD  CONSTRAINT [DF_Course_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Course_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Course] ADD  CONSTRAINT [DF_Course_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Course_IsPublished]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Course] ADD  CONSTRAINT [DF_Course_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Course_IsTemplate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Course] ADD  CONSTRAINT [DF_Course_IsTemplate]  DEFAULT ((0)) FOR [IsTemplate]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseLearningActivity_CourseLearningActivityId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseLearningActivity] ADD  CONSTRAINT [DF_CourseLearningActivity_CourseLearningActivityId]  DEFAULT (NEWID()) FOR [CourseLearningActivityId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseLearningActivity_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseLearningActivity] ADD  CONSTRAINT [DF_CourseLearningActivity_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseLearningActivity_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseLearningActivity] ADD  CONSTRAINT [DF_CourseLearningActivity_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseLearningActivity_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseLearningActivity] ADD  CONSTRAINT [DF_CourseLearningActivity_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseLearningActivity_IsGradeable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseLearningActivity] ADD  CONSTRAINT [DF_CourseLearningActivity_IsGradeable]  DEFAULT ((1)) FOR [IsGradeable]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseLearningActivity_IsExtraCredit]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseLearningActivity] ADD  CONSTRAINT [DF_CourseLearningActivity_IsExtraCredit]  DEFAULT ((0)) FOR [IsExtraCredit]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseSegment_CourseSegmentId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseSegment] ADD  CONSTRAINT [DF_CourseSegment_CourseSegmentId]  DEFAULT (NEWID()) FOR [CourseSegmentId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseSegment_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseSegment] ADD  CONSTRAINT [DF_CourseSegment_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseSegment_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseSegment] ADD  CONSTRAINT [DF_CourseSegment_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseSegment_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseSegment] ADD  CONSTRAINT [DF_CourseSegment_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_CourseSegment_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CourseSegment] ADD  CONSTRAINT [DF_CourseSegment_DisplayOrder]  DEFAULT ((0)) FOR [DisplayOrder]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Discussion_DiscussionId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Discussion] ADD  CONSTRAINT [DF_Discussion_DiscussionId]  DEFAULT (NEWID()) FOR [DiscussionId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Discussion_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Discussion] ADD  CONSTRAINT [DF_Discussion_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Discussion_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Discussion] ADD  CONSTRAINT [DF_Discussion_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Discussion_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Discussion] ADD  CONSTRAINT [DF_Discussion_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Discussion_IsPublished]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Discussion] ADD  CONSTRAINT [DF_Discussion_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningMaterial_LearningMaterialId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningMaterial] ADD  CONSTRAINT [DF_LearningMaterial_LearningMaterialId]  DEFAULT (NEWID()) FOR [LearningMaterialId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningMaterial_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningMaterial] ADD  CONSTRAINT [DF_LearningMaterial_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningMaterial_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningMaterial] ADD  CONSTRAINT [DF_LearningMaterial_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningMaterial_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningMaterial] ADD  CONSTRAINT [DF_LearningMaterial_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningMaterial_IsRequired]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningMaterial] ADD  CONSTRAINT [DF_LearningMaterial_IsRequired]  DEFAULT ((0)) FOR [IsRequired]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningOutcome_LearningOutcomeId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningOutcome] ADD  CONSTRAINT [DF_LearningOutcome_LearningOutcomeId]  DEFAULT (NEWID()) FOR [LearningOutcomeId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningOutcome_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningOutcome] ADD  CONSTRAINT [DF_LearningOutcome_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningOutcome_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningOutcome] ADD  CONSTRAINT [DF_LearningOutcome_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningOutcome_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningOutcome] ADD  CONSTRAINT [DF_LearningOutcome_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_LearningOutcome_IsPublished]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LearningOutcome] ADD  CONSTRAINT [DF_LearningOutcome_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Meeting_MeetingId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Meeting] ADD  CONSTRAINT [DF_Meeting_MeetingId]  DEFAULT (NEWID()) FOR [MeetingId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Meeting_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Meeting] ADD  CONSTRAINT [DF_Meeting_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Meeting_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Meeting] ADD  CONSTRAINT [DF_Meeting_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Meeting_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Meeting] ADD  CONSTRAINT [DF_Meeting_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Program_ProgramId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Program] ADD  CONSTRAINT [DF_Program_ProgramId]  DEFAULT (NEWID()) FOR [ProgramId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Program_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Program] ADD  CONSTRAINT [DF_Program_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Program_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Program] ADD  CONSTRAINT [DF_Program_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Program_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Program] ADD  CONSTRAINT [DF_Program_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Task_TaskId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_TaskId]  DEFAULT (NEWID()) FOR [TaskId]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Task_ActiveFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Task_DateAdded]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_DateAdded]  DEFAULT (GETDATE()) FOR [DateAdded]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Task_DateUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_DateUpdated]  DEFAULT (GETDATE()) FOR [DateUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Task_IsComplete]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_IsComplete]  DEFAULT ((0)) FOR [IsComplete]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CalendarRelationship_Calendar]') AND parent_object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]'))
ALTER TABLE [dbo].[CalendarRelationship]  WITH CHECK ADD  CONSTRAINT [FK_CalendarRelationship_Calendar] FOREIGN KEY([CalendarId])
REFERENCES [dbo].[Calendar] ([CalendarId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CalendarRelationship_Calendar]') AND parent_object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]'))
ALTER TABLE [dbo].[CalendarRelationship] CHECK CONSTRAINT [FK_CalendarRelationship_Calendar]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Course_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[Course]'))
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Course] FOREIGN KEY([ParentCourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Course_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[Course]'))
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Course]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Course_Course2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Course]'))
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Course2] FOREIGN KEY([OriginalCourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Course_Course2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Course]'))
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Course2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Course_Course3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Course]'))
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Course3] FOREIGN KEY([TemplateCourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Course_Course3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Course]'))
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Course3]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseLearningActivity_CourseSegment]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]'))
ALTER TABLE [dbo].[CourseLearningActivity]  WITH CHECK ADD  CONSTRAINT [FK_CourseLearningActivity_CourseSegment] FOREIGN KEY([CourseSegmentId])
REFERENCES [dbo].[CourseSegment] ([CourseSegmentId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseLearningActivity_CourseSegment]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]'))
ALTER TABLE [dbo].[CourseLearningActivity] CHECK CONSTRAINT [FK_CourseLearningActivity_CourseSegment]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseSegment_CourseLearningActivities]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]'))
ALTER TABLE [dbo].[CourseLearningActivity]  WITH CHECK ADD  CONSTRAINT [FK_CourseSegment_CourseLearningActivities] FOREIGN KEY([CourseSegmentId])
REFERENCES [dbo].[CourseSegment] ([CourseSegmentId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseSegment_CourseLearningActivities]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseLearningActivity]'))
ALTER TABLE [dbo].[CourseLearningActivity] CHECK CONSTRAINT [FK_CourseSegment_CourseLearningActivities]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CoursePrerequisite_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[CoursePrerequisite]'))
ALTER TABLE [dbo].[CoursePrerequisite]  WITH CHECK ADD  CONSTRAINT [FK_CoursePrerequisite_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CoursePrerequisite_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[CoursePrerequisite]'))
ALTER TABLE [dbo].[CoursePrerequisite] CHECK CONSTRAINT [FK_CoursePrerequisite_Course]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CoursePrerequisite_Course2]') AND parent_object_id = OBJECT_ID(N'[dbo].[CoursePrerequisite]'))
ALTER TABLE [dbo].[CoursePrerequisite]  WITH CHECK ADD  CONSTRAINT [FK_CoursePrerequisite_Course2] FOREIGN KEY([PrerequisiteCourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CoursePrerequisite_Course2]') AND parent_object_id = OBJECT_ID(N'[dbo].[CoursePrerequisite]'))
ALTER TABLE [dbo].[CoursePrerequisite] CHECK CONSTRAINT [FK_CoursePrerequisite_Course2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseProgram_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseProgram]'))
ALTER TABLE [dbo].[CourseProgram]  WITH CHECK ADD  CONSTRAINT [FK_CourseProgram_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseProgram_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseProgram]'))
ALTER TABLE [dbo].[CourseProgram] CHECK CONSTRAINT [FK_CourseProgram_Course]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseProgram_Program]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseProgram]'))
ALTER TABLE [dbo].[CourseProgram]  WITH CHECK ADD  CONSTRAINT [FK_CourseProgram_Program] FOREIGN KEY([ProgramId])
REFERENCES [dbo].[Program] ([ProgramId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseProgram_Program]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseProgram]'))
ALTER TABLE [dbo].[CourseProgram] CHECK CONSTRAINT [FK_CourseProgram_Program]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseSegment_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseSegment]'))
ALTER TABLE [dbo].[CourseSegment]  WITH CHECK ADD  CONSTRAINT [FK_CourseSegment_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseSegment_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseSegment]'))
ALTER TABLE [dbo].[CourseSegment] CHECK CONSTRAINT [FK_CourseSegment_Course]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseSegment_CourseSegment]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseSegment]'))
ALTER TABLE [dbo].[CourseSegment]  WITH CHECK ADD  CONSTRAINT [FK_CourseSegment_CourseSegment] FOREIGN KEY([ParentSegmentId])
REFERENCES [dbo].[CourseSegment] ([CourseSegmentId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseSegment_CourseSegment]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseSegment]'))
ALTER TABLE [dbo].[CourseSegment] CHECK CONSTRAINT [FK_CourseSegment_CourseSegment]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OriginalEntity_Discussion]') AND parent_object_id = OBJECT_ID(N'[dbo].[Discussion]'))
ALTER TABLE [dbo].[Discussion]  WITH CHECK ADD  CONSTRAINT [FK_OriginalEntity_Discussion] FOREIGN KEY([OriginalDiscussionId])
REFERENCES [dbo].[Discussion] ([DiscussionId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OriginalEntity_Discussion]') AND parent_object_id = OBJECT_ID(N'[dbo].[Discussion]'))
ALTER TABLE [dbo].[Discussion] CHECK CONSTRAINT [FK_OriginalEntity_Discussion]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ParentEntity_Discussion]') AND parent_object_id = OBJECT_ID(N'[dbo].[Discussion]'))
ALTER TABLE [dbo].[Discussion]  WITH CHECK ADD  CONSTRAINT [FK_ParentEntity_Discussion] FOREIGN KEY([ParentDiscussionId])
REFERENCES [dbo].[Discussion] ([DiscussionId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ParentEntity_Discussion]') AND parent_object_id = OBJECT_ID(N'[dbo].[Discussion]'))
ALTER TABLE [dbo].[Discussion] CHECK CONSTRAINT [FK_ParentEntity_Discussion]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EntityLearningOutcome_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[EntityLearningOutcome]'))
ALTER TABLE [dbo].[EntityLearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK_EntityLearningOutcome_LearningOutcome] FOREIGN KEY([LearningOutcomeId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EntityLearningOutcome_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[EntityLearningOutcome]'))
ALTER TABLE [dbo].[EntityLearningOutcome] CHECK CONSTRAINT [FK_EntityLearningOutcome_LearningOutcome]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK64DE4C7960914E33]') AND parent_object_id = OBJECT_ID(N'[dbo].[EntityLearningOutcome]'))
ALTER TABLE [dbo].[EntityLearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK64DE4C7960914E33] FOREIGN KEY([LearningOutcomeId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK64DE4C7960914E33]') AND parent_object_id = OBJECT_ID(N'[dbo].[EntityLearningOutcome]'))
ALTER TABLE [dbo].[EntityLearningOutcome] CHECK CONSTRAINT [FK64DE4C7960914E33]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningLearningOutcome_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningLearningOutcome]'))
ALTER TABLE [dbo].[LearningLearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK_LearningLearningOutcome_LearningOutcome] FOREIGN KEY([LearningOutcomeId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningLearningOutcome_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningLearningOutcome]'))
ALTER TABLE [dbo].[LearningLearningOutcome] CHECK CONSTRAINT [FK_LearningLearningOutcome_LearningOutcome]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningLearningOutcome_LearningOutcome2]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningLearningOutcome]'))
ALTER TABLE [dbo].[LearningLearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK_LearningLearningOutcome_LearningOutcome2] FOREIGN KEY([SupportingId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningLearningOutcome_LearningOutcome2]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningLearningOutcome]'))
ALTER TABLE [dbo].[LearningLearningOutcome] CHECK CONSTRAINT [FK_LearningLearningOutcome_LearningOutcome2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningLearningOutcome]'))
ALTER TABLE [dbo].[LearningLearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK_LearningOutcome] FOREIGN KEY([LearningOutcomeId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningLearningOutcome]'))
ALTER TABLE [dbo].[LearningLearningOutcome] CHECK CONSTRAINT [FK_LearningOutcome]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningMaterial_CourseSegment]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningMaterial]'))
ALTER TABLE [dbo].[LearningMaterial]  WITH CHECK ADD  CONSTRAINT [FK_LearningMaterial_CourseSegment] FOREIGN KEY([CourseSegmentId])
REFERENCES [dbo].[CourseSegment] ([CourseSegmentId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningMaterial_CourseSegment]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningMaterial]'))
ALTER TABLE [dbo].[LearningMaterial] CHECK CONSTRAINT [FK_LearningMaterial_CourseSegment]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningOutcome_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningOutcome]'))
ALTER TABLE [dbo].[LearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK_LearningOutcome_LearningOutcome] FOREIGN KEY([ParentLearningOutcomeId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningOutcome_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningOutcome]'))
ALTER TABLE [dbo].[LearningOutcome] CHECK CONSTRAINT [FK_LearningOutcome_LearningOutcome]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningOutcome_LearningOutcome2]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningOutcome]'))
ALTER TABLE [dbo].[LearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK_LearningOutcome_LearningOutcome2] FOREIGN KEY([OriginalLearningOutcomeId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LearningOutcome_LearningOutcome2]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningOutcome]'))
ALTER TABLE [dbo].[LearningOutcome] CHECK CONSTRAINT [FK_LearningOutcome_LearningOutcome2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OriginalEntity_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningOutcome]'))
ALTER TABLE [dbo].[LearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK_OriginalEntity_LearningOutcome] FOREIGN KEY([OriginalLearningOutcomeId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OriginalEntity_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningOutcome]'))
ALTER TABLE [dbo].[LearningOutcome] CHECK CONSTRAINT [FK_OriginalEntity_LearningOutcome]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ParentEntity_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningOutcome]'))
ALTER TABLE [dbo].[LearningOutcome]  WITH CHECK ADD  CONSTRAINT [FK_ParentEntity_LearningOutcome] FOREIGN KEY([ParentLearningOutcomeId])
REFERENCES [dbo].[LearningOutcome] ([LearningOutcomeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ParentEntity_LearningOutcome]') AND parent_object_id = OBJECT_ID(N'[dbo].[LearningOutcome]'))
ALTER TABLE [dbo].[LearningOutcome] CHECK CONSTRAINT [FK_ParentEntity_LearningOutcome]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Meeting_Calendar]') AND parent_object_id = OBJECT_ID(N'[dbo].[Meeting]'))
ALTER TABLE [dbo].[Meeting]  WITH CHECK ADD  CONSTRAINT [FK_Meeting_Calendar] FOREIGN KEY([CalendarId])
REFERENCES [dbo].[Calendar] ([CalendarId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Meeting_Calendar]') AND parent_object_id = OBJECT_ID(N'[dbo].[Meeting]'))
ALTER TABLE [dbo].[Meeting] CHECK CONSTRAINT [FK_Meeting_Calendar]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Task_Calendar]') AND parent_object_id = OBJECT_ID(N'[dbo].[Task]'))
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Calendar] FOREIGN KEY([CalendarId])
REFERENCES [dbo].[Calendar] ([CalendarId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Task_Calendar]') AND parent_object_id = OBJECT_ID(N'[dbo].[Task]'))
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Calendar]
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_CalendarRelationship_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]'))
ALTER TABLE [dbo].[CalendarRelationship]  WITH CHECK ADD  CONSTRAINT [CK_CalendarRelationship_Type] CHECK  (([type]='Admin' OR [type]='Observer'))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_CalendarRelationship_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[CalendarRelationship]'))
ALTER TABLE [dbo].[CalendarRelationship] CHECK CONSTRAINT [CK_CalendarRelationship_Type]
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK__Commits__CommitI__2EDAF651]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commits]'))
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([CommitId]<>0x00))
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK__Commits__CommitS__2FCF1A8A]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commits]'))
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([CommitSequence]>(0)))
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK__Commits__Headers__31B762FC]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commits]'))
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([Headers] IS NULL OR datalength([Headers])>(0)))
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK__Commits__Items__2DE6D218]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commits]'))
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([Items]>(0)))
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK__Commits__Payload__32AB8735]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commits]'))
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  ((datalength([Payload])>(0)))
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK__Commits__StreamR__2CF2ADDF]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commits]'))
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([StreamRevision]>(0)))
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK__Snapshots__Paylo__367C1819]') AND parent_object_id = OBJECT_ID(N'[dbo].[Snapshots]'))
ALTER TABLE [dbo].[Snapshots]  WITH CHECK ADD CHECK  ((datalength([Payload])>(0)))
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK__Snapshots__Strea__3587F3E0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Snapshots]'))
ALTER TABLE [dbo].[Snapshots]  WITH CHECK ADD CHECK  (([StreamRevision]>(0)))
GO
