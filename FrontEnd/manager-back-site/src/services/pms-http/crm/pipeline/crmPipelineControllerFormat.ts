import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomEmployeePipelineSalerStatusEnum } from "@/constants/DbAtomEmployeePipelineSalerStatusEnum";
import { DbAtomEmployeePipelineProductPurchaseKindEnum } from "@/constants/DbAtomEmployeePipelineProductPurchaseKindEnum";
import { DbAtomEmployeePipelineProductContractKindEnum } from "@/constants/DbAtomEmployeePipelineProductContractKindEnum";
import { DbAtomEmployeePipelineBillStatusEnum } from "@/constants/DbAtomEmployeePipelineBillStatusEnum";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { DbEmployeePipelineStageCheckEnum } from "@/constants/DbEmployeePipelineStageCheckEnum";

//#region 名單
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得多筆名單-請求模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機狀態 */
  a: DbAtomPipelineStatusEnum | null;
  /** 管理者公司名稱 */
  b: string | null;
  /** 窗口姓名 */
  c: string | null;
  /** 窗口Email */
  d: string | null;
  /** 頁面索引 */
  e: number;
  /** 頁面筆數 */
  f: number;
  /** 員工商機-業務員工ID (查詢條件) */
  g: number | null;
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆名單-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機列表 */
  a: MbsCrmPplCtlGetManyEmployeePipelineRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆名單-項目-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineRspItemMdl {
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
  /** 員工商機-業務員工姓名 */
  j: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得名單-請求模型 */
export interface MbsCrmPplCtlGetEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 資料庫-原子-商機-狀態-列舉 */
  a: DbAtomPipelineStatusEnum;
  /** 商機-承辦業務員工ID */
  b: number;
  /** 商機-承辦業務員工名稱 */
  c: string;
  /** 承辦業務地區ID */
  d: number;
  /** 承辦業務地區名稱 */
  e: string;
  /** 承辦業務部門ID */
  f: number;
  /** 承辦業務部門名稱 */
  g: string;
  /** 客戶資訊 */
  h: MbsCrmPplCtlGetEmployeePipelineRspCompanyItemMdl;
  /** 窗口資訊列表 */
  i: MbsCrmPplCtlGetEmployeePipelineRspContacterItemMdl[];
  /** 尚未回覆業務紀錄 */
  j: MbsCrmPplCtlGetEmployeePipelineRspPendingSalerItemMdl | null;
  /** 業務紀錄列表 */
  k: MbsCrmPplCtlGetEmployeePipelineRspSalerItemMdl[];
  /** 業務商機開發紀錄列表 */
  l: MbsCrmPplCtlGetEmployeePipelineRspSalerTrackingItemMdl[];
  /** 商機產品列表 */
  m: MbsCrmPplCtlGetEmployeePipelineRspProductItemMdl[];
  /** 履約期限列表 */
  n: MbsCrmPplCtlGetEmployeePipelineRspDueItemMdl[];
  /** 發票紀錄列表 */
  o: MbsCrmPplCtlGetEmployeePipelineRspBillItemMdl[];
  /** 階段檢核資料 */
  p: MbsCrmPplCtlGetEmployeePipelineRspStageStatusMdl;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-階段檢核資料-模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspStageStatusMdl {
  a: DbEmployeePipelineStageCheckEnum | null;
  b: DbEmployeePipelineStageCheckEnum | null;
  c: DbEmployeePipelineStageCheckEnum | null;
  d: DbEmployeePipelineStageCheckEnum | null;
  e: DbEmployeePipelineStageCheckEnum | null;
  f: DbEmployeePipelineStageCheckEnum | null;
  g: string | null;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-客戶資訊-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspCompanyItemMdl {
  /** 公司統一編號 */
  a: string;
  /** 客戶公司ID */
  b: number;
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
  /** 公司營業地點ID */
  i: number;
  /** 公司營業地點地址 */
  j: string;
  /** 公司營業地點電話 */
  k: string;
  /** 公司營業地點狀態 */
  l: DbAtomManagerCompanyStatusEnum | null;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-窗口資訊-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspContacterItemMdl {
  /** 窗口ID */
  a: number;
  /** 商機窗口-是否為主要窗口 */
  b: boolean;
  /** 窗口姓名 */
  c: string;
  /** 窗口Email */
  d: string;
  /** 窗口手機 */
  e: string;
  /** 窗口部門 */
  f: string;
  /** 窗口職稱 */
  g: string;
  /** 窗口電話(市話) */
  h: string;
  /** 窗口是否個資同意 */
  i: boolean;
  /** 窗口在職狀態 */
  j: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等ID */
  k: DbAtomManagerContacterRatingKindEnum;
  /** 窗口備註 */
  l: string;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-指派業務紀錄項目-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspPendingSalerItemMdl {
  /** 商機業務ID */
  a: number;
  /** 商機業務-業務員工名稱 */
  b: string;
  /** 商機業務-建立時間(指派時間) */
  c: string;
  /** 商機業務-狀態 */
  d: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-建立員工名稱(指派人員) */
  e: string;
  /** 商機業務-是否有拒絕權限 */
  f: boolean;
  /** 商機業務-是否有接受權限 */
  g: boolean;
  /** 商機業務-是否有轉指派權限 */
  h: boolean;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-業務項目-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspSalerItemMdl {
  /** 商機業務ID */
  a: number;
  /** 商機業務-業務員工名稱 */
  b: string;
  /** 商機業務-業務回覆時間 */
  c: string | null;
  /** 商機業務-建立時間(指派時間) */
  d: string;
  /** 商機業務-狀態 */
  e: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-建立員工名稱(指派人員) */
  f: string;
  /** 商機業務-備註 */
  g: string;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-業務商機開發紀錄項目-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspSalerTrackingItemMdl {
  /** 商機業務開發紀錄ID */
  a: number;
  /** 開發時間 */
  b: string;
  /** 窗口名稱 */
  c: string;
  /** 備註 */
  d: string;
  /** 商機業務開發紀錄-建立人員名稱(業務員工名稱) */
  e: string;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-商機產品項目-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspProductItemMdl {
  /** 商機產品ID */
  a: number;
  /** 管理者產品名稱 */
  b: string;
  /** 管理者產品主分類名稱 */
  c: string;
  /** 管理者產品子分類名稱 */
  d: string;
  /** 管理者產品規格名稱 */
  e: string;
  /** 商機產品售價 */
  f: number;
  /** 商機產品成交價 */
  g: number;
  /** 商機產品成本 */
  h: number;
  /** 商機產品毛利 */
  i: number;
  /** 商機產品數量 */
  j: number;
  /** 商機產品新購/續約 */
  k: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 商機產品採購方式 */
  l: DbAtomEmployeePipelineProductContractKindEnum;
  /** 商機產品採購方式文字（當選擇「其他」時） */
  m: string;
  /** 管理者產品主分類-業務毛利率 */
  n: number;
  /** 管理者產品ID */
  o: number;
  /** 管理者產品規格ID */
  p: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-履約期限項目-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspDueItemMdl {
  /** 商機履約通知ID */
  a: number;
  /** 履約日期 */
  b: string;
  /** 備註 */
  c: string;
}

/** 管理者後台-CRM-商機管理-控制器-取得名單-發票紀錄項目-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineRspBillItemMdl {
  /** 商機發票紀錄ID */
  a: number;
  /** 發票期數 */
  b: number;
  /** 發票日期 */
  c: string;
  /** 發票號碼 */
  d: string;
  /** 未稅發票金額 */
  e: number;
  /** 備註 */
  f: string;
  /** 發票狀態 */
  g: DbAtomEmployeePipelineBillStatusEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-新增商機-請求模型 */
export interface MbsCrmPplCtlAddEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者公司ID */
  a: number;
  /** 管理者公司據點ID */
  b: number;
  /** 資料庫-原子-商機-狀態-列舉 */
  c: DbAtomPipelineStatusEnum;
  /** 商機-承辦業務員工ID */
  d: number;
  /** 商機窗口列表 */
  e: MbsCrmPplCtlAddEmployeePipelineReqContacterItemMdl[];
  /** 商機業務開發紀錄列表 */
  f: MbsCrmPplCtlAddEmployeePipelineReqSalerTrackingItemMdl[];
  /** 商機產品列表 */
  g: MbsCrmPplCtlAddEmployeePipelineReqProductItemMdl[];
  /** 商機發票紀錄列表 */
  h: MbsCrmPplCtlAddEmployeePipelineReqBillItemMdl[];
  /** 商機履約期限列表 */
  i: MbsCrmPplCtlAddEmployeePipelineReqDueItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-新增商機-聯絡人-請求項目模型 */
export interface MbsCrmPplCtlAddEmployeePipelineReqContacterItemMdl {
  /** 管理者聯絡人ID */
  a: number;
  /** 員工商機聯絡人是否為主要 */
  b: boolean;
}

/** 管理者後台-CRM-商機管理-控制器-新增商機-商機業務開發紀錄-請求項目模型 */
export interface MbsCrmPplCtlAddEmployeePipelineReqSalerTrackingItemMdl {
  /** 商機業務開發紀錄-開發時間 */
  a: string;
  /** 管理者窗口ID */
  b: number;
  /** 商機業務開發紀錄-備註 */
  c: string;
}

/** 管理者後台-CRM-商機管理-控制器-新增商機-產品-請求項目模型 */
export interface MbsCrmPplCtlAddEmployeePipelineReqProductItemMdl {
  /** 管理者產品ID */
  a: number;
  /** 管理者產品規格ID */
  b: number;
  /** 員工商機產品銷售價格 */
  c: number;
  /** 員工商機產品成交價格 */
  d: number;
  /** 員工商機產品成本價格 */
  e: number;
  /** 員工商機產品數量 */
  f: number;
  /** 員工商機產品採購類型ID */
  g: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 員工商機產品合約類型ID */
  h: DbAtomEmployeePipelineProductContractKindEnum;
  /** 員工商機產品合約文字 */
  i?: string;
}

/** 管理者後台-CRM-商機管理-控制器-新增商機-商機發票紀錄-請求項目模型 */
export interface MbsCrmPplCtlAddEmployeePipelineReqBillItemMdl {
  /** 商機發票紀錄-期數 */
  a: number;
  /** 商機發票紀錄-發票日期 */
  b: string;
  /** 商機發票紀錄-未稅發票金額 */
  c: number;
  /** 商機發票紀錄-備註 */
  d: string | null;
  /** 商機發票紀錄-狀態 */
  e: DbAtomEmployeePipelineBillStatusEnum;
}

/** 管理者後台-CRM-商機管理-控制器-新增商機-商機履約期限-請求項目模型 */
export interface MbsCrmPplCtlAddEmployeePipelineReqDueItemMdl {
  /** 商機履約期限-履約日期 */
  a: string;
  /** 商機履約期限-備註 */
  b: string;
}

/** 管理者後台-CRM-商機管理-控制器-新增商機-回應模型 */
export interface MbsCrmPplCtlAddEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工商機ID */
  a: number;
}

//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-更新商機狀態-請求模型 */
export interface MbsCrmPplCtlUpdatePipelineStatusReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
  /** 資料庫-原子-商機-狀態-列舉 */
  b: DbAtomPipelineStatusEnum;
  /** 需求確認狀態 */
  c: DbEmployeePipelineStageCheckEnum | null;
  /** 時程確認狀態 */
  d: DbEmployeePipelineStageCheckEnum | null;
  /** 預算確認狀態 */
  e: DbEmployeePipelineStageCheckEnum | null;
  /** 簡報確認狀態 */
  f: DbEmployeePipelineStageCheckEnum | null;
  /** 報價確認狀態 */
  g: DbEmployeePipelineStageCheckEnum | null;
  /** 議價確認狀態 */
  h: DbEmployeePipelineStageCheckEnum | null;
  /** 階段備註 */
  i: string | null;
}

/** 管理者後台-CRM-商機管理-控制器-更新商機狀態-回應模型 */
export interface MbsCrmPplCtlUpdatePipelineStatusRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-轉換商機至專案-請求模型 */
export interface MbsCrmPplCtlTransferPipelineToProjectReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
  /** 員工專案代碼 */
  b: string;
  /** 員工專案名稱 */
  c: string;
  /** 員工專案起始時間 */
  d: string;
  /** 員工專案迄止時間 */
  e: string;
  /** 員工專案契約網址 */
  f: string;
  /** 員工專案工作計劃書網址 */
  g: string;
  /** 員工專案成員列表 */
  h: MbsCrmPplCtlTransferPipelineToProjectReqItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-轉換商機至專案-項目-請求模型 */
export interface MbsCrmPplCtlTransferPipelineToProjectReqItemMdl {
  /** 員工ID */
  a: number;
  /** 員工專案成員角色 */
  b: DbEmployeeProjectMemberRoleEnum;
}

/** 管理者後台-CRM-商機管理-控制器-轉換商機至專案-回應模型 */
export interface MbsCrmPplCtlTransferPipelineToProjectRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工專案ID */
  a: number;
}
//---------------------------------------------------------------
//#endregion

//#region 客戶
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得客戶-請求模型 */
export interface MbsCrmPplCtlGetEmployeePipelineCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得客戶-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 公司統一編號 */
  a: string;
  /** 客戶公司名稱 */
  b: string;
  /** 原子-人員規模 */
  c: DbAtomEmployeeRangeEnum | null;
  /** 原子-客戶分級 */
  d: DbAtomCustomerGradeEnum | null;
  /** 公司主類別名稱 */
  e: string;
  /** 公司子類別名稱 */
  f: string;
  /** 原子-縣市 */
  g: DbAtomCityEnum | null;
  /** 公司營業地點地址 */
  h: string;
  /** 公司營業地點電話 */
  i: string;
  /** 公司營業地點狀態 */
  j: DbAtomManagerCompanyStatusEnum | null;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-更新名單客戶-請求模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機ID */
  a: number;
  /** 客戶公司ID */
  b: number;
  /** 客戶公司營業地點ID */
  c: number;
}

/** 管理者後台-CRM-商機管理-控制器-更新名單客戶-回應模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion

//#region 窗口
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-新增名單窗口-請求模型 */
export interface MbsCrmPplCtlAddEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
  /** 窗口ID */
  b: number;
  /** 商機窗口是否為主要窗口 */
  c: boolean;
}

/** 管理者後台-CRM-商機管理-控制器-新增名單窗口-回應模型 */
export interface MbsCrmPplCtlAddEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-更新名單窗口-請求模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機窗口ID */
  a: number;
  /** 窗口ID */
  b: number;
  /** 商機窗口是否為主要窗口 */
  c: boolean;
}

/** 管理者後台-CRM-商機管理-控制器-更新名單窗口-回應模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-刪除名單窗口-請求模型 */
export interface MbsCrmPplCtlRemoveEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機窗口ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-刪除名單窗口-回應模型 */
export interface MbsCrmPplCtlRemoveEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得多筆名單窗口-請求模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆名單窗口-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機窗口列表 */
  a: MbsCrmPplCtlGetManyEmployeePipelineContacterRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆名單窗口-項目-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineContacterRspItemMdl {
  /** 商機窗口ID */
  a: number;
  /** 窗口ID */
  b: number;
  /** 商機窗口是否為主要窗口 */
  c: boolean;
  /** 窗口姓名 */
  d: string;
  /** 窗口Email */
  e: string;
  /** 窗口手機 */
  f: string;
  /** 窗口部門 */
  g: string;
  /** 窗口職稱 */
  h: string;
  /** 窗口電話 */
  i: string;
  /** 窗口是否同意 */
  j: boolean;
  /** 窗口狀態 */
  k: DbAtomManagerContacterStatusEnum;
  /** 窗口評分種類 */
  l: DbAtomManagerContacterRatingKindEnum;
  /** 窗口備註 */
  m: string;
}
//---------------------------------------------------------------
//#endregion

//#region 業務紀錄
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得多筆商機業務-請求模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineSalerReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
  /** 商機業務-狀態 */
  b: DbAtomEmployeePipelineSalerStatusEnum | null;
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆商機業務-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineSalerRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機業務列表 */
  a: MbsCrmPplCtlGetManyEmployeePipelineSalerRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆商機業務-項目-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineSalerRspItemMdl {
  /** 商機業務ID */
  a: number;
  /** 業務員工名稱 */
  b: string;
  /** 商機業務-業務回覆時間 */
  c: string | null;
  /** 商機業務-狀態 */
  d: DbAtomEmployeePipelineSalerStatusEnum;
  /** 指派員工名稱 */
  e: string;
  /** 商機業務-備註 */
  f: string;
  /** 商機業務-建立時間(指派時間) */
  g: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-處理商機業務-請求模型 */
export interface MbsCrmPplCtlHandleEmployeePipelineSalerReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機業務ID */
  a: number;
  /** 商機業務-狀態 (限制: 2-接受, 3-拒絕, 4-轉指派業務) */
  b: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-備註 */
  c: string | null;
  /** 商機業務-轉指派業務員工ID */
  d: number | null;
}

/** 管理者後台-CRM-商機管理-控制器-處理商機業務-回應模型 */
export interface MbsCrmPplCtlHandleEmployeePipelineSalerRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion

//#region 業務開發紀錄
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得多筆商機業務開發紀錄-請求模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆商機業務開發紀錄-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機業務開發紀錄列表 */
  a: MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆商機業務開發紀錄-項目-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspItemMdl {
  /** 商機業務開發紀錄ID */
  a: number;
  /** 開發時間 */
  b: string;
  /** 窗口名稱 */
  c: string;
  /** 備註 */
  d: string;
  /** 商機業務開發紀錄-建立人員名稱(業務員工名稱) */
  e: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-新增商機業務開發紀錄-請求模型 */
export interface MbsCrmPplCtlAddEmployeePipelineSalerTrackingReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
  /** 商機業務開發紀錄-開發時間 */
  b: string;
  /** 窗口ID */
  c: number;
  /** 備註 */
  d: string;
}

/** 管理者後台-CRM-商機管理-控制器-新增商機業務開發紀錄-回應模型 */
export interface MbsCrmPplCtlAddEmployeePipelineSalerTrackingRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機業務開發紀錄ID */
  a: number;
}
//---------------------------------------------------------------
//#endregion

//#region 履約期限通知
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得商機履約期限-請求模型 */
export interface MbsCrmPplCtlGetEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機履約通知ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得商機履約期限-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機履約通知ID */
  a: number;
  /** 商機ID */
  b: number;
  /** 履約日期 */
  c: string;
  /** 備註 */
  d: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得多筆商機履約期限-請求模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆商機履約期限-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機履約期限列表 */
  a: MbsCrmPplCtlGetManyEmployeePipelineDueRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆商機履約期限-項目-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineDueRspItemMdl {
  /** 商機履約通知ID */
  a: number;
  /** 商機ID */
  b: number;
  /** 履約日期 */
  c: string;
  /** 備註 */
  d: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-新增商機履約期限-請求模型 */
export interface MbsCrmPplCtlAddEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
  /** 履約日期 */
  b: string;
  /** 備註 */
  c: string;
}

/** 管理者後台-CRM-商機管理-控制器-新增商機履約期限-回應模型 */
export interface MbsCrmPplCtlAddEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機履約通知ID */
  a: number;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-更新商機履約期限-請求模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機履約通知ID */
  a: number;
  /** 履約日期 */
  b: string | null;
  /** 備註 */
  c: string;
}

