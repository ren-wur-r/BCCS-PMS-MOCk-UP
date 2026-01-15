# Mock API 開發模式使用說明

## 概述

本專案支援 Mock API 模式，讓您在沒有後端連線的情況下也能進行前端開發。這對於在家開發或後端尚未完成時特別有用。

## 使用方式

### 方案一：在家使用 Mock API 開發

1. **設定環境變數**

   編輯 `.env.development.local` 檔案（如果不存在請建立）：
   ```
   VITE_API_BASE_URL=http://localhost:5011/
   VITE_BASE_PATH=/
   VITE_USE_MOCK_DATA=true
   ```

   **重要**：必須使用 `.env.development.local` 而不是 `.env.local`，因為 Vite 的環境變數優先級規則會讓 `.env.development` 覆蓋 `.env.local`

2. **啟動開發伺服器**
   ```bash
   npm run dev
   ```

3. **開始開發**
   - 所有 API 請求會自動使用 Mock 資料
   - Mock 回應會模擬真實 API 的延遲（預設 500ms）
   - 可以正常開發 UI、路由、狀態管理等前端功能

### 方案二：在公司連接真實後端

1. **方法 A：刪除或重新命名 `.env.development.local`**
   ```bash
   mv .env.development.local .env.development.local.backup
   ```
   系統會自動使用 `.env.development` 的設定

2. **方法 B：修改 `.env.development.local`**
   ```
   VITE_API_BASE_URL=http://192.168.3.236:8100/ProjectManagerWebApi/
   VITE_BASE_PATH=/ManagerBackSite/
   VITE_USE_MOCK_DATA=false
   ```

3. **重新啟動開發伺服器**
   ```bash
   npm run dev
   ```

## Mock API 資料說明

目前已實作以下 Mock API 端點：

### 基本功能
- `POST /api/MbsBasic/Login` - 登入（回傳 mock token）
- `POST /api/MbsBasic/Logout` - 登出
- `POST /api/MbsBasic/HeartBeat` - 心跳檢測

### 系統管理
- `POST /api/MbsSystemEmployee/GetMany` - 取得員工列表
- `POST /api/MbsSystemCompany/GetMany` - 取得公司列表

### CRM 功能
- `POST /api/MbsCrmPipeline/GetMany` - 取得商機列表

## 擴充 Mock 資料

如需新增更多 Mock API 端點，請編輯以下檔案：

**檔案位置**: `src/services/mockApi/mockApiClient.ts`

在 `getMockDataForEndpoint` 方法的 `mockDataMap` 中新增：

```typescript
"/api/YourController/YourAction": () => ({
  ErrorCode: 0,
  ErrorMessage: "success",
  YourData: {
    // 您的 mock 資料結構
  },
}),
```

## 環境變數優先順序

Vite 的環境變數載入優先順序（從高到低）：
1. `.env.development.local` - 本地開發環境（會被 git ignore）**← 使用此檔案**
2. `.env.development` - 開發環境
3. `.env.local` - 本地所有環境（會被 git ignore）
4. `.env.staging` - 測試環境
5. `.env.production` - 生產環境
6. `.env` - 所有環境

## 常見問題

### Q: Mock 資料不正確怎麼辦？
A: 編輯 `src/services/mockApi/mockApiClient.ts`，修改對應端點的回傳資料

### Q: 想要測試錯誤情況怎麼辦？
A: 在 mock 資料中回傳錯誤碼：
```typescript
return {
  ErrorCode: 1001,
  ErrorMessage: "測試錯誤訊息",
};
```

### Q: Mock API 的延遲時間可以調整嗎？
A: 可以，在 `mockApiClient.ts` 的 `mockRequest` 方法中修改 `delay` 參數

### Q: 如何知道目前是使用 Mock 還是真實 API？
A: 在瀏覽器 Console 中檢查，Mock API 會輸出警告訊息

## 建議工作流程

### 在家開發
1. 設定 `.env.development.local` 為 Mock 模式
2. 開發 UI 介面和互動邏輯
3. 使用 Mock 資料測試功能流程
4. 提交程式碼（`.env.development.local` 不會被提交）

### 在公司開發
1. 刪除或停用 `.env.development.local`
2. 連接真實後端測試
3. 驗證 API 整合
4. 修正任何資料格式差異

## 技術實作說明

### 架構
```
httpClient.ts
├── 檢查 VITE_USE_MOCK_DATA 環境變數
├── true → 使用 MockApiClient
└── false → 使用 axios (真實 HTTP 請求)
```

### Mock API 客戶端特點
- 實作與 axios 相容的介面
- 支援 `get`、`post`、`put`、`delete` 方法
- 模擬非同步延遲
- 自動回傳符合格式的 Mock 資料

## 注意事項

1. Mock 資料結構應該與真實 API 回應保持一致
2. `.env.development.local` 已在 `.gitignore` 中，不會被提交到版本控制
3. 切換模式後需要重新啟動開發伺服器
4. Mock 模式下不會真正呼叫後端，所有資料都是假的
5. 部署到測試或正式環境時，確保 `VITE_USE_MOCK_DATA` 為 `false`
