@Api
Feature: CourseVersioning
	In order to allow continuous enhancement of course
	As a course builder
	I want to version the course

Background: 
	Given I create the following course
	| Field       | Value                         |
	| Name        | English 1010                  |
	| Code        | ENG101                        |
	| Description | Ranji's awesome English class |
	| TenantId    | 1                             |

Scenario: Create a default version
	When I retrieve 'ENG101' course
	Then the course should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English class |
	| VersionNumber | 1.0.0.0                       |

Scenario: Edit a course version
	When I update 'ENG101' course with the following info
	| Field       | Value                          |
	| Name        | English 10101                  |
	| Code        | ENG101                         |
	| Description | Ranji's terrible English class |
	Then the course 'ENG101' should have the following info
	| Field         | Value                          |
	| Name          | English 10101                  |
	| Code          | ENG101                         |
	| Description   | Ranji's terrible English class |
	| VersionNumber | 1.0.0.0                        |

Scenario: Publish a course version
	When I publish 'ENG101' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	Then the course 'ENG101' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English class |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | true                          |
	| PublishNote   | Blah blah                     |

Scenario: Published version cannot be modified
	Given I publish 'ENG101' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I update 'ENG101' course with the following info
	| Field       | Value                          |
	| Name        | English 10101                  |
	| Code        | ENG101                         |
	| Description | Ranji's terrible English class |
	Then I get 'Forbidden' response

Scenario: Published version cannot be deleted
	Given I publish 'ENG101' course with the following info
	| Field       | Value     |
	| PublishNote | Blah blah |
	When I delete 'ENG101' course
	Then I get 'Forbidden' response

Scenario: Create a course version from a previously-published version
	Given I publish 'ENG101' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I create a new version of 'ENG101' course with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the course 'ENG101' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English class |
	| VersionNumber | 2.0a                          |
	| IsPublished   | false                         |

Scenario: Cannot publish the same version twice
	Given I publish 'ENG101' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I create a new version of 'ENG101' course with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'Conflict' response

Scenario: Cannot create a version off non-existing version
	When I create a new version of 'RandomCourse' course with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'NotFound' response

Scenario: Cannot publish without a version
	When I create a course without a version
	Then I get 'BadRequest' response


