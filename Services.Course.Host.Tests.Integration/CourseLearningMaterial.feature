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
	| Day 1  | First Day is slack time   | TimeSpan | Week 1        |
	And I have the following assets
	| Name  |
	| file1 |
	| file2 |
	| file3 |

Scenario: Create a learning material
    When Create learning material as the following info
		| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial |
		| file1 | Week 1        |             | false      | Material A       |
	Then The 'Material A' learning material has the following info
	    | Asset | CourseSegment | Instruction | IsRequired |
		| file1 | Week 1        |             | false      |

Scenario: Update a learning material
    Given Create learning material as the following info
		| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial |
		| file1 | Week 1        |             | false      | Material A       |
	When Update 'Material A' learning material as the following info
	    | Asset | Instruction      | IsRequired |
	    | file1 | test instruction | true       |
	Then The 'Material A' learning material has the following info
		| Asset | CourseSegment | Instruction      | IsRequired |
		| file1 | Week 1        | test instruction | true       |

Scenario: Delete a learning material
    Given Create learning material as the following info
		| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial |
		| file1 | Week 1        |             | false      | Material A       |
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

Scenario: Can create a section from the course with learning materials
	Given Published the following assets
	    | Name  | PublishNote |
	    | file1 | published   |
	    | file2 | published   |
    And Create learning material as the following info
	    | Asset | CourseSegment | Instruction | IsRequired | LearningMaterial |
	    | file1 | Week 1        |             | false      | Material A       |
	    | file2 | Week 2        |             | false      | Material B       |
	And Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	When The section service returns 'Created'
	And Create section as following
		| CourseName | Name             | Code      | StartDate | EndDate   | OrganizationName |
		| Econ 100   | Econ 100 Section | Test Code | 2/15/2014 | 6/15/2014 | COB              |
    Then I get 'Created' response

Scenario: Create a course from a course template with learning materials
   Given I have the following course templates
		| Name             | Code          | Description            | OrganizationName | CourseType  | IsTemplate |
		| English Template | TemplateCode1 | English Class Template | COB              | Traditional | true       |
    And I have the following course segments for 'English Template'
	    | Name     | Description | Type     | ParentSegment |
	    | Week one | First week  | TimeSpan |               |
	    | Week two | Second week | TimeSpan |               |
	    | Day one  | First Day   | TimeSpan | Week one      |
    And Create learning material as the following info
	    | Asset | CourseSegment | Instruction       | IsRequired | LearningMaterial    |
	    | file1 | Week one      |                   | false      | Template Material A |
	    | file2 | Week two      | test  instruction | true       | Template Material B |
	    | file3 | Day one       | test  instruction | true       | Template Material C |
	When Create a course from the template 'English Template' with the following
	    | Name           | Code    | Description                   | OrganizationName | IsTemplate |
	    | English Course | ENG 200 | My First Course from Template | COB              | false      |
	Then The course 'English Course' has following learning material
	    | Asset | CourseSegment | Instruction       | IsRequired | ParentCourse     |
	    | file1 | Week one      |                   | false      | English Template |
	    | file2 | Week two      | test  instruction | true       | English Template |
	    | file3 | Day one       | test  instruction | true       | English Template |

Scenario: Create a course version from a previously-published version with learning materials
	Given Published the following assets
	    | Name  | PublishNote |
	    | file1 | published   |
	    | file2 | published   |
		| file3 | published   |
    And Create learning material as the following info
	    | Asset | CourseSegment | Instruction   | IsRequired | LearningMaterial |
	    | file1 | Week 1        | instruction 1 | true       | Material A       |
	    | file2 | Week 2        | instruction 2 | false      | Material B       |
	    | file3 | Day 1         | instruction 3 | true       | Material C       |
	And Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	When Create a new version of 'Econ 100' course named 'Econ 100 v1.0.0.1' with the following info
	    | Field         | Value    |
	    | VersionNumber | 1.0.0.1  |
	Then The course 'Econ 100 v1.0.0.1' has following learning material
	    | Asset | CourseSegment | Instruction   | IsRequired | ParentCourse |
	    | file1 | Week 1        | instruction 1 | true       | Econ 100     |
	    | file2 | Week 2        | instruction 2 | false      | Econ 100     |
	    | file3 | Day 1         | instruction 3 | true       | Econ 100     |

Scenario: Publish course with learning materials then the associate assets need publish
    Given Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |
	    | file1 | Week 1        | Material A       |             | false      |
	When Publish the following courses with 'file1' asset does not publish
        | CourseName | Note      |
        | Econ 100   | published |
	Then The asset 'file1' is published
