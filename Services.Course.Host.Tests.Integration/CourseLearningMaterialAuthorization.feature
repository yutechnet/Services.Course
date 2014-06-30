@Api
Feature: CourseLearningMaterialAuthorization
	In order to protect the system
	Lock down learning material endpoints in course service

Background: 
	Given the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| EditCourse    |
	And I have the following assets
	| Name   |
	| asset1 |
	| asset2 |
	And Published the following assets
	| Name   | PublishNote |
	| asset1 | published   |
	| asset2 | published   |
	And I have the following courses
	| Name     | Code | Description    | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets |
	| Econ 100 | E100 | Macroeconomics | COB              | Traditional | False      | {someData} | asset1,asset2   |
	And I have the following course segments for 'Econ 100'
	| Name   | Description              | Type     | ParentSegment |
	| Week 1 | First week is slack time | TimeSpan |               |
	And I have the following assets
	| Name  |
	| file1 |
	| file2 |
	And Create learning material as the following info
	| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial | CustomAttribute  |
	| file1 | Week 1        |             | false      | Material A       | CustomAttributeA |
	
Scenario Outline: I can create a learning material unless I have permission to do so.
	When I am 'TestUser1'	
	And  I have the '<Capability>' capability
	And Create learning material as the following info
	| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial | CustomAttribute  |
	| file1 | Week 1        |             | false      | Material B       | CustomAttributeA |
	Then I get '<StatusCode>' response
Examples:
	| Capability | StatusCode |
	| EditCourse | Created    |
	| CourseView | Forbidden  |
	|            | Forbidden  |

Scenario Outline: I can get a learning material unless I have permission to do so.
	When I am 'TestUser1'	
	And I have the '<Capability>' capability
	And  I get course learning material 'Material A'
	Then I get '<StatusCode>' response
Examples:
	| Capability | StatusCode |
	| CourseView | OK         |
	|            | Forbidden  |

Scenario Outline: I can update a learning material unless I have permission to do so.
	When I am 'TestUser1'
	And I have the '<Capability>' capability
	And Update 'Material A' learning material as the following info
	| Asset | Instruction      | IsRequired | CustomAttribute        |
	| file1 | test instruction | true       | CustomAttributeAUpdate |
	Then I get '<StatusCode>' response
Examples:
	| Capability | StatusCode |
	| EditCourse | NoContent  |
	| CourseView | Forbidden  |
	|            | Forbidden  |


Scenario Outline: I can delete a learning material unless I have permission to do so.
	When I am 'TestUser1'
	And I have the '<Capability>' capability
	And I remove 'Material A' learning material
	Then I get '<StatusCode>' response
Examples:
	| Capability | StatusCode |
	| EditCourse | NoContent  |
	| CourseView | Forbidden  |
	|            | Forbidden  |
