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
/** 管理者後台-CRM-活動管理-Http-新增-請求模型 */
export interface MbsCrmActHttpAddActivityReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動名稱 */
  managerActivityName: string;
  /** 管理者活動類型 */
  managerActivityKind: DbAtomManagerActivityKindEnum;
  /** 管理者活動-開始時間 */
  managerActivityStartTime: string;
  /** 管理者活動-結束時間 */
  managerActivityEndTime: string;
  /** 管理者活動-地點 */
  managerActivityLocation: string;
  /** 管理者活動-期望名單數 */
  managerActivityExpectedLeadCount: number;
  /** 管理者活動-內容 */
  managerActivityContent: string | null;
  /** 管理者活動-活動產品清單 */
  managerActivityProductList: MbsCrmActHttpAddActivityReqProductItemMdl[] | null;
  /** 管理者活動-活動贊助清單 */
  managerActivitySponsorList: MbsCrmActHttpAddActivityReqSponsorItemMdl[] | null;
  /** 管理者活動-活動支出清單 */
  managerActivityExpenseList: MbsCrmActHttpAddActivityReqExpenseItemMdl[] | null;
}

/** 管理者後台-CRM-活動管理-Http-新增-產品項目-請求模型 */
export interface MbsCrmActHttpAddActivityReqProductItemMdl {
  /** 管理者產品ID */
  managerProductID: number;
}

/** 管理者後台-CRM-活動管理-Http-新增-贊助項目-請求模型 */
export interface MbsCrmActHttpAddActivityReqSponsorItemMdl {
  /** 管理者活動贊助商-名稱 */
  managerActivitySponsorName: string;
  /** 管理者活動贊助商-金額 */
  managerActivitySponsorAmount: number;
}

/** 管理者後台-CRM-活動管理-Http-新增-支出項目-請求模型 */
export interface MbsCrmActHttpAddActivityReqExpenseItemMdl {
  /** 管理者活動支出-項目 */
  managerActivityExpenseItem: string;
  /** 管理者活動支出-數量 */
  managerActivityExpenseQuantity: number;
  /** 管理者活動支出-總金額 */
  managerActivityExpenseTotalAmount: number;
}

/** 管理者後台-CRM-活動管理-Http-新增-回應模型 */
export interface MbsCrmActHttpAddActivityRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動ID */
  managerActivityID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-取得單筆-請求模型 */
export interface MbsCrmActHttpGetActivityReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
}

/** 管理者後台-CRM-活動管理-Http-取得單筆-回應模型 */
export interface MbsCrmActHttpGetActivityRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動名稱 */
  managerActivityName: string;
  /** 管理者活動類型 */
  managerActivityKind: DbAtomManagerActivityKindEnum;
  /** 管理者活動-開始時間 */
  managerActivityStartTime: string;
  /** 管理者活動-結束時間 */
  managerActivityEndTime: string;
  /** 管理者活動-地點 */
  managerActivityLocation: string;
  /** 管理者活動-期望名單數 */
  managerActivityExpectedLeadCount: number;
  /** 管理者活動-內容 */
  managerActivityContent: string;
  /** 管理者活動-實際名單數 */
  managerActivityRegisteredCount: number;
  /** 管理者活動-已轉電銷數 */
  managerActivityTransferredToSalesCount: number;
  /** 管理者活動-商機數 */
  managerActivityOpportunityCount: number;
  /** 管理者活動產品列表 */
  managerActivityProductList: MbsCrmActHttpGetManyActivityProductRspItemMdl[];
  /** 管理者活動贊助商列表 */
  managerActivitySponsorList: MbsCrmActHttpGetManyActivitySponsorRspItemMdl[];
  /** 管理者活動支出列表 */
  managerActivityExpenseList: MbsCrmActHttpGetManyActivityExpenseRspItemMdl[];
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-取得多筆-請求模型 */
export interface MbsCrmActHttpGetManyActivityReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動-開始時間 */
  managerActivityStartTime: string | null;
  /** 管理者活動-結束時間 */
  managerActivityEndTime: string | null;
  /** 管理者活動-類型 */
  managerActivityKind: DbAtomManagerActivityKindEnum | null;
  /** 管理者活動-名稱 */
  managerActivityName: string | null;
  /** 頁數 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 管理者後台-CRM-活動管理-Http-取得多筆-回應模型 */
export interface MbsCrmActHttpGetManyActivityRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動列表 */
  managerActivityList: MbsCrmActHttpGetManyActivityRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-CRM-活動管理-Http-取得多筆-回應項目模型 */
