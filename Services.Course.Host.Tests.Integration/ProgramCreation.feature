﻿@Api
Feature: ProgramCreation
	In order to publish a program
	As a program builder
	I want to create, edit and delete a program

Scenario: Create a basic program
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Science | Economics   | MA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then the program 'Bachelor of Science' contains
	| Field          | Value                                |
	| Name           | Bachelor of Science                  |
	| Description    | Economics                            |
	| ProgramType    | MA                                   |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Modify a program
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Science | Economics   | MA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	When I modify the program 'Bachelor of Science' info to reflect the following
	| Field          | Value                                |
	| Name           | Bachelor of Arts                     |
	| Description    | English                              |
	| ProgramType    | BA                                   |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then the program 'Bachelor of Science' contains
	| Field          | Value                                |
	| Name           | Bachelor of Arts                     |
	| Description    | English                              |
	| ProgramType    | BA                                   |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Delete a program
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Science | Economics   | BA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	When I delete the program 'Bachelor of Science'
	And I get the program 'Bachelor of Science'
	Then I get 'NotFound' response

Scenario: Get all programs
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Science | Economics   | MA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Bachelor of Arts    | Philosophy  | AA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then I have the following programs
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Science | Economics   | MA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Bachelor of Arts    | Philosophy  | AA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Unable to create program due to missing info
	When I attempt to create the following programs
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Science |             | MA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	|                     | Economics   | MA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then I get the following responses
	| StatusCode |
	| BadRequest |
	| BadRequest |
