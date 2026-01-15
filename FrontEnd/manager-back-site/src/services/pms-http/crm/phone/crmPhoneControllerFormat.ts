import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomEmployeePipelineSalerStatusEnum } from "@/constants/DbAtomEmployeePipelineSalerStatusEnum";

//#region 活動
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得多筆活動-請求模型 */
export interface MbsCrmPhnCtlGetManyActivityReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者活動-開始時間 */
  a: string | null;
  /** 管理者活動-結束時間 */
  b: string | null;
  /** 管理者活動類型 */
  c: DbAtomManagerActivityKindEnum | null;
  /** 管理者活動-名稱 */
  d: string | null;
  /** 頁碼 */
  e: number;
  /** 每頁筆數 */
  f: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆活動-回應模型 */
export interface MbsCrmPhnCtlGetManyActivityRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 活動清單 */
  a: MbsCrmPhnCtlGetManyActivityRspItemMdl[] | null;
  /** 總筆數 */
  b: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆活動-回應項目模型 */
export interface MbsCrmPhnCtlGetManyActivityRspItemMdl {
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
  /** 管理者活動-實際名單數 */
  f: number;
  /** 管理者活動-已轉電銷數 */
  g: number;
  /** 管理者活動-已撥打數 */
  h: number;
  /** 管理者活動-商機數 */
  i: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得單筆活動-請求模型 */
export interface MbsCrmPhnCtlGetActivityReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 活動ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得單筆活動-回應模型 */
export interface MbsCrmPhnCtlGetActivityRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 活動名稱 */
  a: string;
  /** 管理者活動類型 */
  b: DbAtomManagerActivityKindEnum;
  /** 活動開始時間 */
  c: string;
  /** 活動結束時間 */
  d: string;
  /** 管理者活動-地點 */
  e: string | null;
  /** 管理者活動-期望名單數 */
  f: number | null;
  /** 管理者活動-內容 */
  g: string | null;
  /** 管理者活動-實際名單數 */
  h: number;
  /** 管理者活動-已轉電銷數 */
  i: number;
  /** 管理者活動-商機數 */
  j: number;
  /** 管理者活動-已撥打數(電銷紀錄數) */
  k: number;
  /** 管理者活動產品列表 */
  l: MbsCrmPhnCtlGetActivityProductRspItemMdl[] | null;
  /** 管理者活動贊助商列表 */
  m: MbsCrmPhnCtlGetActivitySponsorRspItemMdl[] | null;
  /** 管理者活動支出列表 */
  n: MbsCrmPhnCtlGetActivityExpenseRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-控制器-取得單筆活動-產品-回應項目模型 */
export interface MbsCrmPhnCtlGetActivityProductRspItemMdl {
  /** 管理者活動產品ID */
  a: number;
  /** 管理者產品名稱 */
  b: string;
  /** 管理者產品-主分類名稱 */
  c: string;
  /** 管理者產品-子分類名稱 */
  d: string;
}

/** 管理者後台-CRM-電銷管理-控制器-取得單筆活動-贊助商-回應項目模型 */
export interface MbsCrmPhnCtlGetActivitySponsorRspItemMdl {
  /** 管理者活動贊助商ID */
  a: number;
  /** 管理者活動贊助商-名稱 */
  b: string;
  /** 管理者活動贊助商-金額 */
  c: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得單筆活動-支出-回應項目模型 */
export interface MbsCrmPhnCtlGetActivityExpenseRspItemMdl {
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

//#endregion

//#region 活動名單
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得多筆名單-請求模型 */
export interface MbsCrmPhnCtlGetManyActivityEmployeePipelineReqMdl {
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

/** 管理者後台-CRM-電銷管理-控制器-取得多筆名單-回應模型 */
export interface MbsCrmPhnCtlGetManyActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 名單列表 */
  a: MbsCrmPhnCtlGetManyActivityEmployeePipelineRspItemMdl[] | null;
  /** 總筆數 */
  b: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆名單-回應項目模型 */
export interface MbsCrmPhnCtlGetManyActivityEmployeePipelineRspItemMdl {
  /** 商機ID */
  a: number;
  /** 商機狀態 */
  b: DbAtomPipelineStatusEnum | null;
  /** 是否有匹配公司 */
  c: boolean;
  /** 管理者公司ID */
  d: number;
  /** 管理者公司名稱 */
  e: string;
  /** 是否有匹配窗口 */
  f: boolean;
  /** 商機窗口ID */
  g: number;
  /** 窗口部門 */
  h: string | null;
  /** 窗口職稱 */
  i: string | null;
  /** 窗口姓名 */
  j: string | null;
  /** 窗口Email */
  k: string | null;
  /** 窗口手機 */
  l: string | null;
  /** 窗口電話 */
  m: string | null;
  /** 最初商機開發時間 */
  n: string | null;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得名單-請求模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得名單-回應模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 資料庫-原子-商機-狀態-列舉 */
  a: DbAtomPipelineStatusEnum;
  /** 原始客戶資訊 */
  b: MbsCrmPhnCtlGetActivityEmployeePipelineRspOriginalCompanyItemMdl | null;
  /** 是否有公司 */
  c: boolean;
  /** 公司資料 */
  d: MbsCrmPhnCtlGetActivityEmployeePipelineRspCompanyItemMdl | null;
  /** 原始窗口資料 */
  e: MbsCrmPhnCtlGetActivityEmployeePipelineRspOriginalContacterItemMdl | null;
  /** 窗口清單 */
  f: MbsCrmPhnCtlGetActivityEmployeePipelineRspContacterItemMdl[] | null;
  /** Teams會議持續時間 */
  g: string | null;
  /** 角色 */
  h: string | null;
  /** 產品列表 */
  i: MbsCrmPhnCtlGetActivityEmployeePipelineRspProductItemMdl[] | null;
  /** 電銷紀錄列表 */
  j: MbsCrmPhnCtlGetActivityEmployeePipelineRspPhoneItemMdl[] | null;
  /** 指派業務記錄列表 */
  k: MbsCrmPhnCtlGetActivityEmployeePipelineRspSalerItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-控制器-取得活動名單-原始公司項目-回應模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineRspOriginalCompanyItemMdl {
  /** 統一編號 */
  a: string;
  /** 客戶公司名稱 */
  b: string;
  /** 原子-人員規模 */
  c: DbAtomEmployeeRangeEnum;
  /** 原子-客戶分級 */
  d: DbAtomCustomerGradeEnum;
  /** 公司主類別名稱 */
  e: string | null;
  /** 公司子類別名稱 */
  f: string | null;
  /** 原子-縣市 */
  g: DbAtomCityEnum;
  /** 公司營業地點地址 */
  h: string | null;
  /** 公司營業地點電話 */
  i: string | null;
  /** 公司營業地點狀態 */
  j: DbAtomManagerCompanyStatusEnum;
}

/** 管理者後台-CRM-電銷管理-控制器-取得活動名單-客戶項目-回應模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineRspCompanyItemMdl {
  /** 公司統一編號 */
  a: string;
  /** 客戶公司ID */
  b: number;
  /** 客戶公司名稱 */
  c: string;
  /** 原子-人員規模 */
  d: DbAtomEmployeeRangeEnum;
  /** 原子-客戶分級 */
  e: DbAtomCustomerGradeEnum;
  /** 公司主類別名稱 */
  f: string | null;
  /** 公司子類別名稱 */
  g: string | null;
  /** 原子-縣市 */
  h: DbAtomCityEnum | null;
  /** 公司營業地點ID */
  i: number | null;
  /** 公司營業地點地址 */
  j: string | null;
  /** 公司營業地點電話 */
  k: string | null;
  /** 公司營業地點狀態 */
  l: DbAtomManagerCompanyStatusEnum;
}

/** 管理者後台-CRM-電銷管理-控制器-取得活動名單-原始窗口項目-回應模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineRspOriginalContacterItemMdl {
  /** 窗口姓名 */
  a: string | null;
  /** 窗口Email */
  b: string | null;
  /** 窗口手機 */
  c: string | null;
  /** 窗口部門 */
  d: string | null;
  /** 窗口職稱 */
  e: string | null;
  /** 窗口電話(市話) */
  f: string | null;
  /** 窗口是否個資同意 */
  g: boolean;
  /** 原子-管理者-聯絡人狀態 */
  h: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等狀態 */
  i: DbAtomManagerContacterRatingKindEnum;
}

/** 管理者後台-CRM-電銷管理-展示層-取得活動名單-窗口項目-回應模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineRspContacterItemMdl {
  /** 窗口ID */
  a: number;
  /** 商機窗口-是否為主要窗口 */
  b: boolean;
  /** 窗口姓名 */
  c: string | null;
  /** 窗口Email */
  d: string | null;
  /** 窗口手機 */
  e: string | null;
  /** 窗口部門 */
  f: string | null;
  /** 窗口職稱 */
  g: string | null;
  /** 窗口電話(市話) */
  h: string | null;
  /** 窗口是否個資同意 */
  i: boolean;
  /** 窗口在職狀態 */
  j: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等ID */
  k: DbAtomManagerContacterRatingKindEnum;
  /** 窗口備註 */
  l: string | null;
}

/** 管理者後台-CRM-電銷管理-展示層-取得活動名單-產品項目-回應模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineRspProductItemMdl {
  /** 商機產品ID*/
  a: number;
  /** 產品ID */
  b: number;
  /** 產品名稱 */
  c: string | null;
  /** 產品主分類ID */
  d: number;
  /** 產品主分類名稱 */
  e: string | null;
  /** 產品子分類ID */
  f: number;
  /** 產品子分類名稱 */
  g: string | null;
  /** 產品規格ID */
  h: number;
  /** 產品規格名稱 */
  i: string | null;
  /** 商機產品-售價 */
  j: number;
}

/** 管理者後台-CRM-電銷管理-展示層-取得活動名單-電銷項目-回應模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineRspPhoneItemMdl {
  /** 商機電銷紀錄ID */
  a: number;
  /** 商機電銷紀錄-紀錄時間 */
  b: string | null;
  /** 窗口名稱 */
  c: string | null;
  /** 商機電銷紀錄-電銷主題 */
  d: string | null;
  /** 商機電銷紀錄-備注 */
  e: string | null;
  /** 商機電銷紀錄-電銷人員名稱 */
  f: string | null;
}

