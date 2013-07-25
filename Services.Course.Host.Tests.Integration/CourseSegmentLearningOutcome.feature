@Api
Feature: CourseSegmentLearningOutcome
	In order to create meaningful course segments
	As a program manager
	I want to specify learning outcomes for course segments

Background: 
	Given the following course exists
	| Field          | Value                                |
	| Name           | Math 101                             |
	| Code           | M101                                 |
	| Description    | Basic Mathematics                    |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	And 'Math 101' course has the following learning outcomes:
	| Description |
	| CLO1        |
	| CLO2        |
	And I add following course segments to 'Math 101'
	| Field       | Value                    |
	| Name        | Week1                    |
	| Description | First week is slack time |
	| Type        | TimeSpan                 |

Scenario: Add learning outcome to a course segment
	When I add the following learning outcomes for 'Week1' course segment:
	| Description |
	| WLO1        |
	| WLO2        |
	Then 'Week1' course segment has the following learning outcomes:
	| Description |
	| WLO1        |
	| WLO2        |

Scenario: Add learning outcome to a course segment which satisfies course outcome
	Given I create the following learning outcomes for 'Week1' course segment:
	| Description |
	| WLO1        |
	| WLO2        |
	When I associate the following segment learning outcomes to 'CLO1' course learning outcome
	| Description |
	| WLO1        |
	| WLO2        |
	Then 'Math 101' course has the following learning outcomes tree
	| Description | SupportingOutcomes |
	| CLO1        | WLO1, WLO2         |
	| CLO2        |                    |
