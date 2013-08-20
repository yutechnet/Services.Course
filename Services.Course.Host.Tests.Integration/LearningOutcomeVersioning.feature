@Api
Feature: LearningOutcomeVersioning
	In order to allow continuous enhancement of learning outcome
	As a course builder
	I want to version the learning outcome

Background: 
	Given I have the following learning outcomes
	| Description     |
	| first learning outcome |

Scenario: Create a default version
	Then the learning outcome 'first learning outcome' should contain
	| Field         | Value                  |
	| Description   | first learning outcome |
	| VersionNumber | 1.0.0.0                |

Scenario: Edit a learning outcome version
	When I update 'first learning outcome' learning outcome with the following info
	| Field       | Value                   |
	| Description | second learning outcome |
	Then the learning outcome 'first learning outcome' should contain
	| Field         | Value                   |
	| Description   | second learning outcome |
	| VersionNumber | 1.0.0.0                 |

Scenario: Publish a learning outcome version
	When I publish the following learning outcomes
	| Name                   | Note      |
	| first learning outcome | Blah blah |
	Then the learning outcome 'first learning outcome' should contain
	| Field         | Value                  |
	| Description   | first learning outcome |
	| VersionNumber | 1.0.0.0                |
	| IsPublished   | true                   |
	| PublishNote   | Blah blah              |

Scenario: Published version cannot be modified
	When I publish the following learning outcomes
	| Name                   | Note      |
	| first learning outcome | Blah blah |
	And I update 'first learning outcome' learning outcome with the following info
	| Field       | Value                  |
	| Description | third learning outcome |
	Then I get 'Forbidden' response

Scenario: Published version cannot be deleted
	When I publish the following learning outcomes
	| Name                   | Note      |
	| first learning outcome | Blah blah |
	And I delete the 'first learning outcome' learning outcome
	Then I get 'Forbidden' response

Scenario: Create a learning outcome version from a previously-published version
	When I publish the following learning outcomes
	| Name                   | Note      |
	| first learning outcome | Blah blah |
	And I create a new version of 'first learning outcome' outcome named 'NewOutcome' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the learning outcome 'NewOutcome' should contain
	| Field         | Value                  |
	| Description   | first learning outcome |
	| VersionNumber | 2.0a                   |
	| IsPublished   | false                  |

Scenario: Cannot publish the same version twice
	When I publish the following learning outcomes
	| Name                   | Note      |
	| first learning outcome | Blah blah |
	And I create a new version of 'first learning outcome' outcome named 'NewOutcome' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'BadRequest' response

Scenario: Cannot publish without a version
	When I create a new version of 'first learning outcome' outcome named 'NewOutcome' with the following info
	| Field         | Value |
	| VersionNumber |       |
	Then I get 'BadRequest' response


