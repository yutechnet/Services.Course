@Api
Feature: Course Learning Material

Background: 
	Given the following organizations exist
	| Name |
	| COB  |
	And I have the following capabilities
	| Capability    |
	| CourseCreate  |
	| CoursePublish |
	| CourseView    |
	And I have the following courses
	| Name     | Code | Description    | OrganizationName | CourseType  | IsTemplate |
	| Econ 100 | E100 | Macroeconomics | COB              | Traditional | False      |
	And I have the following course segments for 'Econ 100'
	| Name   | Description               | Type     | ParentSegment |
	| Week 1 | First week is slack time  | TimeSpan |               |
	| Week 2 | Second week is slack time | TimeSpan |               |
	#Given I add the following course learning activities to 'Week 1' course segment
	#| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             |
	#| Discussion 1 | Discussion | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	#| Assignment 1 | Assignment | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	And I have the following assets
	| Name  |
	| file1 |
	| file2 |
	| file3 |

@Ignore
Scenario: Add course learning material
	When I add the following assets as learning material to 'Discussion 1' learning activity
	| Description |
	| file1       |
	| file2       |
	Then 'Discussion 1' learning activity has the following learning material
	| Description |
	| file1       |
	| file2       |

@Ignore
Scenario: Delete course learning material
	When I add the following assets as learning material to 'Discussion 1' learning activity
	| Description |
	| file1       |
	| file2       |
	| file3       |
	And I delete the following learning meterial
	| Description |
	| file2       |
	Then 'Discussion 1' learning activity has the following learning material
	| Description |
	| file1       |
	| file3       |
@Ignore
Scenario: Add course learning material to published course fails
	When I publish the following courses
    | Name     | Note      |
    | Econ 100 | Published |
	And I add the following assets as learning material to 'Discussion 1' learning activity
	| Description |
	| file1       |
	| file2       |
	Then I get 'Forbidden' response 

Scenario: Publish course publishes library and associated assets
Scenario: Update learning material with unpublished course and unpublished asset
Scenario: Update learning material with unpublished course and published asset
Scenario: Update learning material with published course not allowed
Scenario: Update library item directly changes learning material
Scenario: Update library item fails if library is published






Scenario: Create a learning material
    When Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | AdditionalInstruction | IsRequired |
		| file1 | Week 1        | Material A       |                       | false      |
	Then the learning material has the following info
	    | Field                 | Value  |
	    | Asset                 | file1  |
	    | CourseSegment         | Week 1 |
	    | AdditionalInstruction |        |
	    | IsRequired            | false  |

Scenario: Create learning materials with same asset
    When Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | AdditionalInstruction | IsRequired |
		| file2 | Week 1        | Material A       |                       | false      |
		| file2 | Week 2        | Material B       |                       | false      |
	Then The following learning materials have the following info
	    | LearningMaterial | Asset | CourseSegment | AdditionalInstruction | IsRequired |
	    | Material A       | file2 | Week 1        |                       | false      |
	    | Material B       | file2 | Week 2        |                       | false      |

Scenario: Validate the learning materials only have one asset
    When Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | AdditionalInstruction | IsRequired |
		| file2 | Week 1        | Material A       |                       | false      |
	And Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial | AdditionalInstruction | IsRequired |
	    | file3 | Week 1        | Material A       |                       | false      |
	Then I get 'BadRequest' response

Scenario: Create a Section from the course with learning materials
	Given Published the following assets
	    | Name  | PublishNote |
	    | file1 | published   |
	    | file2 | published   |
    And Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial | AdditionalInstruction | IsRequired |
	    | file1 | Week 1        | Material A       |                       | false      |
	    | file2 | Week 2        | Material B       |                       | false      |
	And Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	When Create section as following
		| CourseName | Name             | Code      | StartDate | EndDate   |
		| Econ 100   | Econ 100 Section | Test Code | 2/15/2014 | 6/15/2014 |
	Then The section 'Econ 100 Section' has following learning materials
	    | LearningMaterial | Asset | CourseSegment | AdditionalInstruction | IsRequired |
	    | Material A       | file2 | Week 1        |                       | false      |
	    | Material B       | file2 | Week 2        |                       | false      |

Scenario: Create a course from a course template with learning materials
   Given I have the following course template
   	    | Name             | Code   | Description            | Tenant Id | OrganizationName | CourseType  | IsTemplate | Credit |
   	    | English Template | Eng100 | English Class Template | 999999    | COB              | Traditional | true       | 5      |
    And I have the following course segments for 'English Template'
	    | Name     | Description | Type     | ParentSegment |
	    | Week one | First week  | TimeSpan |               |
	    | Week two | Second week | TimeSpan |               |
    And Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial    | AdditionalInstruction | IsRequired |
	    | file1 | Week one      | Template Material A |                       | false      |
	    | file2 | Week two      | Template Material B |                       | false      |
	When Create a course from course template as following
	    | Course Template  | CourseName     |
	    | English Template | English Course |
	Then The course 'English Course' has following learning material
	    | LearningMaterial    | Asset | CourseSegment | AdditionalInstruction | IsRequired |
	    | Template Material A | file1 | Week one      |                       | false      |
	    | Template Material B | file2 | Week two      |                       | false      |

Scenario: Publish course with learning materials then the associate assets need publish
    Given Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial | AdditionalInstruction | IsRequired |
	    | file1 | Week 1        | Material A       |                       | false      |
	When Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	Then The asset 'file1' is published

Scenario: Canot modify the asset after the course publishing
    Given Create learning material as following
	    | Asset | CourseSegment | Name       |
		| file1 | Week 1        | Material A |
	And Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	When Modify asset 'file1' as following info
		| Description            |
		| change the description |
	Then I get 'BadRequest' response
