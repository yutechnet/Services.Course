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
Given I have the following programs
	| Name | Description | ProgramType | OrganizationId                       |
	| BA   | BA Program  | BA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
And I have the following courses
	| Name        | Code   | Description                   | OrganizationId                       |
	| English 101 | ENG101 | Ranji's awesome English class | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |

Scenario: Associate a new learning outcome to a program
	When I associate the newly created learning outcomes to 'BA' program
	| Description             |
	| first learning outcome  |
	| second learning outcome |
	Then 'BA' program is associated with the following learning outcomes:
	| Description             |
	| first learning outcome  |
	| second learning outcome |

Scenario: Associate a existing learning outcome to a program
	Given I have the following learning outcomes
	| Description             |
	| first learning outcome  |
	| second learning outcome |
	When I associate the existing learning outcomes to 'BA' program
	| Description             |
	| first learning outcome  |
	| second learning outcome |
	Then 'BA' program is associated with the following learning outcomes:
	| Description             |
	| first learning outcome  |
	| second learning outcome |

Scenario: Duplicate association is idempotent
	When I associate the newly created learning outcomes to 'BA' program
	| Description             |
	| first learning outcome  |
	When I associate the newly created learning outcomes to 'BA' program
	| Description             |
	| first learning outcome  |
	Then 'BA' program is associated with the following learning outcomes:
	| Description             |
	| first learning outcome  | 

#Scenario: Disassociated a learning outcome from a program
#	When I associate the newly created learning outcomes to 'BA' program
#	| Description             |
#	| first learning outcome  |
#	| second learning outcome |
#	When I disassociate the following learning outcomes from 'BA' program:
#	| Description             |
#	| first learning outcome  |
#	Then 'BA' program is associated with the only following learning outcomes:
#	| Description             |
#	| second learning outcome |

#Scenario: Disassociating non-existing learning outcome
#	When I associate the newly created learning outcomes to 'BA' program
#	| Description             |
#	| first learning outcome  |
#	| second learning outcome |
#	Then disassociating the following from 'BA' program should result:
#	| Description             | Disassociation Response |
#	| first learning outcome  | NoContent               |
#	| second learning outcome | NoContent               |
#	| third learning outcome  | NotFound                |

#Scenario: Associate an outcome to another outcome
#	Given I have the following learning outcomes
#	| Description                     |
#	| first program learning outcome  |
#	| second program learning outcome |
#	| first course learning outcome   |
#	| second course learning outcome  |
#	When I assoicate the following outcomes to outcome 'second program learning outcome'
#	| Description                    |
#	| first course learning outcome  |
#	| second course learning outcome |
#	Then 'second program learning outcome' has the following learning outcomes:
#	| Description                    |
#	| first course learning outcome  |
#	| second course learning outcome |
#
#Scenario: Disassociate an outcome from another outcome
#	Given I have the following learning outcomes
#	| Description                     |
#	| first program learning outcome  |
#	| second program learning outcome |
#	| first course learning outcome   |
#	| second course learning outcome  |
#	When I assoicate the following outcomes to outcome 'second program learning outcome'
#	| Description                    |
#	| first course learning outcome  |
#	| second course learning outcome |
#	And I disassociate 'first course learning outcome' from 'second program learning outcome'
#	Then 'second program learning outcome' has the following learning outcomes:
#	| Description                    |
#	| second course learning outcome |
