@Api
Feature: ProgramDeactivation

Background:
	Given I have the following capabilities
	| Capability    |
	| EditProgram   |
	| ViewProgram   |
	And I have the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |

Scenario: Program is activated by default
	Then the program 'Bachelor of Science' contains
	| Field                  | Value                                |
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |
	| IsActivated            | true                                 |

Scenario: Cannot deactivate unpublished program
	When I deactivate the program 'Bachelor of Science'
	Then I get 'BadRequest' response

Scenario: Can deactivate published program
	When I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	When I deactivate the program 'Bachelor of Science'
	Then the program 'Bachelor of Science' contains
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |
	| VersionNumber          | 1.0.0.0                              |
	| IsPublished            | true                                 |
	| PublishNote            | Blah blah                            |
	| IsActivated            | false                                |

Scenario: Can activate deactivated program
	When I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	When I deactivate the program 'Bachelor of Science'
	When I activate the program 'Bachelor of Science'
	Then the program 'Bachelor of Science' contains
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |
	| VersionNumber          | 1.0.0.0                              |
	| IsPublished            | true                                 |
	| PublishNote            | Blah blah                            |
	| IsActivated            | true                                 |
