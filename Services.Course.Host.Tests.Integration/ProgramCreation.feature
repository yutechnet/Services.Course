﻿@Api
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
	And my program information is as follows:
	| Name                  | Description | Tenant | 
	| Bachelor's of Science | Economics   | 1      |

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

Scenario: Delete a program
	Given I have an existing program with following info:
	| Name                  | Description | Tenant | 
	| Bachelor's of Science | Economics   | 1      |
	When I delete the program
	Then the program no longer exists

Scenario: Get all programs
	Given I have a program with following info:
	| Name                  | Description | Tenant |
	| Bachelor's of Science | Economics   | 1      |
	When I submit a request to create a program
	And I submit another request to create another program
	| Name              | Description | Tenant |
	| Bachelor's of Art | Philosophy  | 1      |
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
	When I submit a request to create a program
	Then I should get the expected status code <Status>

	Examples: 
		| Name                  | Description | Status     |
		| Bachelor's of Science |             | BadRequest |
		|                       | Economics   | BadRequest |

#Scenario: Associate a program with multiple tenants
#	Given I have an existing program with following info:
#	| Name                  | Description | Tenant | 
#	| Bachelor's of Science | Economics   | 1      |
#	When I wish to add the same program to another tenant
#	Then the operation is successful