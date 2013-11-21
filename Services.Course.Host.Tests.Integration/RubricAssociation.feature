﻿@Api
Feature: Rubric Association
	In order to assess student's work
	As a Learning Activity Creator
	I want to declare rubric associations to learning activities

Background: 
	And the following organizations exist
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
	| Econ 200 | E200 | Macroeconomics | COB              | Traditional | False      |
	When I publish the following courses
	| Name     | Note   |
	| Econ 200 | a note |
	And I have the following course segments for 'Econ 100'
	| Name   | Description              | Type     | ParentSegment |
	| Week 1 | First week is slack time | TimeSpan |               |
	And I add the following course learning activities to 'Week 1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| Discussion 1 | Discussion | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assignment 1 | Assignment | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Custom 1     | Custom     | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Custom 2     | Custom     | False       | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Custom 3     | Custom     | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	And I have the following rubrics
	| Title          | Description                   | OrganizationId                       | PerformanceLevel | ScoringModel | MinPoint | MaxPoint | IsPublished |
	| Math101 Rubric | Basic math assessment tool    | D2DF063D-E2A1-4F83-9BE0-218EC676C05F | Low, Med, High   | Unset        | 50       | 100      | True        |
	| Econ101 Rubric | Basic econ assessment tool    | D2DF063D-E2A1-4F83-9BE0-218EC676C05F | Low, Med, High   | Unset        | 50       | 100      | True        |
	| Eng101 Rubric  | Basic english assessment tool | D2DF063D-E2A1-4F83-9BE0-218EC676C05F | Low, Med, High   | Unset        | 50       | 100      | False       |

Scenario: Associate multiple rubric id's to learning activity
	When I add rubric 'Math101 Rubric' to 'Custom 1' learning activity
	And I add rubric 'Econ101 Rubric' to 'Custom 1' learning activity
	Then the learning activity 'Custom 1' should have the following rubrics
	| Title          | Description                   | OrganizationId                       | PerformanceLevel | ScoringModel | MinPoint | MaxPoint | IsPublished |
	| Math101 Rubric | Basic math assessment tool    | D2DF063D-E2A1-4F83-9BE0-218EC676C05F | Low, Med, High   | Unset        | 50       | 100      | True        |
	| Econ101 Rubric | Basic econ assessment tool    | D2DF063D-E2A1-4F83-9BE0-218EC676C05F | Low, Med, High   | Unset        | 50       | 100      | True        |
	

Scenario: Verify unpublished rubrics cannot be associated to learning activities
	When I add rubric 'Eng101 Rubric' to 'Custom 1' learning activity
	Then I get 'BadRequest' response

Scenario: Can only add rubrics to learning activities of type Custom
	When I add rubric 'Math101 Rubric' to 'Assignment  1' learning activity
	Then I get 'BadRequest' response

Scenario: Cannot add one rubric more than once to the same learning activity
	When I add rubric 'Math101 Rubric' to 'Custom 1' learning activity
	And I add rubric 'Math101 Rubric' to 'Custom 1' learning activity
	Then I get 'BadRequest' response

Scenario: Can only add rubrics to learning activities that are gradable
	When I add rubric 'Math101 Rubric' to 'Custom 2' learning activity
	Then I get 'BadRequest' response

Scenario: Cannot modify learning activity to update type or gradability if rubric is already assigned
	When I add rubric 'Math101 Rubric' to 'Custom 1' learning activity
	And I update 'Custom 1' learning activity with the following info
	| Field         | Value                                |
	| Name          | Custom 1                             |
	| Type          | Custom                               |
	| IsGradeable   | false                                |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then I get 'BadRequest' response
	And I update 'Custom 3' learning activity with the following info
	| Field         | Value                                |
	| Name          | Custom 1                             |
	| Type          | Assignment                           |
	| IsGradeable   | true                                 |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then I get 'BadRequest' response

Scenario: Course must be unpublished for any rubric association/disassociation (to learning activities)
	When I publish the following courses
	| Name     | Note   |
	| Econ 100 | a note |
	And I add rubric 'Math101 Rubric' to 'Custom 1' learning activity
	Then I get 'BadRequest' response

Scenario: Cannot delete a rubric from a learning activity where the rubric is not associated
	