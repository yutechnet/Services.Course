@Api
Feature: Course Management
	In order to publish a course
	As a course builder
	I want to create, edit and delete a course

Scenario: Create a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId							| TemplateCourseId						|
	| English 101 | ENG101 | Ranji's awesome English Class | 1		   | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
	When I submit a creation request
	Then I should get a success confirmation message

Scenario: Return course by course name
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId							| TemplateCourseId						|
	| English 101 | ENG101 | Ranji's awesome English Class | 1		   | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
	When I submit a creation request
	Then I should get a success confirmation message
	And I can retrieve the course by course name

Scenario: Return course by course code
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId							| TemplateCourseId						|
	| English 101 | ENG101 | Ranji's awesome English Class | 1		   | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
	When I submit a creation request
	Then I should get a success confirmation message
	And I can retrieve the course by course code

Scenario: Return 404 when course name is not found
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId							| TemplateCourseId						|
	| English 101 | ENG101 | Ranji's awesome English Class | 1		   | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
	When I submit a creation request
	And I request a course name that does not exist
	Then I should get a not found message returned

Scenario: Edit a course
	Given I have a course with following info:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId							| TemplateCourseId						|
	| English 101 | ENG101 | Ranji's awesome English Class | 1		   | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
	When I submit a creation request
	And I change the info to reflect the following:
	| Name        | Code   | Description                   | Tenant Id | OrganizationId							| TemplateCourseId						|
	| English 101 | ENG101 | Ranji's awesome English Class | 1		   | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
	Then I should get a success confirmation message
	And my course info is changed

Scenario: Delete a course
	Given I have an existing course with following info:
	 | Name           | Code   | Description                     | OrganizationId						| TemplateCourseId						|
	 | Psychology 101 | PSY101 | Amro's awesome Psychology class | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
	And I delete this course
	Then I should get a success confirmation message
	And my course no longer exists

Scenario Template: Can create a course with same name or code
	Given I have an existing course with following info:
	 | Name           | Code   | Description                     | OrganizationId						| TemplateCourseId						|
	 | Psychology 101 | PSY101 | Amro's awesome Psychology class | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
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
		 | Name           | Code   | Description                             | Tenant Id | Status     |
		 | Psychology 101 |        | Amro's another awesome Psychology class | 1         | BadRequest |
		 |                | PSY101 | Amro's another awesome Psychology class | 1         | BadRequest |
		 | Physcology 103 | PSY103 |                                         | 1         | Created    |

Scenario: Return course by partial name
	Given I have existing courses with following info:
		 | Name             | Code     | Description                           | Tenant Id | OrganizationId						| TemplateCourseId						|
		 | English 101      | ENGL101  | Learn to read and write               | 1         | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
		 | Engineering 200  | ENG200   | If you build it, they will come       | 1         | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
		 | English 220      | ENGL220  | Essays                                | 1         | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
		 | Philosophy 100   | PHIL100  | To be, or not to be                   | 1         | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
		 | Philanthropy 101 | PHILA101 | Don't be greedy                       | 1         | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
		 | Chemistry 350    | CHEM350  | Periodic table of elements to the max | 1         | C3885307-BDAD-480F-8E7C-51DFE5D80387	| ECB05E38-02A6-4F2F-B020-00AA2F619727	|
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
