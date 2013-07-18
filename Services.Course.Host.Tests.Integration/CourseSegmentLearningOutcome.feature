@Api
Feature: CourseSegmentLearningOutcome
	In order to create meaningful course segments
	As a program manager
	I want to specify learning outcomes for course segments

#Background: 
#	Given the following courses exist:
#	| Name     | Code | Description       | OrganizationId |
#	| Math 101 | M101 | Basic mathematics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
#	And 'Math 101' course has the following learning outcomes:
#	| Description             |
#	| CLO1  |
#	| CLO2 |
#	And I add following course segments to 'Math 101':
#	| Name  | Description                    | Type     | ParentSegment |
#	| Week1 | First week is slack time       | TimeSpan |               |
#
#Scenario: Add learning outcome to a course segment
#	When I create the following learning outcomes for 'Week1' course segment:
#	| Description |
#	| SLO1        |
#	| SLO2        |
#	Then 'Week1' course segment has the following learning outcomes:
#	| Description             |
#	| first learning outcome  |
#	| second learning outcome |
#
#Scenario: Add learning outcome to a course segment which satisfies course outcome
#	Given I create the following learning outcomes for 'Week1' course segment:
#	| Description | Satisfies
#	| SLO1        |
#	| SLO2        |
#	And 'Week1' course segment has the following learning outcomes:
#	| Description             |
#	| first learning outcome  |
#	| second learning outcome |
