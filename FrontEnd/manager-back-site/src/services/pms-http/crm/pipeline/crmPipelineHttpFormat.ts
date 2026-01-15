import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomEmployeePipelineSalerStatusEnum } from "@/constants/DbAtomEmployeePipelineSalerStatusEnum";
import { DbAtomEmployeePipelineProductPurchaseKindEnum } from "@/constants/DbAtomEmployeePipelineProductPurchaseKindEnum";
import { DbAtomEmployeePipelineProductContractKindEnum } from "@/constants/DbAtomEmployeePipelineProductContractKindEnum";
import { DbAtomEmployeePipelineBillStatusEnum } from "@/constants/DbAtomEmployeePipelineBillStatusEnum";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { DbEmployeePipelineStageCheckEnum } from "@/constants/DbEmployeePipelineStageCheckEnum";

//#region 名單
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得多筆名單-請求模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機狀態 */
  atomPipelineStatus: DbAtomPipelineStatusEnum | null;
  /** 管理者公司名稱 */
  managerCompanyName: string | null;
  /** 窗口姓名 */
  managerContacterName: string | null;
  /** 窗口Email */
  managerContacterEmail: string | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
  /** 員工商機-業務員工ID (查詢條件) */
  employeePipelineSalerEmployeeID: number | null;
}

/** 管理者後台-CRM-商機管理-Http-取得多筆名單-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機列表 */
  employeePipelineList: MbsCrmPplHttpGetManyEmployeePipelineRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-CRM-商機管理-Http-取得多筆名單-項目-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineRspItemMdl {
  /** 商機ID */
  employeePipelineID: number;
  /** 商機狀態 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  /** 管理公司名稱 */
  managerCompanyName: string;
  /** 窗口部門 */
  managerContacterDepartment: string;
  /** 窗口職稱 */
  managerContacterJobTitle: string;
  /** 窗口姓名 */
  managerContacterName: string;
  /** 窗口Email */
  managerContacterEmail: string;
  /** 窗口手機 */
  managerContacterPhone: string;
  /** 窗口電話 */
  managerContacterTelephone: string;
  /** 員工商機-業務員工姓名 */
  employeePipelineSalerEmployeeName: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得名單-請求模型 */
export interface MbsCrmPplHttpGetEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 資料庫-原子-商機-狀態-列舉 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  /** 商機-承辦業務員工ID */
  employeePipelineSalerEmployeeID: number;
  /** 商機-承辦業務員工名稱 */
  employeePipelineSalerEmployeeName: string;
  /** 承辦業務地區ID */
  employeePipelineSalerRegionID: number;
  /** 承辦業務地區名稱 */
  employeePipelineSalerRegionName: string;
  /** 承辦業務部門ID */
  employeePipelineSalerDepartmentID: number;
  /** 承辦業務部門名稱 */
  employeePipelineSalerDepartmentName: string;
  /** 客戶資訊 */
  company: MbsCrmPplHttpGetEmployeePipelineRspCompanyItemMdl;
  /** 窗口資訊列表 */
  contacterList: MbsCrmPplHttpGetEmployeePipelineRspContacterItemMdl[];
  /** 尚未回覆業務紀錄 */
  pendingEmployeePipelineSaler: MbsCrmPplHttpGetEmployeePipelineRspPendingSalerItemMdl | null;
  /** 業務紀錄列表 */
  employeePipelineSalerList: MbsCrmPplHttpGetEmployeePipelineRspSalerItemMdl[];
  /** 業務商機開發紀錄列表 */
  employeePipelineSalerTrackingList: MbsCrmPplHttpGetEmployeePipelineRspSalerTrackingItemMdl[];
  /** 商機產品列表 */
  employeePipelineProductList: MbsCrmPplHttpGetEmployeePipelineRspProductItemMdl[];
  /** 履約期限列表 */
  employeePipelineDueList: MbsCrmPplHttpGetEmployeePipelineRspDueItemMdl[];
  /** 發票紀錄列表 */
  employeePipelineBillList: MbsCrmPplHttpGetEmployeePipelineRspBillItemMdl[];
  /** 階段檢核資料 */
  stageStatus: MbsCrmPplHttpGetEmployeePipelineRspStageStatusMdl;
}

