@Api
Feature: Course Management
	In order to publish a course
	As a course builder
	I want to create, edit and delete a course

Scenario: Create a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	When I submit a creation request
	Then I should get a success confirmation message

Scenario: Return course by course name
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	When I submit a creation request
	Then I should get a success confirmation message
	And I can retrieve the course by course name

Scenario: Return course by course code
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	When I submit a creation request
	Then I should get a success confirmation message
	And I can retrieve the course by course code

Scenario: Return 404 when course name is not found
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	When I submit a creation request
	And I request a course name that does not exist
	Then I should get a not found message returned

Scenario: Edit a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	When I submit a creation request
	And I change the info to reflect the following:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	Then I should get a success confirmation message
	And my course info is changed

Scenario: Delete a course
	Given I have an existing course with following info:
	 | Name        | Code   | Description                   | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	 | English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	And I delete this course
	Then I should get a success confirmation message
	And my course no longer exists

Scenario Template: Can create a course with same name or code
	Given I have an existing course with following info:
	 | Name        | Code   | Description                   | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	 | English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	When I create a new course with <Name>, <Code>, <Description>
	And I submit a creation request
	Then I should get the status code <Status>

	Examples: 
		 | Name           | Code   | Description                             | Status  |
		 | Psychology 101 | PSY102 | Amro's another awesome Psychology class | Created |
		 | Psychology 102 | PSY101 | Amro's another awesome Psychology class | Created |

Scenario Template: cannot create a course with missing data
	When I create a new course with <Name>, <Code>, <Description>
	And I submit a creation request
	Then I should get the status code <Status>

	Examples: 
		 | Name           | Code   | Description                             | Status     |
		 | Psychology 101 |        | Amro's another awesome Psychology class | BadRequest |
		 |                | PSY101 | Amro's another awesome Psychology class | BadRequest |
		 | Physcology 103 | PSY103 |                                         | Created    |

Scenario: Return course by partial name
	Given I have existing courses with following info:
		 | Name             | Code     | Description                           | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
		 | English 101      | ENGL101  | Learn to read and write               | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
		 | Engineering 200  | ENG200   | If you build it, they will come       | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
		 | English 220      | ENGL220  | Essays                                | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
		 | Philosophy 100   | PHIL100  | To be, or not to be                   | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
		 | Philanthropy 101 | PHILA101 | Don't be greedy                       | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
		 | Chemistry 350    | CHEM350  | Periodic table of elements to the max | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
	Then the course name counts are as follows:
		| Operation  | Argument          | Count |
		| Eq         | Engineering%20200 | 1     |
		| StartsWith | Eng               | 3     |
		| StartsWith | Phil              | 2     |
		| StartsWith | Engl              | 2     |
		| StartsWith | Philo             | 1     |
		| StartsWith | MATH              | 0     |
		| StartsWith | Chemistry         | 1     |
		| StartsWith | En*               | 0     |
		| StartsWith | \                 | 0     |
		| StartsWith | \\                | 0     |
		| StartsWith | ''E''             | 0     |
		| StartsWith | C*%23             | 0     |
		| StartsWith | A%26              | 0     |
	Then the course count is atleast '6' when search term is ''

Scenario: Add organization id to a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId                       |  CourseType  | IsTemplate |
	| English 101 | ENG101 | Ranji's awesome English Class | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 |  Traditional | false      |
	When I submit a creation request
	Then the organization id is returned as part of the request