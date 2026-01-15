# Spec - System Settings (Project Types / Service Items / Stage Templates)

## Overview
Defines delivery templates that drive project creation: project type (single/hybrid), service items, stage templates, outputs, and alert rules. This replaces the legacy “contract type” concept.

## Frontend Logic
- Route: `/system/project-template`.
- View: `FrontEnd/manager-back-site/src/views/system/project-template/SystemProjectTemplateSettings.vue`.
- Store: `FrontEnd/manager-back-site/src/stores/projectTemplateSettings.ts`.
- Data persistence: localStorage (frontend-only) with migration from prior contract-type format.
- UI tabs: Project Types, Service Items (stage management is inside the Service Item page).
- Editing: all edits are in-place within the same view (no separate edit panels).
- Output ordering: drag-and-drop via `vuedraggable@next`, persisted to localStorage.

### Project Type Rules (confirmed)
- Category: single (one service item) or hybrid (multi-service item).
- Hybrid projects display milestones separately per service item/department.
- Requires compliance attachments (Order/SLA) at project creation.

### Default Seed Data (current)
- Project types: 「單一服務案」(single), 「混合案」(hybrid).
- Service items: 雲安維運、聯防設備、顧問諮詢、風險檢測、資安學苑、內部軟體開發、外部軟體開發.
- Stage templates: one per service item by default.
- 「聯防設備」內建資安健診 6 階段與產出（來自 `referenceOfProcess/isDiagnostic.md`）。
- Service-item page shows last editor and last edited timestamp for the selected stage template.

### Storage Migration
- Introduced schema versioning; legacy localStorage is reset to new defaults while preserving reminder rules.

### Responsive Behavior
- Output edit fields (name/type/required) are a single column on narrow screens and three columns on wide screens.
- Output edit rows align horizontally on wide screens with consistent spacing and aligned action buttons.

### Stage Template Settings (Added)
- Per-stage reminder configuration: selectable 3/5/7 days with anchor on project start or project end.
- Per-stage effort estimates: planned days and hours, each with an optional required flag.

### Output Kind Extension (Added)
- When output type is “其他”, UI shows a custom label input and saves `outputKindLabel`.
- Storage design proposal documented in `docs/specs/output_kind_storage.md`.

## Backend Logic (Planned)
- See `docs/specs/work_project_backend_relation.md` for proposed data models and APIs.
- Expected endpoints:
  - Project type CRUD and ordering.
  - Stage template CRUD and ordering.
  - Output template CRUD and ordering.

## Data Entities (from planning)
- ProjectType (name, category, department scope, service items, required attachments).
- ServiceItem (name, department scope, stage template mapping).
- StageTemplate (department, stages, outputs, alert rules).
