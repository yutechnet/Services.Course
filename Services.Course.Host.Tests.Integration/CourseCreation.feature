﻿@Api
Feature: Course Management
	In order to publish a course
	As a course builder
	I want to create, edit and delete a course
Background: 
	Given the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	| EditCourse    |
Scenario: Create a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | Credit | MetaData   | ExtensionAssets                                                           | CorrelationId  |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | 5      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB,6B7D1752-2A8D-4848-B8BC-1B1E42164499 | correlationId1 |
	When I submit a creation request
	Then I get 'Created' response
	And The course 'English 101' has following CorrelationId
	| CorrelationId  |
	| correlationId1 |

Scenario: Edit a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | Credit | MetaData   | ExtensionAssets                      |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | 5      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB |
	When I submit a creation request
	And I change the info to reflect the following:
	| Name        | Code   | Description                  | CourseType  | IsTemplate | Credit | MetaData        | ExtensionAssets                                                           |CorrelationId  |
	| English 101 | ENG101 | John's awesome English Class | Traditional | false      | 10     | {differentData} | B40CE4F4-434A-4987-80A8-58F795C212EB,6B7D1752-2A8D-4848-B8BC-1B1E42164499 |correlationId1 |
	Then I get 'NoContent' response
	And my course info is changed

Scenario: Delete a course
	Given I have an existing course with following info:
	 | Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets                      |
	 | English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB |
	And I delete this course
	Then I get 'NoContent' response
	And my course no longer exists

Scenario Template: Can create a course with same name or code
	Given I have an existing course with following info:
	 | Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets                      |
	 | English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB |
	When I create a new course with <Name>, <Code>, <Description>, <OrganizationName>
	And I submit a creation request
	Then I get '<Status>' response

	Examples: 
		 | Name           | Code   | Description                             | Status  | OrganizationName |
		 | Psychology 101 | PSY102 | Amro's another awesome Psychology class | Created | COB              |
		 | Psychology 102 | PSY101 | Amro's another awesome Psychology class | Created | COB              |

@ignore
#TODO: US4696 says "* Ensure CourseCode is unique if it isn't already (break out into another story if necessary)"
#Unignore this test when this is implemented.
Scenario: Can not create a course with same code
	Given I have an existing course with following info:
	 | Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets                      |
	 | English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB |
	When I create a new course with Psychology 101, SAME, Code1, COB
	And I submit a creation request
	Then I get 'Created' response
	When I create a new course with Psychology 102, SAME, Same as Code1, COB
	And I submit a creation request
	Then I get 'BadRequest' response

Scenario Template: cannot create a course with missing data
	When I create a new course with <Name>, <Code>, <Description>, <OrganizationName>
	And I submit a creation request
	Then I get '<Status>' response

	Examples: 
		 | Name           | Code   | Description                             | OrganizationName | Status     |
		 | Psychology 101 |        | Amro's another awesome Psychology class | COB              | BadRequest |
		 |                | PSY101 | Amro's another awesome Psychology class | COB              | BadRequest |
		 | Physcology 103 | PSY103 |                                         | COB              | Created    |


## Old orgid: C3885307-BDAD-480F-8E7C-51DFE5D80387

Scenario: Return course by partial name
	Given I have existing courses with following info:
		 | Name             | Code     | Description                           | Tenant Id | OrganizationName | CourseType  | IsTemplate | CorrelationId  |
		 | English 101      | ENGL101  | Learn to read and write               | 999999    | COB              | Traditional | false      | correlationId1 |
		 | Engineering 200  | ENG200   | If you build it, they will come       | 999999    | COB              | Traditional | false      | correlationId2 |
		 | English 220      | ENGL220  | Essays                                | 999999    | COB              | Traditional | false      |                |
		 | Philosophy 100   | PHIL100  | To be, or not to be                   | 999999    | COB              | Traditional | false      | correlationId3 |
		 | Philanthropy 101 | PHILA101 | Don't be greedy                       | 999999    | COB              | Traditional | false      | correlationId4 |
		 | Chemistry 350    | CHEM350  | Periodic table of elements to the max | 999999    | COB              | Traditional | false      |                |
    And the user has permission to access these courses:
		 | Name             | 
		 | English 101      | 
		 | Engineering 200  | 
		 | English 220      | 
		 | Philosophy 100   | 
		 | Philanthropy 101 | 
		 | Chemistry 350    | 
    Then the course name counts are as follows:
		| Operation  | Argument          | Count |
		| Eq         | Engineering%20200 | 1     |
		| StartsWith | Eng               | 3     |
		| StartsWith | Phil              | 2     |
		| StartsWith | Engl              | 2     |
		| StartsWith | Philo             | 1     |
		| StartsWith | MATH              | 0     |
		| StartsWith | Chemistry         | 1     |
		| StartsWith | En*               | 0     |
		| StartsWith | \                 | 0     |
		| StartsWith | \\                | 0     |
		| StartsWith | ''E''             | 0     |
		| StartsWith | C*%23             | 0     |
		| StartsWith | A%26              | 0     |
	And the course correlationId counts are as follows:
		| Operation | Argument       | Count |
		| Eq        | correlationId1 | 1     |
		| Eq        | correlationId2 | 1     |
		| Eq        | correlationId3 | 1     |
		| Eq        | correlationId4 | 1     |

Scenario: Add organization id to a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets                      |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB |
	When I submit a creation request
	Then the organization id is returned as part of the request

Scenario: Validate TemplateCourseId is expected to be null in CourseInfoResponse when course is not created from template (DE1583)
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | Credit | MetaData   | ExtensionAssets                                                           |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | 5      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB,6B7D1752-2A8D-4848-B8BC-1B1E42164499 |
	When I submit a creation request
	Then TemplateCourseId should be null in the course 'English 101'

Scenario: Can not create a course use a same correlationId
	When I create course wiht the following info
	| Name        | Code    | Description        | Tenant Id | OrganizationName | CourseType  | IsTemplate | CorrelationId  |
	| English 101 | ENGL101 | CorrelationId test | 999999    | COB              | Traditional | false      | correlationId1 |
	Then I get 'Created' response
	When I create course wiht the following info
	| Name        | Code    | Description        | Tenant Id | OrganizationName | CourseType  | IsTemplate | CorrelationId  |
	| English 102 | ENGL102 | CorrelationId test | 999999    | COB              | Traditional | false      | correlationId1 |
	Then I get 'BadRequest' response
