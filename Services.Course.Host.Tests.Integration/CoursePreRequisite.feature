@Api
Feature: CoursePrerequisites
	As a course creator
	I want to be able to declare the set of courses that need to be accomplished prior to a given course
	So that students can be enrolled in these courses

Background: 
	Given I have the following courses
	| Name     | Code | Description           | OrganizationId                       | CourseType  | IsTemplate |
	| Econ 100 | E100 | Macroeconomics        | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Econ 200 | E200 | Microeconomics        | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Econ 250 | E100 | Intro to Econometrics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Econ 300 | E100 | Applied Econometrics  | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Econ 350 | E350 | Labor Economics       | 7DB51BC8-D0CD-44C5-9C67-C64021068B03 | Traditional | False      |
	| Econ 400 | E400 | Advanced Econometrics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Econ 450 | E100 | Financial Economics   | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Math 101 | M101 | Basic mathematics     | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Math 150 | M101 | Geometry              | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Math 200 | M200 | Calculus              | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |

Scenario: Add a course prerequisite
	When I publish the following courses
	| Name     | Note   |
	| Econ 100 | a note |
	| Econ 200 | a note |
	And I add the following prerequisites to 'Econ 400'
	| Name     | 
	| Econ 100 | 
	| Econ 200 |
	Then the course 'Econ 400' should have the following prerequisites
	| Name     | 
	| Econ 100 | 
	| Econ 200 |

Scenario: Remove a course from the prerequisite list
	When I publish the following courses
	| Name     | Note   |
	| Econ 100 | a note |
	| Econ 200 | a note |
	| Econ 300 | a note |
	| Econ 350 | a note |
	And I add the following prerequisites to 'Econ 450'
	| Name     | 
	| Econ 100 | 
	| Econ 200 |
	And I add the following prerequisites to 'Econ 450'
	| Name     | 
	| Econ 300 | 
	| Econ 350 |
	Then the course 'Econ 450' should have the following prerequisites
	| Name     | 
	| Econ 300 | 
	| Econ 350 |

Scenario: Cannot add a prerequisite to a course that is published
	When I publish the following courses
	| Name     | Note   |
	| Econ 300 | a note |
	And I add the following prerequisites to 'Econ 300'
	| Name     |
	| Math 101 |
	Then I get 'Forbidden' response

Scenario: Cannot add an unpublished course ass a prerequisite
	When I add the following prerequisites to 'Math 200'
	| Name     |
	| Math 150 |
	Then I get 'Forbidden' response