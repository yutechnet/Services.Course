@Api
Feature: LearningOutcomeManagement
	In order to specify the expected outcome of a course or program
	As a course builder or program coordinator
	I want to manage learning outcomes

Scenario: Add a new leanring outcome and get it
	When I create a learning outcome with the description 'SomeDescription'
	Then the learning outcome should be with the description 'SomeDescription'

Scenario: Edit an existing leanring outcome and get it
	Given I have a learning outcome with the description 'SomeDescription'
	When I change the description to 'OtherDescription'
	Then the learning outcome should be with the description 'OtherDescription'

Scenario: Delete an existing leanring outcome
	Given I have a learning outcome with the description 'SomeDescription'
	When I delete this learning outcome
	Then the learning outcome shoud no longer exist

Background: 
Given the following programs exist:
	| Name | Description | OrganizationId | TenantId |
	| BA   | BA Program  | E2DF063D-E2A1-4F83-9BE0-218EC676C05F | 1        |
And the following course exists:
	| Field       | Value                         |
	| Name        | English 101                   |
	| Code        | ENG101                        |
	| Description | Ranji's awesome English class |
	| TenantId    | 1                             |

Scenario: Add a new learning outcome to a program and get it
	When I associate to the 'program' 'BA' a new learning outcome with the description 'SomeDescription'
	Then the learning outcome should be with the description 'SomeDescription'
	And the 'program' 'BA' is associated with learning outcome 'SomeDescription'

Scenario: Associate a new learning outcome to a program
When I associate the following learning outcomes to 'BA' program:
| Description             |
| first learning outcome  |
| second learning outcome |
Then 'BA' program is associated with the following learning outcomes:
| Description             |
| first learning outcome  |
| second learning outcome |

Scenario: Duplicate association is idempotent
When I associate the following learning outcomes to 'BA' program:
| Description             |
| first learning outcome  |
When I associate the following learning outcomes to 'BA' program:
| Description             |
| first learning outcome  |
Then 'BA' program is associated with the following learning outcomes:
| Description             |
| first learning outcome  | 

Scenario: Disassociated a learning outcome from a program
When I associate the following learning outcomes to 'BA' program:
| Description             |
| first learning outcome  |
| second learning outcome |
When I disassociate the following learning outcomes from 'BA' program:
| Description             |
| first learning outcome  |
Then 'BA' program is associated with the only following learning outcomes:
| Description             |
| second learning outcome |

Scenario: Disassociating non-existing learning outcome
When I associate the following learning outcomes to 'BA' program:
| Description             |
| first learning outcome  |
| second learning outcome |
Then disassociating the following from 'BA' program should result:
| Description             | Disassociation Response |
| first learning outcome  | NoContent               |
| second learning outcome | NoContent               |
| third learning outcome  | NotFound                |

Scenario: Associate an outcome to another outcome
	Given I associate the following learning outcomes to 'BA' program:
	| Description                     | TenantId |
	| first program learning outcome  | 1        |
	| second program learning outcome | 1        |
	And I assoicate the following learning outcomes to 'ENG101' course:
	| Description                    | TenantId |
	| first course learning outcome  | 1        |
	| second course learning outcome | 1        |
	When I assoicate the following outcomes to 'second program learning outcome'
	| CourseCode | Description                    |
	| ENG101     | first course learning outcome  |
	| ENG101     | second course learning outcome |
	Then 'second program learning outcome' has the following learning outcomes:
	| Description                    |
	| first course learning outcome  |
	| second course learning outcome |

Scenario: Disassociate an outcome from another outcome
	Given I associate the following learning outcomes to 'BA' program:
	| Description                     | TenantId |
	| first program learning outcome  | 1        |
	| second program learning outcome | 1        |
	And I assoicate the following learning outcomes to 'ENG101' course:
	| Description                    | TenantId |
	| first course learning outcome  | 1        |
	| second course learning outcome | 1        |
	When I assoicate the following outcomes to 'second program learning outcome'
	| CourseCode | Description                    |
	| ENG101     | first course learning outcome  |
	| ENG101     | second course learning outcome |
	And I disassociate 'first course learning outcome' from 'second program learning outcome'
	Then 'second program learning outcome' has the following learning outcomes:
	| Description                   | 
	| second course learning outcome |
