<!--
同步影響報告
==================
版本變更: 1.0.1 → 1.0.2 (更新前端結構與技術棧)

修改原則: 無
新增章節: 無
移除章節: 無

變更內容:
  - 移除未使用的 Validation 套件 (Zod / Vee-Validate / Yup)
  - 補充前端結構：composables, constants, layouts, router, styles, utils

需更新的模板:
  ✅ plan-template.md - 無需變更 (Constitution Check 使用動態查詢)
  ✅ spec-template.md - 無需變更 (技術無關的模板)
  ✅ tasks-template.md - 無需變更 (通用任務結構)

待辦事項: 無
-->

# ProjectManagerSystem 專案憲法

## 核心原則

### I. 三層式架構

所有後端功能必須遵循三層式架構模式：
- **表現層 (Presentation Layer)**：Controllers 僅處理 HTTP 請求/回應
- **商業邏輯層 (Business Logic Layer)**：Logical services 協調商業規則
- **資料存取層 (Data Access Layer)**：Database services 透過 Entity Framework Core 處理持久化

理由：清晰的關注點分離使獨立測試、維護成為可能，並防止商業邏輯洩漏至 controllers 或資料存取程式碼中。

### II. 一致的命名規範

所有程式碼必須遵循既定的命名規範：

**後端縮寫**：
- 領域前綴：`Co` (Core), `Mbs` (ManagerBackSite), `Emp` (Employee), `Mgr` (Manager)
- 模組前綴：`Bsc` (Basic), `Sys` (System), `Crm` (CRM), `Wrk` (Work), `Erp` (ERP)
- 層級後綴：`Ctl` (Controller), `Lgc` (Logical), `Db` (Database), `Mem` (Memory)
- 模型後綴：`ReqMdl` (Request), `RspMdl` (Response)

**前端規範**：
- 元件：PascalCase（例如 `WorkProjectAdd.vue`）
- 服務：camelCase 並遵循 `{domain}{Feature}HttpService.ts` 模式
- Stores：camelCase（例如 `employeeInfo.ts`）
- Composables：`use{Feature}.ts` 模式

理由：一致的命名使人能快速識別程式碼的目的、位置和關係，無需閱讀實作細節。

### III. 型別安全優先

所有程式碼必須是強型別的：
- 前端：TypeScript 啟用 strict mode

前端程式碼禁止使用 `any` 型別（ESLint 會對 `@typescript-eslint/no-explicit-any` 發出警告）。

理由：靜態型別在編譯時期捕捉錯誤、改善 IDE 支援，並作為活文件。

### IV. 依功能組織元件

程式碼必須按功能/模組組織，而非按技術層級：

**後端結構**：
```
ServiceLibrary/
├── Core/{Domain}/           # 核心領域服務
│   ├── DB/                  # 資料庫服務
│   ├── Memory/              # 記憶體服務
│   └── Logical/             # 邏輯服務
├── ManagerBackSite/Logical/{Module}/  # 商業模組
│   ├── Format/              # Request/Response 模型
│   ├── Service/             # 介面 + 實作
│   └── Extension/           # DI 註冊
```

**前端結構**：
```
src/
├── views/{module}/              # 依功能的頁面元件
│   ├── {feature}/               # 功能視圖 (List, Add, Detail)
│   └── components/              # 功能專屬元件
├── components/
│   ├── global/                  # 共用 UI 元件
│   └── feature/                 # 可重用的功能元件
├── composables/                 # 共用邏輯 (use{Feature}.ts)
├── constants/                   # 常數/Enum (對應後端 Database 資料夾)
├── layouts/                     # 頁面佈局 Layout
├── router/                      # 路由設定
├── services/pms-http/{module}/  # 依模組的 API 服務
├── stores/                      # Pinia 狀態 stores
├── styles/                      # CSS 樣式
├── utils/                       # 共用工具函式
```

理由：基於功能的組織將相關程式碼放在一起，簡化導航，並實現獨立的功能開發。

### V. API 合約一致性

