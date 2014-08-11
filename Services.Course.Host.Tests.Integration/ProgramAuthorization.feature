@Api
Feature: ProgramAuthorization
	In order to perform CRUD on program, 
	I would need to have the required permission
	Create/Edit/Delete - requires EditProgram capability
	View - requires ViewProgram capability

Scenario Outline: I can not create a program unless I have permission to do so.
	Given I have the '<Capability>' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | Created    |
	|             | Forbidden  |

Scenario Outline: I can view a program when I do have permission.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	When I get the program 'Bachelor of Science'
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| ViewProgram | OK         |
	|             | Forbidden  |

Scenario Outline: I can edit a program when I do have permission.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	When I am 'TestUser1'	
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	When I modify the program 'Bachelor of Science' info to reflect the following
	| Field                  | Value                                |
	| Name                   | Bachelor of Arts                     |
	| Description            | English                              |
	| ProgramType            | BA                                   |
	| OrganizationId         | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| GraduationRequirements | requirement one update               |
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | OK         |
	|             | Forbidden  |

Scenario Outline: I can delete a program when I do have permission.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	When I am 'TestUser1'	
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	When I delete the program 'Bachelor of Science'
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | NoContent  |
	|             | Forbidden  |

Scenario Outline: I can not publish a program unless I have permission to do so.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	When I am 'TestUser1'	
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	And I have the '<Capability>' capability
	When I publish the following programs
	| Name                | Note |
	| Bachelor of Science | bala |
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | NoContent  |
	|             | Forbidden  |

Scenario Outline: I can not version a program unless I have permission to do so.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	And I publish the following programs
	| Name                | Note |
	| Bachelor of Science | bala |
	When I am 'TestUser1'	
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	And I have the '<Capability>' capability
	When I create a new version of 'Bachelor of Science' program named 'Bachelor of Science v2' with the following info
	| Field         | Value    |
	| VersionNumber | 1.0.0.1  |
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | Created    |
	|             | Forbidden  |

Scenario Outline: I can not deactivate a program unless I have permission to do so.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	And I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	When I am 'TestUser1'	
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	And I have the '<Capability>' capability
	When I deactivate the program 'Bachelor of Science'
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | NoContent  |
	|             | Forbidden  |

Scenario Outline: I can not activate a program unless I have permission to do so.
	Given I have the 'EditProgram' capability
	When I create the following programs
	| Name                | Description | ProgramType | OrganizationName | GraduationRequirements |
	| Bachelor of Science | Economics   | MA          | Default          | requirement one        |
	And I publish the following programs
	| Name                | Note      |
	| Bachelor of Science | Blah blah |
	And I deactivate the program 'Bachelor of Science'
	When I am 'TestUser1'	
	Given I have the following object capabilities
	| ObjectType | ObjectName          | Capability   |
	| program    | Bachelor of Science | <Capability> |
	And I have the '<Capability>' capability
	When I activate the program 'Bachelor of Science'
	Then I get '<StatusCode>' response
Examples:
	| Capability  | StatusCode |
	| EditProgram | NoContent  |
	|             | Forbidden  |

