@Api
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
         | Name          | Code   | Description                     |
         | Psychology 101| PSY101 | Amro's awesome Psychology class |
	And I delete this course
	Then I should get a success confirmation message
	And my course no longer exists

Scenario Template: Cannot create a course with same name or code
	Given I have an existing course with following info:
         | Name           | Code   | Description                     |
         | Psychology 101 | PSY101 | Amro's awesome Psychology class |
	When I create a new course with <Name>, <Code>, <Description>
	And I submit a creation request
    Then I should get the status code <Status>

	Examples: 
         | Name           | Code   | Description                             | Status   |
         | Psychology 101 | PSY102 | Amro's another awesome Psychology class | Conflict |
         | Psychology 102 | PSY101 | Amro's another awesome Psychology class | Conflict |

Scenario Template: cannot create a course with missing data
	When I create a new course with <Name>, <Code>, <Description>
	And I submit a creation request
    Then I should get the status code <Status>

	Examples: 
         | Name           | Code   | Description                             | Tenant Id | Status     |
         | Psychology 101 |        | Amro's another awesome Psychology class | 1         | BadRequest |
         |                | PSY101 | Amro's another awesome Psychology class | 1         | BadRequest |
         | Physcology 103 | PSY103 |                                         | 1         | OK         |
