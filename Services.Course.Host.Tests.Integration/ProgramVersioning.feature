@Api
Feature: ProgramVersioning
	In order to allow continuous enhancement of Program
	As a Program builder
	I want to version the Program

Background:
	Given I have the following capabilities
	| Capability    |
	| EditProgram   |
	| ViewProgram   |
	And I have the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |

Scenario: Create a default version
	Then the program 'Bachelor of Science' contains
	| Field                  | Value                                |
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |
	| VersionNumber          | 1.0.0.0                              |

Scenario: Publish a Program version
	When I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	Then the program 'Bachelor of Science' contains
	| Field                  | Value                                |
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |
	| VersionNumber          | 1.0.0.0                              |
	| IsPublished            | true                                 |
	| PublishNote            | Blah blah                            |

Scenario: Published version cannot be modified
	When I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	And I modify the program 'Bachelor of Science' info to reflect the following
	| Field                  | Value                                |
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |
	Then I get 'BadRequest' response

Scenario: Published version cannot be deleted
	When I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	And I delete the program 'Bachelor of Science'
	Then I get 'BadRequest' response

Scenario: Create a Program version from a previously-published version
	When I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	And I create a new version of 'Bachelor of Science' program named 'Bachelor of Science v2' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the program 'Bachelor of Science v2' contains
	| Field                  | Value                                |
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |
	| VersionNumber          | 2.0a                                 |
	| IsPublished            | false                                |

Scenario: Create a Program version from a previously-published version then publish it
	When I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	And I create a new version of 'Bachelor of Science' program named 'Bachelor of Science v2' with the following info
	| Field         | Value    |
	| VersionNumber | 1.0.0.1  |
	And I publish the following programs
	| Name                   | Note      |
	| Bachelor of Science v2 | Blah blah |
	Then the program 'Bachelor of Science v2' contains
	| Field                  | Value                                |
	| Name                   | Bachelor of Science                  |
	| Description            | Economics                            |
	| ProgramType            | MA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one                      |
	| VersionNumber          | 1.0.0.1                              |
	| IsPublished            | true                                 |

	
Scenario: Cannot publish the same version twice
	When I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	And I create a new version of 'Bachelor of Science' program named 'Bachelor of Science v2' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'BadRequest' response

Scenario: Cannot publish without a version
	When I create a Program without a version
	Then I get 'BadRequest' response

