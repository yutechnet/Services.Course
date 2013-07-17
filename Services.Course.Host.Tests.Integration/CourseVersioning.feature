@Api
Feature: CourseVersioning
	In order to allow continuous enhancement of course
	As a course builder
	I want to version the course

Background: 
	Given I create the following course
	| Field          | Value                         |
	| Name           | English 1010                  |
	| Code           | ENG101                        |
	| Description    | Ranji's awesome English class |
	| TenantId       | 1                             |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Create a default version
	When I retrieve 'English 1010' course
	Then the course should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English class |
	| VersionNumber | 1.0.0.0                       |

Scenario: Edit a course version
	When I update 'English 1010' course with the following info
	| Field       | Value                          |
	| Name        | English 10101                  |
	| Code        | ENG101                         |
	| Description | Ranji's terrible English class |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then the course 'English 1010' should have the following info
	| Field         | Value                          |
	| Name          | English 10101                  |
	| Code          | ENG101                         |
	| Description   | Ranji's terrible English class |
	| VersionNumber | 1.0.0.0                        |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Publish a course version
	When I publish 'English 1010' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English class |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | true                          |
	| PublishNote   | Blah blah                     |

Scenario: Published version cannot be modified
	Given I publish 'English 1010' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I update 'English 1010' course with the following info
	| Field       | Value                          |
	| Name        | English 10101                  |
	| Code        | ENG101                         |
	| Description | Johns's terrible English class |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then I get 'Forbidden' response

Scenario: Published version cannot be deleted
	Given I publish 'English 1010' course with the following info
	| Field       | Value     |
	| PublishNote | Blah blah |
	When I delete 'English 1010' course
	Then I get 'Forbidden' response

Scenario: Create a course version from a previously-published version
	Given I publish 'English 1010' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I create a new version of 'English 1010' course with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English class |
	| VersionNumber | 2.0a                          |
	| IsPublished   | false                         |

Scenario: Cannot publish the same version twice
	Given I publish 'English 1010' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I create a new version of 'English 1010' course with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'BadRequest' response

Scenario: Cannot create a version off non-existing version
	When I create a new version of 'RandomCourse' course with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'NotFound' response

Scenario: Cannot publish without a version
	When I create a course without a version
	Then I get 'BadRequest' response