/** 管理者後台-CRM-商機管理-控制器-更新商機履約期限-回應模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-刪除商機履約期限-請求模型 */
export interface MbsCrmPplCtlRemoveEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機履約通知ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-刪除商機履約期限-回應模型 */
export interface MbsCrmPplCtlRemoveEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion

//#region 商機產品
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得商機產品-請求模型 */
export interface MbsCrmPplCtlGetEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機產品ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得商機產品-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機ID */
  a: number;
  /** 管理者產品主分類ID */
  b: number;
  /** 管理者產品主分類名稱 */
  c: string;
  /** 管理者產品子分類ID */
  d: number;
  /** 管理者產品子分類名稱 */
  e: string;
  /** 管理者產品ID */
  f: number;
  /** 管理者產品名稱 */
  g: string;
  /** 管理者產品規格ID */
  h: number;
  /** 管理者產品規格名稱 */
  i: string;
  /** 商機產品售價 */
  j: number;
  /** 商機產品成交價 */
  k: number;
  /** 商機產品成本 */
  l: number;
  /** 商機產品數量 */
  m: number;
  /** 商機產品新購/續約 */
  n: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 商機產品採購方式 */
  o: DbAtomEmployeePipelineProductContractKindEnum;
  /** 商機產品採購文字 */
  p: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-新增商機產品-請求模型 */
export interface MbsCrmPplCtlAddEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
  /** 管理者產品ID */
  b: number;
  /** 管理者產品規格ID */
  c: number;
  /** 商機產品-售價 */
  d: number;
  /** 商機產品-成交價 */
  e: number;
  /** 商機產品-成本 */
  f: number;
  /** 商機產品-數量 */
  g: number;
  /** 商機產品-新購/續約 */
  h: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 商機產品-採購方式 */
  i: DbAtomEmployeePipelineProductContractKindEnum;
  /** 商機產品-採購文字 */
  j: string;
}

/** 管理者後台-CRM-商機管理-控制器-新增商機產品-回應模型 */
export interface MbsCrmPplCtlAddEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機產品ID */
  a: number;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-更新商機產品-請求模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機產品ID */
  a: number;
  /** 管理者產品ID */
  b: number | null;
  /** 管理者產品規格ID */
  c: number | null;
  /** 商機產品-售價 */
  d: number | null;
  /** 商機產品-成交價 */
  e: number | null;
  /** 商機產品-成本 */
  f: number | null;
  /** 商機產品-數量 */
  g: number | null;
  /** 商機產品-新購/續約 */
  h: DbAtomEmployeePipelineProductPurchaseKindEnum | null;
  /** 商機產品-採購方式 */
  i: DbAtomEmployeePipelineProductContractKindEnum | null;
  /** 商機產品-採購文字 */
  j: string;
}

