@Api
Feature: CourseSegment
	In order to structure my course
	As a curriculum coordinator
	I want to add segments to the course
Background: 
	Given the following courses exist:
	| Name     | Code | Description       |
	| Math 101 | M101 | Basic mathematics |

Scenario: Add course segment
	When I add following course segments to 'Math 101':
	| Name  | Description              | Type     |
	| Week1 | First week is slack time | TimeSpan |
	Then the course 'Math 101' should have these course segments:
	| Name  | Description              | Type     |
	| Week1 | First week is slack time | TimeSpan |

Scenario: Add a nested course segment
	When I add following course segments to 'Math 101':
	| Name       | Description                   | Type       | ParentSegment |
	| Week1      | First week is slack time      | TimeSpan   |               |
	| Discussion | Discussion for the first week | Discussion | Week1         |
	Then the course 'Math 101' should have these course segments:
	| Name       | Description                   | Type       | ParentSegment |
	| Week1      | First week is slack time      | TimeSpan   |               |
	| Discussion | Discussion for the first week | Discussion | Week1         |
