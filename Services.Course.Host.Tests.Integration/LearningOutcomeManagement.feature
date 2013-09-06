@Api
Feature: LearningOutcomeManagement
	In order to specify the expected outcome of a course or program
	As a course builder or program coordinator
	I want to manage learning outcomes

Scenario: Add a new leanring outcome and get it
	Given I have the following learning outcomes
	| Description     |
	| SomeDescription |
	Then the learning outcome 'SomeDescription' should contain
	| Field       | Value           |
	| Description | SomeDescription |

Scenario: Edit an existing learning outcome and get it
	Given I have the following learning outcomes
	| Description     |
	| SomeDescription |
	When I change the 'SomeDescription' learning outcome description to 'OtherDescription'
	Then the learning outcome 'SomeDescription' should contain
	| Description      |
	| OtherDescription |

Scenario: Delete an existing learning outcome
	Given I have the following learning outcomes
	| Description     |
	| SomeDescription |
	When I delete the 'SomeDescription' learning outcome
	And I get the learning outcome 'SomeDescription'
	Then I get 'NotFound' response

Background:
Given I am user "TestUser3"
And the following organizations exist
	| Name | Description | ParentOrganization |
	| COB  | Bus School  |                    |
And I create the following roles
	| Name  | Organization | Capabilities |
	| Role1 | COB          | CourseCreate |
And I give the user role "Role1" for organization COB 
Given I have the following programs
	| Name | Description | ProgramType | OrganizationName |
	| BA   | BA Program  | BA          | COB              |
And I have the following courses
	| Name        | Code   | Description                   | OrganizationName |
	| English 101 | ENG101 | Ranji's awesome English class | COB              |

Scenario: Associate a new learning outcome to a program
	Given I associate the newly created learning outcomes to 'BA' program
	| Description             |
	| first learning outcome  |
	| second learning outcome |
	Then 'BA' program is associated with the following learning outcomes
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
	Then 'BA' program is associated with the following learning outcomes
	| Description             |
	| first learning outcome  |
	| second learning outcome |

Scenario: Duplicate association is idempotent
	Given I associate the newly created learning outcomes to 'BA' program
	| Description             |
	| first learning outcome  |
	Then 'BA' program is associated with the following learning outcomes
	| Description             |
	| first learning outcome  | 

Scenario: Disassociated a learning outcome from a program
	Given I associate the newly created learning outcomes to 'BA' program
	| Description             |
	| first learning outcome  |
	| second learning outcome |
	When I disassociate the following learning outcomes from 'BA' program
	| Description             |
	| first learning outcome  |
	Then 'BA' program is associated with the only following learning outcomes
	| Description             |
	| second learning outcome |

Scenario: Disassociating non-existing learning outcome
	Given I associate the newly created learning outcomes to 'BA' program
	| Description             |
	| first learning outcome  |
	| second learning outcome |
	And I have the following learning outcomes
	| Description             |
	| third learning outcome  |
	When I disassociate the following learning outcomes from 'BA' program
	| Description             |
	| first learning outcome  |
	| second learning outcome |
	| third learning outcome  |
	Then I get the following responses
	| StatusCode |
	| NoContent  |
	| NoContent  |
	| NotFound   |

Scenario: Associate an outcome to another outcome
	Given I have the following learning outcomes
	| Description |
	| PL1         |
	| CL1         |
	| CL2         |
	When the outcome 'PL1' is supported by the following outcomes
	| Description |
	| CL1         |
	| CL2         |
	Then learning outcome 'PL1' is supported by the following learning outcomes
	| Description |
	| CL1         |
	| CL2         |

Scenario: Associate multiple outcomes to another outcome asynchronously
	Given I have the following learning outcomes
	| Description |
	| PL1         |
	| PL2         |
	| PL3         |
	| PL4         |
	| PL5         |
	| PL6         |
	| PL7         |
	| PL8         |
	| PL9         |
	| PL10        |
	| CL1         |
	When the outcome 'CL1' supports the following learning outcomes asynchronously
	| Description |
	| PL1         |
	| PL2         |
	| PL3         |
	| PL4         |
	| PL5         |
	| PL6         |
	| PL7         |
	| PL8         |
	| PL9         |
	| PL10        |
	Then the learning outcome 'CL1' supports the following learning outcomes
	| Description |
	| PL1         |
	| PL2         |
	| PL3         |
	| PL4         |
	| PL5         |
	| PL6         |
	| PL7         |
	| PL8         |
	| PL9         |
	| PL10        |


Scenario: Disassociate an outcome from another outcome
	Given I have the following learning outcomes
	| Description |
	| PL1         |
	| CL1         |
	| CL2         |
	When the outcome 'PL1' is supported by the following outcomes
	| Description |
	| CL1         |
	| CL2         |
	And I disassociate the following learning outcomes from 'PL1' learning outcome
	| Description |
	| CL1         |
	Then learning outcome 'PL1' is supported by the following learning outcomes
	| Description |
	| CL2         |
