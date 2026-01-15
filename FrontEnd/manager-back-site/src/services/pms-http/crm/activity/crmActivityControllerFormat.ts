import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
import { DbMasqKindEnum } from "@/constants/DbMasqKindEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";

//#region 活動
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-新增-請求模型 */
export interface MbsCrmActCtlAddActivityReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動名稱 */
  a: string;
  /** 管理者活動類型 */
  b: DbAtomManagerActivityKindEnum;
  /** 管理者活動-開始時間 */
  c: string;
  /** 管理者活動-結束時間 */
  d: string;
  /** 管理者活動-地點 */
  e: string;
  /** 管理者活動-期望名單數 */
  f: number;
  /** 管理者活動-內容 */
  g: string | null;
  /** 管理者活動-活動產品清單 */
  h: MbsCrmActCtlAddActivityReqProductItemMdl[] | null;
  /** 管理者活動-活動贊助清單 */
  i: MbsCrmActCtlAddActivityReqSponsorItemMdl[] | null;
  /** 管理者活動-活動支出清單 */
  j: MbsCrmActCtlAddActivityReqExpenseItemMdl[] | null;
}

/** 管理者後台-CRM-活動管理-控制器-新增-產品項目-請求模型 */
export interface MbsCrmActCtlAddActivityReqProductItemMdl {
  /** 管理者產品ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-控制器-新增-贊助項目-請求模型 */
export interface MbsCrmActCtlAddActivityReqSponsorItemMdl {
  /** 管理者活動贊助商-名稱 */
  a: string;
  /** 管理者活動贊助商-金額 */
  b: number;
}

/** 管理者後台-CRM-活動管理-控制器-新增-支出項目-請求模型 */
export interface MbsCrmActCtlAddActivityReqExpenseItemMdl {
  /** 管理者活動支出-項目 */
  a: string;
  /** 管理者活動支出-數量 */
  b: number;
  /** 管理者活動支出-總金額 */
  c: number;
}

/** 管理者後台-CRM-活動管理-控制器-新增-回應模型 */
export interface MbsCrmActCtlAddActivityRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-取得單筆-請求模型 */
export interface MbsCrmActCtlGetActivityReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-控制器-取得單筆-回應模型 */
export interface MbsCrmActCtlGetActivityRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動名稱 */
  a: string;
  /** 管理者活動類型 */
  b: DbAtomManagerActivityKindEnum;
  /** 管理者活動-開始時間 */
  c: string;
  /** 管理者活動-結束時間 */
  d: string;
  /** 管理者活動-地點 */
  e: string;
  /** 管理者活動-期望名單數 */
  f: number;
  /** 管理者活動-內容 */
  g: string;
  /** 管理者活動-實際名單數 */
  h: number;
  /** 管理者活動-已轉電銷數 */
  i: number;
  /** 管理者活動-商機數 */
  j: number;
  /** 管理者活動產品列表 */
  k: MbsCrmActCtlGetManyActivityProductRspItemMdl[];
  /** 管理者活動贊助商列表 */
  l: MbsCrmActCtlGetManyActivitySponsorRspItemMdl[];
  /** 管理者活動支出列表 */
  m: MbsCrmActCtlGetManyActivityExpenseRspItemMdl[];
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-取得多筆-請求模型 */
export interface MbsCrmActCtlGetManyActivityReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動-開始時間 */
  a: string | null;
  /** 管理者活動-結束時間 */
  b: string | null;
  /** 管理者活動-類型 */
  c: DbAtomManagerActivityKindEnum | null;
  /** 管理者活動-名稱 */
  d: string | null;
  /** 頁數 */
  e: number;
  /** 每頁筆數 */
  f: number;
}

/** 管理者後台-CRM-活動管理-控制器-取得多筆-回應模型 */
export interface MbsCrmActCtlGetManyActivityRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動列表 */
  a: MbsCrmActCtlGetManyActivityRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-CRM-活動管理-控制器-取得多筆-回應項目模型 */
