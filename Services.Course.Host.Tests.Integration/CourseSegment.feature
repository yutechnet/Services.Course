@Api
Feature: CourseSegment
	In order to structure my course
	As a curriculum coordinator
	I want to add segments to the course

Background: 
	Given I have the following courses
	| Name     | Code | Description       | OrganizationId |
	| Math 101 | M101 | Basic mathematics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Add course segment
	Given I have the following course segments for 'Math 101'
	| Name  | Description                    | Type     | ParentSegment |
	| Week1 | First week is slack time       | TimeSpan |               |
	| Week2 | Second week is more slack time | TimeSpan |               |
	Then the course 'Math 101' should have these course segments
	| Name  | Description                    | Type     | ParentSegment |
	| Week1 | First week is slack time       | TimeSpan |               |
	| Week2 | Second week is more slack time | TimeSpan |               |

Scenario: Add a nested course segment
	Given I have the following course segments for 'Math 101'
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |
	Then the course 'Math 101' should have these course segments
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |

Scenario: Update the course segment info without affecting the segment tree
	Given I have the following course segments for 'Math 101'
	| Name       | Description                   | Type       | ParentSegment |
	| Week1      | First week is slack time      | TimeSpan   |               |
	| Discussion | Discussion for the first week | Discussion | Week1         |
	| Topic      | Topic for a discussion        | Topic      | Discussion    |
	When I update the course segments as follows
	| Name       | Description                | Type       |
	| Discussion | Discussion is important    | Discussion |
	| Topic      | New Topic for a discussion | Topic      |
	Then the course 'Math 101' should have these course segments
	| Name       | Description                | Type       | ParentSegment |
	| Week1      | First week is slack time   | TimeSpan   |               |
	| Discussion | Discussion is important    | Discussion | Week1         |
	| Topic      | New Topic for a discussion | Topic      | Discussion    |

Scenario: Delete the course segment
	Given I have the following course segments for 'Math 101'
	| Name     | Description | Type       | ParentSegment |
	| Segment1 | Segment 1   | TimeSpan   |               |
	| Segment2 | Segment 2   | Discussion | Segment1      |
	| Segment3 | Segment 3   | Topic      | Segment2      |
	| Segment4 | Segment 4   | Topic      | Segment1      |
	| Segment5 | Segment 5   | Topic      | Segment1      |
	When I delete the following segments
	| Name      |
	| Segment2  |
	| Segment4  |
	Then the course 'Math 101' should have these course segments
	| Name     | Description | Type       | ParentSegment |
	| Segment1 | Segment 1   | TimeSpan   |               |
	| Segment5 | Segment 5   | Topic      | Segment1      |

Scenario: Retrieve the course segment tree from anywhere in the structure
	Given I have the following course segments for 'Math 101'
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |
	Then the course segment 'Discussion' should have these children segments
	| Name        | Description                    | Type       |
	| Topic       | Topic for a discussion         | Topic      |

Scenario: Add multiple content to a segment
	Given I have the following course segments for 'Math 101'
	| Name       | Description                   | Type       | ParentSegment |
	| Week1      | First week is slack time      | TimeSpan   |               |
	| Discussion | Discussion for the first week | Discussion | Week1         |
	When I add the following content to 'Discussion' segment
	| Id                                   | Type       |
	| 10257797-ab74-4796-a2f7-4722f8f96abe | Discussion |
	| 806f8cc4-ed16-4a1b-b8f2-1df373b7631a | Assignment |
	Then the course segment 'Discussion' should have this content
	| Id                                   | Type       |
	| 10257797-ab74-4796-a2f7-4722f8f96abe | Discussion |
	| 806f8cc4-ed16-4a1b-b8f2-1df373b7631a | Assignment |

Scenario: Add another parent segment to a course
	Given I have the following course segments for 'Math 101'
	| Name       | Description                   | Type       | ParentSegment |
	| Week1      | First week is slack time      | TimeSpan   |               |
	| Discussion | Discussion for the first week | Discussion | Week1         |
	| Week2      | second week                   | Topic      |               |  
	Then the course 'Math 101' should have these course segments
	| Name       | Description                   | Type       | ParentSegment |
	| Week1      | First week is slack time      | TimeSpan   |               |
	| Discussion | Discussion for the first week | Discussion | Week1         |
	| Week2      | second week                   | Topic      |               |  

