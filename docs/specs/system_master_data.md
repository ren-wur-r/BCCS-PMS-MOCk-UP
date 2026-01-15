# Spec - System Settings (Master Data)

## Overview
Manages organizational and master data shared across CRM and Work modules: regions, departments, roles, employees, customers, contacts, and products.

## Frontend Logic
- Route: `/system/master` (consolidated master data UI).
- Additional routes (legacy or direct): `/system/region`, `/system/department`, `/system/role`, `/system/employee`, `/system/company`, `/system/contacter`, `/system/product`.
- Views: `FrontEnd/manager-back-site/src/views/system/*`.
- Permissions: role-based action controls (view/add/edit/delete) per menu/region.

## Backend Logic
Controllers:
- Regions: `MbsSystemRegionController`.
- Departments: `MbsSystemDepartmentController`.
- Roles: `MbsSystemRoleController`.
- Employees: `MbsSystemEmpolyeeController`.
- Companies: `MbsSystemCompanyController`.
- Contacters: `MbsSystemContacterController`.
- Products: `MbsSystemProductController`.

Key Endpoints (from spec):
- Region: `GetMany`, `Get`, `Add`, `Update`.
- Department: `GetMany`, `Get`, `Add`, `Update`.
- Role: `GetMany`, `Get`, `Add`, `Update`.
- Employee: `GetMany`, `Get`, `Add`, `Update`.
- Company: main/sub kinds, company, location, affiliate CRUD.
- Contacter: contacter, rating reason, rating CRUD.
- Product: main/sub kinds, product, specification CRUD.

## Data Entities (from spec)
- ManagerRegion, ManagerDepartment, ManagerRole, ManagerEmployee.
- ManagerCompany (main/sub kinds, locations, affiliates).
- ManagerContacter (rating, rating reason).
- ManagerProduct (main/sub kinds, products, specs).

