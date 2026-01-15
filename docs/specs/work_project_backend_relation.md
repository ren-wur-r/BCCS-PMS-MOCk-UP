# Spec - Work Project Backend Relation (Planned)

Source: `.specify/spec/work_project_backend_relation.md`

## Purpose
Define backend data models and APIs for project type/stage/output/work log so the frontend can run in non-DB mode now and connect later.

## Proposed Data Models
1) ContractType (rename to ProjectType in latest UI)
- ContractTypeId, ContractTypeName, Description, IsEnable, CreateTime, UpdateTime.

2) ProjectStageTemplate
- StageTemplateId, ContractTypeId, StageName, OwnerRoleId/Name, SortOrder,
  RequiresWorkLog, RequiresOnsiteLog, CreateTime, UpdateTime.

3) ProjectStageOutputTemplate
- OutputTemplateId, StageTemplateId, OutputName, OutputKind, IsRequired, SortOrder,
  CreateTime, UpdateTime.

4) Project (add field)
- EmployeeProjectContractTypeId (project type id).

5) ProjectStageOutput
- OutputId, EmployeeProjectId, StageTemplateId, OutputTemplateId,
  OutputFileName/Size/Type/Path, UploadedBy, UploadedAt.

6) ProjectStageWorkLog
- WorkLogId, EmployeeProjectId, StageTemplateId, LogDate, LogContent,
  CreatedBy, CreatedAt.

## Proposed APIs
### Project type/template
- `POST /api/MbsWorkContractType/GetMany`
- `POST /api/MbsWorkContractType/Add`
- `POST /api/MbsWorkContractType/Update`
- `POST /api/MbsWorkContractType/UpdateOrder`
- `POST /api/MbsWorkContractType/GetStageTemplate`
- `POST /api/MbsWorkContractType/AddStageTemplate`
- `POST /api/MbsWorkContractType/UpdateStageTemplate`
- `POST /api/MbsWorkContractType/UpdateStageOrder`
- `POST /api/MbsWorkContractType/AddOutputTemplate`
- `POST /api/MbsWorkContractType/UpdateOutputTemplate`
- `POST /api/MbsWorkContractType/UpdateOutputOrder`

### Project
- `POST /api/MbsWorkProject/AddProject` (include EmployeeProjectContractTypeId)
- `POST /api/MbsWorkProject/UpdateProject` (include EmployeeProjectContractTypeId)
- `POST /api/MbsWorkProject/GetProject` (return EmployeeProjectContractTypeId)

### Stage outputs / work logs
- `POST /api/MbsWorkProject/GetStageOutputs`
- `POST /api/MbsWorkProject/UploadStageOutput`
- `POST /api/MbsWorkProject/RemoveStageOutput`
- `POST /api/MbsWorkProject/GetStageWorkLogs`
- `POST /api/MbsWorkProject/AddStageWorkLog`

## NAS/SharePoint Upload Mechanism (Planned)
- Upload flow writes to NAS or SharePoint and returns metadata and paths.
- Permissions required for download; avoid exposing raw internal paths.
- SharePoint uses short-lived download URLs.

## Frontend Interim Mode
- Uses localStorage/static seeds; outputs and work logs stored in memory until API integration.