export interface MbsCrmActHttpGetManyActivityRspItemMdl {
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
  /** 管理者活動-地點 */
  managerActivityLocation: string;
  /** 管理者活動-預期名單數 */
  managerActivityExpectedLeadCount: number;
  /** 管理者活動-實際名單數 */
  managerActivityRegisteredCount: number;
  /** 管理者活動-已轉電銷數 */
  managerActivityTransferredToSalesCount: number;
  /** 管理者活動-總贊助金額 */
  managerActivitySponsorTotalSponsorAmount: number;
  /** 管理者活動-總支出 */
  managerActivityExpenseTotalExpenseAmount: number;
  /** 管理者活動-商機數 */
  managerActivityOpportunityCount: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-更新-請求模型 */
export interface MbsCrmActHttpUpdateActivityReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
  /** 管理者活動名稱 */
  managerActivityName: string | null;
  /** 管理者活動-開始時間 */
  managerActivityStartTime: string | null;
  /** 管理者活動-結束時間 */
  managerActivityEndTime: string | null;
  /** 管理者活動-地點 */
  managerActivityLocation: string | null;
  /** 管理者活動-期望名單數 */
  managerActivityExpectedLeadCount: number | null;
  /** 管理者活動-內容 */
  managerActivityContent: string | null;
}

/** 管理者後台-CRM-活動管理-Http-更新-回應模型 */
export interface MbsCrmActHttpUpdateActivityRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------

//#endregion

//#region 活動產品
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-Http-新增-請求模型 */
export interface MbsCrmActHttpAddActivityProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
  /** 管理者產品ID */
  managerProductID: number;
}

/** 管理者後台-CRM-活動管理-活動產品-Http-新增-回應模型 */
export interface MbsCrmActHttpAddActivityProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動產品ID */
  managerActivityProductID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-Http-取得-請求模型 */
export interface MbsCrmActHttpGetActivityProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動產品ID */
  managerActivityProductID: number;
}

/** 管理者後台-CRM-活動管理-活動產品-Http-取得-回應模型 */
export interface MbsCrmActHttpGetActivityProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動產品ID */
  managerActivityProductID: number;
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者-產品-主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品-子分類-ID */
  managerProductSubKindID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-Http-取得多筆-請求模型 */
export interface MbsCrmActHttpGetManyActivityProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
}

/** 管理者後台-CRM-活動管理-活動產品-Http-取得多筆-回應模型 */
export interface MbsCrmActHttpGetManyActivityProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動產品列表 */
  managerActivityProductList: MbsCrmActHttpGetManyActivityProductRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-活動產品-Http-取得多筆-回應項目模型 */
export interface MbsCrmActHttpGetManyActivityProductRspItemMdl {
  /** 管理者活動-產品ID */
  managerActivityProductID: number;
  /** 管理者產品-ID */
  managerProductID: number;
  /** 管理者產品-名稱 */
  managerProductName: string;
  /** 管理者產品-主分類-ID */
  managerProductMainKindID: number;
  /** 管理者產品-主分類名稱 */
  managerProductMainKindName: string;
  /** 管理者產品-子分類-ID */
  managerProductSubKindID: number;
  /** 管理者產品-子分類名稱 */
  managerProductSubKindName: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-Http-刪除-請求模型 */
export interface MbsCrmActHttpRemoveActivityProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動產品ID */
  managerActivityProductID: number;
}

/** 管理者後台-CRM-活動管理-活動產品-Http-刪除-回應模型 */
export interface MbsCrmActHttpRemoveActivityProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動產品-Http-更新-請求模型 */
export interface MbsCrmActHttpUpdateActivityProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動產品ID */
  managerActivityProductID: number;
  /** 管理者產品ID */
  managerProductID: number | null;
}

/** 管理者後台-CRM-活動管理-活動產品-Http-更新-回應模型 */
export interface MbsCrmActHttpUpdateActivityProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------

//#endregion

//#region 活動贊助
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-Http-新增-請求模型 */
export interface MbsCrmActHttpAddActivitySponsorReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
  /** 管理者活動贊助商-名稱 */
  managerActivitySponsorName: string;
  /** 管理者活動贊助商-金額 */
  managerActivitySponsorAmount: number;
}

/** 管理者後台-CRM-活動管理-活動贊助-Http-新增-回應模型 */
export interface MbsCrmActHttpAddActivitySponsorRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動贊助商ID */
  managerActivitySponsorID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-Http-取得-請求模型 */
export interface MbsCrmActHttpGetActivitySponsorReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動贊助商ID */
  managerActivitySponsorID: number;
}

