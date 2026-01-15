import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomEmployeePipelineSalerStatusEnum } from "@/constants/DbAtomEmployeePipelineSalerStatusEnum";

//#region 活動
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得多筆活動-請求模型 */
export interface MbsCrmPhnHttpGetManyActivityReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動-開始時間 */
  managerActivityStartTime: string | null;
  /** 管理者活動-結束時間 */
  managerActivityEndTime: string | null;
  /** 管理者活動類型 */
  managerActivityKind: DbAtomManagerActivityKindEnum | null;
  /** 管理者活動-名稱 */
  managerActivityName: string | null;
  /** 頁碼 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆活動-回應模型 */
export interface MbsCrmPhnHttpGetManyActivityRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 活動清單 */
  managerActivityList: MbsCrmPhnHttpGetManyActivityRspItemMdl[] | null;
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆活動-回應項目模型 */
export interface MbsCrmPhnHttpGetManyActivityRspItemMdl {
  /** 管理者活動ID */
  managerActivityID: number;
  /** 管理者活動-名稱 */
  managerActivityName: string;
  /** 管理者活動-類型 */
  managerActivityKind: DbAtomManagerActivityKindEnum;
  /** 管理者活動-開始時間 */
  managerActivityStartTime: string;
  /** 管理者活動-結束時間 */
  managerActivityEndTime: string;
  /** 管理者活動-實際名單數 */
  managerActivityRegisteredCount: number;
  /** 管理者活動-已轉電銷數 */
  managerActivityTransferredToSalesCount: number;
  /** 管理者活動-已撥打數 */
  managerActivityPhoneCount: number;
  /** 管理者活動-商機數 */
  managerActivityEmployeePipelineCount: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得單筆活動-請求模型 */
export interface MbsCrmPhnHttpGetActivityReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 活動ID */
  managerActivityID: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得單筆活動-回應模型 */
export interface MbsCrmPhnHttpGetActivityRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 活動名稱 */
  managerActivityName: string;
  /** 管理者活動類型 */
  managerActivityKind: DbAtomManagerActivityKindEnum;
  /** 活動開始時間 */
  managerActivityStartTime: string;
  /** 活動結束時間 */
  managerActivityEndTime: string;
  /** 管理者活動-地點 */
  managerActivityLocation: string | null;
  /** 管理者活動-期望名單數 */
  managerActivityExpectedLeadCount: number | null;
  /** 管理者活動-內容 */
  managerActivityContent: string | null;
  /** 管理者活動-實際名單數 */
  managerActivityRegisteredCount: number;
  /** 管理者活動-已轉電銷數 */
  managerActivityTransferredToSalesCount: number;
  /** 管理者活動-商機數 */
  managerActivityEmployeePipelineCount: number;
  /** 管理者活動-已撥打數(電銷紀錄數) */
  managerActivityPhoneCount: number;
  /** 管理者活動產品列表 */
  managerActivityProductList: MbsCrmPhnHttpGetActivityProductRspItemMdl[] | null;
  /** 管理者活動贊助商列表 */
  managerActivitySponsorList: MbsCrmPhnHttpGetActivitySponsorRspItemMdl[] | null;
  /** 管理者活動支出列表 */
  managerActivityExpenseList: MbsCrmPhnHttpGetActivityExpenseRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-控制器-取得單筆活動-產品-回應項目模型） */
export interface MbsCrmPhnHttpGetActivityProductRspItemMdl {
  /** 管理者活動產品ID */
  managerActivityProductID: number;
  /** 管理者產品名稱 */
  managerProductName: string;
  /** 管理者產品-主分類名稱 */
  managerProductMainKindName: string;
  /** 管理者產品-子分類名稱 */
  managerProductSubKindName: string;
}

/**管理者後台-CRM-電銷管理-控制器-取得單筆活動-贊助商-回應項目模型 */
export interface MbsCrmPhnHttpGetActivitySponsorRspItemMdl {
  /** 管理者活動贊助商ID */
  managerActivitySponsorID: number;
  /** 管理者活動贊助商-名稱 */
  managerActivitySponsorName: string;
  /** 管理者活動贊助商-金額 */
  managerActivitySponsorAmount: number;
}

/** 管理者後台-CRM-電銷管理-控制器-取得單筆活動-支出-回應項目模型 */
export interface MbsCrmPhnHttpGetActivityExpenseRspItemMdl {
  /** 管理者活動支出ID */
  managerActivityExpenseID: number;
  /** 管理者活動支出-項目 */
  managerActivityExpenseItem: string;
  /** 管理者活動支出-數量 */
  managerActivityExpenseQuantity: number;
  /** 管理者活動支出-總金額 */
  managerActivityExpenseTotalAmount: number;
}
//----------------------------------------------------------------------------------

//#endregion

//#region 活動名單
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得多筆名單-請求模型 */
export interface MbsCrmPhnHttpGetManyActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 活動ID */
  managerActivityID: number;
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
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆名單-回應模型 */
export interface MbsCrmPhnHttpGetManyActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 名單列表 */
  activityEmployeePipelineList: MbsCrmPhnHttpGetManyActivityEmployeePipelineRspItemMdl[] | null;
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆名單-回應項目模型 */
export interface MbsCrmPhnHttpGetManyActivityEmployeePipelineRspItemMdl {
  /** 商機ID */
  managerPipelineID: number;
  /** 商機狀態 */
  atomPipelineStatus: DbAtomPipelineStatusEnum | null;
  /** 是否有匹配公司 */
  hasCompany: boolean;
  /** 管理者公司ID */
  managerCompanyID: number;
  /** 管理者公司名稱 */
  managerCompanyName: string;
  /** 是否有匹配窗口 */
  hasContacter: boolean;
  /** 商機窗口ID */
  employeePipelineContacterID: number;
  /** 窗口部門 */
  managerContacterDepartment: string | null;
  /** 窗口職稱 */
  managerContacterJobTitle: string | null;
  /** 窗口姓名 */
  managerContacterName: string | null;
  /** 窗口Email */
  managerContacterEmail: string | null;
  /** 窗口手機 */
  managerContacterPhone: string | null;
  /** 窗口電話 */
  managerContacterTelephone: string | null;
  /** 最初商機開發時間 */
  employeePipelineSalerTrackingTime: string | null;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得名單-請求模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得名單-回應模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 資料庫-原子-商機-狀態-列舉 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  /** 原始客戶資訊 */
  originalCompany: MbsCrmPhnHttpGetActivityEmployeePipelineRspOriginalCompanyItemMdl | null;
  /** 是否有公司 */
  hasCompany: boolean;
  /** 公司資料 */
  company: MbsCrmPhnHttpGetActivityEmployeePipelineRspCompanyItemMdl | null;
  /** 原始窗口資料 */
  originalContacter: MbsCrmPhnHttpGetActivityEmployeePipelineRspOriginalContacterItemMdl | null;
  /** 窗口清單 */
  contacterList: MbsCrmPhnHttpGetActivityEmployeePipelineRspContacterItemMdl[] | null;
  /** Teams會議持續時間 */
  teamsMeetingDuration: string | null;
  /** 角色 */
  teamsRole: string | null;
  /** 產品列表 */
  productList: MbsCrmPhnHttpGetActivityEmployeePipelineRspProductItemMdl[] | null;
  /** 電銷紀錄列表 */
  phoneList: MbsCrmPhnHttpGetActivityEmployeePipelineRspPhoneItemMdl[] | null;
  /** 指派業務記錄列表 */
  salerList: MbsCrmPhnHttpGetActivityEmployeePipelineRspSalerItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-Http-取得活動名單-原始公司項目-回應模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineRspOriginalCompanyItemMdl {
  /** 統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 公司名稱 */
  managerCompanyName: string;
  /** 原子-人員規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum;
  /** 原子-客戶分級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  /** 公司主類別名稱 */
  managerCompanyMainKindName: string | null;
  /** 公司子類別名稱 */
  managerCompanySubKindName: string | null;
  /** 資料庫-原子-城市-列舉 */
  atomCity: DbAtomCityEnum;
  /** 公司營業地點地址 */
  managerCompanyLocationAddress: string | null;
  /** 公司營業地點電話 */
  managerCompanyLocationTelephone: string | null;
  /** 公司營業地點狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum;
}

/** 管理者後台-CRM-電銷管理-Http-取得活動名單-客戶項目-回應模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineRspCompanyItemMdl {
  /** 公司統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 客戶公司ID */
  managerCompanyID: number;
  /** 客戶公司名稱 */
  managerCompanyName: string;
  /** 原子-人員規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum;
  /** 原子-客戶分級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  /** 公司主類別名稱 */
  managerCompanyMainKindName: string | null;
  /** 公司子類別名稱 */
  managerCompanySubKindName: string | null;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum | null;
  /** 公司營業地點ID */
  managerCompanyLocationID: number | null;
  /** 公司營業地點地址 */
  managerCompanyLocationAddress: string | null;
  /** 公司營業地點電話 */
  managerCompanyLocationTelephone: string | null;
  /** 公司營業地點狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum;
}

/** 管理者後台-CRM-電銷管理-Http-取得活動名單-原始窗口項目-回應模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineRspOriginalContacterItemMdl {
  /** 窗口姓名 */
  managerContacterName: string | null;
  /** 窗口Email */
  managerContacterEmail: string | null;
  /** 窗口手機 */
  managerContacterPhone: string | null;
  /** 窗口部門 */
  managerContacterDepartment: string | null;
  /** 窗口職稱 */
  managerContacterJobTitle: string | null;
  /** 窗口電話(市話) */
  managerContacterTelephone: string | null;
  /** 窗口是否個資同意 */
  managerContacterIsConsent: boolean;
  /** 原子-管理者-聯絡人狀態 */
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等狀態 */
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
}

/** 管理者後台-CRM-電銷管理-Http-取得活動名單-窗口項目-回應模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineRspContacterItemMdl {
  /** 窗口ID */
  managerContacterID: number;
  /** 商機窗口-是否為主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
  /** 窗口姓名 */
  managerContacterName: string | null;
  /** 窗口Email */
  managerContacterEmail: string | null;
  /** 窗口手機 */
  managerContacterPhone: string | null;
  /** 窗口部門 */
  managerContacterDepartment: string | null;
  /** 窗口職稱 */
  managerContacterJobTitle: string | null;
  /** 窗口電話(市話) */
  managerContacterTelephone: string | null;
  /** 窗口是否個資同意 */
  managerContacterIsConsent: boolean;
  /** 窗口在職狀態 */
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等狀態 */
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
  /** 窗口備註 */
  managerContacterRemark: string | null;
}

/** 管理者後台-CRM-電銷管理-Http-取得名單-產品項目模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineRspProductItemMdl {
  /** 商機產品ID*/
  employeePipelineProductID: number;
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品名稱 */
  managerProductName: string | null;
  /** 主分類ID */
  managerProductMainKindID: number;
  /** 主分類名稱 */
  managerProductMainKindName: string | null;
  /** 產品子分類ID */
  managerProductSubKindID: number;
  /** 產品子分類名稱 */
  managerProductSubKindName: string | null;
  /** 產品規格ID */
  managerProductSpecificationID: number;
  /** 產品規格名稱 */
  managerProductSpecificationName: string | null;
  /** 商機產品-售價 */
  managerProductSpecificationSellPrice: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得活動名單-電銷項目-回應模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineRspPhoneItemMdl {
  /** 商機電銷紀錄ID */
  employeePipelinePhoneID: number;
  /** 商機電銷紀錄-紀錄時間 */
  employeePipelinePhoneRecordTime: string | null;
  /** 窗口名稱 */
  managerContacterName: string | null;
  /** 商機電銷紀錄-電銷主題 */
  employeePipelinePhoneTitle: string | null;
  /** 商機電銷紀錄-備注 */
  employeePipelinePhoneRemark: string | null;
  /** 商機電銷紀錄-電銷人員名稱 */
  employeePipelinePhoneCreateEmployeeName: string | null;
}

/** 管理者後台-CRM-電銷管理-Http-取得活動名單-指派業務項目-回應模型 */
export interface MbsCrmPhnHttpGetActivityEmployeePipelineRspSalerItemMdl {
  /** 商機業務ID */
  employeePipelineSalerID: number;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string | null;
  /** 商機業務-指派人員名稱*/
  employeePipelineSalerCreateEmployeeName: string | null;
  /** 商機業務-業務回覆時間 */
  employeePipelineSalerReplyTime: string | null;
  /** 商機業務-業務員工名稱(回覆業務人員) */
  employeePipelineSalerEmployeeName: string | null;
  /** 商機業務-備註 */
  employeePipelineSalerRemark: string | null;
}

/** 管理者後台-CRM-電銷管理-Http-更新活動名單狀態-請求模型 */
export interface MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-電銷管理-Http-更新活動名單狀態-回應模型 */
export interface MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//---------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得多筆客戶過往活動-請求模型 */
export interface MbsCrmPhnHttpGetManyPastActivityReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
  /** 頁碼 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆客戶過往活動-回應模型 */
export interface MbsCrmPhnHttpGetManyPastActivityRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 最新一筆過往活動 */
  latestPastActivity: MbsCrmPhnHttpGetManyPastActivityRspItemMdl[] | null;
  /** 過往活動清單 */
  pastActivityList: MbsCrmPhnHttpGetManyPastActivityRspItemMdl[] | null;
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆客戶過往活動-回應項目模型 */
export interface MbsCrmPhnHttpGetManyPastActivityRspItemMdl {
  /** 管理者活動名稱 */
  managerActivityName: string | null;
  /** 管理者活動-開始時間 */
  managerActivityStartTime: string | null;
  /** 管理者活動-結束時間 */
  managerActivityEndTime: string | null;
}

//#endregion

//#region 客戶
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得客戶-請求模型 */
export interface MbsCrmPhnHttpGetEmployeePipelineCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得客戶-回應模型 */
export interface MbsCrmPhnHttpGetEmployeePipelineCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 原始公司資料 */
  originalCompany: MbsCrmPhnHttpGetEmployeePipelineCompanyRspItemMdl | null;
  /** 匹配公司資料 */
  company: MbsCrmPhnHttpGetEmployeePipelineCompanyRspItemMdl | null;
}