/** 管理者後台-CRM-電銷管理-展示層-取得活動名單-指派業務項目-回應模型 */
export interface MbsCrmPhnCtlGetActivityEmployeePipelineRspSalerItemMdl {
  /** 商機業務ID */
  a: number;
  /** 商機業務-狀態 */
  b: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-建立時間(指派時間) */
  c: string | null;
  /** 商機業務-指派人員名稱*/
  d: string | null;
  /** 商機業務-業務回覆時間 */
  e: string | null;
  /** 商機業務-業務員工名稱(回覆業務人員) */
  f: string | null;
  /** 商機業務-備註 */
  g: string | null;
}

/** 管理者後台-CRM-電銷管理-控制器-更新活動名單狀態-請求模型 */
export interface MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-更新活動名單狀態-回應模型 */
export interface MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆客戶過往活動-請求模型 */
export interface MbsCrmPhnCtlGetManyPastActivityReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
  /** 頁碼 */
  b: number;
  /** 每頁筆數 */
  c: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆客戶過往活動-回應模型 */
export interface MbsCrmPhnCtlGetManyPastActivityRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 最新一筆過往活動 */
  a: MbsCrmPhnCtlGetManyPastActivityRspItemMdl[] | null;
  /** 過往活動清單 */
  b: MbsCrmPhnCtlGetManyPastActivityRspItemMdl[] | null;
  /** 總筆數 */
  c: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆客戶過往活動-回應項目模型 */