/** 管理者後台-CRM-商機管理-控制器-更新商機產品-回應模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-刪除商機產品-請求模型 */
export interface MbsCrmPplCtlRemoveEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機產品ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-刪除商機產品-回應模型 */
export interface MbsCrmPplCtlRemoveEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion

//#region 發票紀錄
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得商機發票紀錄-請求模型 */
export interface MbsCrmPplCtlGetEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機發票紀錄ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得商機發票紀錄-回應模型 */
export interface MbsCrmPplCtlGetEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機發票紀錄ID */
  a: number;
  /** 發票期數 */
  b: number;
  /** 發票日期 */
  c: string;
  /** 發票號碼 */
  d: string;
  /** 未稅發票金額 */
  e: number;
  /** 備註 */
  f: string;
  /** 發票狀態 */
  g: DbAtomEmployeePipelineBillStatusEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-取得多筆商機發票紀錄-請求模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆商機發票紀錄-回應模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 商機發票紀錄列表 */
  a: MbsCrmPplCtlGetManyEmployeePipelineBillRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-取得多筆商機發票紀錄-回應項目模型 */
export interface MbsCrmPplCtlGetManyEmployeePipelineBillRspItemMdl {
  /** 商機發票紀錄ID */
  a: number;
  /** 發票期數 */
  b: number;
  /** 發票日期 */
  c: string;
  /** 未稅發票金額 */
  d: number;
  /** 備註 */
  e: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-新增多筆商機發票紀錄-請求模型 */
export interface MbsCrmPplCtlAddManyEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
  /** 商機發票紀錄列表 */
  b: MbsCrmPplCtlAddManyEmployeePipelineBillReqItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-新增多筆商機發票紀錄-請求項目模型 */
export interface MbsCrmPplCtlAddManyEmployeePipelineBillReqItemMdl {
  /** 發票期數 */
  a: number;
  /** 發票日期 */
  b: string;
  /** 未稅發票金額 */
  c: number;
  /** 備註 */
  d: string;
}

/** 管理者後台-CRM-商機管理-控制器-新增多筆商機發票紀錄-回應模型 */
export interface MbsCrmPplCtlAddManyEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-更新商機發票紀錄-請求模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機發票紀錄ID */
  a: number;
  /** 發票號碼 */
  b: string;
  /** 發票狀態 */
  c: DbAtomEmployeePipelineBillStatusEnum | null;
  /** 備註 */
  d: string;
}

