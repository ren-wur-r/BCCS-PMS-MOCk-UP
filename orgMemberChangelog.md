# orgMemberChangelog

**Date:** 2026-01-10

## Overview
Consolidated org member data to ensure mock-only employee search works correctly in the project add flow, and normalized department names to match `organization/org.md`.

## Changes
- Rebuilt `FrontEnd/manager-back-site/src/constants/orgMemberDirectory.ts` directly from `organization/orgMember.md` to avoid drift.
- Normalized department labels to match org definitions (e.g., `高雄業務部` → `業務部`, `高雄工程部` → `工程部`, `安全技術實驗室` → `安全實驗室`).
- Removed stray `#` prefixes in department names so member filtering and display are consistent.
- Updated employee dropdown mock logic to filter by region/department and fall back to region/all-company when filters return empty (`FrontEnd/manager-back-site/src/components/feature/search-bar/GetManyEmployeeComboBox.vue`).
