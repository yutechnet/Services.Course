﻿@Api
Feature: CourseCreateCapability
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to
	
#@ignore
Scenario Outline: I can not create a course unless I have permission to do so.
	Given the following organizations exist
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
| Capability    | OrganizationAssignedTo | OrganizationCreatedAttempt | StatusCode   |
| CourseCreate  | OrgTop                 | OrgTop                     | Created      |
| SystemAdmin   | OrgTop                 | OrgTop                     | Created      |
| CoursePublish | OrgTop                 | OrgTop                     | Unauthorized |
|               | OrgTop                 | OrgTop                     | Unauthorized |
| CourseCreate  | OrgTop                 | OrgMiddle                  | Created      |
| CourseCreate  | OrgMiddle              | OrgTop                     | Unauthorized |

#@ignore
Scenario: I can not create a course when capabilities have been removed.
	Given the following organizations exist
	| Name      | Description | ParentOrganization |
	| OrgTop    | Top         |                    |
	And I create the following roles
	| Name  | Organization | Capabilities |
	| Role1 | OrgTop       | CourseCreate |
	And I give the user role "Role1" for organization "OrgTop"
	And I update the role "Role1" with capabilities ""
	When I create a course under organization "OrgTop"
	Then I get "Unauthorized" response