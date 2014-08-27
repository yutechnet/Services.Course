@Api
Feature: SectionCreation

Background: 
	Given I have the following capabilities
		| Capability    |
		| CourseCreate  |
		| CoursePublish |
		| CourseView    |
	And I have the following assets
		| Name   |
		| asset1 |
		| asset2 |
		| asset3 |
	And Published the following assets
		| Name   | PublishNote |
		| asset1 | published   |
		| asset2 | published   |
		| asset3 | published   |
 	And the following organizations exist
		| Name |
		| COB  |
	And I have the following courses
		| Name     | Code | Description       | OrganizationName | Credit | MetaData   | ExtensionAssets |
		| Math 101 | M101 | Basic mathematics | COB              | 12     |	{someData} | asset1,asset2   |

Scenario: Cannot create a section from a course that is not published
	When I create the following sections
		| CourseName | Name     | CourseCode  | SectionCode            | StartDate | EndDate   |
		| Math 101   | Math 334 | MATH334.ABC | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2014 |
	Then I get 'BadRequest' response

Scenario: Can create a section from a course that is published
	When I publish the following courses
         | Name     | Note      |
         | Math 101 | published |
	And The section service returns 'Created'
	And I create the following sections
		| CourseName | Name     |  CourseCode  | SectionCode            | StartDate | EndDate   |
		| Math 101   | Math 334 |  MATH334.ABC | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2014 |
	Then I get 'Created' response

Scenario: Create a section from a course returns status of section service
	When I publish the following courses
         | Name     | Note      |
         | Math 101 | published |
	And The section service returns 'Forbidden'
	And I create the following sections
		| CourseName | Name     |  CourseCode  | SectionCode            |  StartDate | EndDate   |
		| Math 101   | Math 334 |  MATH334.ABC | MATH334.ABCSectionCode |  2/15/2014 | 6/15/2014 |
	Then I get 'Forbidden' response

Scenario: Cannot create a section from a deactivated course
	When I publish the following courses
         | Name     | Note      |
         | Math 101 | published |
	When I deactivate the course 'Math 101'
	And I create the following sections
		| CourseName | Name     |  CourseCode  | SectionCode            | StartDate | EndDate   |
		| Math 101   | Math 334 |  MATH334.ABC | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2014 |
	Then I get 'BadRequest' response

Scenario: Can create a section by course code from a course that is published and course code is unique
	Given I have the following courses for unique code
		| Name     | Code | Description       | OrganizationName | Credit | MetaData   | ExtensionAssets |
		| Math 103 | M103 | Basic mathematics | COB              | 12     |	{someData} | asset1,asset2   |
	When I publish the following courses
        | Name     | Note      |
        | Math 103 | published |
	And The section service returns 'Created'
	And I create the following sections by course code
		| CourseName | Name     | CourseCode | SectionCode            | StartDate | EndDate   |
		| Math 103   | Math 334 | M103       | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2015 |
	Then I get 'Created' response

Scenario: Cannot create a section by course code from a course that is not published
	Given I have the following courses for unique code
		| Name     | Code | Description       | OrganizationName | Credit | MetaData   | ExtensionAssets |
		| Math 103 | M103 | Basic mathematics | COB              | 12     |	{someData} | asset1,asset2   |
	When I create the following sections by course code
		| CourseName | Name     | CourseCode | SectionCode            | StartDate | EndDate   |
		| Math 103   | Math 334 | M103       | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2014 |
	Then I get 'BadRequest' response

Scenario: Create a section by course code from a course returns status of section service
	Given I have the following courses for unique code
		| Name     | Code | Description       | OrganizationName | Credit | MetaData   | ExtensionAssets |
		| Math 103 | M103 | Basic mathematics | COB              | 12     |	{someData} | asset1,asset2   |
	When I publish the following courses
        | Name     | Note      |
        | Math 103 | published |
	And The section service returns 'Forbidden'
	And I create the following sections by course code
		| CourseName | Name     | CourseCode | SectionCode            | StartDate | EndDate   |
		| Math 103   | Math 334 | M103       | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2014 |
	Then I get 'Forbidden' response

Scenario: Cannot create a section by course code from a deactivated course
	Given I have the following courses for unique code
		| Name     | Code | Description       | OrganizationName | Credit | MetaData   | ExtensionAssets |
		| Math 103 | M103 | Basic mathematics | COB              | 12     |	{someData} | asset1,asset2   |
	When I publish the following courses
        | Name     | Note      |
        | Math 103 | published |
	When I deactivate the course 'Math 103'
	And I create the following sections by course code
		| CourseName | Name     | CourseCode | SectionCode            | StartDate | EndDate   |
		| Math 103   | Math 334 | M103       | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2014 |
	Then I get 'BadRequest' response

Scenario: Cannot create a section by course code that course code is not unique
	Given I have the following courses for unique code
		| Name     | Code | Description       | OrganizationName | Credit | MetaData   | ExtensionAssets |
		| Math 103 | M103 | Basic mathematics | COB              | 12     | {someData} | asset1,asset2   |
		| Math 104 | M103 | Basic mathematics | COB              | 10     | {someData} | asset1          |
	When I publish the following courses
        | Name     | Note      |
        | Math 103 | published |
	And I create the following sections by course code
		| CourseName | Name     | CourseCode | SectionCode            | StartDate | EndDate   |
		| Math 103   | Math 334 | M103       | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2015 |
	Then I get 'BadRequest' response

Scenario: Cannot create a section by course code that course code is not exist
	When I create the following sections by course code
		| CourseName | Name     | CourseCode | SectionCode            | StartDate | EndDate   |
		| Math 101   | Math 334 | M103       | MATH334.ABCSectionCode | 2/15/2014 | 6/15/2015 |
	Then I get 'NotFound' response
