@Api
Feature: ProgramAssociation
	In order to add course to programs
	As a course builder
	I want to associate course to programs

Background: 
	Given the following programs exist:
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Art     | BA Program  | BA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Bachelor of Science | BS program  | BS          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	And the following courses exist:
	| Name           | Code   | Description    |
	| English 101    | ENG101 | English 101    |
	| Psychology 101 | PSY101 | Psychology 101 |

Scenario Template: Associate a course with a program
	When I associate '<Course Name>' course with '<Program Name>' program
	Then the course '<Course Name>' includes '<Program Name>' program association

	Examples: 
	| Course Name    | Program Name        |
	| English 101    | Bachelor of Art     |
	| Psychology 101 | Bachelor of Science |
	| English 101    | Bachelor of Science |
	| Psychology 101 | Bachelor of Art     |

Scenario: Associate a course with multiple programs
	When I associate 'English 101' course with the following programs:
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	Then the course 'English 101' includes the following program information:
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |

Scenario: Remove the course from the program
	Given I associate 'English 101' course with the following programs:
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	When I remove 'English 101' course from 'Bachelor of Art'
	Then the course 'English 101' includes 'Bachelor of Science' program association
