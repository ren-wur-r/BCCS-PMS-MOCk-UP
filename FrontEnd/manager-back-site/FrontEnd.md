方法 :  
方法名- 開頭小寫
方法命名是 api接口去掉mbs

模型順序 GetMany Get Add Update
模型屬性與後端命名相同
註解 管理者-

List---------

/\*_ 取得列表 _/
getList

Add、Update-----

/\*_ 取得【】資料 _/
getData

getChangedFields

/\*_ 驗證【】欄位 _/
validateField

//------------------------------------------------------------------------
/\*_ 系統設定-角色-列表-查詢模型 _/ => 列表頁面查詢條件專用
interface SystemRoleListQueryMdl {}

/\*_ 系統設定-角色-列表-項目模型 _/ => 列表頁面用於列表項目顯示的模型
interface SystemRoleListItemMdl {}

/\*_ 系統設定-角色-列表-頁面模型 _/ => 列表頁面用模型(整合)
interface SystemRoleListViewMdl {}

/\*_ 系統設定-角色-列表-頁面物件 _/
const systemRoleListViewObj = reactive<SystemRoleListViewMdl>({});
// -------------------------------------------------------------------------更新送備註(或是可以不用填寫的欄位)
就送 " " 空字串給後端

# 更新日期：2025-11-13

## 頁面撰寫的區塊說明
1. Imports 引入
   原則：由「官方 → 外部 → 專案內部」的順序排列
   順序：Vue > Enums / 常數 > Stores > Composables > Services > Utils > Components > Router

2. 全域狀態 : 放置「外部來的、不屬於本組件的狀態」
   這裡只放「外部狀態初始化」，不放業務邏輯變數

3. 型別定義 : interface 統一放這裡
   註解要求：
   ✅ 每個 interface 必須有 /\*\* \*/ 說明
   ✅ 複雜欄位可加註解說明用途

4. 頁面狀態 : 組件內部的 ref / reactive 狀態
   命名建議 :
   ✅ 每個變數上方加 /\*\* \*/ 說明用途
   ✅ Boolean 用 is、has、show 開頭
   ✅ 複雜物件加上 ...Data、...Obj、...State 後綴

5. 計算屬性 & 監聽器 (如果有的話)

6. 資料操作

7. 表單驗證

8. 事件處理區塊（按功能分群）
   方法命名規範：
   ✅ 點擊事件：click[功能]Btn （例如：clickSaveBtn）
   ✅ 處理事件：handle[功能] （例如：handleProductEdit）
   ✅ 確認/取消：confirm[功能] / cancel[功能]

9. 生命週期 : 必須放在最後
   理由：生命週期是「程式執行起點」，放最後符合「宣告 → 執行」的順序

## 註解規範
1. 函數註解

- 規則 :
  ✅ 每個函數都要有 /\*\* \*/ 註解
  ✅ 說明「做什麼」，不用說明「怎麼做」
  ✅ UI 事件要標註觸發來源（例如：【儲存】按鈕）

- 範例 :
  /** 取得活動資料 \*/
  const getData = async () => {...};
  /** 點擊【儲存】按鈕 \*/
  const clickSaveBtn = async () => {...};

2. 變數註解

- 規則：
  ✅ 重要變數要加註解
  ✅ 簡單變數（如 count、index）可不加註解

- 範例 :
  /** 是否為編輯模式 \*/
  const isEditMode = ref(false);
  /** 確認儲存彈窗 \*/
  const showConfirmDialog = ref(false);

3. Interface 註解
   規則：
   ✅ Interface 必須有整體說明
   ✅ 複雜欄位要加註解說明用途或範圍
   ✅ Enum 類型要註明可能的值

- 範例 :
  /** CRM-活動管理-活動-詳情頁面模型 \*/
  interface ActivityDetailViewMdl {
  /** 活動ID \*/
  activityID: number | null;
  /\*_ 活動名稱 _/
  activityName: string | null;
  /\*_ 活動類型（1:實體活動, 2:線上活動） _/
  activityKind: ActivityKindEnum;
  }

# 更新日期：2025-11-28
使用 defineAsyncComponent 引入不需要立即加載的組件(ex:Modal元件) 有效降低初始 bundle 大小