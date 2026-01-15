# Changelog - Frontend Mocking for Local Development

**Date:** 2026-01-06
**Author:** Gemini Agent

## Overview
Due to the unavailability of the remote SQL Server database in the local development environment, the authentication and dashboard data retrieval processes have been mocked in the frontend to allow for UI/UX development and testing.

## Changes

### 1. Authentication Mocking
*   **File:** `FrontEnd/manager-back-site/src/services/pms-http/basic/basicHttpService.ts`
*   **Change:**
    *   Replaced the implementation of `login` function. It now ignores the backend API and returns a simulated success response (`MbsErrorCodeEnum.Success`) with a fake token after a 500ms delay.
    *   Replaced the implementation of `getEmployee` function. It now returns a hardcoded "Mock Admin" user profile with "General Manager / Admin" role and full permissions (`Everyone` access) for all defined menus.
    *   **Added**: Replaced the implementation of `heartbeat` function to always return `MbsErrorCodeEnum.Success` after a 200ms delay, preventing continuous logout/login loops when the backend is unavailable.
    *   **Added**: Replaced `getAllBasicManagerDepartment` and `getAllBasicManagerRegion` to return mock data (Mock Dept, Mock Region) to allow the login flow to complete without errors.
*   **Action Required for Production:**
    *   **Revert** the `login`, `getEmployee`, `heartbeat`, `getAllBasicManagerDepartment`, and `getAllBasicManagerRegion` functions to their original state (calling `apiJsonClient.post`) to restore actual backend authentication and permission checks.

### 2. Dashboard Data Mocking
*   **File:** `FrontEnd/manager-back-site/src/services/pms-http/dashboard/dashboardHttp.ts`
*   **Change:**
    *   Replaced `mbsDashboardHttpGetInfo` to return static mock data (Total Gross Profit, Risk Statistics, etc.) instead of calling the backend API.
*   **Action Required for Production:**
    *   **Revert** `mbsDashboardHttpGetInfo` to call the backend endpoint `/api/ManagerBackSite/MbsDashboard/GetInfo`.

### 3. Mock Data Store
*   **File:** `FrontEnd/manager-back-site/src/stores/mockDataStore.ts`
*   **Change:**
    *   Created a new Pinia store to handle frontend-only data for "Announcements" and "Certificates" (CSV import) since the backend database tables for these features do not exist yet.
*   **Action Required for Production:**
    *   Once the backend tables and APIs for `Announcement` and `EmployeeCertificate` are implemented, this store should be replaced or refactored to fetch data from the API.

## Summary of Reversion Steps
To restore full production functionality:
1.  Open `FrontEnd/manager-back-site/src/services/pms-http/basic/basicHttpService.ts`.
2.  Restore the `apiJsonClient.post` calls in `login`, `getEmployee`, `heartbeat`, `getAllBasicManagerDepartment`, and `getAllBasicManagerRegion`.
3.  Open `FrontEnd/manager-back-site/src/services/pms-http/dashboard/dashboardHttp.ts`.
4.  Restore the `apiJsonClient.post` call in `mbsDashboardHttpGetInfo`.

---

## 2026-01-06 - Dashboard & Pipeline Enhancements

### Dashboard fixes & extensions
- Updated `FrontEnd/manager-back-site/src/views/home/components/{StaffDashboard.vue,SalesDashboard.vue,ManagerDashboard.vue,GMDashboard.vue}` to use `MbsErrorCodeEnum.Success` instead of a hard-coded `0/1`, preventing successful API responses from being discarded and resulting tiles from constantly showing `0`.
- Added a pipeline tab for the sales dashboard that mirrors the general manager view by surfacing personal 10%/30%/70%/100% stage counts and a drill-down table fed by `PersonalPipelineStageDetails`.

### Project management navigation
- Simplified `FrontEnd/manager-back-site/src/views/work/project/WorkProjectList.vue` so the screen focuses on the searchable list; the legacy dashboard tab, API call, and supporting UI/state were removed because project health metrics now live on the global dashboard.

### Opportunity stage logic
- Refined the stage-condition section in `FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue` to document the 10%/30%/70% criteria, relaxed/updated the validation mapping accordingly, and presented the new drop-downs plus optional remark area that the sales team can use before promoting a stage.

