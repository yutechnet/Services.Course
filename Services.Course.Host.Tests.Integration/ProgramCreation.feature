@Api
Feature: ProgramCreation
	In order to publish a program
	As a program builder
	I want to create, edit and delete a program

Scenario: Create a basic program
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	Then the program 'Bachelor of Science' contains
	| Field                  | Value                                |
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |

Scenario: Modify a program
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	When I modify the program 'Bachelor of Science' info to reflect the following
	| Field                  | Value                                |
	| Name                   | Bachelor of Arts                     |
	| Description            | English                              |
	| ProgramType            | BA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one update               |
	Then the program 'Bachelor of Science' contains
	| Field                  | Value                                |
	| Name                   | Bachelor of Arts                     |
	| Description            | English                              |
	| ProgramType            | BA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one update               |

Scenario: Delete a program
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationName |
	| Bachelor of Science | Economics   | BA          | Default          |
	When I delete the program 'Bachelor of Science'
	And I get the program 'Bachelor of Science'
	Then I get 'NotFound' response

Scenario: Get all programs
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	| Bachelor of Arts    | Philosophy  | AA          | Default          |                        |
	Then I have the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	| Bachelor of Arts    | Philosophy  | AA          | Default          |                        |

Scenario: Unable to create program due to missing info
	When I attempt to create the following programs
	| Name                | Description | ProgramType | OrganizationName |
	| Bachelor of Science |             | MA          | Default          |
	|                     | Economics   | MA          | Default          |
	Then I get the following responses
	| StatusCode |
	| BadRequest |
	| BadRequest |

Scenario: Search for programs
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationName |
	| Bachelor of Science | Economics   | MA          | Default          |
	| Bachelor of Art     | Art program | BA          | Default          |
	Then the organization with 'E2DF063D-E2A1-4F83-9BE0-218EC676C05F' id has the following programs
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Science | Economics   | MA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Bachelor of Art     | Art program | BA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
