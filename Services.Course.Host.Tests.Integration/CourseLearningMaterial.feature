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
	| Name   | Description               | Type     | ParentSegment |
	| Week 1 | First week is slack time  | TimeSpan |               |
	| Week 2 | Second week is slack time | TimeSpan |               |
	| Day 1  | First Day is slack time   | TimeSpan | Week 1        |
	And I have the following assets
	| Name  |
	| file1 |
	| file2 |
	| file3 |
	| file4 |

Scenario: Create a learning material
    When Create learning material as the following info
		| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial | MetaData         |
		| file1 | Week 1        |             | false      | Material A       | MetaDataA |
	Then The 'Material A' learning material has the following info
	    | Asset | CourseSegment | Instruction | IsRequired |MetaData  |
		| file1 | Week 1        |             | false      |MetaDataA |

Scenario: Update a learning material
    Given Create learning material as the following info
		| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial | MetaData         |
		| file1 | Week 1        |             | false      | Material A       | MetaDataA |
	When Update 'Material A' learning material as the following info
	    | Asset | Instruction      | IsRequired | MetaData        |
	    | file1 | test instruction | true       | MetaDataAUpdate |
	Then The 'Material A' learning material has the following info
		| Asset | CourseSegment | Instruction      | IsRequired |MetaData        |
		| file1 | Week 1        | test instruction | true       |MetaDataAUpdate |

Scenario: Delete a learning material
    Given Create learning material as the following info
		| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial |MetaData  |
		| file1 | Week 1        |             | false      | Material A       |MetaDataA |
	When I remove 'Material A' learning material
	And I retrieve the learning material 'Material A'
	Then I get 'NotFound' response

Scenario: Create learning materials with same asset
    When Create learning material as the following info
		| Asset | CourseSegment | LearningMaterial | Instruction | IsRequired |MetaData  |
		| file2 | Week 1        | Material A       |             | false      |MetaDataA |
		| file2 | Week 2        | Material B       |             | false      |MetaDataB |
	Then The following learning materials have the following info
	    | LearningMaterial | Asset | CourseSegment | Instruction | IsRequired |MetaData  |
	    | Material A       | file2 | Week 1        |             | false      |MetaDataA |
	    | Material B       | file2 | Week 2        |             | false      |MetaDataB |

Scenario: Can create a section from the course with learning materials
	Given Published the following assets
	    | Name  | PublishNote |
	    | file1 | published   |
	    | file2 | published   |
    And Create learning material as the following info
	    | Asset | CourseSegment | Instruction | IsRequired | LearningMaterial | MetaData  |
	    | file1 | Week 1        |             | false      | Material A       | MetaDataA |
	    | file2 | Week 2        |             | false      | Material B       | MetaDataB |
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
		| Name             | Code          | Description            | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets |
		| English Template | TemplateCode1 | English Class Template | COB              | Traditional | true       |	{someData} | asset1,asset2   |
    And I have the following course segments for 'English Template'
	    | Name     | Description | Type     | ParentSegment |
	    | Week one | First week  | TimeSpan |               |
	    | Week two | Second week | TimeSpan |               |
	    | Day one  | First Day   | TimeSpan | Week one      |
    And Create learning material as the following info
	    | Asset | CourseSegment | Instruction       | IsRequired | LearningMaterial    |MetaData  |
	    | file1 | Week one      |                   | false      | Template Material A |MetaDataA |
	    | file2 | Week two      | test  instruction | true       | Template Material B |MetaDataB |
	    | file3 | Day one       | test  instruction | true       | Template Material C |MetaDataC |
    And Create learning material as the following info
	    | Asset | Course           | Instruction | IsRequired | LearningMaterial    | MetaData  |
	    | file4 | English Template |             | false      | Template Material D | MetaDataD |
	When Create a course from the template 'English Template' with the following
	    | Name           | Code    | Description                   | OrganizationName | IsTemplate |
	    | English Course | ENG 200 | My First Course from Template | COB              | false      |
	Then The course 'English Course' has following learning material
	    | Asset | CourseSegment | Instruction       | IsRequired | ParentCourse     | MetaData  |
	    | file1 | Week one      |                   | false      | English Template | MetaDataA |
	    | file2 | Week two      | test  instruction | true       | English Template | MetaDataB |
	    | file3 | Day one       | test  instruction | true       | English Template | MetaDataC |
	    | file4 |               |                   | false      | English Template | MetaDataD |