/** 管理者後台-CRM-電銷管理-Http-取得客戶-回應項目模型 */
export interface MbsCrmPhnHttpGetEmployeePipelineCompanyRspItemMdl {
  /** 公司統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 客戶公司名稱 */
  managerCompanyName: string;
  /** 原子-人員規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum;
  /** 原子-客戶分級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  /** 公司主類別名稱 */
  managerCompanyMainKindName: string;
  /** 公司子類別名稱 */
  managerCompanySubKindName: string;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum;
  /** 公司營業地點名稱 */
  managerCompanyLocationName: string;
  /** 公司營業地點地址 */
  managerCompanyLocationAddress: string;
  /** 公司營業地點電話 */
  managerCompanyLocationTelephone: string;
  /** 公司營業地點狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum;
}

/** 管理者後台-CRM-電銷管理-Http-更新名單客戶-請求模型 */
export interface MbsCrmPhnHttpUpdateEmployeePipelineCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
  /** 客戶公司ID */
  managerCompanyID: number | null;
  /** 客戶公司營業地點ID */
  managerCompanyLocationID: number | null;
}

/** 管理者後台-CRM-電銷管理-Http-更新名單客戶-回應模型 */
export interface MbsCrmPhnHttpUpdateEmployeePipelineCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//#endregion

