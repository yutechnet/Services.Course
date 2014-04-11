@Api
Feature: CourseLearningActivityAuthorization
	In order to protect the system
	Lock down learning activity endpoints in course service

Background: 
	Given the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| EditCourse    |
	And I have the following courses
	| Name     | Code | Description    | OrganizationName | CourseType  | IsTemplate |
	| Econ 100 | E100 | Macroeconomics | COB              | Traditional | False      |
	And I have the following course segments for 'Econ 100'
	| Name   | Description              | Type     | ParentSegment |
	| Week 1 | First week is slack time | TimeSpan |               |
	And I add the following learning activity to 'Week 1' course segment
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Discussion                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Description   | Desc                                 |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario Outline: I can create a learning activity unless I have permission to do so.
	When I am 'TestUser1'	
	And  I have the '<Capability>' capability
	And I add the following learning activity to 'Week 1' course segment
	| Field         | Value                                |
	| Name          | Discussion 2                         |
	| Type          | Discussion                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | False                                |
	| Description   | Desc                                 |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	Then I get '<StatusCode>' response
Examples:
	| Capability | StatusCode |
	| EditCourse | Created    |
	| CourseView | Forbidden  |
	|            | Forbidden  |

Scenario Outline: I can get a learning activity unless I have permission to do so.
	When I am 'TestUser1'	
	And I have the '<Capability>' capability
	And  I get course learning activity 'Discussion 1'
	Then I get '<StatusCode>' response
Examples:
	| Capability | StatusCode |
	| CourseView | OK         |
	|            | Forbidden  |

Scenario Outline: I can update a learning activity unless I have permission to do so.
	When I am 'TestUser1'
	And I have the '<Capability>' capability
	And I update 'Discussion 1' learning activity with the following info
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
	Then I get '<StatusCode>' response
Examples:
	| Capability | StatusCode |
	| EditCourse | OK         |
	| CourseView | Forbidden  |
	|            | Forbidden  |


Scenario Outline: I can delete a learning activity unless I have permission to do so.
	When I am 'TestUser1'
	And I have the '<Capability>' capability
	And I remove "Discussion 1" learning activity
	Then I get '<StatusCode>' response
Examples:
	| Capability | StatusCode |
	| EditCourse | NoContent  |
	| CourseView | Forbidden  |
	|            | Forbidden  |