export interface MbsCrmActCtlGetManyActivityRspItemMdl {
  /** 管理者活動ID */
  a: number;
  /** 管理者活動-名稱 */
  b: string;
  /** 管理者活動-類型 */
  c: DbAtomManagerActivityKindEnum;
  /** 管理者活動-開始時間 */
  d: string;
  /** 管理者活動-結束時間 */
  e: string;
  /** 管理者活動-地點 */
  f: string;
  /** 管理者活動-預期名單數 */
  g: number;
  /** 管理者活動-實際名單數 */
  h: number;
  /** 管理者活動-已轉電銷數 */
  i: number;
  /** 管理者活動-總贊助金額 */
  j: number;
  /** 管理者活動-總支出 */
  k: number;
  /** 管理者活動-商機數 */
  l: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-更新-請求模型 */
export interface MbsCrmActCtlUpdateActivityReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
  /** 管理者活動名稱 */
  b: string | null;
  /** 管理者活動-開始時間 */
  c: string | null;
  /** 管理者活動-結束時間 */
  d: string | null;
  /** 管理者活動-地點 */
  e: string | null;
  /** 管理者活動-期望名單數 */
  f: number | null;
  /** 管理者活動-內容 */
  g: string | null;
}

/** 管理者後台-CRM-活動管理-控制器-更新-回應模型 */
export interface MbsCrmActCtlUpdateActivityRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------

//#endregion

//#region 活動產品
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-控制器-新增-請求模型 */
export interface MbsCrmActCtlAddActivityProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
  /** 管理者產品ID */
  b: number;
}

/** 管理者後台-CRM-活動管理-活動產品-控制器-新增-回應模型 */
export interface MbsCrmActCtlAddActivityProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動產品ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-控制器-取得-請求模型 */
export interface MbsCrmActCtlGetActivityProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動產品ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動產品-控制器-取得-回應模型 */
export interface MbsCrmActCtlGetActivityProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動產品ID */
  a: number;
  /** 管理者產品ID */
  b: number;
  /** 管理者-產品-主分類-ID */
  c: number;
  /** 管理者-產品-子分類-ID */
  d: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-控制器-取得多筆-請求模型 */
export interface MbsCrmActCtlGetManyActivityProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動產品-控制器-取得多筆-回應模型 */
export interface MbsCrmActCtlGetManyActivityProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動產品列表 */
  a: MbsCrmActCtlGetManyActivityProductRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-活動產品-控制器-取得多筆-回應項目模型 */
export interface MbsCrmActCtlGetManyActivityProductRspItemMdl {
 /** 管理者活動-產品ID */
  a: number;
  /** 管理者產品-ID */
  b: number;
  /** 管理者產品-名稱 */
  c: string;
  /** 管理者產品-主分類-ID */
  d: number;
  /** 管理者產品-主分類名稱 */
  e: string;
  /** 管理者產品-子分類-ID */
  f: number;
  /** 管理者產品-子分類名稱 */
  g: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-控制器-刪除-請求模型 */
export interface MbsCrmActCtlRemoveActivityProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動產品ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動產品-控制器-刪除-回應模型 */
export interface MbsCrmActCtlRemoveActivityProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-控制器-更新-請求模型 */
export interface MbsCrmActCtlUpdateActivityProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動產品ID */
  a: number;
  /** 管理者產品ID */
  b: number | null;
}

/** 管理者後台-CRM-活動管理-活動產品-控制器-更新-回應模型 */
export interface MbsCrmActCtlUpdateActivityProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------

//#endregion

//#region 活動贊助
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-控制器-新增-請求模型 */
export interface MbsCrmActCtlAddActivitySponsorReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
  /** 管理者活動贊助商-名稱 */
  b: string;
  /** 管理者活動贊助商-金額 */
  c: number;
}

/** 管理者後台-CRM-活動管理-活動贊助-控制器-新增-回應模型 */
export interface MbsCrmActCtlAddActivitySponsorRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動贊助商ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-控制器-取得-請求模型 */
export interface MbsCrmActCtlGetActivitySponsorReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動贊助商ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動贊助-控制器-取得-回應模型 */
export interface MbsCrmActCtlGetActivitySponsorRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動贊助商-名稱 */
  a: string;
  /** 管理者活動贊助商-金額 */
  b: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-控制器-取得多筆-請求模型 */
export interface MbsCrmActCtlGetManyActivitySponsorReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動贊助-控制器-取得多筆-回應模型 */
export interface MbsCrmActCtlGetManyActivitySponsorRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動贊助商列表 */
  a: MbsCrmActCtlGetManyActivitySponsorRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-活動贊助-控制器-取得多筆-回應項目模型 */
