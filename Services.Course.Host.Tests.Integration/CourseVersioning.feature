@Api
Feature: CourseVersioning
	In order to allow continuous enhancement of course
	As a course builder
	I want to version the course

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
	| EditProgram   |
	| ViewProgram   |
	And I have the following assets
	| Name   |
	| asset1 |
	| asset2 |
	| asset3 |
	And Published the following assets
	| Name   | PublishNote |
	| asset1 | published   |
	| asset2 | published   |
	| asset3 | published   |
	And I have the following courses 
	| Name           | Code   | Description                   | OrganizationName | CourseType  | IsTemplate | MetaData   | ExtensionAssets |
	| English 1010   | ENG101 | Ranji's awesome English Class | COB              | Traditional | false      | {someData} | asset1,asset2   |
	| English 101011 | E10011 | Macroeconomics                | COB              | Traditional | false      | {someData} | asset3          |

Scenario: Create a default version
	Then the course 'English 1010' should have the following info
	| Field           | Value                         |
	| Name            | English 1010                  |
	| Code            | ENG101                        |
	| Description     | Ranji's awesome English Class |
	| VersionNumber   | 1.0.0.0                       |
	| MetaData        | {someData}                    |
	And the course 'English 1010' should have the following reference info
	| Field           | Value         |
	| ExtensionAssets | asset1,asset2 |

Scenario: Edit a course version
	When I update 'English 1010' course with the following info
	| Field           | Value                          |
	| Name            | English 10101                  |
	| Code            | ENG10101                       |
	| Description     | Ranji's terrible English Class |
	| IsTemplate      | true                           |
	| MetaData        | {updatedData}                  |
	| ExtensionAssets | asset2                         |
	Then the course 'English 1010' should have the following info
	| Field            | Value                          |
	| Name             | English 10101                  |
	| Code             | ENG10101                       |
	| Description      | Ranji's terrible English Class |
	| VersionNumber    | 1.0.0.0                        |
	| OrganizationName | COB                            |
	| IsTemplate       | true                           |
	| MetaData         | {updatedData}                  |
	And the course 'English 1010' should have the following reference info
	| Field           | Value  |
	| ExtensionAssets | asset2 |

Scenario: Publish a course version
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	Then the course 'English 1010' should have the following info
	| Field         | Value                         |
	| Name          | English 1010                  |
	| Code          | ENG101                        |
	| Description   | Ranji's awesome English Class |
	| VersionNumber | 1.0.0.0                       |
	| IsPublished   | true                          |
	| PublishNote   | Blah blah                     |
	| MetaData      | {someData}                    |
	And the course 'English 1010' should have the following reference info
	| Field           | Value         |
	| ExtensionAssets | asset1,asset2 |

Scenario: Publish a course,course programs should be published and not be modified
	Given I have the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | BS program  | BS          | Default          | requirement one        |
	| Bachelor of Art     | BA Program  | BA          | Default          |                        |
	And the user has permission to access these programs:
	| Name                |
	| Bachelor of Art     |
	| Bachelor of Science |
	When I associate 'English 1010' course with the following programs
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
	And I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |		
	Then I have the following programs
	| Name                | Description | ProgramType | OrganizationName | VersionNumber | IsPublished | PublishNote |
	| Bachelor of Art     | BA Program  | BA          | Default          | 1.0.0.0       | true        | Blah blah   |
	| Bachelor of Science | BS program  | BS          | Default          | 1.0.0.0       | true        | Blah blah   |
	When I modify the program 'Bachelor of Science' info to reflect the following
	| Field                  | Value                                |
	| Name                   | Bachelor of Arts                     |
	| Description            | English                              |
	| ProgramType            | BA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one update               |
	Then I get 'BadRequest' response
	When I delete the program 'Bachelor of Science'
	Then I get 'BadRequest' response


Scenario: Published version cannot be modified
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I update 'English 1010' course with the following info
	| Field           | Value                                |
	| Name            | English 10101                        |
	| Code            | ENG101                               |
	| Description     | Johns's terrible English Class       |
	| OrganizationId  | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| MetaData        | {updatedData}                        |
	| ExtensionAssets | asset2                               |
	Then I get 'BadRequest' response

Scenario: Published version cannot be deleted
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I delete 'English 1010' course
	Then I get 'BadRequest' response

Scenario: Create a course version from a previously-published version
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v2' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the course 'English 1010 v2' should have the following info
	| Field           | Value                                |
	| Name            | English 1010                         |
	| Code            | ENG101                               |
	| Description     | Ranji's awesome English Class        |
	| VersionNumber   | 2.0a                                 |
	| IsPublished     | false                                |
	| MetaData        | {someData}                           |
	And the course 'English 1010 v2' should have the following reference info
	| Field           | Value          |
	| ExtensionAssets | asset1,asset2 |


