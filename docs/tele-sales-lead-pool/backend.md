# Tele-Sales Lead Pool - Backend Design

## Core Entities
### 1) TeleSalesLeadPool
Suggested fields:
- LeadPoolID (PK)
- SourceKind (enum: activity_transfer, manual)
- ActivityID (nullable, FK)
- ActivityName (denormalized for list)
- CompanyName
- CompanyPhone
- MobilePhone
- ContactName
- ContactDepartment
- ContactTitle
- ContactEmail
- Industry
- EvaluationRange
- DiscussItem
- InterestLevel
- Note
- ListStatus (enum: 未撥打/已撥打/已派單/不再聯繫)
- CustomerValue (enum: 高/中/低/待評估)
- AssignedRegion
- AssignedSalesEmployeeID (nullable, FK)
- AssignedSalesEmployeeName (denormalized)
- CallDate (date)
- LastEditedAt (datetime)
- CreatedAt, CreatedBy
- UpdatedAt, UpdatedBy

### 2) TeleSalesLeadPoolActivityLink (optional)
Use if one lead can link to multiple activities.
- LeadPoolID (FK)
- ActivityID (FK)

## Suggested APIs
- GET /api/TeleSales/LeadPool/List
  - Filters: Industry, CustomerValue, CallDate, EvaluationRange, Status
- POST /api/TeleSales/LeadPool/Add
  - SourceKind = manual
- POST /api/TeleSales/LeadPool/AddFromActivity
  - Payload includes ActivityID + Lead IDs
- PUT /api/TeleSales/LeadPool/Update
  - Edits call result fields (industry/evaluation/interest/note/etc.)

## Sync Rules
- Activity transfer keeps original list; lead is copied into pool.
- Lead pool is the long-term backlog for tele-sales follow-up.
- CallDate updates when a record is edited.

