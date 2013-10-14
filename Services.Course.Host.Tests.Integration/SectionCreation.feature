﻿@Api
Feature: Create Section

Background: 
	Given I have the following capabilities
		| Capability    |
		| CourseCreate  |
		| CoursePublish |
		| CourseView    |
 	And the following organizations exist
		| Name |
		| COB  |
	And I have the following courses
		| Name     | Code | Description       | OrganizationName |
		| Math 101 | M101 | Basic mathematics | COB              |

Scenario: Cannot create a section from a course that is not published
	When I create the following sections
		| CourseName | Name     | Code        | StartDate | EndDate   |
		| Math 101   | Math 334 | MATH334.ABC | 2/15/2014 | 6/15/2014 |
	Then I get 'BadRequest' response

Scenario: Can create a section from a course that is published
	When I publish the following courses
         | Name     | Note      |
         | Math 101 | published |
	And the section service returns 'Created'
	And I create the following sections
		| CourseName | Name     | Code        | StartDate | EndDate   |
		| Math 101   | Math 334 | MATH334.ABC | 2/15/2014 | 6/15/2014 |
	Then I get 'Created' response

Scenario: Create a section from a course returns status of section service
	When I publish the following courses
         | Name     | Note      |
         | Math 101 | published |
	And the section service returns 'Forbidden'
	And I create the following sections
		| CourseName | Name     | Code        | StartDate | EndDate   |
		| Math 101   | Math 334 | MATH334.ABC | 2/15/2014 | 6/15/2014 |
	Then I get 'Forbidden' response