// #region 窗口
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得多筆名單窗口-請求模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆名單窗口-回應模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 名單窗口清單 */
  employeePipelineContacterList: MbsCrmPhnHttpGetManyEmployeePipelineContacterRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆名單窗口-回應項目模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineContacterRspItemMdl {
  /** 商機窗口ID */
  employeePipelineContacterID: number;
  /** 窗口ID */
  managerContacterID: number;
  /** 是否主窗口 */
  employeePipelineContacterIsPrimary: boolean;
  /** 窗口姓名 */
  managerContacterName: string | null;
  /** 窗口Email */
  managerContacterEmail: string | null;
  /** 窗口手機 */
  managerContacterPhone: string | null;
  /** 窗口部門 */
  managerContacterDepartment: string | null;
  /** 窗口職稱 */
  managerContacterJobTitle: string | null;
  /** 窗口電話 */
  managerContacterTelephone: string | null;
  /** 是否同意 */
  managerContacterIsConsent: boolean;
  /** 窗口狀態 */
  managerContacterStatus: DbAtomManagerContacterStatusEnum | null;
  /** 評分分類 */
  atomRatingKind: DbAtomManagerContacterRatingKindEnum | null;
  /** 備註 */
  managerContacterRemark: string | null;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得原始窗口-請求模型 */
export interface MbsCrmPhnHttpGetOriginalEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得原始窗口-回應模型 */
export interface MbsCrmPhnHttpGetOriginalEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 窗口姓名 */
  managerContacterName: string | null;
  /** 窗口Email */
  managerContacterEmail: string | null;
  /** 窗口手機 */
  managerContacterPhone: string | null;
  /** 窗口部門 */
  managerContacterDepartment: string | null;
  /** 窗口職稱 */
  managerContacterJobTitle: string | null;
  /** 窗口電話 */
  managerContacterTelephone: string | null;
  /** 是否同意 */
  managerContacterIsConsent: boolean;
  /** 狀態 */
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 評分分類 */
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-控制器-新增名單窗口-請求模型 */
export interface MbsCrmPhnHttpAddEmployeePipelineContacterReqMdl {
  /** 員工登入Token */
  employeeLoginToken: string;
  /** 名單ID */
  employeePipelineID: number;
  /** 窗口ID */
  managerContacterID: number;
  /** 是否主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
}

/** 管理者後台-CRM-電銷管理-控制器-新增名單窗口-回應模型 */
export interface MbsCrmPhnHttpAddEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-更新名單窗口-請求模型 */
export interface MbsCrmPhnHttpUpdateEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 名單窗口ID */
  employeePipelineContacterID: number;
  /** 窗口ID */
  managerContacterID: number | null;
  /** 是否主窗口 */
  employeePipelineContacterIsPrimary: boolean;
}

/** 管理者後台-CRM-電銷管理-Http-更新名單窗口-回應模型 */
export interface MbsCrmPhnHttpUpdateEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-刪除名單窗口-請求模型 */
export interface MbsCrmPhnHttpRemoveEmployeePipelineContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 名單窗口ID */
  employeePipelineContacterID: number;
}