export interface MbsCrmPhnCtlGetManyPastActivityRspItemMdl {
  /** 管理者活動名稱*/
  a: string | null;
  /** 管理者活動-開始時間 */
  b: string | null;
  /** 管理者活動-結束時間 */
  c: string | null;
}

//#endregion

//#region 客戶
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得客戶-請求模型 */
export interface MbsCrmPhnCtlGetEmployeePipelineCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得客戶-回應模型 */
export interface MbsCrmPhnCtlGetEmployeePipelineCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 原始公司資料 */
  a: MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl | null;
  /** 匹配公司資料 */
  b: MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl | null;
}

/** 管理者後台-CRM-電銷管理-控制器-取得客戶-回應項目模型 */
export interface MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl {
  /** 公司統一編號 */
  a: string;
  /** 客戶公司名稱 */
  b: string;
  /** 原子-人員規模 */
  c: DbAtomEmployeeRangeEnum;
  /** 原子-客戶分級 */
  d: DbAtomCustomerGradeEnum;
  /** 公司主類別名稱 */
  e: string;
  /** 公司子類別名稱 */
  f: string;
  /** 原子-縣市 */
  g: DbAtomCityEnum;
  /** 公司營業地點名稱 */
  h: string;
  /** 公司營業地點地址 */
  i: string;
  /** 公司營業地點電話 */
  j: string;
  /** 公司營業地點狀態 */
  k: DbAtomManagerCompanyStatusEnum;
}

