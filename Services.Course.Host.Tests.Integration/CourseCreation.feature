﻿@Api
Feature: Course Management
	In order to publish a course
	As a course builder
	I want to create, edit and delete a course

Scenario: Create a course
	Given I have a course with following info:
	| Name        | Code   | Description                   |
	| English 101 | ENG101 | Ranji's awesome English Class |
	When I submit a creation request
	Then I should get a success confirmation message

Scenario: Return course by course name
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id |
	| English 101 | ENG101 | Ranji's awesome English Class | 1         |
	When I submit a creation request
	Then I should get a success confirmation message
	And I can retrieve the course by course name

Scenario: Return course by course code
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id |
	| English 101 | ENG101 | Ranji's awesome English Class | 1         |
	When I submit a creation request
	Then I should get a success confirmation message
	And I can retrieve the course by course code

Scenario: Return 404 when course name is not found
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id |
	| English 101 | ENG101 | Ranji's awesome English Class | 1         |
	When I submit a creation request
	And I request a course name that does not exist
	Then I should get a not found message returned

Scenario: Edit a course
	Given I have a course with following info:
	| Name        | Code   | Description                   |
	| English 101 | ENG101 | Ranji's awesome English Class |
	When I submit a creation request
	And I change the info to reflect the following:
	| Name        | Code   | Description                   |
	| English 202 | ENG202 | Ranji's awesome English Class |
	Then I should get a success confirmation message
	And my course info is changed

Scenario: Delete a course
	Given I have an existing course with following info:
		 | Name           | Code   | Description                     |
		 | Psychology 101 | PSY101 | Amro's awesome Psychology class |
	And I delete this course
	Then I should get a success confirmation message
	And my course no longer exists

Scenario Template: Can create a course with same name or code
	Given I have an existing course with following info:
		 | Name           | Code   | Description                     |
		 | Psychology 101 | PSY101 | Amro's awesome Psychology class |
	When I create a new course with <Name>, <Code>, <Description>
	And I submit a creation request
	Then I should get the status code <Status>

	Examples: 
		 | Name           | Code   | Description                             | Status  |
		 | Psychology 101 | PSY102 | Amro's another awesome Psychology class | Created |
		 | Psychology 102 | PSY101 | Amro's another awesome Psychology class | Created |

Scenario Template: cannot create a course with missing data
	When I create a new course with <Name>, <Code>, <Description>
	And I submit a creation request
	Then I should get the status code <Status>

	Examples: 
		 | Name           | Code   | Description                             | Tenant Id | Status     |
		 | Psychology 101 |        | Amro's another awesome Psychology class | 1         | BadRequest |
		 |                | PSY101 | Amro's another awesome Psychology class | 1         | BadRequest |
		 | Physcology 103 | PSY103 |                                         | 1         | Created    |

Scenario Template: Return course by partial name
	When I create a new course with <Name>, <Code>, <Description>
	And I submit a creation request
	Then the course name counts are as follows:
		| Starts With | Count |
		| ENG         | 3     |
		| Phil        | 2     |
		| ENGL        | 2     |
		| PHILO       | 1     |
		| MATH        | 0     |
		| Chemistry   | 1     |
		| En*         | 0     |
		| \           | 0     |
		| \\          | 0     |
		| 'E'         | 0     |
		| C*#         | 0     |
		| A&          | 0     |

	Examples: 
		 | Name             | Code     | Description                           | Tenant Id |
		 | English 101      | ENGL101  | Learn to read and write               | 1         |
		 | Engineering 200  | ENG200   | If you build it, they will come       | 1         |
		 | English 220      | ENGL220  | Essays                                | 1         |
		 | Philosophy 100   | PHIL100  | To be, or not to be                   | 1         |
		 | Philanthropy 101 | PHILA101 | Don't be greedy                       | 1         |
		 | Chemistry 350    | CHEM350  | Periodic table of elements to the max | 1         |