Scenario: Create a course version from a previously-published version with learning materials
	Given Published the following assets
	    | Name  | PublishNote |
	    | file1 | published   |
	    | file2 | published   |
		| file3 | published   |
		| file4 | published   |
    And Create learning material as the following info
	    | Asset | CourseSegment | Instruction   | IsRequired | LearningMaterial |MetaData  |
	    | file1 | Week 1        | instruction 1 | true       | Material A       |MetaDataA |
	    | file2 | Week 2        | instruction 2 | false      | Material B       |MetaDataB |
	    | file3 | Day 1         | instruction 3 | true       | Material C       |MetaDataC |
	And Create learning material as the following info
	    | Asset | Course   | Instruction   | IsRequired | LearningMaterial | MetaData  |
	    | file4 | Econ 100 | instruction 4 | true       | Material D       | MetaDataD |
	And Publish the following courses
        | CourseName | Note      |
        | Econ 100   | published |
	When Create a new version of 'Econ 100' course named 'Econ 100 v1.0.0.1' with the following info
	    | Field         | Value    |
	    | VersionNumber | 1.0.0.1  |
	Then The course 'Econ 100 v1.0.0.1' has following learning material
	    | Asset | CourseSegment | Instruction   | IsRequired | ParentCourse | MetaData  |
	    | file1 | Week 1        | instruction 1 | true       | Econ 100     | MetaDataA |
	    | file2 | Week 2        | instruction 2 | false      | Econ 100     | MetaDataB |
	    | file3 | Day 1         | instruction 3 | true       | Econ 100     | MetaDataC |
	    | file4 |               | instruction 4 | true       | Econ 100     | MetaDataD |

Scenario: Publish course with learning materials then the associate assets need publish
    Given Create learning material as the following info
	    | Asset | CourseSegment | LearningMaterial | Instruction | IsRequired | MetaData  |
	    | file1 | Week 1        | Material A       |             | false      | MetaDataA |
	When Publish the following courses with 'file1' asset does not publish
        | CourseName | Note      |
        | Econ 100   | published |
	Then The asset 'file1' is published



Scenario: Create a course learning material
    When Create learning material as the following info
		| Asset | Course   | Instruction | IsRequired | LearningMaterial | MetaData  |
		| file1 | Econ 100 |             | false      | Material A       | MetaDataA |
	Then The 'Material A' learning material has the following info
	    | Asset | Course   | Instruction | IsRequired | MetaData  |
	    | file1 | Econ 100 |             | false      | MetaDataA |

Scenario: Update a course learning material
    Given Create learning material as the following info
		| Asset | Course   | Instruction | IsRequired | LearningMaterial | MetaData  |
		| file1 | Econ 100 |             | false      | Material A       | MetaDataA |
	When Update 'Material A' learning material as the following info
	    | Asset | Instruction      | IsRequired | MetaData        |
	    | file1 | test instruction | true       | MetaDataAUpdate |
	Then The 'Material A' learning material has the following info
		| Asset | Course   | Instruction      | IsRequired | LearningMaterial | MetaData  |
		| file1 | Econ 100 | test instruction | true       | Material A       | MetaDataA |

Scenario: Delete a course learning material
    Given Create learning material as the following info
		| Asset | Course   | Instruction | IsRequired | LearningMaterial | MetaData  |
		| file1 | Econ 100 |             | false      | Material A       | MetaDataA |
	When I remove 'Material A' learning material
	And I retrieve the learning material 'Material A'
	Then I get 'NotFound' response

Scenario: Can not create a learning material with empty assetId
    When Create learning material as the following info
		| Asset | CourseSegment | Instruction | IsRequired | LearningMaterial | MetaData  |
		|       | Week 1        |             | false      | Material A       | MetaDataA |
	Then I get 'BadRequest' response