/** 管理者後台-CRM-活動管理-活動贊助-Http-取得-回應模型 */
export interface MbsCrmActHttpGetActivitySponsorRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動贊助商-名稱 */
  managerActivitySponsorName: string;
  /** 管理者活動贊助商-金額 */
  managerActivitySponsorAmount: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-Http-取得多筆-請求模型 */
export interface MbsCrmActHttpGetManyActivitySponsorReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
}

/** 管理者後台-CRM-活動管理-活動贊助-Http-取得多筆-回應模型 */
export interface MbsCrmActHttpGetManyActivitySponsorRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動贊助商列表 */
  managerActivitySponsorList: MbsCrmActHttpGetManyActivitySponsorRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-活動贊助-Http-取得多筆-回應項目模型 */
export interface MbsCrmActHttpGetManyActivitySponsorRspItemMdl {
  /** 管理者活動贊助商ID */
  managerActivitySponsorID: number;
  /** 管理者活動贊助商-名稱 */
  managerActivitySponsorName: string;
  /** 管理者活動贊助商-金額 */
  managerActivitySponsorAmount: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-Http-刪除-請求模型 */
export interface MbsCrmActHttpRemoveActivitySponsorReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動贊助商ID */
  managerActivitySponsorID: number;
}

/** 管理者後台-CRM-活動管理-活動贊助-Http-刪除-回應模型 */
export interface MbsCrmActHttpRemoveActivitySponsorRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動贊助-Http-更新-請求模型 */
export interface MbsCrmActHttpUpdateActivitySponsorReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動贊助商ID */
  managerActivitySponsorID: number;
  /** 管理者活動贊助商-名稱 */
  managerActivitySponsorName: string | null;
  /** 管理者活動贊助商-金額 */
  managerActivitySponsorAmount: number | null;
}

/** 管理者後台-CRM-活動管理-活動贊助-Http-更新-回應模型 */
export interface MbsCrmActHttpUpdateActivitySponsorRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
//#endregion

//#region 活動支出
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動支出-Http-新增-請求模型 */
export interface MbsCrmActHttpAddActivityExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
  /** 管理者活動支出-項目 */
  managerActivityExpenseItem: string;
  /** 管理者活動支出-數量 */
  managerActivityExpenseQuantity: number;
  /** 管理者活動支出-總金額 */
  managerActivityExpenseTotalAmount: number;
}

/** 管理者後台-CRM-活動管理-活動支出-Http-新增-回應模型 */
export interface MbsCrmActHttpAddActivityExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動支出ID */
  managerActivityExpenseID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動支出-Http-取得-請求模型 */
export interface MbsCrmActHttpGetActivityExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動支出ID */
  managerActivityExpenseID: number;
}

/** 管理者後台-CRM-活動管理-活動支出-Http-取得-回應模型 */
export interface MbsCrmActHttpGetActivityExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
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
/** 管理者後台-CRM-活動管理-活動支出-Http-取得多筆-請求模型 */
export interface MbsCrmActHttpGetManyActivityExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
}

/** 管理者後台-CRM-活動管理-活動支出-Http-取得多筆-回應模型 */
export interface MbsCrmActHttpGetManyActivityExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者活動支出列表 */
  managerActivityExpenseList: MbsCrmActHttpGetManyActivityExpenseRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-活動支出-Http-取得多筆-回應項目模型 */
export interface MbsCrmActHttpGetManyActivityExpenseRspItemMdl {
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
/** 管理者後台-CRM-活動管理-活動支出-Http-刪除-請求模型 */
export interface MbsCrmActHttpRemoveActivityExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動支出ID */
  managerActivityExpenseID: number;
}

/** 管理者後台-CRM-活動管理-活動支出-Http-刪除-回應模型 */
export interface MbsCrmActHttpRemoveActivityExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-活動支出-Http-更新-請求模型 */
export interface MbsCrmActHttpUpdateActivityExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動支出ID */
  managerActivityExpenseID: number;
  /** 管理者活動支出-項目 */
  managerActivityExpenseItem: string | null;
  /** 管理者活動支出-數量 */
  managerActivityExpenseQuantity: number | null;
  /** 管理者活動支出-總金額 */
  managerActivityExpenseTotalAmount: number | null;
}

/** 管理者後台-CRM-活動管理-活動支出-Http-更新-回應模型 */
export interface MbsCrmActHttpUpdateActivityExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
//#endregion

//#region 活動名單
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-新增活動名單-請求模型 */
export interface MbsCrmActHttpAddActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
  /** 客戶公司ID */
  managerCompanyID: number;
  /** 客戶公司營業地點ID */
  managerCompanyLocationID: number;
  /** 窗口ID */
  managerContacterID: number;
  /** 商機狀態 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
}

/** 管理者後台-CRM-活動管理-Http-新增活動名單-回應模型 */
export interface MbsCrmActHttpAddActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工商機ID */
  employeePipelineID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-取得活動名單-請求模型 */
export interface MbsCrmActHttpGetActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
}

/** 管理者後台-CRM-活動管理-Http-取得活動名單-回應模型 */
export interface MbsCrmActHttpGetActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 資料庫-原子-商機-狀態-列舉 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
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
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
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
  /** Teams會議持續時間 */
  teamsMeetingDuration: string;
  /** 角色 */
  teamsRole: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-取得多筆活動名單-請求模型 */
export interface MbsCrmActHttpGetManyActivityEmployeePipelineReqMdl {
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

/** 管理者後台-CRM-活動管理-Http-取得多筆活動名單-回應模型 */
export interface MbsCrmActHttpGetManyActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工商機列表 */
  employeePipelineList: MbsCrmActHttpGetManyActivityEmployeePipelineRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-CRM-活動管理-Http-取得多筆活動名單-回應項目模型 */
export interface MbsCrmActHttpGetManyActivityEmployeePipelineRspItemMdl {
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
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-刪除多筆活動名單-請求模型 */
export interface MbsCrmActHttpRemoveManyActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID列表 */
  employeePipelineIDList: number[];
}

/** 管理者後台-CRM-活動管理-Http-刪除多筆活動名單-回應模型 */
export interface MbsCrmActHttpRemoveManyActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-更新多筆活動名單-請求模型 */
export interface MbsCrmActHttpUpdateManyActivityEmployeePipelineReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID列表 */
  employeePipelineIDList: number[];
  /** 商機狀態 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
}

/** 管理者後台-CRM-活動管理-Http-更新多筆活動名單-回應模型 */
export interface MbsCrmActHttpUpdateManyActivityEmployeePipelineRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-取得多筆客戶過往活動-請求模型 */
export interface MbsCrmActHttpGetManyPastActivityReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工商機ID */
  employeePipelineID: number;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** 管理者後台-CRM-活動管理-Http-取得多筆客戶過往活動-回應模型 */
export interface MbsCrmActHttpGetManyPastActivityRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 最新過往活動 */
  latestPastActivity: MbsCrmActHttpGetManyPastActivityRspItemMdl;
  /** 過往活動列表 */
  pastActivityList: MbsCrmActHttpGetManyPastActivityRspItemMdl[];
  /** 過往活動總數 */
  totalCount: number;
}

/** 管理者後台-CRM-活動管理-Http-取得多筆客戶過往活動-項目-回應模型 */
export interface MbsCrmActHttpGetManyPastActivityRspItemMdl {
  /** 管理者活動名稱 */
  managerActivityName: string;
  /** 管理者活動-開始時間 */
  managerActivityStartTime: string;
  /** 管理者活動-結束時間 */
  managerActivityEndTime: string;
}

//----------------------------------------------------------------------------------
//#endregion

//#region eDM
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-下載eDM範本-請求模型 */
export interface MbsCrmActHttpDownloadEdmTemplateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
}

/** 管理者後台-CRM-活動管理-Http-下載eDM範本-回應模型 */
export type MbsCrmActHttpDownloadEdmTemplateRspMdl = Blob;

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-匯入eDM-請求模型 */
export interface MbsCrmActHttpImportEdmReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
  /** 匯入的eDM檔案 */
  edmFile: File;
}

/** 管理者後台-CRM-活動管理-Http-匯入eDM-回應模型 */
export interface MbsCrmActHttpImportEdmRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 錯誤資料列表 */
  errorDataList: MbsCrmActHttpImportEdmRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-Http-匯入eDM-項目-回應模型 */
export interface MbsCrmActHttpImportEdmRspItemMdl {
  /** EDM Email */
  edmContacterEmail: string;
  /** EDM First Name */
  edmFirstName: string;
  /** EDM Last Name */
  edmLastName: string;
  /** EDM Telephone */
  edmContacterTelephone: string;
  /** EDM 行動電話 */
  eEdmContacterPhone: string;
  /** EDM 公司名稱 */
  edmCompanyName: string;
  /** EDM 職稱 */
  edmContacterJobTitle: string;
  /** EDM 公司電話 */
  edmCompanyPhone: string;
  /** EDM 公司傳真 */
  edmCompanyFax: string;
  /** EDM 公司地址 */
  edmCompanyAddress: string;
  /** EDM 備注 */
  edmRemark: string;
  /** EDM 部門 */
  edmContacterDepartment: string;
  /** EDM 主類別碼 */
  edmCompanyMainKind: string;
  /** EDM 分類碼 */
  edmCompanySubKind: string;
  /** EDM Account Sales */
  edmAccountSales: string;
  /** EDM 區域 */
  edmRegion: string;
  /** EDM Created Date */
  edmCreatedDate: string;
  /** EDM device */
  edmDevice: string;
}

