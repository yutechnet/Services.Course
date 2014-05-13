@Api
Feature: ProgramAuthorization
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario Outline: I can not create a program unless I have permission to do so.
	Given I have the '<Capability>' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | Created    |
	|             | Forbidden  |

Scenario Outline: I can view a program when I do have permission.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	When I get the program 'Bachelor of Science'
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| ViewProgram | OK         |
	|             | Forbidden  |


Scenario Outline: I can edit a program when I do have permission.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	When I am 'TestUser1'	
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	When I modify the program 'Bachelor of Science' info to reflect the following
	| Field                  | Value                                |
	| Name                   | Bachelor of Arts                     |
	| Description            | English                              |
	| ProgramType            | BA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one update               |
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | OK         |
	|             | Forbidden  |

Scenario Outline: I can delete a program when I do have permission.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	When I am 'TestUser1'	
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	When I delete the program 'Bachelor of Science'
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | NoContent  |
	|             | Forbidden  |
