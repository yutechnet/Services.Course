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
	| Name | Description | TenantId |
	| BA   | BA Program  | 1        |

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
