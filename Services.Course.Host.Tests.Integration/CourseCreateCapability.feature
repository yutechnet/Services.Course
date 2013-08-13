﻿@Api
Feature: CourseCreateCapability
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to
	
@ignore
Scenario Outline: I can not create a course unless I have permission to do so.
	And I create an organization "OrgTop" with no parent
	And I create an organization "OrgMiddle" with parent organization "OrgTop"
	And I create a role "Role1"
	And I give capability <Capability> to role "Role1"
	And I give test user role "Role1" for object <OrganizationAssignedTo> of type Organization
	When I create a course under organization <OrganizationCreatedAttempt>
	Then The message returned should be <StatusCode> passed in as a string
Examples:
| Capability    | OrganizationAssignedTo | OrganizationCreatedAttempt | StatusCode   |
| CourseCreate  | OrgTop                 | OrgTop                     | Created      |
| SystemAdmin   | OrgTop                 | OrgTop                     | Created      |
| CoursePublish | OrgTop                 | OrgTop                     | Unauthorized |
|               | OrgTop                 | OrgTop                     | Unauthorized |
| CourseCreate  | OrgTop                 | OrgMiddle                  | Created      |
| CourseCreate  | OrgMiddle              | OrgTop                     | Unauthorized |