/** 管理者後台-CRM-電銷管理-Http-刪除名單窗口-回應模型 */
export interface MbsCrmPhnHttpRemoveEmployeePipelineContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//#endregion

//#region 指派業務紀錄
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-新增指派業務-請求模型 */
export interface MbsCrmPhnHttpAddEmployeePipelineSalerReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 名單ID */
  employeePipelineID: number;
  /** 被指派的業務員工ID */
  employeePipelineSalerEmployeeID: number;
}

/** 管理者後台-CRM-電銷管理-Http-新增指派業務-回應模型 */
export interface MbsCrmPhnHttpAddEmployeePipelineSalerRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得多筆指派業務-請求模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineSalerReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆指派業務-回應模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineSalerRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 名單指派業務清單 */
  employeePipelineSalerList: MbsCrmPhnHttpGetManyEmployeePipelineSalerRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆指派業務-回應項目模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineSalerRspItemMdl {
  /** 商機業務ID */
  employeePipelineSalerID: number;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum | null;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string | null;
  /** 商機業務-指派人員名稱 */
  employeePipelineSalerCreateEmployeeName: string | null;
  /** 商機業務-業務回覆時間 */
  employeePipelineSalerReplyTime: string | null;
  /** 商機業務-業務員工名稱(回覆業務人員) */
  employeePipelineSalerEmployeeName: string | null;
  /** 商機業務-備注 */
  employeePipelineSalerRemark: string | null;
}
//----------------------------------------------------------------------------------
//#endregion