## 2026-01-07 - Opportunity Surfacing & Activity UX

### Dashboard opportunity visibility
- `FrontEnd/manager-back-site/src/views/home/components/SalesDashboard.vue` now surfaces personal pipeline percentages next to raw counts, adds guidance that the cards include in-progress opportunities, and allows clicking a row in the drill-down table to jump directly to `/crm/pipeline/pipeline/detail/:id`.
- `FrontEnd/manager-back-site/src/views/home/components/GMDashboard.vue` mirrors the same percentage badges, explanatory copy, and click-through navigation so general managers can inspect both regionalised counts and individual opportunities without leaving the dashboard.

### Activity/telemarketing list UX refresh
- `FrontEnd/manager-back-site/src/views/crm/activity/activity/CrmActivityActivityList.vue` and `.../crm/phone/activity/CrmPhoneActivityList.vue` drop the inline “操作” menu in favor of row-level navigation. Hovering now highlights the full row (when the user has view permission), and clicking jumps straight into the activity detail tab view.
- Introduced a reusable tab header component `FrontEnd/manager-back-site/src/components/feature/activity/ActivityDetailTabs.vue` and wired it into every activity-related surface: CRM activity detail/list/pipeline-detail (`CrmActivityActivityDetail.vue`, `CrmActivityPipelineList.vue`, `CrmActivityPipelineDetail.vue`) and the telemarketing counterparts (`CrmPhoneActivityDetail.vue`, `CrmPhonePipelineList.vue`, `CrmPhonePipelineDetail.vue`). Users can now flip between “檢視活動”與“檢視名單” via tabs instead of the former standalone buttons/routes, while existing deep links such as `/pipeline/detail/:id` continue to function.
- Removed the redundant “檢視名單” buttons from detail pages and ensured each list and pipeline-detail view shows the tab bar so the experience stays consistent no matter which tab is active.

## 2026-01-07 - Marketing Module Front-End Test Spec

- Added `marketing.md` to describe the standalone front-end testing plan for the marketing (活動/電銷) module, including the visual style guide, tab interaction model, mock-data expectations for活動/名單/商機、商機百分比條件、以及匯入流程與權限假設，so the UI team can build a demo without backend support.
- Documented ten currently available backend activities/telemarketing entries (time range, name, type, counts) so the FE team has ready-to-use mock datasets when seeding the demo.

## 2026-01-07 - Frontend Mock Data (CRM + Work)

- Expanded mock datasets for CRM activities, CRM pipelines, and phone sales customers, and added work project/job mock data to support UI testing without the backend.
- Updated `FrontEnd/manager-back-site/src/services/mockApi/mockApiClient.ts` to serve CRM activity list/detail, CRM pipeline list/detail, phone sales list/detail, work project list, and work job list endpoints.
- Fixed mock heartbeat endpoint naming and return codes so login/heartbeat flows stay alive while running with mock data.
- Improved mock pagination parsing to accept different request shapes used across modules.
- Enabled mock mode in development via `FrontEnd/manager-back-site/.env.development` with `VITE_USE_MOCK_DATA=true`.

## 2026-01-07 - Project Management List & Spec Update

- Updated the project management list layout to remove the “操作” column and added a hint that rows are clickable for detail navigation (`projectManagement.md`).
- Recorded the new project-type stage template, work-log, and alerting requirements in the work project spec, plus an implementation log entry (`.specify/spec/work_project_spec.md`).

## 2026-01-07 - Work Project List & Type Template Draft UI

- Made work project list rows clickable with hover/cursor affordance and removed the action column (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectList.vue`).
- Added a draft project-type/stage-template selector with reminder rules and required outputs in the project add form (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).

## 2026-01-07 - Project Template Settings & Stage Outputs

- Added shared seed data for project types, stages, outputs, and contract templates (`FrontEnd/manager-back-site/src/views/work/project/projectTemplateSeeds.ts`).
- Created a standalone settings page to add/edit/sort project types, stages, outputs, and bind contract templates (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectTemplateSettings.vue`, `FrontEnd/manager-back-site/src/router/index.ts`).
- Replaced fixed contract/work sections in project detail with stage-based outputs, reminder rules, and work log drafting UI (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).

