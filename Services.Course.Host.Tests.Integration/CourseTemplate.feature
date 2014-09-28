@Api
Feature: CourseTemplate
In order to create courses easily and quickly
As a course builder
I want to create courses from a template

Background: 
	And the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	| EditProgram   |
	| ViewProgram   |
	| EditCourse    |
	And I have the following assets
	| Name   |
	| asset1 |
	| asset2 |
	| asset3 |
	And Published the following assets
	| Name   | PublishNote |
	| asset1 | published   |
	| asset2 | published   |
	| asset3 | published   |
	And I have the following programs
	| Name                | Description | ProgramType | OrganizationName |
	| Bachelor of Art     | BA Program  | BA          | COB              |
	| Bachelor of Science | BS program  | BS          | COB              |
	And I have the following course templates
	| Name       | Code          | Description              | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets | CorrelationId  |
	| Template 1 | TemplateCode1 | My First Course Template | COB              | Traditional | true       | {someData} | asset1,asset2   | correlationId1 |
	And I have the following course segments for 'Template 1'
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |


Scenario: Create a course from a template
When I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description      | OrganizationName | IsTemplate |
	| Course 1 | CourseCode1 | My First Course  | COB              | false      |
And I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description      | OrganizationName | IsTemplate | CorrelationId  |
	| Course 2 |             | My Second Course | COB              | true       | correlationId2 |
And I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description      | OrganizationName | IsTemplate |
	| Course 3 |             |                  | COB              | true       |
Then the course 'Course 1' should have the following info
	| Field         | Value           |
	| Name          | Course 1        |
	| Code          | CourseCode1     |
	| Description   | My First Course |
	| CourseType    | Traditional     |
	| IsTemplate    | false           |
	| MetaData      | {someData}      |
	| CorrelationId |                 |
And the course 'Course 1' should have the following reference info
	| Field           | Value         |
	| ExtensionAssets | asset1,asset2 |
And the course 'Course 1' should have the following segment info
	| Name  | Description              | Type     | SourceSegment |
	| Week1 | First week is slack time | TimeSpan | Week1         |
And the course 'Course 2' should have the following info
	| Field         | Value            |
	| Name          | Course 2         |
	| Code          | TemplateCode1    |
	| Description   | My Second Course |
	| CourseType    | Traditional      |
	| IsTemplate    | true             |
	| MetaData      | {someData}       |
	| CorrelationId | correlationId2   |
And the course 'Course 2' should have the following reference info
	| Field           | Value         |
	| ExtensionAssets | asset1,asset2 |
And the course 'Course 2' should have the following segment info
	| Name  | Description              | Type     | SourceSegment |
	| Week1 | First week is slack time | TimeSpan | Week1         |
And the course 'Course 3' should have the following info
	| Field         | Value                    |
	| Name          | Course 3                 |
	| Code          | TemplateCode1            |
	| Description   | My First Course Template |
	| CourseType    | Traditional              |
	| IsTemplate    | true                     |
	| MetaData      | {someData}               |
	| CorrelationId |                          |
And the course 'Course 3' should have the following reference info
	| Field           | Value         |
	| ExtensionAssets | asset1,asset2 |
And the course 'Course 3' should have the following segment info
	| Name  | Description              | Type     | SourceSegment |
	| Week1 | First week is slack time | TimeSpan | Week1         |

Scenario: Ignore course Type in the request when creating from template
When I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description              | OrganizationName |  IsTemplate |
	| Course 2 | CourseCode2 | My First Course Template | COB              |  false      |
Then the course 'Course 2' should have the following info
	| Field       | Value                    |
	| Name        | Course 2                 |
	| Code        | CourseCode2              |
	| Description | My First Course Template |
	| CourseType  | Traditional              |
	| IsTemplate  | false                    |
	| MetaData    | {someData}       |
And the course 'Course 2' should have the following reference info
	| Field           | Value         |
	| ExtensionAssets | asset1,asset2 |

Scenario: Verify programs are copied from course template
When I associate 'Template 1' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
And I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description                   | OrganizationName | IsTemplate |
	| Course 3 | CourseCode3 | My First Course from Template | COB              | false      |
Then the course 'Course 3' includes the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |

Scenario: Verify segments are copied from course template
When I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description                   | OrganizationName | IsTemplate |
	| Course 4 | CourseCode4 | My First Course from Template | COB              | false      |
Then the course 'Course 4' should have these course segments
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |

Scenario: Verify outcomes are copied from course template
When I associate the newly created learning outcomes to 'Template 1' course
	| Title    | Description                    |
	| Outcome1 | first course learning outcome  |
	| Outcome2 | second course learning outcome |
And I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description                   | OrganizationName | IsTemplate |
	| Course 5 | CourseCode5 | My First Course from Template | COB              | false      |
Then the course 'Template 1' should have the following learning outcomes
	| Title    | Description                    |
	| Outcome1 | first course learning outcome  |
	| Outcome2 | second course learning outcome |

Scenario: Publish a course version, created from a template
	Given I associate 'Template 1' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	And I create a course from the template 'Template 1' with the following
	| Name         | Code   | Description                   | OrganizationName | IsTemplate |
	| English 1010 | ENG101 | Ranji's awesome English Class | COB              | false      |
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| CourseType    | Traditional                   |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | true                          |
	| PublishNote   | Blah blah                     |
	And the course 'English 1010' includes the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |

Scenario: Version a course, which was created from a template
	Given I associate 'Template 1' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	And I create a course from the template 'Template 1' with the following
	| Name         | Code   | Description                   | OrganizationName | IsTemplate |
	| English 1010 | ENG101 | Ranji's awesome English Class | COB              | false      |
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v2' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the course 'English 1010 v2' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| CourseType    | Traditional                   |
	| IsPublished   | false                         |
	| VersionNumber | 2.0a                          |
	And the course 'English 1010 v2' includes the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |

Scenario: Cannot get the template after create a course version from a previously-published version（DE395）
	When I create a course from the template 'Template 1' with the following
    | Name         | Code        | Description              | OrganizationName | IsTemplate |
    | English 2020 | CourseCode1 | My First Course Template | COB              | false      |
	And I publish the following courses
	| Name         | Note     |
	| English 2020 | Blah blah |
	And I create a new version of 'English 2020' course named 'English 2020 v1.0.0.1' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.1 |
	Then The course 'English 2020 v1.0.0.1' should have the template named 'Template 1'

Scenario: Verify programs and prerequisites should be maintained  when converting a course to a template
	Given I have the following courses
	| Name     | Code | Description           | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets |
	| Econ 100 | E100 | Macroeconomics        | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Econ 400 | E400 | Advanced Econometrics | COB              | Traditional | False      | {someData} | asset1,asset2   |
	When I publish the following courses
	| Name     | Note   |
	| Econ 100 | a note |
	| Econ 400 | a note |
	And I have an existing course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets                      | ProgramName     | Prerequisites     |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB | Bachelor of Art | Econ 100,Econ 400 |
	And I change the info to reflect the following:
	| Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets                      | ProgramName     | Prerequisites     |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | true       | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB | Bachelor of Art | Econ 100,Econ 400 |
	And I publish the following courses
    | Name        | Note   |
    | English 101 | a note |
	Then the course 'English 101' includes the following program information
	| Program Name        |
	| Bachelor of Art     |
	And the course 'English 101' should have the following prerequisites
	| Name     | 
	| Econ 100 | 
	| Econ 400 | 
	