Scenario: Check segment tree is loaded 
	Given I have the following course segments for 'Math 101'
	| Name          | Description                       | Type       | ParentSegment |
	| Week1         | First week is slack time          | TimeSpan   |               |
	| Week2         | second week                       | Topic      |               |
	| Discussion1   | Discussion for the first week     | Discussion | Week1         |
	| Discussion2   | Discussion for the first week     | Discussion | Week1         |
	| Discussion1.1 | Sub Discussion for the first week | Discussion | Discussion1   |
	| Discussion1.2 | Sub Discussion for the first week | Discussion | Discussion1   |
	Then the course 'Math 101' should have this course segment tree
	| Name          | Description                       | Type       | ParentSegment | ChildCount |
	| Week1         | First week is slack time          | TimeSpan   |               | 2          |
	| Week2         | second week                       | Topic      |               | 0          |
	| Discussion1   | Discussion for the first week     | Discussion | Week1         | 2          |
	| Discussion2   | Discussion for the first week     | Discussion | Week1         | 0          |
	| Discussion1.1 | Sub Discussion for the first week | Discussion | Discussion1   | 0          |
	| Discussion1.2 | Sub Discussion for the first week | Discussion | Discussion1   | 0          |

Scenario: Ensure Course Segment display order is persisted on Save
	Given I have the following course segments for 'Math 101'
	| Name          | Description                       | Type       | ParentSegment | DisplayOrder |
	| Week2         | second week                       | Topic      |               | 1            |
	| Week1         | First week is slack time          | TimeSpan   |               | 0            |
	| Discussion2   | Discussion for the first week     | Discussion | Week1         | 3            |
	| Discussion1   | Discussion for the first week     | Discussion | Week1         | 2            |
	| Discussion1.1 | Sub Discussion for the first week | Discussion | Discussion1   | 4            |
	| Discussion1.2 | Sub Discussion for the first week | Discussion | Discussion1   | 5            |
	Then The course 'Math 101' segments retrieved match the display order entered
	| Name          | DisplayOrder |
	| Week1         | 0            |
	| Week2         | 1            |
	| Discussion1   | 2            |
	| Discussion2   | 3            |
	| Discussion1.1 | 4            |
	| Discussion1.2 | 5            |
	
Scenario: Ensure Course Segment display order is persisted on Update
	Given I have the following course segments for 'Math 101'
	| Name          | Description                       | Type       | ParentSegment | DisplayOrder |
	| Week1         | First week is slack time          | TimeSpan   |               | 0            |
	| Week2         | second week                       | Topic      |               | 1            |
	| Discussion1   | Discussion for the first week     | Discussion | Week1         | 2            |
	| Discussion2   | Discussion for the first week     | Discussion | Week1         | 3            |
	| Discussion1.1 | Sub Discussion for the first week | Discussion | Discussion1   | 4            |
	| Discussion1.2 | Sub Discussion for the first week | Discussion | Discussion1   | 5            |
	When I update the course segments as follows
	| Name          | Description                       | Type       | ParentSegment | DisplayOrder |
	| Week1         | First week is slack time          | TimeSpan   |               | 5            |
	| Week2         | second week                       | Topic      |               | 4            |
	| Discussion1   | Discussion for the first week     | Discussion | Week1         | 3            |
	| Discussion2   | Discussion for the first week     | Discussion | Week1         | 2            |
	| Discussion1.1 | Sub Discussion for the first week | Discussion | Discussion1   | 1            |
	| Discussion1.2 | Sub Discussion for the first week | Discussion | Discussion1   | 0            |
	Then The course 'Math 101' segments retrieved match the display order entered
	| Name          | DisplayOrder |
	| Week1         | 5            |
	| Week2         | 4            |
	| Discussion1   | 3            |
	| Discussion2   | 2            |
	| Discussion1.1 | 1            |
	| Discussion1.2 | 0            |

