@Api
Feature: CourseTemplate
In order to create courses easily and quickly
As a course builder
I want to create courses from a template

Background: 
Given I have the following course template
| Name       | Code          | Description              | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
| Template 1 | TemplateCode1 | My First Course Template | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |

Scenario: Create a course from a template
When I create a course from the template 'Template 1' with the following:
| Name       | Code          | Description              | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
| Template 1 | TemplateCode1 | My First Course Template | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |
Then the course should have the following info:
| Name       | Code          | Description              | OrganizationId                       | CourseType  | IsTemplate |
| Template 1 | TemplateCode1 | My First Course Template | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Traditional | false      |

Scenario: Change name, code, description and organization id-- validates mutable fields
When I create a course from the template with the following:
| Name         | Code          | Description                 | Tenant Id | OrganizationId                       | CourseType  | IsTemplate |
| New Template | TemplateCode2 | Yet another course template | 999999    | b50cada2-b1ba-4b2e-b82c-8ca7125fb39b | Traditional | false      |
Then the course should have the following info:
| Name         | Code          | Description                 | OrganizationId                       | CourseType  | IsTemplate |
| New Template | TemplateCode2 | Yet another course template | b50cada2-b1ba-4b2e-b82c-8ca7125fb39b | Traditional | false      |

#Scenario: Cannot alter coureType field
#When I create a course from the template 'Template 1' with the following:
#| Name       | Code          | Description              | Tenant Id | OrganizationId                       | CourseType | IsTemplate |
#| Template 1 | TemplateCode1 | My First Course Template | 999999    | C3885307-BDAD-480F-8E7C-51DFE5D80387 | Competency | false      |
#Then I should get a 'BadRequest' status