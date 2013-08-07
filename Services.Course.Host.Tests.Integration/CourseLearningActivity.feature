@Api
Feature: CourseLearningActivity
	As a course creator
	I would like to specify grading attributes to learning activities
	So that I can turn them into gradebook items

Background: 
Given the following courses and their published status:
	| Name     | Code | Description    | OrganizationId                       | CourseType  | IsTemplate | IsPublished  |
	| Econ 100 | E100 | Macroeconomics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      | NotPublished |
Given I add following course segments to 'Econ 100':
	| Name  | Description                        | Type     | ParentSegment |
	| Week1 | First week is slack time           | TimeSpan |               |

Scenario: Add a learning activity
	When I add the following learning activity:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| 999999   | Assignment 1 | Assignment | True        | False         | 50     | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| 999999   | Assignment 2 | Assignment | True        | False         | 50     | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then my learning activity contains the following:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| 999999   | Assignment 1 | Assignment | True        | False         | 50     | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| 999999   | Assignment 2 | Assignment | True        | False         | 50     | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Update a learning activity NEW
	Given the following learning activity:
	| Field         | Value                                |
	| TenantId      | 999999                               |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| IsGradeable   | true                                 |
	| IsExtraCredit | true                                 |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	When I update 'Discussion 1' learning activity with the following info
	| Field         | Value                                |
	| TenantId      | 999999                               |
	| Name          | Discussion 1                         |
	| Type          | Assignment                           |
	| IsGradeable   | false                                |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then the learning activity 'Discussion 1' should have the following info
	| Field         | Value                                |
	| TenantId      | 999999                               |
	| Name          | Discussion 1                         |
	| Type          | Assignment                           |
	| IsGradeable   | false                                |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Delete a learning activity
	Given the following learning activity:
	| Field         | Value                                |
	| TenantId      | 999999                               |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| IsGradeable   | true                                 |
	| IsExtraCredit | true                                 |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	When I remove "Discussion 1" learning activity
	Then I get 'NotFound' response

Scenario: Cannot add a learning activity to a course that is already published
	Given I publish 'Econ 100' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I add a learning activity to a course that has already been published
	| Field         | Value        |
	| TenantId      | 999999       |
	| Name          | Discussion 1 |
	| Type          | Discussion   |
	| IsGradeable   | true         |
	| IsExtraCredit | true         |
	| Weight        | 100          |
	| MaxPoint      | 20           |
	| ObjectId      | Null         |
	Then I get 'Forbidden' response