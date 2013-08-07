@Api
Feature: GetEntityLearningOutcome
	Given a course, program or segment 
	As a program manager
	I want to know what LearningOutcomes are associated with them

Background: 
	Given I have the following programs
	| Name     | Description | ProgramType | OrganizationId                       |
	| Program1 | Program1    | BA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Program2 | Program2    | BS          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Program3 | Program3    | MA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	And I have the following courses
	| Name    | Code | Description | OrganizationId                       |
	| Course1 | 1    | Course1     | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Course2 | 2    | Course2     | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
    And I have the following course segments for 'Course1'
	| Name        | Description                    | Type       | ParentSegmentName |
	| Week1       | First week is slack time       | TimeSpan   |                   |
	| Discussion  | Discussion for the first week  | Discussion | Week1             |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1             |
	| Topic       | Topic for a discussion         | Topic      | Discussion        |
	And I have the following learning outcomes
	| Description |
	| LO1         |
	| LO2         |
	| LO3         |
	| LO4         |
	| LO5         |

Scenario: Can get entity learning outcomes for programs
	When I associate the existing learning outcomes to 'Program1' program
	| Description |
	| LO1  |
	And I associate the existing learning outcomes to 'Program2' program
	| Description |
	| LO2  |
	| LO3  |
	Then I get the entity learning outcomes as follows:
	| EntityType | EntityName | LearningOutcomeName |
	| Program    | Program1   | LO1                 |
	| Program    | Program2   | LO1, LO2            |
	| Program    | Program3   |                     |