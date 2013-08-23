@Api
Feature: ProgramAssociation
	In order to add course to programs
	As a course builder
	I want to associate course to programs

Background: 
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationName |
	| Bachelor of Art     | BA Program  | BA          | Default          |
	| Bachelor of Science | BS program  | BS          | Default          |
	And I have the following courses
	| Name           | Code   | Description    | OrganizationName |
	| English 101    | ENG101 | English 101    | Default          |
	| Psychology 101 | PSY101 | Psychology 101 | Default          |

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
	When I associate 'English 101' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	Then the course 'English 101' includes the following program information
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |

Scenario: Remove the course from the program
	When I associate 'English 101' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	And I remove 'English 101' course from 'Bachelor of Art'
	Then the course 'English 101' includes 'Bachelor of Science' program association

Scenario: Get program with all courses that belong to it
	When I associate 'English 101' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	And I associate 'Psychology 101' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	Then the program 'Bachelor of Art' include the following course information
	| Course Name    |
	| English 101    |
	| Psychology 101 |