export interface MbsCrmActCtlGetManyActivitySponsorRspItemMdl {
  /** 管理者活動贊助商ID */
  a: number;
  /** 管理者活動贊助商-名稱 */
  b: string;
  /** 管理者活動贊助商-金額 */
  c: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-控制器-刪除-請求模型 */
export interface MbsCrmActCtlRemoveActivitySponsorReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動贊助商ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動贊助-控制器-刪除-回應模型 */
export interface MbsCrmActCtlRemoveActivitySponsorRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-控制器-更新-請求模型 */
export interface MbsCrmActCtlUpdateActivitySponsorReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動贊助商ID */
  a: number;
  /** 管理者活動贊助商-名稱 */
  b: string | null;
  /** 管理者活動贊助商-金額 */
  c: number | null;
}

/** 管理者後台-CRM-活動管理-活動贊助-控制器-更新-回應模型 */
export interface MbsCrmActCtlUpdateActivitySponsorRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
//#endregion

//#region 活動支出
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動支出-控制器-新增-請求模型 */
export interface MbsCrmActCtlAddActivityExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
  /** 管理者活動支出-項目 */
  b: string;
  /** 管理者活動支出-數量 */
  c: number;
  /** 管理者活動支出-總金額 */
  d: number;
}

/** 管理者後台-CRM-活動管理-活動支出-控制器-新增-回應模型 */
export interface MbsCrmActCtlAddActivityExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動支出ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動支出-控制器-取得-請求模型 */
export interface MbsCrmActCtlGetActivityExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動支出ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動支出-控制器-取得-回應模型 */
export interface MbsCrmActCtlGetActivityExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動支出ID */
  a: number;
  /** 管理者活動支出-項目 */
  b: string;
  /** 管理者活動支出-數量 */
  c: number;
  /** 管理者活動支出-總金額 */
  d: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動支出-控制器-取得多筆-請求模型 */
export interface MbsCrmActCtlGetManyActivityExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動支出-控制器-取得多筆-回應模型 */
export interface MbsCrmActCtlGetManyActivityExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者活動支出列表 */
  a: MbsCrmActCtlGetManyActivityExpenseRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-活動支出-控制器-取得多筆-回應項目模型 */
export interface MbsCrmActCtlGetManyActivityExpenseRspItemMdl {
  /** 管理者活動支出ID */
  a: number;
  /** 管理者活動支出-項目 */
  b: string;
  /** 管理者活動支出-數量 */
  c: number;
  /** 管理者活動支出-總金額 */
  d: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動支出-控制器-刪除-請求模型 */
export interface MbsCrmActCtlRemoveActivityExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動支出ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-活動支出-控制器-刪除-回應模型 */
export interface MbsCrmActCtlRemoveActivityExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動支出-控制器-更新-請求模型 */
export interface MbsCrmActCtlUpdateActivityExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動支出ID */
  a: number;
  /** 管理者活動支出-項目 */
  b: string | null;
  /** 管理者活動支出-數量 */
  c: number | null;
  /** 管理者活動支出-總金額 */
  d: number | null;
}

/** 管理者後台-CRM-活動管理-活動支出-控制器-更新-回應模型 */
export interface MbsCrmActCtlUpdateActivityExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
//#endregion

//#region 活動名單
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-新增活動名單-請求模型 */
export interface MbsCrmActCtlAddActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
  /** 客戶公司ID */
  b: number;
  /** 客戶公司營業地點ID */
  c: number;
  /** 窗口ID */
  d: number;
  /** 商機狀態 */
  e: DbAtomPipelineStatusEnum;
}

/** 管理者後台-CRM-活動管理-控制器-新增活動名單-回應模型 */
export interface MbsCrmActCtlAddActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工商機ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-取得活動名單-請求模型 */
export interface MbsCrmActCtlGetActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
}

/** 管理者後台-CRM-活動管理-控制器-取得活動名單-回應模型 */
export interface MbsCrmActCtlGetActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 資料庫-原子-商機-狀態-列舉 */
  a: DbAtomPipelineStatusEnum;
  /** 公司統一編號 */
  b: string;
  /** 客戶公司名稱 */
  c: string;
  /** 原子-人員規模 */
  d: DbAtomEmployeeRangeEnum | null;
  /** 原子-客戶分級 */
  e: DbAtomCustomerGradeEnum | null;
  /** 公司主類別名稱 */
  f: string;
  /** 公司子類別名稱 */
  g: string;
  /** 原子-縣市 */
  h: DbAtomCityEnum | null;
  /** 公司營業地點地址 */
  i: string;
  /** 公司營業地點電話 */
  j: string;
  /** 公司營業地點狀態 */
  k: DbAtomManagerCompanyStatusEnum | null;
  /** 窗口姓名 */
  l: string;
  /** 窗口Email */
  m: string;
  /** 窗口手機 */
  n: string;
  /** 窗口部門 */
  o: string;
  /** 窗口職稱 */
  p: string;
  /** 窗口電話(市話) */
  q: string;
  /** 窗口是否個資同意 */
  r: boolean;
  /** 窗口在職狀態 */
  s: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等ID */
  t: DbAtomManagerContacterRatingKindEnum;
  /** Teams會議持續時間 */
  u: string;
  /** 角色 */
  v: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-取得多筆活動名單-請求模型 */
export interface MbsCrmActCtlGetManyActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 活動ID */
  a: number;
  /** 商機狀態 */
  b: DbAtomPipelineStatusEnum | null;
  /** 管理者公司名稱 */
  c: string | null;
  /** 窗口姓名 */
  d: string | null;
  /** 窗口Email */
  e: string | null;
  /** 頁面索引 */
  f: number;
  /** 頁面筆數 */
  g: number;
}

/** 管理者後台-CRM-活動管理-控制器-取得多筆活動名單-回應模型 */
export interface MbsCrmActCtlGetManyActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工商機列表 */
  a: MbsCrmActCtlGetManyActivityEmployeePipelineRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-CRM-活動管理-控制器-取得多筆活動名單-回應項目模型 */
export interface MbsCrmActCtlGetManyActivityEmployeePipelineRspItemMdl {
  /** 商機ID */
  a: number;
  /** 商機狀態 */
  b: DbAtomPipelineStatusEnum;
  /** 管理公司名稱 */
  c: string;
  /** 窗口部門 */
  d: string;
  /** 窗口職稱 */
  e: string;
  /** 窗口姓名 */
  f: string;
  /** 窗口Email */
  g: string;
  /** 窗口手機 */
  h: string;
  /** 窗口電話 */
  i: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-刪除多筆活動名單-請求模型 */
export interface MbsCrmActCtlRemoveManyActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID列表 */
  a: number[];
}

/** 管理者後台-CRM-活動管理-控制器-刪除多筆活動名單-回應模型 */
export interface MbsCrmActCtlRemoveManyActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-更新多筆活動名單-請求模型 */
export interface MbsCrmActCtlUpdateManyActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID列表 */
  a: number[];
  /** 商機狀態 */
  b: DbAtomPipelineStatusEnum;
}

/** 管理者後台-CRM-活動管理-控制器-更新多筆活動名單-回應模型 */
export interface MbsCrmActCtlUpdateManyActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-取得多筆客戶過往活動-請求模型 */
export interface MbsCrmActCtlGetManyPastActivityReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
  /** 頁面索引 */
  b: number;
  /** 頁面筆數 */
  c: number;
}

/** 管理者後台-CRM-活動管理-控制器-取得多筆客戶過往活動-回應模型 */
export interface MbsCrmActCtlGetManyPastActivityRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 最新過往活動 */
  a: MbsCrmActCtlGetManyPastActivityRspItemMdl;
  /** 過往活動列表 */
  b: MbsCrmActCtlGetManyPastActivityRspItemMdl[];
  /** 過往活動總數 */
  c: number;
}

/** 管理者後台-CRM-活動管理-控制器-取得多筆客戶過往活動-項目-回應模型 */
export interface MbsCrmActCtlGetManyPastActivityRspItemMdl {
  /** 管理者活動名稱 */
  a: string;
  /** 管理者活動-開始時間 */
  b: string;
  /** 管理者活動-結束時間 */
  c: string;
}

//----------------------------------------------------------------------------------
//#endregion

//#region eDM
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-下載eDM範本-請求模型 */
export interface MbsCrmActCtlDownloadEdmTemplateReqMdl {
  /** 員工登入令牌 */
  aa: string;
}

/** 管理者後台-CRM-活動管理-控制器-下載eDM範本-回應模型 */
export type MbsCrmActCtlDownloadEdmTemplateRspMdl = Blob;

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-匯入eDM-請求模型 */
export interface MbsCrmActCtlImportEdmReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
  /** 匯入的eDM檔案 */
  b: File;
}

/** 管理者後台-CRM-活動管理-控制器-匯入eDM-回應模型 */
export interface MbsCrmActCtlImportEdmRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 錯誤資料列表 */
  a: MbsCrmActCtlImportEdmRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-控制器-匯入eDM-項目-回應模型 */
