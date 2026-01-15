# Spec - Work Project

## Overview
Manages project lifecycle: project list/detail, members, milestones, tasks, costs, and required outputs. Supports project-type-driven stage templates with alerting rules.

## Frontend Logic
- Routes:
  - `/work/project` (list)
  - `/work/project/add` (create)
  - `/work/project/detail/:id` (detail)
- Views: `FrontEnd/manager-back-site/src/views/work/project/*`.
- UI behaviors:
  - Project list rows are clickable for detail navigation; action column removed.
  - Project create flow uses project type -> service item selection; template preview by service item.
  - Compliance attachments (Order/SLA) required at create; file upload UI present.
  - Detail view includes template tab with stage outputs and work logs.
- State/storage:
  - Project template settings stored in localStorage (from System Settings).
  - Selected project type/service items stored per project in localStorage (frontend-only).

## Backend Logic
- Controller: `MbsWorkProjectController`.
- Key endpoints (from spec):
  - Dashboard: `GetDashboard`.
  - Project: `GetManyProject`, `GetProject`, `AddProject`, `UpdateProject`.
  - Contract/work plan: `AddContract`, `AddWork`.
  - Members: `AddMember`, `RemoveMember`, `GetManyMemberRoleAsync`.
  - Milestones: `GetManyProjectStone`, `GetProjectStone`, `AddProjectStone`, `UpdateProjectStone`, `RemoveProjectStone`.
  - Tasks: `GetManyProjectStoneTask`, `GetProjectStoneTask`, `UpdateProjectStoneTask`, `UpdateProjectStoneTaskBucket`.
  - Expenses: `GetExpense`, `GetManyExpense`, `AddExpense`, `UpdateExpense`.

## Data Entities (from spec)
- EmployeeProject
- EmployeeProjectMember
- EmployeeProjectStone (milestone)
- EmployeeProjectStoneJob (task)
- EmployeeProjectStoneJobBucket (checklist)
- EmployeeProjectExpense
- EmployeeProjectContract
- EmployeeProjectWork

## Project Type / Template Rules (from planning)
- Project type category: single vs hybrid.
- Service items map to stage templates; hybrid uses separate milestones per service item/department.
- Alert rules: notify supervisor at -5 days, GM at -3 days; mark as risk.

