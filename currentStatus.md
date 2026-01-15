# Project Manager System (PMS) - Current System PRD

**Version**: 1.0  
**Date**: 2026-01-06  
**Status**: Live / Production  

## 1. System Overview

The **Project Manager System (PMS)** is a comprehensive enterprise management platform designed to streamline internal operations, customer relationship management (CRM), and project execution. It is built with a **.NET Core Web API** backend and a **Vue.js** frontend (`manager-back-site`). The system is modular, covering System Settings, CRM, and Work Management.

### Core Modules
1.  **Basic / Common**: Authentication, user profile, and shared data services.
2.  **System Settings**: Configuration of organizational structure, user access, and master data (Products, Clients).
3.  **CRM**: Management of Marketing Activities, Telemarketing (Phone Sales), and Sales Pipelines.
4.  **Work Management**: Project lifecycle management, from initiation (converting leads) to execution (milestones, tasks) and tracking (work logs, expenses).

---

## 2. Basic / Common Module

**Purpose**: Provides essential infrastructure for user authentication, session management, and cross-module shared data access.

### Key Features
*   **Authentication**:
    *   **Login**: Account/Password based authentication returning a `LoginToken`.
    *   **Logout**: Invalidates session and clears local tokens.
    *   **Heartbeat**: Periodic API calls (every 6 mins) to validate token freshness; auto-logout on failure.
    *   **Password Management**: Users can update their own passwords with validation rules (min 6 chars).
*   **Shared Data Services (Dropdowns/Lookups)**:
    *   Centralized APIs (`MbsBasicController`) to fetch active lists for:
        *   Employees (filtered by department/role)
        *   Departments & Regions
        *   Roles & Permissions
        *   Products (Main/Sub kinds, Specifications)
        *   Companies (Clients) & Contacters (Contacts)
*   **Initialization**: On login, the system pre-fetches essential user context (Employee info, permissions, reachable regions/departments) to configure the UI.

---

## 3. System Settings Module

**Purpose**: Manages the organization's master data, security, and operational configurations.

### 3.1 Region Management
*   **Functionality**: Manage geographic regions for organization and permission scoping.
*   **Key Operations**: List (paged/filtered), Add, Update (Name, Active Status).
*   **System Defaults**: "Global" (ID: -1) and "No Permission" (ID: -2).

### 3.2 Department Management
*   **Functionality**: Manage internal departments.
*   **Key Operations**: List (search by name/status), Add, Update.
*   **System Defaults**: "Admin Dept" (ID: -99), "General Manager Office" (ID: -1).

### 3.3 Role Management
*   **Functionality**: Define roles and their specific permission sets.
*   **Permission Matrix**: Permissions are defined per **Menu** (System Module), **Region**, and **Action** (View, Add, Edit, Delete).
*   **Scope Control**: Permissions can be scoped to **Everyone**, **Self** (own data only), or **None**.
*   **Key Operations**: CRUD for Roles; complex UI for managing the permission matrix.

### 3.4 Employee Management
*   **Functionality**: Manage user accounts and access.
*   **Key Data**: Account (unique), Name, Password, Role, Status.
*   **Logic**: Email is auto-generated (`account@bccs.com.tw`). Department/Region are inherited from the assigned Role but can be overridden.
*   **Key Operations**: List (filter by dept/role/status), Add (validates account uniqueness), Update (supports password reset).

### 3.5 Company (Client) Management
*   **Functionality**: Manage external client companies.
*   **Key Data**: Name, Unified Number (Tax ID - unique), Address, Phone.
*   **Classifications**: Hierarchical categorization via **Main Kind** and **Sub Kind**.
*   **Locations**: Manage multiple operating locations (Branches) per company.
*   **Affiliates**: Track relationships (Parent/Child companies).

### 3.6 Contacter (Window) Management
*   **Functionality**: Manage individual contacts associated with Companies.
*   **Key Data**: Name, Phone, Email, Title, Department.
*   **Ratings**: Development rating system (White/Gray/Black list) with mandatory "Rating Reasons" for Gray/Black lists.
*   **Key Operations**: CRUD for Contacters and Rating Reasons.

### 3.7 Product Management
*   **Functionality**: Manage the catalog of products and services offered.
*   **Structure**:
    *   **Main/Sub Kinds**: Hierarchical categorization with defined **Commission Rates**.
    *   **Product**: The core item (Equipment or Service), flagged as "Main Product" or not.
    *   **Specifications**: Variants of a product (e.g., different sizes/models) with specific **Selling Price** and **Cost**.
