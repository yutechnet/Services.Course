﻿@Api
Feature: CourseCreateCapability
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to
	
#@ignore
Scenario Outline: I can not create a course unless I have permission to do so.
	Given I am user "TestUser3"
	And the following organizations exist
	| Name      | Description | ParentOrganization |
	| OrgTop    | Top         |                    |
	| OrgMiddle | Middle      | OrgTop             |
	And I create the following roles
	| Name  | Organization | Capabilities |
	| Role1 | OrgTop       | <Capability> |
	And I give the user role "Role1" for organization <OrganizationAssignedTo>
	When I create a course under organization <OrganizationCreatedAttempt>
	Then I get <StatusCode> response
Examples:
| Capability    | OrganizationAssignedTo | OrganizationCreatedAttempt | StatusCode |
| CourseCreate  | OrgTop                 | OrgTop                     | Created    |
| CoursePublish | OrgTop                 | OrgTop                     | Forbidden  |
|               | OrgTop                 | OrgTop                     | Forbidden  |
| CourseCreate  | OrgTop                 | OrgMiddle                  | Created    |
| CourseCreate  | OrgMiddle              | OrgTop                     | Forbidden  |

#This is ignored pending DE377 : https://rally1.rallydev.com/#/10482122379ud/detail/defect/13871086436
@ignore
Scenario: I can not create a course when capabilities have been removed.
	Given I am user "TestUser3"
	And the following organizations exist
	| Name      | Description | ParentOrganization |
	| OrgTop    | Top         |                    |
	And I create the following roles
	| Name  | Organization | Capabilities |
	| Role1 | OrgTop       | CourseCreate |
	And I give the user role "Role1" for organization OrgTop
	And I update the role "Role1" with capabilities ""
	When I create a course under organization OrgTop
	Then I get "Unauthorized" response