所有 API 端點必須遵循一致的模式：
- Request/Response 模型使用清晰的命名（Req/Rsp 後綴）
- 透過 `MbsErrorCodeEnum` 標準化錯誤碼

前端服務必須與後端合約匹配，使用強型別的 request/response 模型。

理由：一致的 API 合約減少整合錯誤，並實現前後端並行開發。

### VI. 狀態管理紀律

**後端**：
- Memory services 用於快取/session 狀態
- 服務中禁止靜態可變狀態
- 所有服務使用依賴注入

**前端**：
- Pinia stores 用於全域狀態
- 透過 `pinia-plugin-persistedstate` 持久化關鍵狀態（tokens, permissions）
- Composables 用於可重用的有狀態邏輯

理由：集中式狀態管理防止狀態分散、便於除錯，並確保可預測的行為。

### VII. 簡單優於抽象

程式碼必須優先考慮簡單性：
- 避免過早抽象；僅在重複 3 次以上時才提取模式
- 從直接實作開始；當複雜度合理時再重構
- 完全刪除未使用的程式碼；禁止註解掉的程式碼或向後相容的權宜之計

理由：簡單的程式碼更容易理解、除錯和修改。過度工程化會增加維護負擔。

## 專案特定標準

### 後端 (.NET 9 / ASP.NET Core)

**技術棧**：
- Runtime: .NET 9
- Framework: ASP.NET Core Web API
- ORM: Entity Framework Core 9 with SQL Server
- 背景工作: Hangfire
- 測試: xUnit v3 with Dependency Injection
- 日誌: NLog with JSON format

**資料庫標準**：
- 定序：`Latin1_General_100_CS`（區分大小寫）用於一般欄位
- 索引命名：`PK_{TableName}`, `UK_{Abbr}_{Field}`, `IDX_{Abbr}_{Field}`
- 連線：環境變數 `DB_MAIN_CONNECTION`

**建置/執行**：
```bash
dotnet build BackEnd/ProjectManagerSystem/ProjectManagerSystem.sln
dotnet run --project BackEnd/ProjectManagerSystem/ProjectManagerWebApi
```

### 前端 (Vue 3 / TypeScript)

**技術棧**：
- Framework: Vue 3 with Composition API (`<script setup>`)
- Language: TypeScript 5.7+ (strict mode)
- Build: Vite 6
- UI: Vuetify 3 + TailwindCSS
- State: Pinia with persistedstate plugin

**程式碼品質**：
- ESLint with TypeScript and Vue recommended rules
- Prettier formatting (double quotes, semicolons, 100 char width)
- 允許多字元件名稱 (vue/multi-word-component-names: OFF)

**建置/執行**：
```bash
cd FrontEnd/manager-back-site
npm install
npm run dev
```

## 開發工作流程

**程式碼審查要求**：
- 所有 PR 必須驗證符合命名規範
- 所有 PR 必須維持三層式架構分離
- 所有 PR 必須為新的 API 合約包含強型別模型

**測試期望**：
- 後端：使用 xUnit 對 logical services 進行單元測試
- 前端：目前未配置測試框架；需手動測試
- 整合測試聚焦於 API 合約和資料庫操作

**文件**：
- 使用專案根目錄的 CLAUDE.md 作為開發者快速參考
- `BackEnd/ProjectManagerSystem/Specification/` 中的規範檔案提供詳細標準

## 治理

本憲法管轄 ProjectManagerSystem 儲存庫內的所有開發工作。

**修訂流程**：
1. 透過 PR 提出變更並說明理由
2. 遵循語意化版本更新憲法版本：
   - MAJOR：原則移除或重新定義
   - MINOR：新增原則或章節
   - PATCH：澄清或措辭變更
3. 如果原則變更，更新相依的模板

**合規性**：
- 所有 PR 和程式碼審查必須驗證與憲法原則的一致性
- 超出此處定義模式的複雜度必須在 PR 描述中說明理由
- 使用 CLAUDE.md 和規範檔案作為執行時開發指南

**版本**: 1.0.2 | **批准日期**: 2025-12-22 | **最後修訂**: 2025-12-22