//#region 電銷紀錄
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得多筆電銷紀錄-請求模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelinePhoneReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆電銷紀錄-回應模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 電銷紀錄清單 */
  employeePipelinePhoneList: MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspItemMdl[] | null;
}

/** 管理者後台-CRM-電銷管理-Http-取得多筆電銷紀錄-回應項目模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspItemMdl {
  /** 商機電銷紀錄ID */
  employeePipelinePhoneID: number;
  /** 商機電銷紀錄-時間 */
  employeePipelinePhoneRecordTime: string;
  /** 商機電銷紀錄-窗口名稱 */
  managerContacterName: string | null;
  /** 商機電銷紀錄-電銷主題 */
  employeePipelinePhoneTitle: string | null;
  /** 商機電銷紀錄-電銷員工名稱 */
  employeePipelinePhoneCreateEmployeeName: string | null;
  /** 商機電銷紀錄-備注 */
  employeePipelinePhoneRemark: string | null;
}
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-新增電銷紀錄-請求模型 */
export interface MbsCrmPhnHttpAddEmployeePipelinePhoneReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 商機ID */
  employeePipelineID: number;
  /** 通話時間 */
  employeePipelinePhoneRecordTime: string;
  /** 窗口ID */
  managerContacterID: number | null;
  /** 通話標題 */
  employeePipelinePhoneTitle: string | null;
  /** 備註 */
  employeePipelinePhoneRemark: string | null;
}