*   **Key Operations**: Comprehensive CRUD for Kinds, Products, and Specifications.

---

## 4. CRM Module

**Purpose**: Drives business growth through marketing, telemarketing, and sales pipeline management.

### 4.1 Activity Management (Marketing)
*   **Functionality**: Plan and track marketing events/campaigns.
*   **Key Features**:
    *   **Event Details**: Name, Date, Location, Budget vs. Actual Cost.
    *   **Related Data**: Products promoted, Sponsors, Expenses.
    *   **Surveys**: Create custom surveys (Single/Multi-select, Text) to gather feedback.
    *   **Import**: Bulk import participant lists from eDM (Excel) or Teams integration.

### 4.2 Telemarketing (Phone Sales)
*   **Functionality**: Manage call lists derived from Activities.
*   **Workflow**:
    *   **Lists**: "Employee Pipeline" items (leads) assigned to sales reps.
    *   **Execution**: Log call records (Time, Topic, Remark), update contact info, and manage recommended products.
    *   **Assignment**: Supervisors can assign/reassign leads to specific sales reps.
    *   **Status Tracking**: Track progress from "New" to "Call Completed".

### 4.3 Pipeline Management (Sales Opportunities)
*   **Functionality**: Manage sales opportunities from initial contact to closing.
*   **Key Features**:
    *   **Opportunity**: Name, Estimated Amount, Stage, Probability, Expected Close Date.
    *   **Relations**: Link to Company and Contacts (identifying decision-makers).
    *   **Products**: Add products to the opportunity to auto-calculate amounts.
    *   **Invoicing**: Track invoice issuance and payment status.
    *   **Conversion**: "Won" opportunities can be converted directly into **Projects** in the Work module.

---

## 5. Work Management Module

**Purpose**: Execution and tracking of sold projects and internal tasks.

### 5.1 Project Management
*   **Functionality**: Manage the lifecycle of a project.
*   **Key Features**:
    *   **Project Data**: Code, Name, Start/End Dates, Status.
    *   **Members**: Assign employees to the project with specific roles.
    *   **Milestones**: Define key phases (Stones) with deadlines and pre-notifications.
    *   **Financials**: Track contract documents and project expenses (Bill number, Amount).
    *   **Dashboard**: Overview of delayed projects, risks, and unassigned tasks.

### 5.2 Job (Task) Management
*   **Functionality**: Granular task management within Project Milestones.
*   **Key Features**:
    *   **Jobs**: Specific tasks with assigned hours and executors.
    *   **Checklists (Buckets)**: Simple to-do lists within a Job for tracking micro-steps.
    *   **Work Records (Logs)**:
        *   Employees log actual work done (Start/End time, Remarks, Attachments).
        *   Supports **Independent Work Records** (logs not tied to a specific project) for general administrative tasks.
    *   **Validation**: Ensures log times are valid (Start < End).

---

## 6. Technical Stack & Architecture

*   **Backend**: ASP.NET Core Web API
    *   **Controller Layer**: `ProjectManagerWebApi/Controllers` (e.g., `MbsBasicController`, `MbsSystemRegionController`).
    *   **Service Layer**: `ServiceLibrary/ManagerBackSite/Logical` (Business logic).
    *   **Data Layer**: `DataModelLibrary` (Entity Framework Core entities).
*   **Frontend**: Vue.js (Vite)
    *   **Project**: `FrontEnd/manager-back-site`.
    *   **State Management**: Pinia.
    *   **Routing**: Vue Router (maps to Controller modules).
    *   **UI Library**: Tailwind CSS (inferred from config), Custom Components (Modals, Tables).

---

## 7. Known Constraints & Boundaries

*   **Deletions**: Many "Delete" operations (e.g., Client, Activity) are restricted or soft-deletes in the current version to maintain data integrity.
*   **Validation**: String lengths (e.g., Names 10-50 chars), unique constraints (Account, Tax ID), and format checks (Email, Taiwan Phone formats) are strictly enforced.
*   **Permissions**: All critical actions require valid `LoginToken` and check against the `EmployeePermission` table.
*   **System Data**: IDs < 0 are reserved for system use (e.g., Global Region, Admin Dept) and cannot be modified/deleted.
