@Api
Feature: CoursePrerequisites
	As a course creator
	I want to be able to declare the set of courses that need to be accomplished prior to a given course
	So that students can be enrolled in these courses

Background: 
	And the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	| EditCourse    |
	And I have the following assets
	| Name   |
	| asset1 |
	| asset2 |
	And Published the following assets
	| Name   | PublishNote |
	| asset1 | published   |
	| asset2 | published   |
	And I have the following courses
	| Name     | Code | Description           | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets |
	| Econ 100 | E100 | Macroeconomics        | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Econ 200 | E200 | Microeconomics        | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Econ 250 | E100 | Intro to Econometrics | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Econ 300 | E100 | Applied Econometrics  | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Econ 350 | E350 | Labor Economics       | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Econ 400 | E400 | Advanced Econometrics | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Econ 450 | E100 | Financial Economics   | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Math 101 | M101 | Basic mathematics     | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Math 150 | M101 | Geometry              | COB              | Traditional | False      | {someData} | asset1,asset2   |
	| Math 200 | M200 | Calculus              | COB              | Traditional | False      | {someData} | asset1,asset2   |

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
	Then I get 'BadRequest' response

Scenario: Cannot add an unpublished course ass a prerequisite
	When I add the following prerequisites to 'Math 200'
	| Name     |
	| Math 150 |
	Then I get 'BadRequest' response

Scenario: Verify prerequisites are copied from course template
	Given I have the following course templates
	| Name       | Code          | Description              | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets |
	| Template 1 | TemplateCode1 | My First Course Template | COB              | Traditional | true       |	{someData} | asset1,asset2   |
	When I publish the following courses
	| Name       | Note   |
	| Econ 100   | a note |
	| Econ 200   | a note |
	And I add the following prerequisites to 'Template 1'
	| Name     | 
	| Econ 100 | 
	| Econ 200 |
	And I create a course from the template 'Template 1' with the following
    | Name         | Code        | Description              | OrganizationName | IsTemplate |
    | English 2020 | CourseCode1 | My First Course Template | COB              | false      |
	Then the course 'English 2020' should have the following prerequisites
	| Name     | 
	| Econ 100 | 
	| Econ 200 |

Scenario: Verify prerequisites should be saved when creating a course
	When I publish the following courses
	| Name     | Note   |
	| Econ 100 | a note |
	| Econ 200 | a note |
	And I have an existing course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets                      | Prerequisites     |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB | Econ 100,Econ 200 |
	Then the course 'English 101' should have the following prerequisites
	| Name     | 
	| Econ 100 | 
	| Econ 200 |

Scenario: Verify prerequisites should be saved when editng a course
	When I publish the following courses
	| Name     | Note   |
	| Econ 100 | a note |
	| Econ 200 | a note |
	| Econ 300 | a note |
	| Econ 400 | a note |
	And I have an existing course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets                      | Prerequisites     |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | COB              | Traditional | false      | {someData} | B40CE4F4-434A-4987-80A8-58F795C212EB | Econ 100,Econ 200 |
	And I change the info to reflect the following:
	| Name        | Code   | Description                  | CourseType  | IsTemplate | Credit | MetaData        | ExtensionAssets                      | Prerequisites     |
	| English 101 | ENG101 | John's awesome English Class | Traditional | false      | 10     | {differentData} | B40CE4F4-434A-4987-80A8-58F795C212EB | Econ 300,Econ 400 |
	Then the course 'English 101' should have the following prerequisites
	| Name     | 
	| Econ 300 | 
	| Econ 400 |