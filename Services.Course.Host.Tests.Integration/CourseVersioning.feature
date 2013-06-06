@Api
Feature: CourseVersioning
	In order to allow continuous enhancement of course
	As a course builder
	I want to version the course

Scenario: Create a default version
	Given I create the following course
	| Field       | Value                         |
	| Name        | English 1010                  |
	| Code        | ENG101                        |
	| Description | Ranji's awesome English class |
	| TenantId    | 1                             |
	When I retrieve 'ENG101' course
	Then the course should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English class |
	| VersionNumber | 1.0                           |

