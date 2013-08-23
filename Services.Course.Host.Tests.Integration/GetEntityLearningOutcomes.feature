@Api
Feature: GetEntityLearningOutcome
	Given a course, program or segment 
	As a program manager
	I want to know what LearningOutcomes are associated with them

Background: 
	Given I have the following programs
	| Name     | Description | ProgramType | OrganizationName |
	| Program1 | Program1    | BA          | Default          |
	| Program2 | Program2    | BS          | Default          |
	| Program3 | Program3    | MA          | Default          |
	And I have the following courses
	| Name    | Code | Description | OrganizationName |
	| Course1 | 1    | Course1     | Default          |
	| Course2 | 2    | Course2     | Default          |
	| Course3 | 3    | Course3     | Default          |
    And I have the following course segments for 'Course1'
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |
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
	Then I get the entity learning outcomes as follows
	| EntityType | EntityName | LearningOutcomes |
	| Program    | Program1   | LO1              |
	| Program    | Program2   | LO2, LO3         |
	| Program    | Program3   |                  |

Scenario: Can get entity learning outcomes for courses
	When I associate the existing learning outcomes to 'Course1' course
	| Description |
	| LO1  |
	And I associate the existing learning outcomes to 'Course3' course
	| Description |
	| LO2  |
	| LO3  |
	Then I get the entity learning outcomes as follows
	| EntityType | EntityName | LearningOutcomes |
	| Course     | Course1    | LO1              |
	| Course     | Course2    |                  |
	| Course     | Course3    | LO2, LO3         |
	| Program    | Program1   |                  |
	| Program    | Program2   |                  |
	| Program    | Program3   |                  |

Scenario: Can get entity learning outcomes for segments
	When I associate the existing learning outcomes to 'Week1' segment
	| Description |
	| LO1         |
	When I associate the existing learning outcomes to 'Topic' segment
	| Description |
	| LO1         |
	| LO2         |
	| LO3         |
	Then I get the entity learning outcomes as follows
	| EntityType | EntityName  | LearningOutcomes |
	| Segment    | Week1       | LO1              |
	| Segment    | Topic       | LO1, LO2, LO3    |
	| Segment    | Discussion  |                  |
	| Segment    | Discussion2 |                  |
	| Course     | Course1     |                  |
	| Course     | Course2     |                  |
	| Course     | Course3     |                  |
	| Program    | Program1    |                  |
	| Program    | Program2    |                  |
	| Program    | Program3    |                  |