@Api
Feature: CourseVersioning
	In order to allow continuous enhancement of course
	As a course builder
	I want to version the course

Background: 
	Given I have the following courses
	| Name         | Code   | Description                   | OrganizationId                       | CourseType  | IsTemplate |
	| English 1010 | ENG101 | Ranji's awesome English Class | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |

Scenario: Create a default version
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| VersionNumber | 1.0.0.0                       |

Scenario: Edit a course version
	When I update 'English 1010' course with the following info
	| Field       | Value                          |
	| Name        | English 10101                  |
	| Code        | ENG10101                       |
	| Description | Ranji's terrible English Class |
	| IsTemplate  | true                           |
	Then the course 'English 1010' should have the following info
	| Field          | Value                                |
	| Name           | English 10101                        |
	| Code           | ENG10101                             |
	| Description    | Ranji's terrible English Class       |
	| VersionNumber  | 1.0.0.0                              |
	| OrganizationId | C3885307-BDAD-480F-8E7C-51DFE5D80387 |
	| IsTemplate     | true                                 |

Scenario: Publish a course version
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | true                          |
	| PublishNote   | Blah blah                     |

Scenario: Published version cannot be modified
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I update 'English 1010' course with the following info
	| Field          | Value                                |
	| Name           | English 10101                        |
	| Code           | ENG101                               |
	| Description    | Johns's terrible English Class       |
	| OrganizationId | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then I get 'Forbidden' response

Scenario: Published version cannot be deleted
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I delete 'English 1010' course
	Then I get 'Forbidden' response

Scenario: Create a course version from a previously-published version
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v2' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the course 'English 1010 v2' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| VersionNumber | 2.0a                          |
	| IsPublished   | false                         |

Scenario: Cannot publish the same version twice
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v2' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'BadRequest' response

Scenario: Cannot publish without a version
	When I create a course without a version
	Then I get 'BadRequest' response
