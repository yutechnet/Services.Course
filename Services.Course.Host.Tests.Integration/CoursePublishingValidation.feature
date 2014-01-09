@Api
Feature: CoursePublishingValidation

Background:
	Given the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	And I have the following courses
	| Name     | Code | Description    | OrganizationName | CourseType  | IsTemplate |
	| Econ 100 | E100 | Macroeconomics | COB              | Traditional | False      |
	And I have the following course segments for 'Econ 100'
	| Name   | Description              | Type     | ParentSegment |
	| Week 1 | First week is slack time | TimeSpan |               |
	And I add the following course learning activities to 'Week 1' course segment
	| Name         | AssessmentType | IsGradeable | IsExtraCredit | Weight | MaxPoint |
	| Assignment 1 | Essay          | True        | true          | 100    | 20       |

Scenario: Publishing a course with Learning Activity without assessment fails
	When I publish the following courses
    | Name     | Note             |
    | Econ 100 | fails validation |
	Then I get 'BadRequest' response

Scenario: Associate a published assessment to an existing learning activity
	Given I have the following assessments
	| Name        | Instructions | AssessmentType | IsPublished |
	| Assessment1 | Do this      | Essay          | true        |
	When I update 'Assignment 1' learning activity with the following info
	| Field         | Value                                |
	| Name          | Assignment 1                         |
	| Type          | Custom                               |
	| IsGradeable   | false                                |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assessment    | Assessment1                          |
	And I publish the following courses
    | Name     | Note                |
    | Econ 100 | validation succeeds |
	Then I get 'NoContent' response
