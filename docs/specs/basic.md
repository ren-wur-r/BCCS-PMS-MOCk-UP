# Spec - Basic / Common

## Overview
Provides authentication and shared lookup data used across all modules.

## Frontend Logic
- Routes: `/login`, `/home`.
- Views: `FrontEnd/manager-back-site/src/views/basic/Login.vue`, `FrontEnd/manager-back-site/src/views/home/Home.vue`.
- Services: `FrontEnd/manager-back-site/src/services/pms-http/basic/*` (login/logout/heartbeat/shared lookups).
- State: session token and user profile loaded at login; used for permissions and menu visibility.
- Mock mode: login/heartbeat/employee data may be mocked for local development (see `changelog.md`).

## Backend Logic
- Controller: `BackEnd/ProjectManagerSystem/ProjectManagerWebApi/Controllers/MbsBasicController.cs`.
- Endpoints (from spec):
  - Auth/session: `POST /api/MbsBasic/Login`, `POST /api/MbsBasic/Logout`, `POST /api/MbsBasic/Heartbeat`.
  - User profile: `POST /api/MbsBasic/GetEmployee`, `POST /api/MbsBasic/UpdateEmployeePassword`.
  - Shared lists: employees, regions, departments, roles, products, companies, contacts, project members, milestones, tasks.
- Permissions: token validation and permission filtering are enforced on backend.

## Data Entities (from spec)
- Employee, Department, Region, Role, Product (main/sub/spec), Company, Contacter.