/** 管理者後台-CRM-電銷管理-控制器-更新名單客戶-請求模型 */
export interface MbsCrmPhnCtlUpdateEmployeePipelineCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
  /** 客戶公司ID */
  b: number | null;
  /** 客戶公司營業地點ID */
  c: number | null;
}

/** 管理者後台-CRM-電銷管理-控制器-更新名單客戶-回應模型 */
export interface MbsCrmPhnCtlUpdateEmployeePipelineCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//#endregion

// #region 窗口
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得原始窗口-請求模型 */
export interface MbsCrmPhnCtlGetOriginalEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得原始窗口-回應模型 */
export interface MbsCrmPhnCtlGetOriginalEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 窗口姓名 */
  a: string | null;
  /** 窗口Email */
  b: string | null;
  /** 窗口手機 */
  c: string | null;
  /** 窗口部門 */
  d: string | null;
  /** 窗口職稱 */
  e: string | null;
  /** 窗口電話 */
  f: string | null;
  /** 是否同意 */
  g: boolean;
  /** 狀態 */
  h: DbAtomManagerContacterStatusEnum;
  /** 評分分類 */
  i: DbAtomManagerContacterRatingKindEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-新增名單窗口-請求模型 */
export interface MbsCrmPhnCtlAddEmployeePipelineContacterReqMdl {
  /** 員工登入Token */
  aa: string;
  /** 名單ID */
  a: number;
  /** 窗口ID */
  b: number;
  /** 是否主要窗口 */
  c: boolean;
}

/** 管理者後台-CRM-電銷管理-控制器-新增名單窗口-回應模型 */
export interface MbsCrmPhnCtlAddEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: number;
}
//------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-更新名單窗口-請求模型 */
export interface MbsCrmPhnCtlUpdateEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 名單窗口ID */
  a: number;
  /** 窗口ID */
  b: number | null;
  /** 是否主窗口 */
  c: boolean;
}

/** 管理者後台-CRM-電銷管理-控制器-更新名單窗口-回應模型 */
export interface MbsCrmPhnCtlUpdateEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-刪除名單窗口-請求模型 */
export interface MbsCrmPhnCtlRemoveEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 名單窗口ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-刪除名單窗口-回應模型 */
export interface MbsCrmPhnCtlRemoveEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得多筆名單窗口-請求模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆名單窗口-回應模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 名單窗口清單 */
  a: MbsCrmPhnCtlGetManyEmployeePipelineContacterRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆名單窗口-回應項目模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineContacterRspItemMdl {
  /** 商機窗口ID */
  a: number;
  /** 窗口ID */
  b: number;
  /** 是否主窗口 */
  c: boolean;
  /** 窗口姓名 */
  d: string | null;
  /** 窗口Email */
  e: string | null;
  /** 窗口手機 */
  f: string | null;
  /** 窗口部門 */
  g: string | null;
  /** 窗口職稱 */
  h: string | null;
  /** 窗口電話 */
  i: string | null;
  /** 是否同意 */
  j: boolean;
  /** 窗口狀態 */
  k: DbAtomManagerContacterStatusEnum | null;
  /** 評分分類 */
  l: DbAtomManagerContacterRatingKindEnum;
  /** 備註 */
  m: string | null;
}
//#endregion

//#region 指派業務紀錄
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-指派業務-請求模型 */
export interface MbsCrmPhnCtlAddEmployeePipelineSalerReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 名單ID */
  a: number;
  /** 被指派的業務員工ID */
  b: number;
}

