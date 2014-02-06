@Api
Feature: CourseLearningActivity
	As a course creator
	I would like to specify grading attributes to learning activities
	So that I can turn them into gradebook items

Background: 
	And the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability   |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	And I have the following courses
	| Name     | Code | Description    | OrganizationName | CourseType  | IsTemplate |
	| Econ 100 | E100 | Macroeconomics | COB              | Traditional | False      |
	And I have the following course segments for 'Econ 100'
	| Name   | Description              | Type     | ParentSegment |
	| Week 1 | First week is slack time | TimeSpan |               |

Scenario: Add a learning activity
	When I add the following learning activity to 'Week 1' course segment
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Description   | Desc                                 |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then my course learning activity 'Discussion 1' contains the following
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| Description   | Desc                                 |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Add a learning activity with acitve, inactive, duedate
	When I add the following learning activity to 'Week 1' course segment
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Description   | Desc                                 |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| ActiveDate    | 15                                   |
	| InactiveDate  | 30                                   |
	| DueDate       | 60                                   |
	Then my course learning activity 'Discussion 1' contains the following
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Description   | Desc                                 |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| ActiveDate    | 15                                   |
	| InactiveDate  | 30                                   |
	| DueDate       | 60                                   |


Scenario: Get a list of all learning activities
	Given I add the following course learning activities to 'Week 1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| Discussion 1 | Discussion | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assignment 1 | Assignment | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Quiz 1       | Quiz       | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assessment 1 | Assessment | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then the segment 'Week 1' should have the following learning activities
	| Name         |
	| Discussion 1 |
	| Assignment 1 |
	| Quiz 1       |
	| Assessment 1 |

Scenario: Update a learning activity NEW
	Given I add the following course learning activities to 'Week 1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| Discussion 1 | Discussion | True        | true         | 100     | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	When I update 'Discussion 1' learning activity with the following info
	| Field           | Value                                                                        |
	| Name            | Discussion 1                                                                 |
	| Type            | Assignment                                                                   |
	| IsGradeable     | false                                                                        |
	| IsExtraCredit   | false                                                                        |
	| Description     | Desc                                                                         |
	| Weight          | 100                                                                          |
	| MaxPoint        | 100                                                                          |
	| ObjectId        | D2DF063D-E2A1-4F83-9BE0-218EC676C05F                                         |
	| CustomAttribute | {"id":"49","summary":"asdf","type":"Parabola.Web.Contract.Discussion.Forum"} |
	Then my course learning activity 'Discussion 1' contains the following
	| Field           | Value                                                                        |
	| Name            | Discussion 1                                                                 |
	| Type            | Assignment                                                                   |
	| IsGradeable     | false                                                                        |
	| IsExtraCredit   | false                                                                        |
	| Description     | Desc                                                                         |
	| Weight          | 100                                                                          |
	| MaxPoint        | 100                                                                          |
	| ObjectId        | D2DF063D-E2A1-4F83-9BE0-218EC676C05F                                         |
	| CustomAttribute | {"id":"49","summary":"asdf","type":"Parabola.Web.Contract.Discussion.Forum"} |

Scenario: Delete a learning activity
	Given I add the following course learning activities to 'Week 1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	| Discussion 1 | Discussion | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
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
	Then I get 'BadRequest' response

Scenario: Add a new learning activity with a custom type
	When I add the following learning activity to 'Week 1' course segment
	| Field           | Value                                                                        |
	| Name            | Custom 1                                                                     |
	| Type            | Custom                                                                       |
	| IsGradeable     | True                                                                         |
	| IsExtraCredit   | False                                                                        |
	| Weight          | 100                                                                          |
	| MaxPoint        | 20                                                                           |
	| ObjectId        | D2DF063D-E2A1-4F83-9BE0-218EC676C05F                                         |
	| CustomAttribute | {"id":"49","summary":"asdf","type":"Parabola.Web.Contract.Discussion.Forum"} |
	Then my course learning activity 'Custom 1' contains the following
	| Field           | Value                                                                        |
	| Name            | Custom 1                                                                     |
	| Type            | Custom                                                                       |
	| IsGradeable     | True                                                                         |
	| IsExtraCredit   | False                                                                        |
	| Weight          | 100                                                                          |
	| MaxPoint        | 20                                                                           |
	| ObjectId        | D2DF063D-E2A1-4F83-9BE0-218EC676C05F                                         |
	| CustomAttribute | {"id":"49","summary":"asdf","type":"Parabola.Web.Contract.Discussion.Forum"} |
