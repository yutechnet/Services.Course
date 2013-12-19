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
	And I have the following assets
	| Name  |
	| file1 |
	| file2 |
	| file3 |

Scenario: Create a learning material
    When Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
		| file1 | Week 1        | Material A       |             | false      |
	Then The 'Material A' learning material has the following info
	    | Asset | CourseSegment | Instruction | IsRequired |
		| file1 | Week 1        |             | false      |

Scenario: Update a learning material
    Given Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
		| file1 | Week 1        | Material A       |             | false      |
	When Update 'Material A' learning material as the following info
	    | Asset | CourseSegment | Instruction | IsRequired |
	    | file1 | Week 2        |             | true       |
	Then The 'Material A' learning material has the following info
		| Asset | CourseSegment | Instruction | IsRequired |
		| file1 | Week 2        |             | true       |

Scenario: Delete a learning material
    Given Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
		| file1 | Week 1        | Material A       |             | false      |
	When I remove 'Material A' learning material
	And I retrieve the learning material 'Material A'
	Then I get 'NotFound' response

Scenario: Create learning materials with same asset
    When Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
		| file2 | Week 1        | Material A       |             | false      |
		| file2 | Week 2        | Material B       |             | false      |
	Then The following learning materials have the following info
	    | LearningMaterial | Asset | CourseSegment | Instruction | IsRequired |
	    | Material A       | file2 | Week 1        |             | false      |
	    | Material B       | file2 | Week 2        |             | false      |

Scenario: Validate the learning materials only have one asset
    When Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
		| file2 | Week 1        | Material A       |             | false      |
	And Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
	    | file3 | Week 1        | Material A       |             | false      |
	Then I get 'BadRequest' response

Scenario: Create a Section from the course with learning materials
	Given Published the following assets
	    | Name  | PublishNote |
	    | file1 | published   |
	    | file2 | published   |
    And Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
	    | file1 | Week 1        | Material A       |             | false      |
	    | file2 | Week 2        | Material B       |             | false      |
	And Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	When Create section as following
		| CourseName | Name             | Code      | StartDate | EndDate   |
		| Econ 100   | Econ 100 Section | Test Code | 2/15/2014 | 6/15/2014 |
	Then The section 'Econ 100 Section' has following learning materials
	    | LearningMaterial | Asset | CourseSegment | Instruction | IsRequired |
	    | Material A       | file2 | Week 1        |             | false      |
	    | Material B       | file2 | Week 2        |             | false      |

Scenario: Create a course from a course template with learning materials
   Given I have the following course template
   	    | Name             | Code   | Description            | Tenant Id | OrganizationName | CourseType  | IsTemplate | Credit |
   	    | English Template | Eng100 | English Class Template | 999999    | COB              | Traditional | true       | 5      |
    And I have the following course segments for 'English Template'
	    | Name     | Description | Type     | ParentSegment |
	    | Week one | First week  | TimeSpan |               |
	    | Week two | Second week | TimeSpan |               |
    And Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial    | Instruction | IsRequired |
	    | file1 | Week one      | Template Material A |             | false      |
	    | file2 | Week two      | Template Material B |             | false      |
	When Create a course from course template as following
	    | Course Template  | CourseName     |
	    | English Template | English Course |
	Then The course 'English Course' has following learning material
	    | LearningMaterial    | Asset | CourseSegment | Instruction | IsRequired |
	    | Template Material A | file1 | Week one      |             | false      |
	    | Template Material B | file2 | Week two      |             | false      |

Scenario: Publish course with learning materials then the associate assets need publish
    Given Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
	    | file1 | Week 1        | Material A       |             | false      |
	When Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	Then The asset 'file1' is published

Scenario: Canot modify the asset after the course publishing
    Given Create learning material as following
	    | Asset | CourseSegment | Name       | Instruction | IsRequired |
	    | file1 | Week 1        | Material A |             | false      |
	And Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	When Modify asset 'file1' as following info
		| Description            |
		| change the description |
	Then I get 'BadRequest' response
