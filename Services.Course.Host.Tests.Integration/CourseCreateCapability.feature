﻿@Api
Feature: CourseCreateCapability
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to
	
@ignore
Scenario Outline: I can not create a course unless I have permission to do so.
	Given the following organizations exist
	| Name      | Description | ParentOrganization |
	| OrgTop    | Top         |                    |
	| OrgMiddle | Middle      | OrgTop             |
	And I create the following roles
	| Name  | Organization |
	| Role1 | OrgTop       |
	And I give capability <Capability> to role "Role1"
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