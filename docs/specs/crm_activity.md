# Spec - CRM Activity (Marketing)

## Overview
Manages marketing events, activity products/sponsors/expenses, attendee lists, surveys, and data import.

## Frontend Logic
- Routes:
  - `/crm/activity/activity` (activity list/add/detail)
  - `/crm/activity/activity/detail/:activityId/pipeline` (attendee list)
- Views: `FrontEnd/manager-back-site/src/views/crm/activity/*`.
- Tabs: Activity detail uses shared tab header component `components/feature/activity/ActivityDetailTabs.vue`.
- List UX: row click navigation in list/pipeline views.

## Backend Logic
- Controller: `MbsCrmActivityController`.
- Key endpoints (from spec):
  - Activity CRUD: `GetManyActivity`, `GetActivity`, `AddActivity`, `UpdateActivity`.
  - Products: `GetManyActivityProduct`, `GetActivityProduct`, `AddActivityProduct`, `UpdateActivityProduct`, `RemoveActivityProduct`.
  - Sponsors: `GetManyActivitySponsor`, `GetActivitySponsor`, `AddActivitySponsor`, `UpdateActivitySponsor`, `RemoveActivitySponsor`.
  - Expenses: `GetManyActivityExpense`, `GetActivityExpense`, `AddActivityExpense`, `UpdateActivityExpense`, `RemoveActivityExpense`.
  - Attendee lists: `GetManyActivityEmployeePipeline`, `GetActivityEmployeePipeline`, `AddActivityEmployeePipeline`, `UpdateManyActivityEmployeePipeline`, `RemoveManyActivityEmployeePipeline`.
  - Past activities: `GetManyPastActivity`.
  - Survey: `AddActivitySurveyQuestion`, `GetActivitySurveyQuestion`, `UpdateActivitySurveyQuestion`, `RemoveActivitySurveyQuestion`, `GetManyActivitySurveyAnswerer`.
  - Import: `DownloadEdmTemplate`, `ImportEdm`, `DownloadTeamsTemplate`, `ImportTeams`.

## Data Entities (from spec)
- Activity, ActivityProduct, ActivitySponsor, ActivityExpense, ActivitySurveyQuestion, ActivityEmployeePipeline.

