# Spec - CRM Pipeline (Sales Opportunities)

## Overview
Manages sales opportunities (pipelines), stage progress, related companies/contacts/products, billing, and conversion to projects.

## Frontend Logic
- Routes: `/crm/pipeline/pipeline` (list/add/detail).
- Views: `FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/*`.
- Stage progression UX: validation based on stage percentage conditions; optional remarks.
- Dashboard surfaces personal and regional pipeline stage counts.

## Backend Logic
- Controller: `MbsCrmPipelineController`.
- Key endpoints (from spec):
  - Pipeline: `GetManyEmployeePipeline`, `GetEmployeePipeline`, `AddEmployeePipeline`, `UpdatePipelineStatus`, `TransferPipelineToProject`.
  - Company: `GetEmployeePipelineCompany`, `UpdateEmployeePipelineCompany`.
  - Contacters: `GetManyEmployeePipelineContacter`, `AddEmployeePipelineContacter`, `UpdateEmployeePipelineContacter`, `RemoveEmployeePipelineContacter`.
  - Sales tracking: `GetManyEmployeePipelineSaler`, `HandleEmployeePipelineSaler`.
  - Sales tracking history: `GetManyEmployeePipelineSalerTracking`, `AddEmployeePipelineSalerTracking`.
  - Due dates: `GetEmployeePipelineDue`, `GetManyEmployeePipelineDue`, `AddEmployeePipelineDue`, `UpdateEmployeePipelineDue`, `RemoveEmployeePipelineDue`.
  - Products: `GetEmployeePipelineProduct`, `AddEmployeePipelineProduct`, `UpdateEmployeePipelineProduct`, `RemoveEmployeePipelineProduct`.
  - Billing: `GetEmployeePipelineBill`, `GetManyEmployeePipelineBill`, `AddManyEmployeePipelineBill`, `UpdateEmployeePipelineBill`, `UpdateManyEmployeePipelineBill`, `RemoveManyEmployeePipelineBill`, `NotifyBillIssue`, `ConfirmBillIssue`.

## Data Entities (from spec)
- EmployeePipeline, EmployeePipelineCompany, EmployeePipelineContacter, EmployeePipelineProduct, EmployeePipelineBill, EmployeePipelineDue.

