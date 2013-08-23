@Api
Feature: CourseSegmentLearningOutcome
	In order to create meaningful course segments
	As a program manager
	I want to specify learning outcomes for course segments

Background: 
	Given I have the following courses
	| Name     | Code | Description       | OrganizationName |
	| Math 101 | M101 | Basic mathematics | Default          |
	And I associate the newly created learning outcomes to 'Math 101' course
	| Description |
	| CLO1        |
	| CLO2        |
	And I have the following course segments for 'Math 101'
	| Name   | Description              | Type     | ParentSegment |
	| Week 1 | First week is slack time | TimeSpan |               |

Scenario: Add learning outcome to a course segment
	Given I associate the newly created learning outcomes to 'Week 1' segment
	| Description |
	| WLO1        |
	| WLO2        |
	Then the segment 'Week 1' includes the following learning outcomes
	| Description |
	| WLO1        |
	| WLO2        |

Scenario: Add learning outcome to a course segment which satisfies course outcome
	Given I associate the newly created learning outcomes to 'Week 1' segment
	| Description |
	| WLO1        |
	| WLO2        |
	When the outcome 'CLO1' is supported by the following outcomes
	| Description |
	| WLO1        |	
	| WLO2        |	
	Then the course 'Math 101' has the following learning outcomes
	| Description | SupportingOutcomes |
	| CLO1        | WLO1, WLO2         |
	| CLO2        |                    |
