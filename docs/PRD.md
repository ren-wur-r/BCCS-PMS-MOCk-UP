# Project Manager System (PMS) - PRD (Consolidated)

Version: 2026-01-08 (draft)
Source: currentStatus.md, .specify/spec/*, changelog.md, singleAndhybridProjectDefinietion.md, codebase routes/controllers

## 1. Product Vision
PMS is an internal enterprise platform that unifies CRM, system master data, and project execution management. It supports end-to-end flow from lead generation (marketing), telemarketing follow-up, sales opportunity management, and project delivery with milestones, tasks, and work logs.

## 2. Business Goals
- Standardize sales-to-project conversion and delivery tracking.
- Provide consistent master data (customers, contacts, products, org structure) across modules.
- Enable role-based access control (RBAC) and visibility by region/department.
- Support project delivery templates with milestone stages, required outputs, and alerting on missed deadlines.

## 3. Primary Users and Roles
- Sales and marketing: manage activities, telemarketing, and pipeline progression.
- Project manager / department head: create projects, manage milestones, members, and risk.
- Executors: update tasks, work logs, and outputs.
- General manager / observers: view-only access to project status and risks.
- System admins: maintain master data and roles/permissions.

## 4. Scope by Module
### Basic (Common)
- Authentication (login/logout/heartbeat).
- Shared lookup APIs for employees, regions, departments, roles, products, customers, and contacts.

### System Settings
- Region, department, role, employee, customer, contact, product master data.
- Consolidated “Master Data” module in the UI.
- Project type/service item/stage template settings (renamed from contract types).

### CRM
- Activity management: events, products, sponsors, expenses, attendee lists, surveys, import (EDM/Teams).
- Telemarketing: call lists derived from activities, call logs, lead assignment, status tracking.
- Pipeline: opportunities with stage percentage, products, bills, and conversion to projects.

### Work Management
- Project management: project list, details, members, milestones, costs, attachments.
- Job management: tasks under milestones, bucket checklist, work logs.
- Templates: project type -> service items -> stage templates -> outputs, with alerting rules.

## 5. Key Cross-Module Workflows
1) Login -> preload shared data -> access role-based modules.
2) CRM Activity -> telemarketing -> pipeline -> convert to project (100% stage).
3) Project creation -> select project type -> select service items -> auto-generate milestones/stages -> assign members -> track outputs and work logs.

## 6. Project Type / Service Item / Stage Templates (Confirmed Design)
- “Contract type” renamed to “Project type”.
- Project type category: single (one service item) or hybrid (multi service items).
- Service items map to stage templates (milestones) and outputs.
- Hybrid project stages are presented as separate milestones per service item/department.
- Compliance attachments required at project creation (Order/SLA; PDF preferred, Word/PNG allowed).

## 7. Technical Stack
- Backend: ASP.NET Core Web API, controller-based routing (/api/{Controller}/{Action}).
- Frontend: Vue.js (Vite), Vue Router, Pinia, Tailwind-based utility classes.
- Data: SQL Server (backend), localStorage used for project template settings in current front-end iteration.

## 8. Known Constraints and In-Progress Work
- Several front-end flows use mock data to support development when backend is unavailable (see changelog).
- Project template settings and attachments currently persist in localStorage (frontend only).
- Some error dialogs are triggered in mock mode when missing mock endpoints.

## 9. Success Criteria (High-Level)
- Users can complete project creation within 5 steps.
- Milestones and tasks align with selected project type and service items.
- Role-based access prevents unauthorized edits.
- CRM to project conversion is traceable and auditable.

