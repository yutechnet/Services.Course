@Api
Feature: CourseLearningActivity
	As a course creator
	I would like to specify grading attributes to learning activities
	So that I can turn them into gradebook items

Background: 
Given I have the following courses
	| Name     | Code | Description    | OrganizationId                       | CourseType  | IsTemplate |
	| Econ 100 | E100 | Macroeconomics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | Traditional | False      |
And I have the following course segments for 'Econ 100'
	| Name  | Description                        | Type     | ParentSegment |
	| Week 1 | First week is slack time           | TimeSpan |               |

Scenario: Add a learning activity
	When I add the following learning activity to 'Week 1' course segment
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then my course learning activity 'Discussion 1' contains the following
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Update a learning activity NEW
	Given I add the following course learning activities to 'Week 1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| Discussion 1 | Discussion | True        | true         | 100     | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	When I update 'Discussion 1' learning activity with the following info
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Assignment                           |
	| IsGradeable   | false                                |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then my course learning activity 'Discussion 1' contains the following
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Assignment                           |
	| IsGradeable   | false                                |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Delete a learning activity
	Given I add the following course learning activities to 'Week 1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| Discussion 1 | Discussion | True        | true         | 100     | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	When I remove "Discussion 1" learning activity
	And I retrieve the course learning activity 'Discussion 1'
	Then I get 'NotFound' response

Scenario: Cannot add a learning activity to a course that is already published
	When I publish the following courses
	| Name     | Note      |
	| Econ 100 | Blah blah |
	And I add the following learning activity to 'Week 1' course segment
	| Field         | Value                                |
	| Name          | Assignment 2                         |
	| Type          | Assignment                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Weight        | 50                                   |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then I get 'Forbidden' response