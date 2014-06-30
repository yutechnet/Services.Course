@Api
Feature: CourseAuthorization
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to

Background:
	And I have the following assets
	| Name   |
	| asset1 |
	And Published the following assets
	| Name   | PublishNote |
	| asset1 | published   |

Scenario Outline: I can not create a course unless I have permission to do so.
	Given I have the '<Capability>' capability
	When I create the course
	| Field            | Value           |
	| Name             | English 100     |
	| Code             | ENG100          |
	| Description      | College English |
	| OrganizationName | Default         |
	Then I get '<StatusCode>' response
Examples:
| Capability    | StatusCode |
| CourseCreate  | Created    |
| CoursePublish | Forbidden  |
|               | Forbidden  |

Scenario: I can view a course when I do have permission.
	Given I have the 'CourseCreate' capability
	And I have the following courses
	| Name     | Code | Description       | OrganizationName | MetaData   | ExtensionAssets |
	| Math 101 | M101 | Basic mathematics | Default          | {someData} | asset1          |
	And I have the following object capabilities
	| ObjectType | ObjectName | Capability |
	| Course     | Math 101   | CourseView |
	When I get the course 'Math 101'
	Then I get 'OK' response

Scenario: I can not view a course when I do not have permission.
	Given I have the 'CourseCreate' capability
	And I have the following courses
	| Name     | Code | Description       | OrganizationName | MetaData   | ExtensionAssets |
	| Math 101 | M101 | Basic mathematics | Default          | {someData} | asset1          |
	When I get the course 'Math 101'
	Then I get 'Forbidden' response

