@Api
Feature: LearningOutcomeVersioning
	In order to allow continuous enhancement of learning outcome
	As a course builder
	I want to version the learning outcome

Background: 
	Given I create the following learning outcome
	| Field       | Value                  |
	| Description | first learning outcome |

Scenario: Create a default version
	When I retrieve 'first learning outcome' learning outcome
	Then the learning outcome should have the following info
	| Field         | Value                  |
	| Description   | first learning outcome |
	| VersionNumber | 1.0.0.0                |

Scenario: Edit a learning outcome version
	When I update 'first learning outcome' learning outcome with the following info
	| Field       | Value                   |
	| Description | second learning outcome |
	Then the learning outcome 'second learning outcome' should have the following info
	| Field         | Value                   |
	| Description   | second learning outcome |
	| VersionNumber | 1.0.0.0                 |

Scenario: Publish a learning outcome version
	When I publish 'first learning outcome' learning outcome with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	Then the learning outcome 'first learning outcome' should have the following info
	| Field         | Value                  |
	| Description   | first learning outcome |
	| VersionNumber | 1.0.0.0                |
	| IsPublished   | true                   |
	| PublishNote   | Blah blah              |

Scenario: Published version cannot be modified
	Given I publish 'first learning outcome' course with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I update 'first learning outcome' learning outcome with the following info
	| Field       | Value                  |
	| Description | third learning outcome |
	Then I get 'Forbidden' response

Scenario: Published version cannot be deleted
	Given I publish 'first learning outcome' learning outcome with the following info
	| Field       | Value     |
	| PublishNote | Blah blah |
	When I delete 'first learning outcome' learning outcome
	Then I get 'Forbidden' response

Scenario: Create a learning outcome version from a previously-published version
	Given I publish 'first learning outcome' learning outcome with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I create a new version of 'first learning outcome' with the following info
	| Field         | Value |
	| VersionNumber | 2.0a  |
	Then the learning outcome 'first learning outcome' should have the following info
	| Field         | Value                  |
	| Description   | first learning outcome |
	| VersionNumber | 2.0a                   |
	| IsPublished   | false                  |

Scenario: Cannot publish the same version twice
	Given I publish 'first learning outcome' learning outcome with the following info
	| Field         | Value     |
	| PublishNote   | Blah blah |
	When I create a new version of 'first learning outcome' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'Conflict' response

Scenario: Cannot create a version off non-existing version
	When I create a new version of 'RandomOutcome' with the following info
	| Field         | Value   |
	| VersionNumber | 1.0.0.0 |
	Then I get 'NotFound' response

Scenario: Cannot publish without a version
	When I create a learning outcome without a version
	Then I get 'BadRequest' response


