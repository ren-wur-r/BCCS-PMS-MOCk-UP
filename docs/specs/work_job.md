# Spec - Work Job (Tasks)

## Overview
Tracks tasks (jobs) under project milestones, with checklist buckets and work log records.

## Frontend Logic
- Routes:
  - `/work/job` (list)
  - `/work/job/detail/:id` (detail)
- Views: `FrontEnd/manager-back-site/src/views/work/job/*`.
- UX: list rows are clickable, action column removed; detail uses tabs for task content and work logs.

## Backend Logic
- Controller: `MbsWorkJobController`.
- Key endpoints (from spec):
  - Jobs: `GetManyProjectStoneJob`, `GetProjectStoneJob`, `UpdateProjectStoneJob`.
  - Work logs: `GetProjectStoneJobWork`, `AddProjectStoneJobWork`, `UpdateProjectStoneJobWork`, `GetManyProjectStoneJobWork`, `RemoveProjectStoneJobWork`.

## Data Entities (from spec)
- EmployeeProjectStoneJob
- EmployeeProjectStoneJobBucket
- EmployeeProjectStoneJobWork (work log)

