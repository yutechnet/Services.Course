@Api
Feature: CourseDeactivation

Background:
	Given the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	| EditCourse    |
	And I have the following courses   
	| Name           | Code   | Description                   | OrganizationName | CourseType  | IsTemplate |
	| English 1010   | ENG101 | Ranji's awesome English Class | COB              | Traditional | false      |

Scenario: Course is activated by default
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | false                         |
	| PublishNote   |                               |
	| IsActivated   | true                          |

Scenario: Cannot deactivate unpublished course
	When I deactivate the course 'English 1010'
	Then I get 'BadRequest' response

Scenario: Can deactivate published course
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	When I deactivate the course 'English 1010'
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | true                          |
	| PublishNote   | Blah blah                     |
	| IsActivated   | false                         |

Scenario: Can activate deactivated course
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	When I deactivate the course 'English 1010'
	When I activate the course 'English 1010'
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | true                          |
	| PublishNote   | Blah blah                     |
	| IsActivated   | true                          |

