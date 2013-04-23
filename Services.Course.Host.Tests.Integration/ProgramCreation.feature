Feature: ProgramCreation
	In order to publish a program
	As a program builder
	I want to create, edit and delete a program

Scenario: Create a basic program
	Given I have a program with following info:
	| Name                  | Description | Tenant | 
	| Bachelor's of Science | Economics   | 1      |
	When I submit a request to create a program
	Then the operation is successful

Scenario: Modify a program
	Given I have a program with following info:
	| Name                  | Description | Tenant | 
	| Bachelor's of Science | Economics   | 1      |
	When I submit a request to create a program
	And I modify the program info to reflect the following:
	| Name              | Description | Tenant |
	| Bachelor's of Art | English     | 1      |
	Then the operation is successful
	And my program information is changed
	And the program updates are reflected in all tenants

Scenario: Delete a program
	Given I have an existing program with following info:
	| Name                  | Description | Tenant | 
	| Bachelor's of Science | Economics   | 1      |
	When I delete the program
	Then the program no longer exists

Scenario Template: Unable to create program due to missing info
	When I create a new program with <Name>, <Description>, <Tenant>
	When I submit a request to create a program
	Then I should get the expected status code <Status>

	Examples: 
		| Name                  | Description | Status     |
		| Bachelor's of Science |             | BadRequest |
		|                       | Economics   | BadRequest |

Scenario: Associate a program with multiple tenants
	Given I have an existing program with following info:
	| Name                  | Description | Tenant | 
	| Bachelor's of Science | Economics   | 1      |
	When I wish to add the same program to another tenant
	Then the operation is successful
