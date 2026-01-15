# Attendance (Visit) Integration Plan

## Goal
Enable sales staff to record visits (拜訪/駐點/簡報) from the PWA, link the visit to an existing pipeline, and surface visit counts in the business dashboard.

## Proposed DB Schema (Backend)
### 1) SalesVisitLog
- VisitLogID (PK)
- EmployeeID (FK)
- EmployeeName
- PipelineID (nullable FK)
- VisitType (enum: onsite, visit, presentation)
- VisitTime (datetime)
- Location (string)
- Remark (string)
- CreatedAt, CreatedBy

### 2) PipelineVisitLink (optional)
Use if you want to attach multiple visits to a pipeline with extra metadata.
- PipelineID (FK)
- VisitLogID (FK)

## Proposed APIs
### POST /api/SalesVisit/Add
- Payload: EmployeeID, PipelineID, VisitType, VisitTime, Location, Remark
- Response: VisitLogID

### GET /api/SalesVisit/GetMany
- Filters: EmployeeID, PipelineID, VisitType, DateRange

### GET /api/SalesVisit/GetDashboardSummary
- Filters: EmployeeID, DateRange
- Response: visitCount, visitCountByType, lastVisitTime

## PWA Flow (Future)
1) Open PWA → select existing Pipeline → tap “打卡”
2) Submit SalesVisit/Add
3) Backend writes SalesVisitLog and links to Pipeline
4) Dashboard pulls summary via GetDashboardSummary

## Dashboard Dependency
- Business dashboard “拜訪目標” progress compares target vs. SalesVisitLog counts.

