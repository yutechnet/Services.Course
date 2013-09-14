@Api
Feature: CourseAuthorization
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to
@ignore
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
	When I create a course <course> under organization <OrganizationCreatedAttempt>
	Then I get '<StatusCode>' response
Examples:
| course | Capability    | OrganizationAssignedTo | OrganizationCreatedAttempt | StatusCode |
| eng101 | CourseCreate  | OrgTop                 | OrgTop                     | Created    |
| eng101 | CoursePublish | OrgTop                 | OrgTop                     | Forbidden  |
| eng101 |               | OrgTop                 | OrgTop                     | Forbidden  |
| eng101 | CourseCreate  | OrgTop                 | OrgMiddle                  | Created    |
| eng101 | CourseCreate  | OrgMiddle              | OrgTop                     | Forbidden  |

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
	When I create a course eng101 under organization OrgTop
	Then I get 'Unauthorized' response

@ignore
Scenario: Create a course as a guest
	Given That I am guest
	When I submit an authorized creation request
	Then I should get a failure response



@ignore
Scenario Outline: I can not view a course unless I have permission to do so.
	Given I am user "TestUser3"
	And the following organizations exist
	| Name      | Description | ParentOrganization |
	| OrgTop    | Top         |                    |
	| OrgMiddle | Middle      | OrgTop             |
	And I create the following roles
	| Name                    | Organization | Capabilities    |
	| CreateCourseRole        | OrgTop       | CourseCreate    |
	| ViewOrNothingCourseRole | <OrgLevel>   | <OrgCapability> |
	And I give the user role "CreateCourseRole" for organization OrgTop
	And I give the user role "ViewCourseRole" for organization <OrgLevel>
	And I create a course 'eng101' under organization 'OrgMiddle'
	#And I create a course 'math101' under organization 'OrgMiddle'
	#When I view 'eng101' course 
	Then I get '<StatusCode>' response
Examples:
| OrgCapability | OrgLevel  | ObjectCapability | ObjectAssignedTo | CourseAssignedCapability | StatusCode | Description                             |
|               |           | CourseView       | eng101           | eng101                   | Success    | #No org level, permission at object     |
| CourseView    | OrgMiddle |                  |                  |                          | Success    | #Org level permission, no object level  |
| CourseView    | OrgTop    |                  |                  |                          | Success    | #parent org level perm, no object level |
| CourseView    | OrgTop    | CourseView       | eng101           |                          | Success    | #org level and object level permission  |
|               |           |                  |                  |                          | Fail       | #no permissions                         |