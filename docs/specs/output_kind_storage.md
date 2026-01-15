# Output Type Extension - Storage Design Proposal

Goal: support custom output type labels when users select "其他" in output templates, and persist it for later rendering and audit.

## Frontend (current)
- Output template object includes `outputKind` (enum) and optional `outputKindLabel`.
- When `outputKind === "other"`, UI shows an input for `outputKindLabel`.

## Backend Data Model Options
### Option A: Add a column
- Table: `ProjectStageOutputTemplate`
- New column: `OutputKindLabel` (nvarchar(50) or nvarchar(100), nullable)
- Rules:
  - If `OutputKind != Other`, `OutputKindLabel = NULL`
  - If `OutputKind == Other`, `OutputKindLabel` is required
- Pros: simplest, minimal joins, easy queries
- Cons: requires schema change

### Option B: Lookup table
- Table: `OutputKindDictionary`
  - `OutputKindId`, `OutputKindCode` (document/meeting/report/other), `DisplayName`
- Table: `ProjectStageOutputTemplate`
  - `OutputKindId` (FK)
  - `CustomLabel` (nullable)
- Rules: only used when `OutputKindCode == other`
- Pros: centralized list, extensible for translations
- Cons: more joins

### Option C: JSON field (if DB supports)
- Table: `ProjectStageOutputTemplate`
- New column: `OutputKindMeta` (nvarchar(max) JSON)
  - example: `{ "label": "客製類型" }`
- Pros: flexible, no new table
- Cons: harder validation/querying

## API Changes (draft)
- `AddOutputTemplate` / `UpdateOutputTemplate`:
  - Add `OutputKindLabel` (string, optional)
  - Validation: required when `OutputKind == Other`
- `GetStageTemplate`:
  - Return `OutputKindLabel`

## Validation
- `outputKind == other` AND `outputKindLabel` empty -> reject
- `outputKind != other` -> clear label