//----------------------------------------------------------------------------------
//#endregion

//#region Teams
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-下載Teams範本-請求模型 */
export interface MbsCrmActHttpDownloadTeamsTemplateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
}

/** 管理者後台-CRM-活動管理-Http-下載Teams範本-回應模型 */
export type MbsCrmActHttpDownloadTeamsTemplateRspMdl = Blob;

//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-匯入Teams-請求模型 */
export interface MbsCrmActHttpImportTeamsReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者活動ID */
  managerActivityID: number;
  /** 匯入的Teams檔案 */
  teamsFile: File;
}

/** 管理者後台-CRM-活動管理-Http-匯入Teams-回應模型 */
export interface MbsCrmActHttpImportTeamsRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 錯誤資料列表 */
  errorDataList: MbsCrmActHttpImportTeamsRspItemMdl[];
}

/** 管理者後台-CRM-活動管理-Http-匯入Teams-項目-回應模型 */
export interface MbsCrmActHttpImportTeamsRspItemMdl {
  /** Teams名稱 */
  teamsName: string;
  /** Teams首次加入時間 */
  teamsFirstJoinTime: string | null;
  /** Teams上次離開時間 */
  teamsLastLeaveTime: string | null;
  /** Teams會議持續時間 */
  teamsMeetingDuration: string;
  /** Teams電子郵件 */
  teamsEmail: string;
  /** 參與者識別碼 (UPN) */
  teamsParticipantId: string;
  /** Teams角色 */
  teamsRole: string;
  /** Teams註冊名子 */
  teamsContacterRegisterLastName: string;
  /** Teams註冊姓氏 */
  teamsContacterRegisterFirstName: string;
  /** Teams註冊電子郵件 */
  teamsContacterRegisterEmail: string;
  /** Teams註冊時間 */
  teamsRegistrationTime: string | null;
  /** Teams註冊狀態 */
  teamsRegistrationStatus: string;
  /** Teams公司名稱 */
  teamsCompanyName: string;
  /** Teams部門 */
  teamsContacterDepartment: string;
  /** Teams職稱 */
  teamsContacterJobTitle: string;
  /** Teams公司電話 */
  teamsCompanyTelephone: string;
  /** Teams手機號碼 */
  teamsContacterPhone: string;
  /** Teams活動資訊來源 */
  teamsActivityInfoSource: string;
  /** Teams窗口是否個資同意 */
  teamsContacterIsConsent: boolean;
}

//----------------------------------------------------------------------------------
//#endregion

//#region 活動問卷
//----------------------------------------------------------------------------------
/** 管理者後台-CRM-活動管理-Http-新增活動問卷問題-請求模型 */
export interface MbsCrmActHttpAddActivitySurveyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-活動問卷問題-活動ID */
  managerActivityID: number;
  /** 管理者-活動問卷問題-問卷ID */
  managerActivitySurveyID: number;
  /** 管理者-活動問卷問題-問題類型 */
  managerActivitySurveyQuestionKind: DbMasqKindEnum;
  /** 管理者-活動問卷問題-問題標題 */
  managerActivitySurveyQuestionTitle: string;
  /** 管理者-活動問卷問題-問題內容 */
  managerActivitySurveyQuestionContent: string;
  /** 管理者-活動問卷問題-是否為其他 */
  isOther: boolean;
  /** 管理者-活動問卷問題-排序 */
  managerActivitySurveyQuestionSort: number;
  /** 管理者-活動問卷問題項目列表 */
  managerActivitySurveyQuestionItemList: MbsCrmActHttpAddActivitySurveyReqItemMdl[];
}

/** 管理者後台-CRM-活動管理-Http-新增活動問卷問題-請求項目模型 */
export interface MbsCrmActHttpAddActivitySurveyReqItemMdl {
  /** 管理者-活動問卷問題項目-名稱 */
  managerActivitySurveyQuestionItemName: string;
  /** 管理者-活動問卷問題項目-排序 */
  managerActivitySurveyQuestionItemSort: number;
}

/** 管理者後台-CRM-活動管理-Http-新增活動問卷問題-回應模型 */
export interface MbsCrmActHttpAddActivitySurveyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-活動問卷-ID */
  managerActivitySurveyID: number;
}

//----------------------------------------------------------------------------------
//#endregion
