@Api
Feature: Course Learning Material

Background: 
	Given the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability   |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	And I have the following courses
	| Name     | Code | Description    | OrganizationName | CourseType  | IsTemplate |
	| Econ 100 | E100 | Macroeconomics | COB              | Traditional | False      |
	And I have the following course segments for 'Econ 100'
	| Name   | Description              | Type     | ParentSegment |
	| Week 1 | First week is slack time | TimeSpan |               |
	Given I add the following course learning activities to 'Week 1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| Discussion 1 | Discussion | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assignment 1 | Assignment | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	And I have the following assets
	| Name  |
	| file1 |
	| file2 |

Scenario: Add course learning material
	When I add the following assets as learning material to 'Discussion 1' learning activity
	| Name  |
	| file1 |
	| file2 |
	Then 'Discussion 1' learning activity has the following learning material
	| Name  |
	| file1 |
	| file2 |

