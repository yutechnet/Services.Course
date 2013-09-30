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
	| Capability   |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	And I have the following programs
	| Name                | Description | ProgramType | OrganizationName |
	| Bachelor of Art     | BA Program  | BA          | COB              |
	| Bachelor of Science | BS program  | BS          | COB              |
	And I have the following course templates
	| Name       | Code          | Description              | OrganizationName | CourseType  | IsTemplate |
	| Template 1 | TemplateCode1 | My First Course Template | COB              | Traditional | true       |
	And I have the following course segments for 'Template 1'
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |


Scenario: Create a course from a template
When I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description              | OrganizationName | CourseType  | IsTemplate |
	| Course 1 | CourseCode1 | My First Course Template | COB              | Traditional | false      |
Then the course 'Course 1' should have the following info
	| Field       | Value                    |
	| Name        | Course 1                 |
	| Code        | CourseCode1              |
	| Description | My First Course Template |
	| CourseType  | Traditional              |
	| IsTemplate  | false                    |

Scenario: Ignore course Type in the request when creating from template
When I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description              | OrganizationName | CourseType | IsTemplate |
	| Course 2 | CourseCode2 | My First Course Template | COB              | Competency | false      |
Then the course 'Course 2' should have the following info
	| Field       | Value                    |
	| Name        | Course 2                 |
	| Code        | CourseCode2              |
	| Description | My First Course Template |
	| CourseType  | Traditional              |
	| IsTemplate  | false                    |

Scenario: Verify programs are copied from course template
When I associate 'Template 1' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
And I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description                   | OrganizationName | CourseType  | IsTemplate |
	| Course 3 | CourseCode3 | My First Course from Template | COB              | Traditional | false      |
Then the course 'Course 3' includes the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |

Scenario: Verify segments are copied from course template
When I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description                   | OrganizationName | CourseType  | IsTemplate |
	| Course 4 | CourseCode4 | My First Course from Template | COB              | Traditional | false      |
Then the course 'Course 4' should have these course segments
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |

Scenario: Verify outcomes are copied from course template
When I associate the newly created learning outcomes to 'Template 1' course
	| Description                    |
	| first course learning outcome  |
	| second course learning outcome |
And I create a course from the template 'Template 1' with the following
	| Name     | Code        | Description                   | OrganizationName | CourseType  | IsTemplate |
	| Course 5 | CourseCode5 | My First Course from Template | COB              | Traditional | false      |
Then the course 'Template 1' should have the following learning outcomes
	| Description                    | 
	| first course learning outcome  | 
	| second course learning outcome |

Scenario: Publish a course version, created from a template
	Given I associate 'Template 1' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	And I create a course from the template 'Template 1' with the following
	| Name         | Code   | Description                   | OrganizationName | CourseType | IsTemplate |
	| English 1010 | ENG101 | Ranji's awesome English Class | COB              | Competency | false      |
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
	| Name         | Code   | Description                   | OrganizationName | CourseType | IsTemplate |
	| English 1010 | ENG101 | Ranji's awesome English Class | COB              | Competency | false      |
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

@ignore
Scenario: I can see course templates for an organization I have CreateCourse capability on.
    Given I am user "TestUser3"
	And I create an organization "Org1" with no parent
	And I create an organization "Org2" with no parent
	And I create a course template "FindMe1" for organization "Org1"
	And I create a course template "FindMe2" for organization "Org1"
	And I create a course template "DontFindMe" for organization "Org2"
	And I create a role "CurriculumCoordinator"
	And I add capability CourseCreate to role "CurriculumCoordinator"
	And I give role "CurriculumCoordinator" to user "TestUser3" for object "Org1" of type "organization"
	When I get the course templates for organization "Org1" to scenario context name "templateIds"
	Then the course template Ids in "templateIds" are:
	| OrgName |
	| FindMe1 |
	| FindMe2 |

@ignore
Scenario: I can not see course templates for an organization I do have CreateCourse capability on.
    Given I am user "TestUser3"
	And I create an organization "Org1" with no parent
	And I create an organization "Org2" with no parent
	And I create a course template "FindMe1" for organization "Org1"
	And I create a course template "DontFindMe" for organization "Org2"
	And I create a role "CurriculumCoordinator"
	And I add capability CourseCreate to role "CurriculumCoordinator"
	And I give role "CurriculumCoordinator" to user "TestUser3" for object "Org1" of type "organization"
	When I get the course templates for organization "Org2" to scenario context name "templateIds"
	Then the course template Ids in "templateIds" are:
	| OrgName |

Scenario: Cannot get the template after create a course version from a previously-published version（DE395）
	When I create a course from the template 'Template 1' with the following
    | Name         | Code        | Description              | OrganizationName | CourseType  | IsTemplate |
    | English 2020 | CourseCode1 | My First Course Template | COB              | Traditional | false      |
	And I publish the following courses
	| Name         | Note     |
	| English 2020 | Blah blah |
	And I create a new version of 'English 2020' course named 'English 2020 v1.0.0.1' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.1 |
	Then The course 'English 2020 v1.0.0.1' should have the template named 'Template 1'
	