export interface MbsCrmActCtlImportEdmRspItemMdl {
  /** EDM Email */
  a: string;
  /** EDM First Name */
  b: string;
  /** EDM Last Name */
  c: string;
  /** EDM Telephone */
  d: string;
  /** EDM 行動電話 */
  e: string;
  /** EDM 公司名稱 */
  f: string;
  /** EDM 職稱 */
  g: string;
  /** EDM 公司電話 */
  h: string;
  /** EDM 公司傳真 */
  i: string;
  /** EDM 公司地址 */
  j: string;
  /** EDM 備注 */
  k: string;
  /** EDM 部門 */
  l: string;
  /** EDM 主類別碼 */
  m: string;
  /** EDM 分類碼 */
  n: string;
  /** EDM Account Sales */
  o: string;
  /** EDM 區域 */
  p: string;
  /** EDM Created Date */
  q: string;
  /** EDM device */
  r: string;
}

//----------------------------------------------------------------------------------
//#endregion

//#region Teams
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-下載Teams範本-請求模型 */
export interface MbsCrmActCtlDownloadTeamsTemplateReqMdl {
  /** 員工登入令牌 */
  aa: string;
}

/** 管理者後台-CRM-活動管理-控制器-下載Teams範本-回應模型 */
export type MbsCrmActCtlDownloadTeamsTemplateRspMdl = Blob;

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-匯入Teams-請求模型 */
export interface MbsCrmActCtlImportTeamsReqMdl {
  /** 管理者登入令牌 */
  aa: string;
  /** 管理者活動ID */
  a: number;
  /** 匯入的Teams檔案 */
  b: File;
}

/** 管理者後台-CRM-活動管理-控制器-匯入Teams-回應模型 */
export interface MbsCrmActCtlImportTeamsRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 錯誤資料列表 */
  a: MbsCrmActCtlImportTeamsRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-控制器-匯入Teams-項目-回應模型 */
export interface MbsCrmActCtlImportTeamsRspItemMdl {
  /** Teams名稱 */
  a: string;
  /** Teams首次加入時間 */
  b: string | null;
  /** Teams上次離開時間 */
  c: string | null;
  /** Teams會議持續時間 */
  d: string;
  /** Teams電子郵件 */
  e: string;
  /** 參與者識別碼 (UPN) */
  f: string;
  /** Teams角色 */
  g: string;
  /** Teams註冊名子 */
  h: string;
  /** Teams註冊姓氏 */
  i: string;
  /** Teams註冊電子郵件 */
  j: string;
  /** Teams註冊時間 */
  k: string | null;
  /** Teams註冊狀態 */
  l: string;
  /** Teams公司名稱 */
  m: string;
  /** Teams部門 */
  n: string;
  /** Teams職稱 */
  o: string;
  /** Teams公司電話 */
  p: string;
  /** Teams手機號碼 */
  q: string;
  /** Teams活動資訊來源 */
  r: string;
  /** Teams窗口是否個資同意 */
  s: boolean;
}

//----------------------------------------------------------------------------------
//#endregion

//#region 活動問卷
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-控制器-新增活動問卷問題-請求模型 */
export interface MbsCrmActCtlAddActivitySurveyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-活動問卷問題-活動ID */
  a: number;
  /** 管理者-活動問卷問題-問卷ID */
  b: number;
  /** 管理者-活動問卷問題-問題類型 */
  c: DbMasqKindEnum;
  /** 管理者-活動問卷問題-問題標題 */
  d: string;
  /** 管理者-活動問卷問題-問題內容 */
  e: string;
  /** 管理者-活動問卷問題-是否為其他 */
  f: boolean;
  /** 管理者-活動問卷問題-排序 */
  g: number;
  /** 管理者-活動問卷問題項目列表 */
  h: MbsCrmActCtlAddActivitySurveyReqItemMdl[];
}

/** 管理者後台-CRM-活動管理-控制器-新增活動問卷問題-請求項目模型 */
export interface MbsCrmActCtlAddActivitySurveyReqItemMdl {
  /** 管理者-活動問卷問題項目-名稱 */
  a: string;
  /** 管理者-活動問卷問題項目-排序 */
  b: number;
}

/** 管理者後台-CRM-活動管理-控制器-新增活動問卷問題-回應模型 */
export interface MbsCrmActCtlAddActivitySurveyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-活動問卷-ID */
  a: number;
}

//----------------------------------------------------------------------------------
//#endregion