/** 管理者後台-CRM-電銷管理-Http-新增電銷紀錄-回應模型 */
export interface MbsCrmPhnHttpAddEmployeePipelinePhoneRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//#endregion

//#region 產品
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-管理者取得多筆電銷產品 - 請求模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-電銷管理-Http-管理者取得多筆電銷產品-回應模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 電銷產品清單 */
  employeePipelineProductList: MbsCrmPhnHttpGetManyEmployeePipelineProductRspItemMdl[] | null;
}
/** 管理者後台-CRM-電銷管理-Http-管理者取得多筆電銷產品-項目模型 */
export interface MbsCrmPhnHttpGetManyEmployeePipelineProductRspItemMdl {
  /** 電銷產品ID */
  employeePipelineProductID: number;
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品名稱 */
  managerProductName: string | null;
  /** 管理者產品主分類ID */
  managerProductMainKindID: number;
  /** 管理者產品主分類名稱 */
  managerProductMainKindName: string | null;
  /** 管理者產品子分類ID */
  managerProductSubKindID: number;
  /** 管理者產品子分類名稱 */
  managerProductSubKindName: string | null;
  /** 管理者產品規格ID */
  managerProductSpecificationID: number;
  /** 管理者產品規格名稱 */
  managerProductSpecificationName: string | null;
  /** 規格售價 */
  managerProductSpecificationSellPrice: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-取得電銷產品-請求模型 */
export interface MbsCrmPhnHttpGetEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 名單產品ID */
  employeePipelineProductID: number;
}

/** 管理者後台-CRM-電銷管理-Http-取得電銷產品-回應模型 */
export interface MbsCrmPhnHttpGetEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 名單ID */
  employeePipelineID: number;
  /** 主分類ID */
  managerProductMainKindID: number;
  /** 主分類名稱 */
  managerProductMainKindName: string;
  /** 次分類ID */
  managerProductSubKindID: number;
  /** 次分類名稱 */
  managerProductSubKindName: string;
  /** 產品ID */
  managerProductID: number;
  /** 產品名稱 */
  managerProductName: string | null;
  /** 規格ID */
  managerProductSpecificationID: number;
  /** 規格名稱 */
  managerProductSpecificationName: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-新增電銷產品-請求模型 */
export interface MbsCrmPhnHttpAddEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 名單ID */
  employeePipelineID: number;
  /** 產品ID */
  managerProductID: number;
  /** 規格ID */
  managerProductSpecificationID: number;
}

/** 管理者後台-CRM-電銷管理-Http-新增電銷產品-回應模型 */
export interface MbsCrmPhnHttpAddEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 名單產品ID */
  employeePipelineProductID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-更新電銷產品-請求模型 */
export interface MbsCrmPhnHttpUpdateEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 名單產品ID */
  employeePipelineProductID: number;
  /** 產品ID */
  managerProductID: number;
  /** 規格ID */
  managerProductSpecificationID: number;
}

/** 管理者後台-CRM-電銷管理-Http-更新電銷產品-回應模型 */
export interface MbsCrmPhnHttpUpdateEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-Http-刪除電銷產品-請求模型 */
export interface MbsCrmPhnHttpRemoveEmployeePipelineProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 名單產品ID */
  employeePipelineProductID: number;
}

/** 管理者後台-CRM-電銷管理-Http-刪除電銷產品-回應模型 */
export interface MbsCrmPhnHttpRemoveEmployeePipelineProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//#endregion
