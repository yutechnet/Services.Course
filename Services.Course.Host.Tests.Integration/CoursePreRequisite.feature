@Ignore
@Api
Feature: CoursePrerequisites
	As a course creator
	I want to be able to declare the set of courses that need to be accomplished prior to a given course
	So that students can be enrolled in these courses

Background: 
	Given the following courses are published:
	| Name     | Code | Description           | OrganizationId                       | CourseType  | IsTemplate |
	| Math 101 | M101 | Basic mathematics     | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Econ 400 | E400 | Advanced Econometrics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Poly 220 | P220 | Comparative Politics  | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Econ 200 | E200 | Intro to Econometrics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	| Econ 100 | E100 | Macroeconomics        | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
@Ignore
Scenario: Add a course prerequisite
	When I add the following prerequisites to 'Econ 400'
	| Name     | 
	| Econ 200 | 
	| Econ 100 |
	Then the course 'Econ 400' should have the following prerequisites
	| Name     | 
	| Econ 200 | 
	| Econ 100 |
@Ignore
Scenario: Cannot add a prerequisite to a course that is not published
	Given the following courses exist:
		| Name     | Code | Description | OrganizationId                       | CourseType  | IsTemplate |
		| Math 200 | M200 | Calculus    | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	When I add the following prerequisites to 'Math 200'
		| Name     | Code | Description       | OrganizationId                       | CourseType  | IsTemplate |
		| Math 101 | M101 | Basic mathematics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
	Then I get 'BadRequest' response

@Ignore
Scenario: Cannot add a prerequisite to a course using the wrong organization id
@Ignore
Scenario: Verify pre-req course id is the version id