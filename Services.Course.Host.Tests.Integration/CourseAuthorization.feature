@Api
Feature: CourseAuthorization
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to

Scenario: Create a course as a guest
	Given That I am guest
	When I submit an authorized creation request
	Then I should get a failure response