Scenario: Create a course version from a previously-published version then publish it
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v1.0.0.1' with the following info
	| Field         | Value    |
	| VersionNumber | 1.0.0.1  |
	And I publish the following courses
	| Name                  | Note            |
	| English 1010 v1.0.0.1 | Blah blah DE396 |	
	Then the course 'English 1010 v1.0.0.1' should have the following info
	| Field           | Value                                |
	| Name            | English 1010                         |
	| Code            | ENG101                               |
	| Description     | Ranji's awesome English Class        |
	| VersionNumber   | 1.0.0.1                              |
	| IsPublished     | true                                 |
	| MetaData        | {someData}                           |
	And the course 'English 1010 v1.0.0.1' should have the following reference info
	| Field           | Value          |
	| ExtensionAssets | asset1,asset2 |

Scenario: Create a course version from a previously-published version with prerequisites
	When I publish the following courses
	| Name           | Note      |
	| English 101011 | Blah blah |
	And I add the following prerequisites to 'English 1010'
	| Name			 | 
	| English 101011 | 
	And I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v2' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the course 'English 1010 v2' should have the following info
	| Field           | Value                                |
	| Name            | English 1010                         |
	| Code            | ENG101                               |
	| Description     | Ranji's awesome English Class        |
	| VersionNumber   | 2.0a                                 |
	| IsPublished     | false                                |
	| MetaData        | {someData}                           |
	And the course 'English 1010 v2' should have the following reference info
	| Field           | Value          |
	| ExtensionAssets | asset1,asset2 |
	And the course 'English 1010 v2' should have the following prerequisites
	| Name           |
	| English 101011 |

Scenario: Create a course version from a previously-published version with segments
	Given I have the following course segments for 'English 1010'
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v2' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the course 'English 1010 v2' should have the following info
	| Field           | Value                                |
	| Name            | English 1010                         |
	| Code            | ENG101                               |
	| Description     | Ranji's awesome English Class        |
	| VersionNumber   | 2.0a                                 |
	| IsPublished     | false                                |
	| MetaData        | {someData}                           |
	And the course 'English 1010 v2' should have the following reference info
	| Field           | Value          |
	| ExtensionAssets | asset1,asset2 |
	And the course 'English 1010 v2' should have these course segments
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |

Scenario: Create a course version from a previously-published version with segments and learning activities
	Given I have the following assessments
	| Name        | Instructions | AssessmentType | IsPublished | VersionNumber |
	| Assessment1 | Do this      | Essay          | true        | 1.1           |
	And I have the following course segments for 'English 1010'
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |
	And I add the following course learning activities to 'Week1' course segment
	| Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId                             | Assessment  |
	| Discussion 1 | Discussion | True        | true          | 100    | 20       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F | Assessment1 |
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v2' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the course 'English 1010 v2' should have the following info
	| Field           | Value                                |
	| Name            | English 1010                         |
	| Code            | ENG101                               |
	| Description     | Ranji's awesome English Class        |
	| VersionNumber   | 2.0a                                 |
	| IsPublished     | false                                |
	| MetaData        | {someData}                           |
	And the course 'English 1010 v2' should have the following reference info
	| Field           | Value          |
	| ExtensionAssets | asset1,asset2 |
	And the course 'English 1010 v2' should have these course segments
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |
	And my course learning activity 'Discussion 1' contains the following
	| Field          | Value                                |
	| Name           | Discussion 1                         |
	| Type           | Discussion                           |
	| IsGradeable    | true                                 |
	| IsExtraCredit  | true                                 |
	| Weight         | 100                                  |
	| MaxPoint       | 20                                   |
	| ObjectId       | D2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Assessment     | Assessment1                          |
	| AssessmentType | Essay                                |

Scenario: Cannot publish the same version twice
	When I publish the following courses
	| Name         | Note      |
	| English 1010 | Blah blah |
	And I create a new version of 'English 1010' course named 'English 1010 v2' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'BadRequest' response

Scenario: Cannot publish without a version
	When I create a course without a version
	Then I get 'BadRequest' response

Scenario: Search for published course
	Given the user has permission to access these courses:
	| Name           |
	| English 1010   |
	| English 101011 |
	When I publish the following courses
	| Name           | Note      |
	| English 1010   | Blah blah |
	Then published courses for orgniazation 'COB' contains the following courses
	| Name           |
	| English 1010   |
	And published courses for orgniazation 'COB' does not contain the following courses
	| Name           |
	| English 101011 |
