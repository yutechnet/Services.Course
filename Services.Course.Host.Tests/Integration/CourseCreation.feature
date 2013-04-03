@Api
Feature: Course Creation
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
