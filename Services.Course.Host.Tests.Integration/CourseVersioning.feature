﻿@Api
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
	| VersionNumber | 1.0.0.0                       |

Scenario: Edit a course version
	Given I create the following course
	| Field       | Value                         |
	| Name        | English 1010                  |
	| Code        | ENG101                        |
	| Description | Ranji's awesome English class |
	| TenantId    | 1                             |
	When I update 'ENG101' course with the following info
	| Field       | Value                          |
	| Name        | English 10101                  |
	| Code        | ENG101                         |
	| Description | Ranji's terrible English class |
	| TenantId    | 1                              |
	Then the course 'ENG101' should have the following info
	| Field         | Value                          |
	| Name          | English 10101                  |
	| Code          | ENG101                         |
	| Description   | Ranji's terrible English class |
	| VersionNumber | 1.0.0.0                        |

Scenario: Publish a course version
	Given I create the following course
	| Field       | Value                         |
	| Name        | English 1010                  |
	| Code        | ENG101                        |
	| Description | Ranji's awesome English class |
	| TenantId    | 1                             |
	When I publish 'ENG101' course with the following info
	| Field       | Value     |
	| PublishNote | Blah blah |
	Then the course 'ENG101' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English class |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | true                          |
	| PublishNote   | Blah blah                     |

Scenario: Published version cannot be modified
	Given I create the following course
	| Field       | Value                         |
	| Name        | English 1010                  |
	| Code        | ENG101                        |
	| Description | Ranji's awesome English class |
	| TenantId    | 1                             |
	And I publish 'ENG101' course with the following info
	| Field       | Value     |
	| PublishNote | Blah blah |
	When I update 'ENG101' course with the following info
	| Field       | Value                          |
	| Name        | English 10101                  |
	| Code        | ENG101                         |
	| Description | Ranji's terrible English class |
	| TenantId    | 1                              |
	Then I get 'Forbidden' response

Scenario: Published version cannot be deleted
	Given I create the following course
	| Field       | Value                         |
	| Name        | English 1010                  |
	| Code        | ENG101                        |
	| Description | Ranji's awesome English class |
	| TenantId    | 1                             |
	And I publish 'ENG101' course with the following info
	| Field | Value     |
	| PublishNote  | Blah blah |
	When I delete 'ENG101' course
	Then I get 'Forbidden' response