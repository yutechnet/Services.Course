﻿@Api
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
	| Name  | Description                    | Type     | ParentSegment |
	| Week1 | First week is slack time       | TimeSpan |               |
	| Week2 | Second week is more slack time | TimeSpan |               |
	Then the course 'Math 101' should have these course segments:
	| Name  | Description                    | Type     | ParentSegment |
	| Week1 | First week is slack time       | TimeSpan |               |
	| Week2 | Second week is more slack time | TimeSpan |               |

Scenario: Add a nested course segment
	When I add following course segments to 'Math 101':
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |
	Then the course 'Math 101' should have these course segments:
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |

Scenario: Update the course segment info without affecting the segment tree
	When I add following course segments to 'Math 101':
	| Name       | Description                   | Type       | ParentSegment |
	| Week1      | First week is slack time      | TimeSpan   |               |
	| Discussion | Discussion for the first week | Discussion | Week1         |
	| Topic      | Topic for a discussion        | Topic      | Discussion    |
	And I update the course segments as following:
	| Name       | Description                | Type       |
	| Discussion | Discussion is important    | Discussion |
	| Topic      | New Topic for a discussion | Topic      |
	Then the course 'Math 101' should have these course segments:
	| Name       | Description                | Type       | ParentSegment |
	| Week1      | First week is slack time   | TimeSpan   |               |
	| Discussion | Discussion is important    | Discussion | Week1         |
	| Topic      | New Topic for a discussion | Topic      | Discussion    |

Scenario: Retrieve the course segment tree from anywhere in the structure
	When I add following course segments to 'Math 101':
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |
	Then the course segment 'Discussion' should have these children segments:
	| Name        | Description                    | Type       |
	| Topic       | Topic for a discussion         | Topic      |
