@Api
Feature: CourseLearningActivity
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
Given the following courses exist:
	| Name     | Code | Description       | OrganizationId |
	| Math 101 | M101 | Basic mathematics | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
Given I add following course segments to 'Math 101':
	| Name  | Description                    | Type     | ParentSegment |
	| Week1 | First week is slack time       | TimeSpan |               |

Scenario: Add a learning activity
	When I add the following learning activity:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId |
	| 999999   | Assignment 1 | Assignment | True        | False         | 100    | 20       | Null     |
	Then my learning activity contains the following:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId |
	| 999999   | Assignment 1 | Assignment | True        | False         | 100    | 20       | Null     |

Scenario: Update a learning activity
	When I add the following learning activity:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId |
	| 999999   | Assignment 1 | Assignment | True        | False         | 100    | 20       | Null     |
	And I update the learning activity with the following:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId |
	| 999999   | Assignment 1 | Discussion | false       | false         | 80     | 10       | Null     |
	Then my learning activity contains the following:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId |
	| 999999   | Assignment 1 | Discussion | false       | false         | 80     | 10       | Null     |

Scenario: Delete a learning activity
	When I add the following learning activity:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId |
	| 999999   | Assignment 1 | Assignment | True        | False         | 100    | 20       | Null     |
	And I remove the learning activity
	Then the learning activity below no longer exists:
	| TenantId | Name         | Type       | IsGradeable | IsExtraCredit | Weight | MaxPoint | ObjectId |
	| 999999   | Assignment 1 | Assignment | True        | False         | 100    | 20       | Null     |
#Scenario: Cannot add a learning activity to a course that is already published