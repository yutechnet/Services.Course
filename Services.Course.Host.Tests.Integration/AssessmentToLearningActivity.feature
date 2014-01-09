@Api
Feature: AssessmentToLearningActivity
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

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
	And I add the following course learning activities to 'Week 1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint |
	| Discussion 1 | Discussion | True        | true          | 100    | 20       |

Scenario: Create a learning activity with an assessment
	Given I have the following assessments
	| Name        | Instructions | AssessmentType | IsPublished |
	| Assessment1 | Do this      | Essay          | true        |
	When I add the following learning activity to 'Week 1' course segment
	| Field         | Value                                |
	| Name          | Discussion 2                         |
	| Type          | Discussion                           |
	| IsGradeable   | True                                 |
	| IsExtraCredit | True                                 |
	| Weight        | 100                                  |
	| MaxPoint      | 20                                   |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assessment    | Assessment1                          |  
	Then the segment 'Week 1' should have the following learning activities
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | Assessment  | AssessmentType |
	| Discussion 1 | Discussion | True        | true          | 100    | 20       |             | Custom         |
	| Discussion 2 | Discussion | True        | true          | 100    | 20       | Assessment1 | Essay          |

Scenario: Associate a published assessment to an existing learning activity
	Given I have the following assessments
	| Name        | Instructions | AssessmentType | IsPublished |
	| Assessment1 | Do this      | Essay          | true        |
	When I update 'Discussion 1' learning activity with the following info
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Custom                               |
	| IsGradeable   | false                                |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assessment    | Assessment1                          |
	Then the segment 'Week 1' should have the following learning activities
	| Name         | Type   | IsGradeable | IsExtraCredit | Weight | MaxPoint | Assessment  | AssessmentType |
	| Discussion 1 | Custom | false       | False         | 100    | 100      | Assessment1 | Essay          |

Scenario: Associate a non-existing assessment
	When I update 'Discussion 1' learning activity with the following info
	| Field         | Value                                |
	| Name          | Discussion 1                         |
	| Type          | Custom                               |
	| IsGradeable   | false                                |
	| IsExtraCredit | false                                |
	| Weight        | 100                                  |
	| MaxPoint      | 100                                  |
	| ObjectId      | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assessment    | Non-Existing                         |
	Then I get the following responses
	| StatusCode |
	| NotFound   |