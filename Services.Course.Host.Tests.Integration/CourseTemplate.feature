﻿@Api
Feature: CourseTemplate
In order to create courses easily and quickly
As a course builder
I want to create courses from a template

Background: 
Given the following programs exist:
	| Name                | Description | ProgramType | OrganizationId                       |
	| Bachelor of Art     | BA Program  | BA          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
	| Bachelor of Science | BS program  | BS          | E2DF063D-E2A1-4F83-9BE0-218EC676C05F |
And I associate the following learning outcomes to 'Bachelor of Art' program:
	| Description                     | TenantId |
	| first program learning outcome  | 999999   |
	| second program learning outcome | 999999   |
And I have the following course template
	| Name       | Code          | Description              | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| Template 1 | TemplateCode1 | My First Course Template | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
And I associate 'Template 1' course with the following programs:
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |
And I assoicate the following learning outcomes to 'Template 1' course:
	| Description                    | TenantId |
	| first course learning outcome  | 999999   |
	| second course learning outcome | 999999   |
And I assoicate the following outcomes to 'second program learning outcome'
	| CourseCode    | Description                    |
	| TemplateCode1 | first course learning outcome  |
	| TemplateCode1 | second course learning outcome |
And I add following course segments to 'Template 1':
	| Name        | Description                    | Type       | ParentSegment |
	| Week1       | First week is slack time       | TimeSpan   |               |
	| Discussion  | Discussion for the first week  | Discussion | Week1         |
	| Discussion2 | Discussion2 for the first week | Discussion | Week1         |
	| Topic       | Topic for a discussion         | Topic      | Discussion    |


Scenario: Create a course from a template
When I create a course from the template 'Template 1' with the following:
	| Name     | Code        | Description              | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| Course 1 | CourseCode1 | My First Course Template | 999999    | b50cada2-b1ba-4b2e-b82c-8ca7125fb39b | Traditional | false      |
Then the course should have the following info:
	| Name     | Code        | Description              | OrganizationId                       | CourseType  | IsTemplate |
	| Course 1 | CourseCode1 | My First Course Template | b50cada2-b1ba-4b2e-b82c-8ca7125fb39b | Traditional | false      |

Scenario: Cannot alter coureType field
When I create a course from the template 'Template 1' with the following:
	| Name       | Code          | Description              | Tenant Id | OrganizationId                       | CourseType | IsTemplate |
	| Template 1 | TemplateCode1 | My First Course Template | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Competency | false      |
Then I should get a 'BadRequest' status

Scenario: Verify programs are copied from course template
When I create a course from the template 'Template 1' with the following:
	| Name       | Code          | Description              | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
	| Template 1 | TemplateCode1 | My First Course Template | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
Then the course 'Template 1' includes the following program information:
	| Program Name        |
	| Bachelor of Art     |
	| Bachelor of Science |