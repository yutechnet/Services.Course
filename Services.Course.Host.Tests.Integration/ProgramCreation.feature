@Api
Feature: ProgramCreation
	In order to publish a program
	As a program builder
	I want to create, edit and delete a program

Scenario: Create a basic program
	Given I have a program with following info:
	| Name                  | Description | OrganizationId | Tenant | 
	| Bachelor's of Science | Economics   | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | 1      |
	When I submit a request to create a program
	Then my program information is as follows:
	| Name                  | Description | OrganizationId | Tenant | 
	| Bachelor's of Science | Economics   | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | 1      |

Scenario: Modify a program
	Given I have a program with following info:
	| Name                  | Description | OrganizationId | Tenant | 
	| Bachelor's of Science | Economics   | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | 1      |
	When I submit a request to create a program
	And I modify the program info to reflect the following:
	| Name              | Description | OrganizationId | Tenant |
	| Bachelor's of Art | English     | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | 1      |
	Then my program information is changed

Scenario: Delete a program
	Given I have an existing program with following info:
	| Name                  | Description | OrganizationId | Tenant | 
	| Bachelor's of Science | Economics   | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | 1      |
	When I delete the program
	Then the program no longer exists

Scenario: Get all programs
	Given I have a program with following info:
	| Name                  | Description | OrganizationId | Tenant |
	| Bachelor's of Science | Economics   | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | 1      |
	When I submit a request to create a program
	And I submit another request to create another program
	| Name              | Description | OrganizationId | Tenant |
	| Bachelor's of Art | Philosophy  | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | 1      |
	And I request to get all programs
	Then my program is returned

Scenario Template: Return 404 when program id is not found
	When I request a program id that does not exist
	Then I should get the expected status code <Status>

	Examples: 
		| Status   |
		| NotFound |

Scenario Template: Unable to create program due to missing info
	When I create a new program with <Name>, <Description>
	Then I should get the expected status code <Status>

	Examples: 
		| Name                  | Description | Status     |
		| Bachelor's of Science |             | BadRequest |
		|                       | Economics   | BadRequest |
