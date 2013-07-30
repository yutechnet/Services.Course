@Api
Feature: CourseAuthorization
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to

#Background:
#	Given these roles already exist:
#	| RoleKey | Name  | TenantId | Capabilities |
#	| Role1   | Role1 | 999999   | 2            |
#	And these users already exist:
#	| UserKey | FirstName | LastName | Password | Email             | TenantId | Roles |
#	| User1   | User      | One      | password | user1@example.com | 999999   | Role1 |
#	And these objects already exist:
#	| ObjectKey         |
#	| Object1           |
#	And these organizations already exist:
#	| OrganizationKey | Name | ParentKey     | TenantId |
#	| Organization1   | Org1 |               | 999999   |
#	| Organization2   | Org2 | Organization1 | 999999   |


Scenario: Create a course as an administrator
	Given That I am admin
	When I submit an authorized creation request
	Then I should get a success response

Scenario: Create a course as a guest
	Given That I am guest
	When I submit an authorized creation request
	Then I should get a failure response

# Scenario: Create a course as a course creator
#	Given That I am user "User1"
#	When I submit an authorized creation request for organization "Organization1"
#	Then I should get a success response