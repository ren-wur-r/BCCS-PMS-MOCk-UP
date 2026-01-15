# Tele-Sales Lead Pool

## Overview
This module creates a dedicated "lead pool" for tele-sales. Leads can enter the pool from:
- Activity transfer (活動轉電銷) after calling.
- Manual creation by tele-sales (電銷自建).

The lead pool serves as a single backlog for follow-up and assignment, independent of a specific activity.

## UX Goals
- One list for all tele-sales leads (regardless of source).
- Clear filters for quick triage (industry, customer value, call date, evaluation range).
- Low-friction add flow (single button near list).
- No horizontal scrolling; columns fit into the table layout.

## Current UI Notes
- Two tabs at tele-sales entry: "活動轉電銷" and "電銷名單".
- Activity transfer flow can multi-select leads and add them to the pool.
- Lead pool includes a "新增名單" button styled like the Add Opportunity button.

## Local Mock Storage (Frontend)
- localStorage key: `cache.crm.phone.leadPool`
- Created from activity pipeline or manual add.

