@Api
Feature: ProgramAssociation
	In order to add course to programs
	As a course builder
	I want to associate course to programs

Background:
	And the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	| EditProgram   |
	| ViewProgram   |
	And I have the following programs
	| Name                | Description | ProgramType | OrganizationName |
	| Bachelor of Art     | BA Program  | BA          | COB              |
	| Bachelor of Science | BS program  | BS          | COB              |
	And I have the following courses
	| Name           | Code   | Description    | OrganizationName |
	| English 101    | ENG101 | English 101    | COB              |
	| Psychology 101 | PSY101 | Psychology 101 | COB              |
	| Econ 100       | E100   | Macroeconomics | COB              |
	| Econ 400       | E400   | Microeconomics | COB              |

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
	| Course Name    | Code   | Description    | 
	| English 101    | ENG101 | English 101    |
	| Psychology 101 | PSY101 | Psychology 101 |

Scenario: Verify a course version can be created from a previously published version with prerequisites
	When I associate 'Econ 100' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	And I publish the following courses
	| Name     | Note   |
	| Econ 100 | a note |
	And I associate 'Econ 400' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	And I add the following prerequisites to 'Econ 400'
	| Name     | 
	| Econ 100 | 
	And I publish the following courses
	| Name     | Note   |
	| Econ 400 | a note |
	And I create a new version of 'Econ 400' course named 'Econ 400 v1.0.0.1' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.1 |
	Then the course 'Econ 400 v1.0.0.1' should have the following prerequisites
	| Name     | 
	| Econ 100 | 

Scenario: Search for programs with courses
	When I associate 'English 101' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	And I associate 'Psychology 101' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	Then the organization 'COB' has the following programs
	| Name                | Description | ProgramType | OrganizationName |
	| Bachelor of Art     | BA Program  | BA          | COB              |
	| Bachelor of Science | BS program  | BS          | COB              |