/** 管理者後台-CRM-電銷管理-控制器-指派業務-回應模型 */
export interface MbsCrmPhnCtlAddEmployeePipelineSalerRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得多筆指派業務-請求模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineSalerReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆指派業務-回應模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineSalerRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 名單指派業務清單 */
  a: MbsCrmPhnCtlGetManyEmployeePipelineSalerRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆指派業務-回應項目模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineSalerRspItemMdl {
  /** 商機業務-ID*/
  a: number;
  /** 商機業務-狀態 */
  b: DbAtomEmployeePipelineSalerStatusEnum | null;
  /** 商機業務-建立時間(指派時間) */
  c: string | null;
  /** 商機業務-指派人員名稱 */
  d: string | null;
  /** 商機業務-業務回覆時間 */
  e: string | null;
  /** 商機業務-業務員工名稱(回覆業務人員) */
  f: string | null;
  /** 商機業務-備注 */
  g: string | null;
}

//#endregion

//#region 電銷紀錄
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得多筆電銷紀錄-請求模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelinePhoneReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆電銷紀錄-回應模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 電銷紀錄清單 */
  a: MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆電銷紀錄-回應項目模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspItemMdl {
  /** 商機電銷紀錄ID */
  a: number;
  /** 商機電銷紀錄-時間 */
  b: string;
  /** 商機電銷紀錄-窗口名稱 */
  c: string | null;
  /** 商機電銷紀錄-電銷主題 */
  d: string | null;
  /** 商機電銷紀錄-電銷員工名稱 */
  e: string | null;
  /** 商機電銷紀錄-備注 */
  f: string | null;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-新增電銷紀錄-請求模型 */
export interface MbsCrmPhnCtlAddEmployeePipelinePhoneReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
  /** 通話時間 */
  b: string;
  /** 窗口ID */
  c: number | null;
  /** 通話標題 */
  d: string | null;
  /** 備註 */
  e: string | null;
}

/** 管理者後台-CRM-電銷管理-控制器-新增電銷紀錄-回應模型 */
export interface MbsCrmPhnCtlAddEmployeePipelinePhoneRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//#endregion

//#region 產品
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得多筆電銷產品-請求模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆電銷產品-回應模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 電銷產品列表 */
  a: MbsCrmPhnCtlGetManyEmployeePipelineProductRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-控制器-取得多筆電銷產品-項目模型 */
export interface MbsCrmPhnCtlGetManyEmployeePipelineProductRspItemMdl {
  /** 電銷產品ID */
  a: number;
  /** 管理者產品ID */
  b: number;
  /** 管理者產品名稱 */
  c: string | null;
  /** 管理者產品主分類ID */
  d: number;
  /** 管理者產品主分類名稱 */
  e: string | null;
  /** 管理者產品子分類ID */
  f: number;
  /** 管理者產品子分類名稱 */
  g: string | null;
  /** 管理者產品規格ID */
  h: number;
  /** 管理者產品規格名稱 */
  i: string | null;
  /** 規格售價 */
  j: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-取得電銷產品-請求模型 */
export interface MbsCrmPhnCtlGetEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 名單產品ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得電銷產品-回應模型 */
export interface MbsCrmPhnCtlGetEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 名單ID */
  a: number;
  /** 主分類ID */
  b: number;
  /** 主分類名稱 */
  c: string;
  /** 次分類ID */
  d: number;
  /** 次分類名稱 */
  e: string;
  /** 產品ID */
  f: number;
  /** 產品名稱 */
  g: string | null;
  /** 規格ID */
  h: number;
  /** 規格名稱 */
  i: string | null;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-新增電銷產品-請求模型 */
export interface MbsCrmPhnCtlAddEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 名單ID */
  a: number;
  /** 產品ID */
  b: number;
  /** 規格ID */
  c: number;
}

/** 管理者後台-CRM-電銷管理-控制器-新增電銷產品-回應模型 */
export interface MbsCrmPhnCtlAddEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 名單產品ID */
  a: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-更新電銷產品-請求模型 */
export interface MbsCrmPhnCtlUpdateEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 名單產品ID */
  a: number;
  /** 產品ID */
  b: number;
  /** 規格ID */
  c: number;
}

/** 管理者後台-CRM-電銷管理-控制器-更新電銷產品-回應模型 */
export interface MbsCrmPhnCtlUpdateEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-刪除電銷產品-請求模型 */
export interface MbsCrmPhnCtlRemoveEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 名單產品ID */
  a: number;
}

/** 管理者後台-CRM-電銷管理-控制器-刪除電銷產品-回應模型 */
export interface MbsCrmPhnCtlRemoveEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//#endregion