/** 管理者後台-CRM-商機管理-控制器-更新商機發票紀錄-回應模型 */
export interface MbsCrmPplCtlUpdateEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-更新多筆商機發票紀錄-請求模型 */
export interface MbsCrmPplCtlUpdateManyEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
  /** 商機發票紀錄列表 */
  b: MbsCrmPplCtlUpdateManyEmployeePipelineBillReqItemMdl[];
}

/** 管理者後台-CRM-商機管理-控制器-更新多筆商機發票紀錄-請求項目模型 */
export interface MbsCrmPplCtlUpdateManyEmployeePipelineBillReqItemMdl {
  /** 發票期數 */
  a: number;
  /** 發票日期 */
  b: string;
  /** 未稅發票金額 */
  c: number;
  /** 備註 */
  d: string | null;
}

/** 管理者後台-CRM-商機管理-控制器-更新多筆商機發票紀錄-回應模型 */
export interface MbsCrmPplCtlUpdateManyEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-刪除多筆商機發票紀錄-請求模型 */
export interface MbsCrmPplCtlRemoveManyEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 商機ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-刪除多筆商機發票紀錄-回應模型 */
export interface MbsCrmPplCtlRemoveManyEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-通知開立發票-請求模型 */
export interface MbsCrmPplCtlNotifyBillIssueReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機發票ID */
  a: number;
}

/** 管理者後台-CRM-商機管理-控制器-通知開立發票-回應模型 */
export interface MbsCrmPplCtlNotifyBillIssueRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-控制器-確認開立發票-請求模型 */
export interface MbsCrmPplCtlConfirmBillIssueReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工商機發票ID */
  a: number;
  /** 員工商機發票號碼 */
  b: string;
  /** 員工商機發票備註 */
  c: string;
}

/** 管理者後台-CRM-商機管理-控制器-確認開立發票-回應模型 */
export interface MbsCrmPplCtlConfirmBillIssueRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion
