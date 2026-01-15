# Spec - CRM Phone (Telemarketing)

## Overview
Manages telemarketing activities, call lists, contact updates, product recommendations, assignments, and call logs.

## Frontend Logic
- Routes:
  - `/crm/phone/activity` (activity list/detail)
  - `/crm/phone/activity/detail/:activityId/pipeline` (phone list)
- Views: `FrontEnd/manager-back-site/src/views/crm/phone/*`.
- Tabs: shared activity tabs for activity/pipeline.
- List UX: row click navigation in list/pipeline views.

## Backend Logic
- Controller: `MbsCrmPhoneController`.
- Key endpoints (from spec):
  - Activity: `GetManyActivity`, `GetActivity`, `GetManyPastActivity`.
  - Phone list: `GetManyActivityEmployeePipeline`, `GetActivityEmployeePipeline`, `UpdateActivityEmployeePipelineStatus`.
  - Company: `GetEmployeePipelineCompany`, `UpdateEmployeePipelineCompany`.
  - Contacters: `GetOriginalEmployeePipelineContacter`, `GetManyEmployeePipelineContacter`, `AddEmployeePipelineContacter`, `UpdateEmployeePipelineContacter`, `RemoveEmployeePipelineContacter`.
  - Saler assignment: `AddEmployeePipelineSaler`, `GetManyEmployeePipelineSaler`.
  - Call logs: `AddEmployeePipelinePhone`, `GetManyEmployeePipelinePhone`.
  - Products: `GetEmployeePipelineProduct`, `GetManyEmployeePipelineProduct`, `AddEmployeePipelineProduct`, `UpdateEmployeePipelineProduct`, `RemoveEmployeePipelineProduct`.

## Data Entities (from spec)
- ActivityEmployeePipeline, EmployeePipelineCompany, EmployeePipelineContacter, EmployeePipelinePhone, EmployeePipelineProduct.