## 2026-01-08 - Project Detail Upload UI & Mock Fixes

- Added mock responses for `/api/MbsWorkProject/GetProject` and `/api/MbsWorkProject/GetManyMemberRole` to prevent empty error dialogs in mock mode (`FrontEnd/manager-back-site/src/services/mockApi/mockApiClient.ts`).
- Switched stage output UI from URL input to file upload list with status labels and local draft state (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Replaced add-project URL inputs with a note that uploads occur per stage after project creation (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).
- Documented backend linkage, fields, and NAS/SharePoint upload mechanism (`.specify/spec/work_project_backend_relation.md`).

## 2026-01-08 - System Project Templates & Master Data Merge

- Added a system settings module for contract types and standard stages, with local persistence (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`, `FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts`).
- Moved standard stage outputs into a dedicated project detail tab and sourced data from system settings (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Updated project add flow to reference contract types and link to system settings (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).
- Merged system master data (contacter/company/product/employee/role/department/region) into a tabbed module and updated navigation (`FrontEnd/manager-back-site/src/views/system/master/SystemMasterData.vue`, `FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue`, `FrontEnd/manager-back-site/src/router/index.ts`).

## 2026-01-08 - Project Template Source & Tabs

- Switched standard stage/output sourcing to system settings (contract type + standard stage), and exposed as a dedicated project tab (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Added system modules for contract types and standard stages plus a merged master-data module (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`, `FrontEnd/manager-back-site/src/views/system/master/SystemMasterData.vue`).

## 2026-01-08 - Project Template Defaults & Service-Item Stage Editing

- Renamed system menu and route title to “專案標準階段” and removed the standalone standard-stage tab, moving stage management under the service item page (`FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue`, `FrontEnd/manager-back-site/src/router/index.ts`, `FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`).
- Seeded default project types (單一服務案 / 混合案), service items (雲安維運、聯防設備、顧問諮詢、風險檢測、資安學苑、內部軟體開發、外部軟體開發), and linked per-service stage templates (`FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts`).
- Preloaded “聯防設備” stage template with the 資安健診 six-stage flow and outputs from `referenceOfProcess/isDiagnostic.md` (`FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts`).
- Added schema versioning and auto-migration to reset legacy localStorage to the new defaults (preserving reminder rule) (`FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts`).
- Added a stage-summary copy block sourced from 資安健診流程 and fixed template tag mismatch in the system settings page (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`).
- Switched system project-type and service-item edits to in-place forms, removed extra summary cards, and simplified service-item list tiles (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`).
- Added drag-and-drop sorting for stage outputs with `vuedraggable@next` and persisted ordering to localStorage (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`, `FrontEnd/manager-back-site/package.json`, `FrontEnd/manager-back-site/package-lock.json`).
- Replaced stage summary copy with last-editor/last-edited timestamp and removed reminder-rule text (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`, `FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts`).
- Made output edit fields responsive (single column on narrow screens, three columns on wide screens) (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`).
- Added per-stage reminder settings (3/5/7 days, project start/end anchor) and effort estimates (days/hours with required flags) in stage templates (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`, `FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts`).
- Enlarged checkbox controls for output required flags and stage options; aligned stage/edit and output edit layouts (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`).
- Added custom output type labels when selecting “其他”, stored as `outputKindLabel`, plus a storage proposal doc (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`, `FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts`, `docs/specs/output_kind_storage.md`).
- Replaced stage header copy with last editor/time, removed reminder-rule line, and fixed layout alignment for stage/output edit rows (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`).
- Improved output edit row spacing/alignment and RWD layout for consistent horizontal alignment on wide screens (`FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`).

## 2026-01-09 - Project Naming Draft

- Updated draft rules for auto-generated project name and project code (industry/region + YYMMDD + sequence) in `projectStandardSetting.md`.

## 2026-01-10 - Project Add Flow, Org Mapping, Mock Mode, and Project List Tabs

### Work project add flow (modal + steps)
- Moved “+新增專案” to open a modal add flow and implemented a multi-step wizard with preview confirmation in `FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue` and `FrontEnd/manager-back-site/src/views/work/project/WorkProjectList.vue`.
- Added region selection (A/B/C), project code auto-generation `P + 區域 + 民國年月日 + 序號`, and project name auto-generation from service item + product selections (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).
- Implemented AtomCity → 北/中/南區 mapping and auto-fill region from company location (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).
- Reworked step 1/2/3 UI to align with the project-type/services layout; ensured date range is date-only and buttons are centered (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).

### Project members (mock-driven)
- Added default “操作者/總經理” row, service-based default department inference, and a new “新增專案人員” row action (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).
- Allowed “跨區” selection for member region, filtered departments by region and org allowlist, and normalized department names to avoid stray prefixes (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).
- Switched employee and company combo boxes to mock-first behavior for local testing (`FrontEnd/manager-back-site/src/components/feature/search-bar/GetManyEmployeeComboBox.vue`, `FrontEnd/manager-back-site/src/components/feature/search-bar/GetManyManagerCompanyComboBox.vue`).

### Mock mode enforcement
- Forced mock API usage for local development and added mock endpoints for company/department/employee list flows (`FrontEnd/manager-back-site/src/services/httpClient.ts`, `FrontEnd/manager-back-site/src/services/mockApi/mockApiClient.ts`).

### Organization data
- Rebuilt `orgMemberDirectory.ts` from `organization/orgMember.md` and normalized department naming to match `organization/org.md` (`FrontEnd/manager-back-site/src/constants/orgMemberDirectory.ts`).
- Generated and used an org department directory allowlist for department filtering (`FrontEnd/manager-back-site/src/constants/orgDepartmentDirectory.ts`).

### Project list tabs
- Removed the search row, removed the per-row status badge, and added status tabs with counts to filter project lists (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectList.vue`).

### Project detail + milestones (mock)
- Cleaned up the “標準階段與產出” template structure and ensured stage cards render without extra headers (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Removed the 工項 tab button while keeping milestone display driven by standard stages (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Added mock endpoints for project stone/task/expense lists to avoid non-domain API errors (`FrontEnd/manager-back-site/src/services/mockApi/mockApiClient.ts`).
- Updated stage view header to include selected product names, removed product card wrappers, and moved upload help text to hover tooltip (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Fixed expense table markup and closed missing template containers (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).

### CRM pipeline list actions
- Added explicit 檢視/修改 actions and ensured +新增商機 always navigates to add (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue`).

### Project assignment alerts
- Added mock assignment list with actions and pending alert banner, seeded one assignment for the current user (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAssign.vue`).
- Added a pending-assignment red dot indicator on the sidebar item (`FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue`).
- Raised navbar/dropdown z-index to avoid UI overlap (`FrontEnd/manager-back-site/src/components/global/layout/Navbar.vue`).
- Added collapsible stage sections with up/down toggle in project detail (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).

### Mixed project grouping
- Grouped product selection and stage previews by service item for mixed projects, improving readability (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).
- Split project member assignment by service item and kept fixed members separate (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue`).
- Split project detail stage view by service item with per-service headers (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Added project type column to the project list (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectList.vue`).

### CRM pipeline basic info
- Added customer region/name/unified number to basic info and tied display status to stage condition auto status (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/sections/PipelineBasicInfoSection.vue`, `FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).

### Project detail navigation
- Reloaded project detail context when route param changes so list clicks render the correct project (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Replaced the stage section header with the selected service/product title (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Added per-stage owner filtering with a “顯示全部階段” toggle (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue`).
- Updated tab styling to pill-style tabs (`FrontEnd/manager-back-site/src/styles/main.css`).
- Added spacing below project status tabs (`FrontEnd/manager-back-site/src/views/work/project/WorkProjectList.vue`).
- Added bottom spacing for all tabs to keep separation from tables (`FrontEnd/manager-back-site/src/styles/main.css`).
- Widened main content area by aligning layout width with the sidebar size (`FrontEnd/manager-back-site/src/layouts/DefaultLayout.vue`).
- Added a multi-step “新增商機” modal and local mock persistence (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`, `FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue`, `FrontEnd/manager-back-site/src/services/mockApi/mockApiClient.ts`).
- Expanded the 商機新增基本資訊 step with stage condition table, customer region, and auto-filled owner (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Added 商機類型/服務項目/產品/標準階段 selection to 商機新增第一步 (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Derived 商機狀態 from the condition table and show it only in the confirmation step (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Aligned customer region/customer/unified number fields into one row and matched dropdown styling (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Switched stage status to quick-select buttons and restored notes as text input (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Shortened condition labels and tightened table column widths in 商機條件表 (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Removed condition descriptions from the 商機條件表 (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Removed the description column from the 商機條件表 (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Adjusted 商機條件表 column widths to widen status and tighten notes (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Added "取消建立" action to the 商機新增 footer and removed the header cancel button (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Enabled quick-select status buttons to toggle off when clicked again (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Added a presenter dropdown for the 簡報備註 field (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Customized status labels per condition and gated the presenter dropdown to 已簡報 only (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue`).
- Synced 商機詳情「階段條件紀錄」表格 with 新增商機 behavior (buttons, labels, presentation dropdown) (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Fixed duplicate import for DbEmployeePipelineStageCheckEnum (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Converted POC 區塊 to a list-based assignment table with department + assignee selection (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Removed horizontal scroll wrappers and tightened condition column width in 商機詳情 (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Added region selection to POC assignments and further tightened condition column width (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Split POC assignment rows into two lines with an added remark field (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Separated POC table headers into two aligned header rows (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Split POC assignment layout into two separate tables (assignment vs. result/notes) (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Updated POC block to start empty and show a centered add button until rows exist (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Filtered POC department options by selected region (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Swapped the POC action button to “刪除 POC” since POC is single-instance (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`).
- Locked 商機 editing and auto-opened project transfer when reaching 100%, while keeping view-only access (`FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue`, `FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue`).
- Added 商機類型/服務項目/產品/標準階段選擇區塊 to 新增商機 and aligned stage preview with 新增專案規則 (產品必選、依產品顯示階段與產出) (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue).
- Matched 新增專案的服務項目/產品選取規則 (切換類型清空選擇、產品多選) (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue).
- Standardized template product description binding to template settings fields (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue).
- Added Booking Code field and region label mapping in 新增商機確認資訊 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue).
- Mapped 客戶所在地區 to 北/中/南 based on城市 in 商機基本資訊 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/sections/PipelineBasicInfoSection.vue).
- Filled mock CRM detail city from商機選擇區域 (FrontEnd/manager-back-site/src/services/mockApi/mockApiClient.ts).
- Added Booking Code input and region label display in 新增商機基本/確認資訊 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue).
- Ensured list merges local cached商機 to show newly created items immediately (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue).
- Mapped 客戶所在地區 to 北/中/南 in 商機基本資訊 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/sections/PipelineBasicInfoSection.vue).
- Derived mock CRM detail city from新增商機的區域選擇 (FrontEnd/manager-back-site/src/services/mockApi/mockApiClient.ts).
- Added Booking Code display in 商機詳情基本資訊 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/sections/PipelineBasicInfoSection.vue, FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue).
- Simplified 商機基本資訊狀態顯示為 10/30/70/100 與成立專案，移除進度條區塊 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/sections/PipelineBasicInfoSection.vue).
- Enabled Booking Code editing before 100%/成立專案 and persisted to local cache (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/sections/PipelineBasicInfoSection.vue, FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue).
- Standardized POC add button width to full container and hid 備註 until POC exists (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue).
- Simplified 商機基本資訊狀態顯示與 Booking Code 可編輯邏輯 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/sections/PipelineBasicInfoSection.vue, FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue).
- Added Booking Code field to 新增商機與確認頁、並顯示客戶所在地區名稱 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue).
- Ensured 新增商機即時顯示於列表並補上區域對應城市 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue, FrontEnd/manager-back-site/src/services/mockApi/mockApiClient.ts).
- Adjusted 新增商機服務項目顯示與選取規則 (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue).
- Added last-updated timestamp for 商機階段/POC edits and surfaced it in the 階段條件紀錄 header (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue).
- Added 待接單列表並用電銷交接資料建立商機接單流程 (FrontEnd/manager-back-site/src/views/crm/pipeline/handoff/CrmPipelineHandoffList.vue, FrontEnd/manager-back-site/src/router/index.ts, FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue).
- Rebuilt 電銷管理列表為可群組的名單卡片視圖，含查詢條件、看板/列表切換、群組與選取控制 (FrontEnd/manager-back-site/src/views/crm/phone/pipeline/CrmPhonePipelineList.vue).
- Added global role switcher in the header and removed the dashboard-only switcher (FrontEnd/manager-back-site/src/components/global/layout/Navbar.vue, FrontEnd/manager-back-site/src/views/home/components/DashboardContainer.vue).
- Applied role-based menu visibility and permissions for quick testing (FrontEnd/manager-back-site/src/stores/employeeInfo.ts, FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue).
- Added GM 商機圓餅圖與區域篩選限制為北/中/南 (FrontEnd/manager-back-site/src/views/home/components/GMDashboard.vue).
- Added dashboard mock fallback data so offline roles show counts and charts (FrontEnd/manager-back-site/src/utils/buildDashboardMockData.ts, FrontEnd/manager-back-site/src/views/home/components/GMDashboard.vue, FrontEnd/manager-back-site/src/views/home/components/ManagerDashboard.vue, FrontEnd/manager-back-site/src/views/home/components/StaffDashboard.vue, FrontEnd/manager-back-site/src/views/home/components/SalesDashboard.vue).
- Added product manager dashboard card for presentation participation count (FrontEnd/manager-back-site/src/views/home/components/StaffDashboard.vue).
- Updated staff dashboard metrics by role (tele-sales/activity/project) using mock data sources instead of generic project totals (FrontEnd/manager-back-site/src/views/home/components/StaffDashboard.vue, FrontEnd/manager-back-site/src/services/mockApi/mockDataSets.ts, FrontEnd/manager-back-site/src/views/crm/phone/pipeline/CrmPhonePipelineList.vue).
- Added project change request flow with submission and approval list (FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue, FrontEnd/manager-back-site/src/views/work/project/WorkProjectAssign.vue).
- Restricted standard stage hour inputs to department managers in project template settings (FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue).
- Added product search and default 5-item list with expand/collapse in 標準階段設定 (FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue).
- Added stage-template copy workflow for products with confirmation modal and cloned template IDs (FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue).
- Disabled 人天/時數欄位 for non-department managers in stage edit form (FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue).
- Added 異動紀錄最後更新時間 and pending-approval notice in 異動核可 / 異動申請 sections (FrontEnd/manager-back-site/src/views/work/project/WorkProjectAssign.vue, FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue).
- Added sidebar notification dot for pending 異動核可 (FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue).
- Seeded 聯防設備產品清單（B_A_ENG/B_B_ENG/B_C_ENG）並在載入時補齊缺失資料 (FrontEnd/manager-back-site/src/data/jointDefenseProducts.ts, FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts).
- Added 活動人員儀表板列表：即將到來活動與未轉電銷活動 (FrontEnd/manager-back-site/src/views/home/components/StaffDashboard.vue).
- Adjusted 產品顯示：移除 Renewal/續約字樣並避免重複品牌 (FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue).
- Added 續約/委外選項到新增專案與新增商機，並在列表顯示案別/委外欄位 (FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue, FrontEnd/manager-back-site/src/views/work/project/WorkProjectList.vue, FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/components/CrmPipelinePipelineAddModal.vue, FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue).
- Seeded project mock data with 續約/委外 flags (FrontEnd/manager-back-site/src/services/mockApi/mockDataSets.ts).
- Normalized main layout padding to 10px and moved sidebar spacing to container padding to reduce blank areas (FrontEnd/manager-back-site/src/layouts/DefaultLayout.vue).
- Added safe store export for project template settings to fix async dashboard import error (FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts).
- Matched POC “新增 POC” button width to the standard full-width dashed action style (FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue).
- Switched ManagerDashboard “注意” card to yellow styling for consistency (FrontEnd/manager-back-site/src/views/home/components/ManagerDashboard.vue).
- Added 量能管理 module with capacity records stored locally and role-based edit permissions (FrontEnd/manager-back-site/src/views/work/capacity/WorkCapacityManage.vue, FrontEnd/manager-back-site/src/stores/capacityStore.ts).
- Added capacity utilities for assigned-hours estimation from project membership (FrontEnd/manager-back-site/src/utils/capacityUtils.ts).
- Added capacity summary and overload/idle warnings to department manager dashboard (FrontEnd/manager-back-site/src/views/home/components/ManagerDashboard.vue).
- Surfaced remaining capacity in 指派管理 list (FrontEnd/manager-back-site/src/views/work/project/WorkProjectAssign.vue).
- Wired 量能管理 navigation and route (FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue, FrontEnd/manager-back-site/src/router/index.ts).
- Seeded certification records from organization/certification.md and defaulted mock certificates to the seeded list (FrontEnd/manager-back-site/src/constants/orgCertificationDirectory.ts, FrontEnd/manager-back-site/src/stores/mockDataStore.ts).
- Added GM dashboard certificate total card with region filtering (FrontEnd/manager-back-site/src/views/home/components/GMDashboard.vue).
- Fixed certification mapping to align with 合計欄位 and forced seed refresh when version changes (FrontEnd/manager-back-site/src/constants/orgCertificationDirectory.ts, FrontEnd/manager-back-site/src/stores/mockDataStore.ts).
- Re-generated certification seed from updated organization/certification.md and bumped seed version to refresh counts (FrontEnd/manager-back-site/src/constants/orgCertificationDirectory.ts, FrontEnd/manager-back-site/src/stores/mockDataStore.ts).
- Replaced certification source with full employee table and refreshed seed version for counts (organization/certification.md, FrontEnd/manager-back-site/src/constants/orgCertificationDirectory.ts, FrontEnd/manager-back-site/src/stores/mockDataStore.ts).
- Removed 電銷管理 page header and added mandatory rejection reason modal for 異動核可; project names now wrap per service item (FrontEnd/manager-back-site/src/views/crm/phone/activity/CrmPhoneActivityList.vue, FrontEnd/manager-back-site/src/views/work/project/WorkProjectChangeManage.vue).

## 2026-01-15 - UI/Workflow Updates

- Scoped annotations to active tab/page/visibility, added scroll/resize rebuilds, and prevent bubbles showing behind modals (FrontEnd/manager-back-site/src/components/global/annotation/AnnotationOverlay.vue, FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue, FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue).
- Synced dashboard tabs to query params to isolate per-tab annotation states (FrontEnd/manager-back-site/src/views/home/components/SalesDashboard.vue, FrontEnd/manager-back-site/src/views/home/components/GMDashboard.vue).
- Added project sub-route titles and split 文件管理 into its own module under 專案管理 (FrontEnd/manager-back-site/src/router/index.ts, FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue, FrontEnd/manager-back-site/src/views/work/project/WorkProjectFileManage.vue, FrontEnd/manager-back-site/src/views/work/project/WorkProjectChangeManage.vue).
- Added file review statuses + send-review actions to standard stages and split work logs into 階段/日常 tabs (FrontEnd/manager-back-site/src/views/work/project/WorkProjectDetail.vue).
- Upgraded 文件管理 to assistant/manager dual-status reviews with role-based actions and legacy data normalization (FrontEnd/manager-back-site/src/views/work/project/WorkProjectFileManage.vue).
- Hid 公司公告 block and removed 電銷客戶管理 from sidebar menu (FrontEnd/manager-back-site/src/views/home/components/AnnouncementBoard.vue, FrontEnd/manager-back-site/src/components/global/layout/Sidebar.vue).
- Added product search for 신규專案產品挑選 and adjusted 商機狀態欄位寬度/badge sizing (FrontEnd/manager-back-site/src/views/work/project/WorkProjectAdd.vue, FrontEnd/manager-back-site/src/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue).

## 2026-01-12 - Consultant Template Updates

- Added consultant service template stages, outputs, Onsite/Offsite hours, and deadline settings (FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts, FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue).
- Documented consultant template mapping in process reference (referenceOfProcess/consultantProcess/consultantProjectStart.md).
