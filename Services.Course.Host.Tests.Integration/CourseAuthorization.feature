@Api
Feature: CourseAuthorization
	In order to protect the system
	As a system owner
	I want to make sure that users cannot access resources they are not allowed to

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
	| QADept    | QA Dept     |                    |
	And I create the following roles
	| Name          | Organization    | Capabilities |
	| CourseCreator | OrgTop          | CourseCreate |
	| CourseViewer  | <OrgAssignedTo> | CourseView   |
	And I give the user role "CourseCreator" for organization OrgTop
	And I create a course 'eng101' under organization 'OrgMiddle'
	And I create a course 'math101' under organization 'OrgMiddle'
	And I give the user role "CourseViewer" for organization <OrgAssignedTo>
	And I give the user role "CourseViewer" for object <ObjectAssignedTo>
	When I view 'eng101' course 
	Then I get '<StatusCode>' response
Examples:
| OrgAssignedTo | ObjectAssignedTo | StatusCode | Description                               |
| QADept        | eng101           | OK         | #No org level, permission at object       |
| QADept        | math101          | Forbidden  | #No org level, permission at wrong object |
| OrgMiddle     |                  | OK         | #Org level permission, no object level    |
| OrgTop        |                  | OK         | #parent org level perm, no object level   |
| OrgTop        | eng101           | OK         | #org level and object level permission    |
| QADept        |                  | Forbidden  | #no permissions in right org or obj       |