/** 商機階段檢核資料 */
export interface MbsCrmPplHttpGetEmployeePipelineRspStageStatusMdl {
  businessNeedStatus: DbEmployeePipelineStageCheckEnum | null;
  businessTimelineStatus: DbEmployeePipelineStageCheckEnum | null;
  businessBudgetStatus: DbEmployeePipelineStageCheckEnum | null;
  businessPresentationStatus: DbEmployeePipelineStageCheckEnum | null;
  businessQuotationStatus: DbEmployeePipelineStageCheckEnum | null;
  businessNegotiationStatus: DbEmployeePipelineStageCheckEnum | null;
  businessStageRemark: string | null;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-客戶資訊-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspCompanyItemMdl {
  /** 公司統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 客戶公司ID */
  managerCompanyID: number;
  /** 客戶公司名稱 */
  managerCompanyName: string;
  /** 原子-人員規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  /** 原子-客戶分級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  /** 公司主類別名稱 */
  managerCompanyMainKindName: string;
  /** 公司子類別名稱 */
  managerCompanySubKindName: string;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum | null;
  /** 公司營業地點ID */
  managerCompanyLocationID: number;
  /** 公司營業地點地址 */
  managerCompanyLocationAddress: string;
  /** 公司營業地點電話 */
  managerCompanyLocationTelephone: string;
  /** 公司營業地點狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-窗口資訊-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspContacterItemMdl {
  /** 窗口ID */
  managerContacterID: number;
  /** 商機窗口-是否為主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
  /** 窗口姓名 */
  managerContacterName: string;
  /** 窗口Email */
  managerContacterEmail: string;
  /** 窗口手機 */
  managerContacterPhone: string;
  /** 窗口部門 */
  managerContacterDepartment: string;
  /** 窗口職稱 */
  managerContacterJobTitle: string;
  /** 窗口電話(市話) */
  managerContacterTelephone: string;
  /** 窗口是否個資同意 */
  managerContacterIsConsent: boolean;
  /** 窗口在職狀態 */
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等ID */
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
  /** 窗口備註 */
  managerContacterRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-指派業務紀錄項目-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspPendingSalerItemMdl {
  /** 商機業務ID */
  employeePipelineSalerID: number;
  /** 商機業務-業務員工名稱 */
  employeePipelineSalerEmployeeName: string;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-建立員工名稱(指派人員) */
  employeePipelineSalerCreateEmployeeName: string;
  /** 商機業務-是否有拒絕權限 */
  hasRejectPermissions: boolean;
  /** 商機業務-是否有接受權限 */
  hasAcceptPermissions: boolean;
  /** 商機業務-是否有轉指派權限 */
  hasReassignPermissions: boolean;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-業務項目-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspSalerItemMdl {
  /** 商機業務ID */
  employeePipelineSalerID: number;
  /** 商機業務-業務員工名稱 */
  employeePipelineSalerEmployeeName: string;
  /** 商機業務-業務回覆時間 */
  employeePipelineSalerReplyTime: string | null;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-建立員工名稱(指派人員) */
  employeePipelineSalerCreateEmployeeName: string;
  /** 商機業務-備註 */
  employeePipelineSalerRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-業務商機開發紀錄項目-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspSalerTrackingItemMdl {
  /** 商機業務開發紀錄ID */
  employeePipelineSalerTrackingID: number;
  /** 開發時間 */
  employeePipelineSalerTrackingTime: string;
  /** 窗口名稱 */
  managerContacterName: string;
  /** 備註 */
  employeePipelineSalerTrackingRemark: string;
  /** 商機業務開發紀錄-建立人員名稱(業務員工名稱) */
  employeePipelineSalerTrackingCreateEmployeeName: string;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-商機產品項目-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspProductItemMdl {
  /** 商機產品ID */
  employeePipelineProductID: number;
  /** 管理者產品名稱 */
  managerProductName: string;
  /** 管理者產品主分類名稱 */
  managerProductMainKindName: string;
  /** 管理者產品子分類名稱 */
  managerProductSubKindName: string;
  /** 管理者產品規格名稱 */
  managerProductSpecificationName: string;
  /** 商機產品售價 */
  employeePipelineProductSellPrice: number;
  /** 商機產品成交價 */
  employeePipelineProductClosingPrice: number;
  /** 商機產品成本 */
  employeePipelineProductCostPrice: number;
  /** 商機產品毛利 */
  employeePipelineProductGrossProfit: number;
  /** 商機產品數量 */
  employeePipelineProductCount: number;
  /** 商機產品新購/續約 */
  employeePipelineProductPurchaseKind: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 商機產品採購方式 */
  employeePipelineProductContractKind: DbAtomEmployeePipelineProductContractKindEnum;
  /** 商機產品採購方式文字（當選擇「其他」時） */
  employeePipelineProductContractText: string;
  /** 管理者產品主分類-業務毛利率 */
  managerProductMainKindCommissionRate: number;
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品規格ID */
  managerProductSpecificationID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-履約期限項目-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspDueItemMdl {
  /** 商機履約通知ID */
  employeePipelineDueID: number;
  /** 履約日期 */
  employeePipelineDueTime: string;
  /** 備註 */
  employeePipelineDueRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-取得名單-發票紀錄項目-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineRspBillItemMdl {
  /** 商機發票紀錄ID */
  employeePipelineBillID: number;
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 發票號碼 */
  employeePipelineBillBillNumber: string;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string;
  /** 發票狀態 */
  employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-新增商機-請求模型 */
export interface MbsCrmPplHttpAddEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司ID */
  managerCompanyID: number;
  /** 管理者公司據點ID */
  managerCompanyLocationID: number;
  /** 資料庫-原子-商機-狀態-列舉 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  /** 商機-承辦業務員工ID */
  employeePipelineSalerEmployeeID: number;
  /** 商機窗口列表 */
  contacterList: MbsCrmPplHttpAddEmployeePipelineReqContacterItemMdl[];
  /** 商機業務開發紀錄列表 */
  salerTrackingList: MbsCrmPplHttpAddEmployeePipelineReqSalerTrackingItemMdl[];
  /** 商機產品列表 */
  productList: MbsCrmPplHttpAddEmployeePipelineReqProductItemMdl[];
  /** 商機發票紀錄列表 */
  billList: MbsCrmPplHttpAddEmployeePipelineReqBillItemMdl[];
  /** 商機履約期限列表 */
  dueList: MbsCrmPplHttpAddEmployeePipelineReqDueItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-新增商機-聯絡人-請求項目模型 */
export interface MbsCrmPplHttpAddEmployeePipelineReqContacterItemMdl {
  /** 管理者聯絡人ID */
  managerContacterID: number;
  /** 員工商機聯絡人是否為主要 */
  employeePipelineContacterIsPrimary: boolean;
}

/** 管理者後台-CRM-商機管理-Http-新增商機-商機業務開發紀錄-請求項目模型 */
export interface MbsCrmPplHttpAddEmployeePipelineReqSalerTrackingItemMdl {
  /** 商機業務開發紀錄-開發時間 */
  employeePipelineSalerTrackingTime: string;
  /** 管理者窗口ID */
  managerContacterID: number;
  /** 商機業務開發紀錄-備註 */
  employeePipelineSalerTrackingRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-新增商機-產品-請求項目模型 */
export interface MbsCrmPplHttpAddEmployeePipelineReqProductItemMdl {
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品規格ID */
  managerProductSpecificationID: number;
  /** 員工商機產品銷售價格 */
  employeePipelineProductSellPrice: number;
  /** 員工商機產品成交價格 */
  employeePipelineProductClosingPrice: number;
  /** 員工商機產品成本價格 */
  employeePipelineProductCostPrice: number;
  /** 員工商機產品數量 */
  employeePipelineProductCount: number;
  /** 員工商機產品採購類型ID */
  employeePipelineProductPurchaseKindID: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 員工商機產品合約類型ID */
  employeePipelineProductContractKindID: DbAtomEmployeePipelineProductContractKindEnum;
  /** 員工商機產品合約文字 */
  employeePipelineProductContractText?: string;
}

/** 管理者後台-CRM-商機管理-Http-新增商機-商機發票紀錄-請求項目模型 */
export interface MbsCrmPplHttpAddEmployeePipelineReqBillItemMdl {
  /** 商機發票紀錄-期數 */
  employeePipelineBillPeriodNumber: number;
  /** 商機發票紀錄-發票日期 */
  employeePipelineBillBillTime: string;
  /** 商機發票紀錄-未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 商機發票紀錄-備註 */
  employeePipelineBillRemark: string | null;
  /** 商機發票紀錄-狀態 */
  employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum;
}

/** 管理者後台-CRM-商機管理-Http-新增商機-商機履約期限-請求項目模型 */
export interface MbsCrmPplHttpAddEmployeePipelineReqDueItemMdl {
  /** 商機履約期限-履約日期 */
  employeePipelineDueTime: string;
  /** 商機履約期限-備註 */
  employeePipelineDueRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-新增商機-回應模型 */
export interface MbsCrmPplHttpAddEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工商機ID */
  employeePipelineID: number;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-更新商機狀態-請求模型 */
export interface MbsCrmPplHttpUpdatePipelineStatusReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
  /** 資料庫-原子-商機-狀態-列舉 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  /** 需求確認狀態 */
  businessNeedStatus: DbEmployeePipelineStageCheckEnum | null;
  /** 時程確認狀態 */
  businessTimelineStatus: DbEmployeePipelineStageCheckEnum | null;
  /** 預算確認狀態 */
  businessBudgetStatus: DbEmployeePipelineStageCheckEnum | null;
  /** 簡報確認狀態 */
  businessPresentationStatus: DbEmployeePipelineStageCheckEnum | null;
  /** 報價確認狀態 */
  businessQuotationStatus: DbEmployeePipelineStageCheckEnum | null;
  /** 議價確認狀態 */
  businessNegotiationStatus: DbEmployeePipelineStageCheckEnum | null;
  /** 階段備註 */
  businessStageRemark: string | null;
}

/** 管理者後台-CRM-商機管理-Http-更新商機狀態-回應模型 */
export interface MbsCrmPplHttpUpdatePipelineStatusRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-轉換商機至專案-請求模型 */
export interface MbsCrmPplHttpTransferPipelineToProjectReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
  /** 員工專案代碼 */
  employeeProjectCode: string;
  /** 員工專案名稱 */
  employeeProjectName: string;
  /** 員工專案起始時間 */
  employeeProjectStartTime: string;
  /** 員工專案迄止時間 */
  employeeProjectEndTime: string;
  /** 員工專案契約網址 */
  employeeProjectContractUrl: string;
  /** 員工專案工作計劃書網址 */
  employeeProjectWorkUrl: string;
  /** 員工專案成員列表 */
  employeeProjectMemberEmployeeList: MbsCrmPplHttpTransferPipelineToProjectReqItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-轉換商機至專案-項目-請求模型 */
export interface MbsCrmPplHttpTransferPipelineToProjectReqItemMdl {
  /** 員工ID */
  employeeID: number;
  /** 員工專案成員角色 */
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum;
}

/** 管理者後台-CRM-商機管理-Http-轉換商機至專案-回應模型 */
export interface MbsCrmPplHttpTransferPipelineToProjectRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工專案ID */
  employeeProjectID: number;
}
//---------------------------------------------------------------
//#endregion

//#region 客戶
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得客戶-請求模型 */
export interface MbsCrmPplHttpGetEmployeePipelineCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得客戶-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 公司統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 客戶公司名稱 */
  managerCompanyName: string;
  /** 原子-人員規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  /** 原子-客戶分級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  /** 公司主類別名稱 */
  managerCompanyMainKindName: string;
  /** 公司子類別名稱 */
  managerCompanySubKindName: string;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum | null;
  /** 公司營業地點地址 */
  managerCompanyLocationAddress: string;
  /** 公司營業地點電話 */
  managerCompanyLocationTelephone: string;
  /** 公司營業地點狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum | null;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-更新名單客戶-請求模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
  /** 客戶公司ID */
  managerCompanyID: number;
  /** 客戶公司營業地點ID */
  managerCompanyLocationID: number;
}

/** 管理者後台-CRM-商機管理-Http-更新名單客戶-回應模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion

//#region 窗口
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-新增名單窗口-請求模型 */
export interface MbsCrmPplHttpAddEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 窗口ID */
  managerContacterID: number;
  /** 商機窗口是否為主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
}

/** 管理者後台-CRM-商機管理-Http-新增名單窗口-回應模型 */
export interface MbsCrmPplHttpAddEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-更新名單窗口-請求模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機窗口ID */
  employeePipelineContacterID: number;
  /** 窗口ID */
  managerContacterID: number;
  /** 商機窗口是否為主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
}

/** 管理者後台-CRM-商機管理-Http-更新名單窗口-回應模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-刪除名單窗口-請求模型 */
export interface MbsCrmPplHttpRemoveEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機窗口ID */
  employeePipelineContacterID: number;
}

/** 管理者後台-CRM-商機管理-Http-刪除名單窗口-回應模型 */
export interface MbsCrmPplHttpRemoveEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得多筆名單窗口-請求模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得多筆名單窗口-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機窗口列表 */
  employeePipelineContacterList: MbsCrmPplHttpGetManyEmployeePipelineContacterRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-取得多筆名單窗口-項目-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineContacterRspItemMdl {
  /** 商機窗口ID */
  employeePipelineContacterID: number;
  /** 窗口ID */
  managerContacterID: number;
  /** 商機窗口是否為主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
  /** 窗口姓名 */
  managerContacterName: string;
  /** 窗口Email */
  managerContacterEmail: string;
  /** 窗口手機 */
  managerContacterPhone: string;
  /** 窗口部門 */
  managerContacterDepartment: string;
  /** 窗口職稱 */
  managerContacterJobTitle: string;
  /** 窗口電話 */
  managerContacterTelephone: string;
  /** 窗口是否同意 */
  managerContacterIsConsent: boolean;
  /** 窗口狀態 */
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 窗口評分種類 */
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
  /** 窗口備註 */
  managerContacterRemark: string;
}
//---------------------------------------------------------------
//#endregion

//#region 業務紀錄
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得多筆商機業務-請求模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineSalerReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum | null;
}

/** 管理者後台-CRM-商機管理-Http-取得多筆商機業務-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineSalerRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機業務列表 */
  employeePipelineSalerList: MbsCrmPplHttpGetManyEmployeePipelineSalerRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-取得多筆商機業務-項目-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineSalerRspItemMdl {
  /** 商機業務ID */
  employeePipelineSalerID: number;
  /** 業務員工名稱 */
  employeePipelineSalerEmployeeName: string;
  /** 商機業務-業務回覆時間 */
  employeePipelineSalerReplyTime: string | null;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum;
  /** 指派員工名稱 */
  employeePipelineSalerCreateEmployeeName: string;
  /** 商機業務-備註 */
  employeePipelineSalerRemark: string;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-處理商機業務-請求模型 */
export interface MbsCrmPplHttpHandleEmployeePipelineSalerReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 商機業務-狀態 (限制: 2-接受, 3-拒絕, 4-轉指派業務) */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-備註 */
  employeePipelineSalerRemark: string | null;
  /** 商機業務-轉指派業務員工ID */
  employeePipelineSalerEmployeeID: number | null;
}

/** 管理者後台-CRM-商機管理-Http-處理商機業務-回應模型 */
export interface MbsCrmPplHttpHandleEmployeePipelineSalerRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion

//#region 業務開發紀錄
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得多筆商機業務開發紀錄-請求模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得多筆商機業務開發紀錄-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機業務開發紀錄列表 */
  employeePipelineSalerTrackingList: MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-取得多筆商機業務開發紀錄-項目-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingRspItemMdl {
  /** 商機業務開發紀錄ID */
  employeePipelineSalerTrackingID: number;
  /** 開發時間 */
  employeePipelineSalerTrackingTime: string;
  /** 窗口名稱 */
  managerContacterName: string;
  /** 備註 */
  employeePipelineSalerTrackingRemark: string;
  /** 商機業務開發紀錄-建立人員名稱(業務員工名稱) */
  employeePipelineSalerTrackingCreateEmployeeName: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-新增商機業務開發紀錄-請求模型 */
export interface MbsCrmPplHttpAddEmployeePipelineSalerTrackingReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 商機業務開發紀錄-開發時間 */
  employeePipelineSalerTrackingTime: string;
  /** 窗口ID */
  managerContacterID: number;
  /** 備註 */
  employeePipelineSalerTrackingRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-新增商機業務開發紀錄-回應模型 */
export interface MbsCrmPplHttpAddEmployeePipelineSalerTrackingRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機業務開發紀錄ID */
  employeePipelineSalerTrackingID: number;
}
//---------------------------------------------------------------
//#endregion

//#region 履約期限通知
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得商機履約期限-請求模型 */
export interface MbsCrmPplHttpGetEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機履約通知ID */
  employeePipelineDueID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得商機履約期限-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機履約通知ID */
  employeePipelineDueID: number;
  /** 商機ID */
  employeePipelineID: number;
  /** 履約日期 */
  employeePipelineDueTime: string;
  /** 備註 */
  employeePipelineDueRemark: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得多筆商機履約期限-請求模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得多筆商機履約期限-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機履約期限列表 */
  employeePipelineDueList: MbsCrmPplHttpGetManyEmployeePipelineDueRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-取得多筆商機履約期限-項目-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineDueRspItemMdl {
  /** 商機履約通知ID */
  employeePipelineDueID: number;
  /** 商機ID */
  employeePipelineID: number;
  /** 履約日期 */
  employeePipelineDueTime: string;
  /** 備註 */
  employeePipelineDueRemark: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-新增商機履約期限-請求模型 */
export interface MbsCrmPplHttpAddEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 履約日期 */
  employeePipelineDueTime: string;
  /** 備註 */
  employeePipelineDueRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-新增商機履約期限-回應模型 */
export interface MbsCrmPplHttpAddEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機履約通知ID */
  employeePipelineDueID: number;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-更新商機履約期限-請求模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機履約通知ID */
  employeePipelineDueID: number;
  /** 履約日期 */
  employeePipelineDueTime: string | null;
  /** 備註 */
  employeePipelineDueRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-更新商機履約期限-回應模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-刪除商機履約期限-請求模型 */
export interface MbsCrmPplHttpRemoveEmployeePipelineDueReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機履約通知ID */
  employeePipelineDueID: number;
}

/** 管理者後台-CRM-商機管理-Http-刪除商機履約期限-回應模型 */
export interface MbsCrmPplHttpRemoveEmployeePipelineDueRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion

//#region 商機產品
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得商機產品-請求模型 */
export interface MbsCrmPplHttpGetEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機產品ID */
  employeePipelineProductID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得商機產品-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機ID */
  employeePipelineID: number;
  /** 管理者產品主分類ID */
  managerProductMainKindID: number;
  /** 管理者產品主分類名稱 */
  managerProductMainKindName: string;
  /** 管理者產品子分類ID */
  managerProductSubKindID: number;
  /** 管理者產品子分類名稱 */
  managerProductSubKindName: string;
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品名稱 */
  managerProductName: string;
  /** 管理者產品規格ID */
  managerProductSpecificationID: number;
  /** 管理者產品規格名稱 */
  managerProductSpecificationName: string;
  /** 商機產品售價 */
  employeePipelineProductSellPrice: number;
  /** 商機產品成交價 */
  employeePipelineProductClosingPrice: number;
  /** 商機產品成本 */
  employeePipelineProductCostPrice: number;
  /** 商機產品數量 */
  employeePipelineProductCount: number;
  /** 商機產品新購/續約 */
  employeePipelineProductPurchaseKindID: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 商機產品採購方式 */
  employeePipelineProductContractKindID: DbAtomEmployeePipelineProductContractKindEnum;
  /** 商機產品採購文字 */
  employeePipelineProductContractText: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-新增商機產品-請求模型 */
export interface MbsCrmPplHttpAddEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品規格ID */
  managerProductSpecificationID: number;
  /** 商機產品-售價 */
  employeePipelineProductSellPrice: number;
  /** 商機產品-成交價 */
  employeePipelineProductClosingPrice: number;
  /** 商機產品-成本 */
  employeePipelineProductCostPrice: number;
  /** 商機產品-數量 */
  employeePipelineProductCount: number;
  /** 商機產品-新購/續約 */
  employeePipelineProductPurchaseKindID: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 商機產品-採購方式 */
  employeePipelineProductContractKindID: DbAtomEmployeePipelineProductContractKindEnum;
  /** 商機產品-採購文字 */
  employeePipelineProductContractText: string;
}

/** 管理者後台-CRM-商機管理-Http-新增商機產品-回應模型 */
export interface MbsCrmPplHttpAddEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機產品ID */
  employeePipelineProductID: number;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-更新商機產品-請求模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機產品ID */
  employeePipelineProductID: number;
  /** 管理者產品ID */
  managerProductID: number | null;
  /** 管理者產品規格ID */
  managerProductSpecificationID: number | null;
  /** 商機產品-售價 */
  employeePipelineProductSellPrice: number | null;
  /** 商機產品-成交價 */
  employeePipelineProductClosingPrice: number | null;
  /** 商機產品-成本 */
  employeePipelineProductCostPrice: number | null;
  /** 商機產品-數量 */
  employeePipelineProductCount: number | null;
  /** 商機產品-新購/續約 */
  employeePipelineProductPurchaseKindID: DbAtomEmployeePipelineProductPurchaseKindEnum | null;
  /** 商機產品-採購方式 */
  employeePipelineProductContractKindID: DbAtomEmployeePipelineProductContractKindEnum | null;
  /** 商機產品-採購文字 */
  employeePipelineProductContractText: string;
}

/** 管理者後台-CRM-商機管理-Http-更新商機產品-回應模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-刪除商機產品-請求模型 */
export interface MbsCrmPplHttpRemoveEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機產品ID */
  employeePipelineProductID: number;
}

/** 管理者後台-CRM-商機管理-Http-刪除商機產品-回應模型 */
export interface MbsCrmPplHttpRemoveEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion

//#region 發票紀錄
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得商機發票紀錄-請求模型 */
export interface MbsCrmPplHttpGetEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機發票紀錄ID */
  employeePipelineBillID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得商機發票紀錄-回應模型 */
export interface MbsCrmPplHttpGetEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機發票紀錄ID */
  employeePipelineBillID: number;
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 發票號碼 */
  employeePipelineBillBillNumber: string;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string;
  /** 發票狀態 */
  employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-取得多筆商機發票紀錄-請求模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-商機管理-Http-取得多筆商機發票紀錄-回應模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 商機發票紀錄列表 */
  employeePipelineBillList: MbsCrmPplHttpGetManyEmployeePipelineBillRspItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-取得多筆商機發票紀錄-回應項目模型 */
export interface MbsCrmPplHttpGetManyEmployeePipelineBillRspItemMdl {
  /** 商機發票紀錄ID */
  employeePipelineBillID: number;
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-新增多筆商機發票紀錄-請求模型 */
export interface MbsCrmPplHttpAddManyEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 商機發票紀錄列表 */
  employeePipelineBillList: MbsCrmPplHttpAddManyEmployeePipelineBillReqItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-新增多筆商機發票紀錄-請求項目模型 */
export interface MbsCrmPplHttpAddManyEmployeePipelineBillReqItemMdl {
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-新增多筆商機發票紀錄-回應模型 */
export interface MbsCrmPplHttpAddManyEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-更新商機發票紀錄-請求模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機發票紀錄ID */
  employeePipelineBillID: number;
  /** 發票號碼 */
  employeePipelineBillNumber: string;
  /** 發票狀態 */
  employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum | null;
  /** 備註 */
  employeePipelineBillRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-更新商機發票紀錄-回應模型 */
export interface MbsCrmPplHttpUpdateEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-更新多筆商機發票紀錄-請求模型 */
export interface MbsCrmPplHttpUpdateManyEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 商機發票紀錄列表 */
  employeePipelineBillList: MbsCrmPplHttpUpdateManyEmployeePipelineBillReqItemMdl[];
}

/** 管理者後台-CRM-商機管理-Http-更新多筆商機發票紀錄-請求項目模型 */
export interface MbsCrmPplHttpUpdateManyEmployeePipelineBillReqItemMdl {
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string | null;
}

/** 管理者後台-CRM-商機管理-Http-更新多筆商機發票紀錄-回應模型 */
export interface MbsCrmPplHttpUpdateManyEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-刪除多筆商機發票紀錄-請求模型 */
export interface MbsCrmPplHttpRemoveManyEmployeePipelineBillReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-商機管理-Http-刪除多筆商機發票紀錄-回應模型 */
export interface MbsCrmPplHttpRemoveManyEmployeePipelineBillRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-通知開立發票-請求模型 */
export interface MbsCrmPplHttpNotifyBillIssueReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機發票ID */
  employeePipelineBillID: number;
}

/** 管理者後台-CRM-商機管理-Http-通知開立發票-回應模型 */
export interface MbsCrmPplHttpNotifyBillIssueRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
/** 管理者後台-CRM-商機管理-Http-確認開立發票-請求模型 */
export interface MbsCrmPplHttpConfirmBillIssueReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機發票ID */
  employeePipelineBillID: number;
  /** 員工商機發票號碼 */
  employeePipelineBillNumber: string;
  /** 員工商機發票備註 */
  employeePipelineBillRemark: string;
}

/** 管理者後台-CRM-商機管理-Http-確認開立發票-回應模型 */
export interface MbsCrmPplHttpConfirmBillIssueRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------
//#endregion
