# POC 分支/階段設計草案（商機管理 ↔ 專案管理）

## 目標
- POC 是商機內的一個「分支/階段」，不另建新專案。
- 業務在商機 30% 建立 POC，選部門與指派人員後即可開始追蹤。
- 工程/顧問在專案管理的 POC tab 更新進度，回寫商機端 POC 區塊。
- POC 結果可記錄「會下單 / 不會下單」與原因。

## 流程
1. 業務建立商機（30%）。
2. 於商機頁的 POC 區塊建立 POC，先選部門，再選工程師/顧問（或由部門經理指派）。
3. 儲存後：
   - 綁定現有專案（同一專案分支/階段）。
   - 生成 POC 分支紀錄，並套用標準階段待辦。
4. 工程/顧問在專案管理的 POC tab 更新「進程/追蹤待辦」，回寫商機 POC 區塊。
5. 業務在商機頁點 POC，可直接跳到該專案的 POC tab。
6. POC 結果：會下單 / 不會下單；不會下單需填原因（含其他可自填）。

## 商機管理（POC 區塊欄位）
- pocDurationWeeks：POC 週期（2~4）
- pocProgress：進程（文字）
- pocTodo：追蹤待辦（文字）
- pocResult：會下單 / 不會下單
- pocRejectReason：工程師技術不到位 / 業務溝通問題 / 產品功能不符合需求 / 其他
- pocRejectReasonNote：若選其他，需輸入文字
- pocDepartmentId：部門（必選）
- pocAssigneeId：指派人員（工程師/顧問）

## 專案管理（POC tab）
- 顯示該專案的 POC 分支資訊（週期、進程、待辦、結果）。
- 顯示標準階段待辦（沿用模板）。
- 工程/顧問更新進度時，回寫商機 POC。

## 串接關係
- EmployeePipeline.ID ↔ EmployeeProject.EmployeePipelineID
- POC 為同一專案內的「分支/階段」，不新增專案。
- POC 記錄綁定至 EmployeeProjectID + EmployeePipelineID。

## API 清單（建議）
CRM/商機端
- GET `/crm/pipeline/{pipelineId}/poc`
- POST `/crm/pipeline/{pipelineId}/poc`
- PUT `/crm/pipeline/{pipelineId}/poc`
- PUT `/crm/pipeline/{pipelineId}/poc/result`

專案端
- GET `/work/project/{projectId}/poc`
- PUT `/work/project/{projectId}/poc`
- PUT `/work/project/{projectId}/poc/progress`

指派/列表
- POST `/work/project/{projectId}/poc/assign`
- GET `/work/project/poc/assignee/{employeeId}`

## 前端頁面位置（路徑）
- 商機詳情：`/crm/pipeline/pipeline/detail/:pipelineId`
  - 於 30% 商機階段顯示 POC 區塊
- 專案詳情：`/work/project/detail/:id`
  - 新增 POC tab（工程/顧問更新進度）

## DB 欄位明細（建議）
既有：
- EmployeePipeline.ID (PK)
- EmployeeProject.ID (PK)
- EmployeeProject.EmployeePipelineID (FK)

新增（建議新增一張 POC 分支表）
### EmployeeProjectPOC
- ID (PK)
- EmployeePipelineID (FK)
- EmployeeProjectID (FK)
- DepartmentID
- AssigneeEmployeeID
- DurationWeeks (int)
- ProgressText (text)
- TodoText (text)
- Result (enum: Won/Lost/Unset)
- RejectReason (enum + Other)
- RejectReasonNote (text, nullable)
- UpdatedBy
- UpdatedAt

可選擴充（若需階段待辦追蹤）
### EmployeeProjectPOCStage
- ID (PK)
- EmployeeProjectPOCID (FK)
- StageTemplateID
- StageName
- Status (NotStarted/InProgress/Done)
- DueDate

## 介面行為摘要
- 商機端 POC 區塊與專案 POC tab 共用同一筆 POC 資料。
- 若選「不會下單」則必填 RejectReason；選「其他」才顯示 RejectReasonNote。
- 只有部門經理/工程主管可改指派人員。
