@Api
Feature: Course Management
	In order to publish a course
	As a course builder
	I want to create, edit and delete a course

Scenario: Create a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id |
	| English 101 | ENG101 | Ranji's awesome English Class | 1         |	
	When I submit a creation request
	Then I should get a success confirmation message

Scenario: Edit a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id |
	| English 101 | ENG101 | Ranji's awesome English Class | 1         |
	When I submit a creation request
	And I change the info to reflect the following:
	| Name        | Code   | Description                   | Tenant Id |
	| English 202 | ENG202 | Ranji's awesome English Class | 1         |
	Then I should get a success confirmation message
	And my course info is changed

Scenario: Delete a course
	Given I have an existing course with following info:
         | Name          | Code   | Description                     | Tenant Id  |
         | Psychology 101| PSY101 | Amro's awesome Psychology class |    1       |
	And I delete this course
	Then I should get a success confirmation message
	And my course no longer exists

Scenario Template: Cannot create a course with same name or code
	Given I have an existing course with following info:
         | Name          | Code   | Description                     | Tenant Id  |
         | Psychology 101| PSY101 | Amro's awesome Psychology class |    1       |
	When I create a new course with <Name>, <Code>, <Description>, <Tenant Id>
	And I submit a creation request
    Then I should get the status code <Status>

	Examples: 
         | Name           | Code   | Description                             | Tenant Id | Status   |
         | Psychology 101 | PSY102 | Amro's another awesome Psychology class | 1         | Conflict |
         | Psychology 102 | PSY101 | Amro's another awesome Psychology class | 1         | Conflict |

Scenario Template: cannot create a course with missing data
	When I create a new course with <Name>, <Code>, <Description>, <Tenant Id>
	And I submit a creation request
    Then I should get the status code <Status>

	Examples: 
         | Name           | Code   | Description                             | Tenant Id | Status     |
         | Psychology 101 |        | Amro's another awesome Psychology class | 1         | BadRequest |
         |                | PSY101 | Amro's another awesome Psychology class | 1         | BadRequest |
         | Physcology 103 | PSY103 |                                         | 1         | OK         |
         | Physcology 103 | PSY103 | Amro's another awesome Psychology class |           | BadRequest |
