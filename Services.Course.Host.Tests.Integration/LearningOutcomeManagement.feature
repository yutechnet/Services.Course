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

Scenario: Add a new learning outcome to a program and get it
	Given the following programs exist:
	| Name | Description |
	| BA   | BA Program  |
	When I associate to the 'program' 'BA' a new learning outcome with the description 'SomeDescription'
	Then the learning outcome should be with the description 'SomeDescription'
	And the 'program' 'BA' is associated with learning outcome 'SomeDescription'
