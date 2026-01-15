import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomSecurityGradeEnum } from "@/constants/DbAtomSecurityGradeEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//#region 管理者後台-系統-名單公司
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得多筆公司主分類-請求模型 */
export interface MbsSysComHttpGetManyCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司主分類名稱 */
  managerCompanyMainKindName: string | null;
  /** 管理者-公司主分類-是否啟用 */
  managerCompanyMainKindIsEnable: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** 管理者後台-系統-名單公司-Http-取得多筆公司主分類-回應模型 */
export interface MbsSysComHttpGetManyCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司主分類-列表 */
  managerCompanyMainKindList: MbsSysComHttpGetManyCompanyMainKindRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-名單公司-Http-取得多筆公司主分類-回應模型-項目 */
export interface MbsSysComHttpGetManyCompanyMainKindRspItemMdl {
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者-公司主分類-名稱 */
  managerCompanyMainKindName: string;
  /** 管理者-公司主分類-是否啟用 */
  managerCompanyMainKindIsEnable: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得公司主分類-請求模型 */
export interface MbsSysComHttpGetCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
}

/** 管理者後台-系統-名單公司-Http-取得公司主分類-回應模型 */
export interface MbsSysComHttpGetCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司主分類名稱 */
  managerCompanyMainKindName: string | null;
  /** 管理者-公司主分類-是否啟用 */
  managerCompanyMainKindIsEnable: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-新增公司主分類-請求模型 */
export interface MbsSysComHttpAddCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司主分類-名稱 */
  managerCompanyMainKindName: string;
  /** 管理者-公司主分類-是否啟用 */
  managerCompanyMainKindIsEnable: boolean;
}

/** 管理者後台-系統-名單公司-Http-新增公司主分類-回應模型 */
export interface MbsSysComHttpAddCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-更新公司主分類-請求模型 */
export interface MbsSysComHttpUpdateCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者-公司主分類-名稱 */
  managerCompanyMainKindName: string | null;
  /** 管理者-公司主分類-是否啟用 */
  managerCompanyMainKindIsEnable: boolean | null;
}

/** 管理者後台-系統-名單公司-Http-更新公司主分類-回應模型 */
export interface MbsSysComHttpUpdateCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得多筆公司子分類-請求模型 */
export interface MbsSysComHttpGetManyCompanySubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司子分類-名稱*/
  managerCompanySubKindName: string | null;
  /** 管理者-公司子分類-是否啟用 */
  managerCompanySubKindIsEnable: boolean | null;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** 管理者後台-系統-名單公司-Http-取得多筆公司子分類-回應模型 */
export interface MbsSysComHttpGetManyCompanySubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司子分類-列表 */
  managerCompanySubKindList: MbsSysComHttpGetManyCompanySubKindRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-名單公司-Http-取得多筆公司子分類-回應模型-項目 */
export interface MbsSysComHttpGetManyCompanySubKindRspItemMdl {
  /** 管理者-公司子分類-ID */
  managerCompanySubKindID: number;
  /** 管理者-公司子分類-名稱 */
  managerCompanySubKindName: string | null;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者-公司主分類-名稱 */
  managerCompanyMainKindName: string | null;
  /** 管理者-公司子分類-是否啟用 */
  managerCompanySubKindIsEnable: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得公司子分類-請求模型 */
export interface MbsSysComHttpGetCompanySubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司子分類-ID */
  managerCompanySubKindID: number;
}

/** 管理者後台-系統-名單公司-Http-取得公司子分類-回應模型 */
export interface MbsSysComHttpGetCompanySubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司子分類-名稱 */
  managerCompanySubKindName: string ;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者-公司子分類-是否啟用 */
  managerCompanySubKindIsEnable: boolean;
  /** 管理者-公司主分類-名稱 */
  managerCompanyMainKindName: string ;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-新增公司子分類-請求模型 */
export interface MbsSysComHttpAddCompanySubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司子分類-名稱 */
  managerCompanySubKindName: string;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者-公司子分類-是否啟用 */
  managerCompanySubKindIsEnable: boolean;
}

/** 管理者後台-系統-名單公司-Http-新增公司子分類-回應模型 */
export interface MbsSysComHttpAddCompanySubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司子分類-ID */
  managerCompanySubKindID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-更新公司子分類-請求模型 */
export interface MbsSysComHttpUpdateCompanySubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司子分類-ID */
  managerCompanySubKindID: number;
  /** 管理者-公司子分類-名稱 */
  managerCompanySubKindName: string | null;
  /** 管理者-公司子分類-是否啟用 */
  managerCompanySubKindIsEnable: boolean | null;
}

/** 管理者後台-系統-名單公司-Http-更新公司子分類-回應模型 */
export interface MbsSysComHttpUpdateCompanySubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得多筆公司-請求模型 */
export interface MbsSysComHttpGetManyCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 原子-客戶等級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number | null;
  /** 管理者-公司子分類-ID */
  managerCompanySubKindID: number | null;
  /** 管理者-公司-名稱 */
  managerCompanyName: string | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** 管理者後台-系統-名單公司-Http-取得多筆公司-回應模型 */
export interface MbsSysComHttpGetManyCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司列表 */
  managerCompanyList: MbsSysComHttpGetManyCompanyRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-名單公司-Http-取得多筆公司-回應項目模型 */
export interface MbsSysComHttpGetManyCompanyRspItemMdl {
  /** 管理者-公司-ID */
  managerCompanyID: number;
  /** 管理者-公司-統一編號 */
  managerCompanyUnifiedNumber: string | null;
  /** 管理者-公司-名稱 */
  managerCompanyName: string | null;
  /** 原子-客戶等級 */
  atomCustomerGrade: number;
  /** 管理者-公司主分類-名稱 */
  managerCompanyMainKindName: string | null;
  /** 管理者-公司子分類-名稱 */
  managerCompanySubKindName: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得單筆公司-請求模型 */
export interface MbsSysComHttpGetCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司-ID */
  managerCompanyID: number;
}

/** 管理者後台-系統-名單公司-Http-取得單筆公司-回應模型 */
export interface MbsSysComHttpGetCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司-統一編號 */
  managerCompanyUnifiedNumber: string | null;
  /** 管理者-公司-名稱 */
  managerCompanyName: string | null;
  /** 原子-管理者公司狀態 */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者-公司主分類-名稱 */
  managerCompanyMainKindName: string | null;
  /** 管理者-公司子分類-ID */
  managerCompanySubKindID: number;
  /** 管理者-公司子分類-名稱 */
  managerCompanySubKindName: string | null;
  /** 原子-客戶等級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  /** 原子-安全等級 */
  atomSecurityGrade: DbAtomSecurityGradeEnum;
  /** 原子-員工規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-新增公司-請求模型 */
export interface MbsSysComHttpAddCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司-統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 管理者-公司-名稱 */
  managerCompanyName: string;
  /** 原子-管理者公司狀態 */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者-公司子分類-ID */
  managerCompanySubKindID: number;
  /** 原子-客戶等級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  /** 原子-安全等級 */
  atomSecurityGrade: DbAtomSecurityGradeEnum;
  /** 原子-員工規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum;
  /** 管理者-關係公司列表 */
  managerCompanyAffiliateList: MbsSysComHttpAddCompanyAffiliateReqMdl[];
  /** 管理者-公司營業地點列表 */
  managerCompanyLocationList: MbsSysComHttpAddCompanyLocationReqMdl[];
}

/** 管理者後台-系統-名單公司-Http-新增公司-回應模型 */
export interface MbsSysComHttpAddCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司-ID */
  managerCompanyID: number;
}

/** 管理者後台-系統-名單公司-Http-新增公司-營業地點-請求項目模型 */
export interface MbsSysComHttpAddCompanyLocationReqItemMdl {
  /** 管理者-公司營業地點-名稱 */
  managerCompanyLocationName: string;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum | null;
  /** 管理者-公司營業地點-地址 */
  managerCompanyLocationAddress: string;
  /** 管理者-公司營業地點-電話 */
  managerCompanyLocationTelephone: string | null;
  /** 原子-管理者-公司-狀態 */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** 管理者後台-系統-名單公司-Http-新增關係公司-請求項目模型 */
export interface MbsSysComHttpAddCompanyAffiliateReqItemMdl {
  /** 管理者-公司關係-統一編號 */
  managerCompanyAffiliateUnifiedNumber: string;
  /** 管理者-公司關係-名稱 */
  managerCompanyAffiliateName: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-更新公司-請求模型 */
export interface MbsSysComHttpUpdateCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司-ID */
  managerCompanyID: number;
  /** 管理者-公司-統一編號 */
  managerCompanyUnifiedNumber: string | null;
  /** 管理者-公司-名稱 */
  managerCompanyName: string | null;
  /** 原子-管理者-公司狀態 */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number | null;
  /** 管理者-公司子分類-ID */
  managerCompanySubKindID: number | null;
  /** 原子-客戶等級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  /** 原子-安全等級 */
  atomSecurityGrade: DbAtomSecurityGradeEnum;
  /** 原子-員工規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum;
}

/** 管理者後台-系統-名單公司-Http-更新公司-回應模型 */
export interface MbsSysComHttpUpdateCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得多筆公司營業地點-請求模型 */
export interface MbsSysComHttpGetManyCompanyLocationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司-ID */
  managerCompanyID: number;
}

/** 管理者後台-系統-名單公司-Http-取得多筆公司營業地點-回應模型 */
export interface MbsSysComHttpGetManyCompanyLocationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司營業地點-列表 */
  managerCompanyLocationList: MbsSysComHttpGetManyCompanyLocationRspItemMdl[];
}

/** 管理者後台-系統-名單公司-Http-取得多筆公司營業地點-回應模型-項目 */
export interface MbsSysComHttpGetManyCompanyLocationRspItemMdl {
  /** 管理者-公司營業地點-ID */
  managerCompanyLocationID: number;
  /** 管理者-公司營業地點-名稱 */
  managerCompanyLocationName: string;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum;
  /** 管理者-公司營業地點-地址 */
  managerCompanyLocationAddress: string;
  /** 管理者-公司營業地點-電話 */
  managerCompanyLocationTelephone: string;
  /** 原子-管理者-公司-狀態 */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得單筆公司營業地點-請求模型 */
export interface MbsSysComHttpGetCompanyLocationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司營業地點-ID */
  managerCompanyLocationID: number;
}

/** 管理者後台-系統-名單公司-Http-取得單筆公司營業地點-回應模型 */
export interface MbsSysComHttpGetCompanyLocationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司營業地點-ID */
  managerCompanyLocationID: number;
  /** 管理者-公司營業地點-名稱 */
  managerCompanyLocationName: string;
  /** 原子-縣市 */
  atomCity: number;
  /** 管理者-公司營業地點-地址 */
  managerCompanyLocationAddress: string ;
  /** 管理者-公司營業地點-電話 */
  managerCompanyLocationTelephone: string;
  /** 原子-管理者-公司-狀態 */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司營業地點-是否已刪除 */
  managerCompanyLocationIsDeleted: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-新增公司營業地點-請求模型 */
export interface MbsSysComHttpAddCompanyLocationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司-ID */
  managerCompanyID: number;
  /** 管理者公司營業地點-名稱 */
  managerCompanyLocationName: string;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum | null;
  /** 管理者公司營業地點-地址 */
  managerCompanyLocationAddress: string;
  /** 管理者公司營業地點-電話 */
  managerCompanyLocationTelephone: string | null;
  /** 原子-管理者公司-狀態  */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** 管理者後台-系統-名單公司-Http-新增公司營業地點-回應模型 */
export interface MbsSysComHttpAddCompanyLocationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司營業地點-ID */
  managerCompanyLocationID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-更新公司營業地點-請求模型 */
export interface MbsSysComHttpUpdateCompanyLocationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司營業地點-ID */
  managerCompanyLocationID: number;
  /** 管理者-公司營業地點-名稱 */
  managerCompanyLocationName: string | null;
  /** 原子-縣市 (DbAtomCityEnum) */
  atomCity: DbAtomCityEnum;
  /** 管理者-公司營業地點-地址 */
  managerCompanyLocationAddress: string | null;
  /** 管理者-公司營業地點-電話 */
  managerCompanyLocationTelephone: string | null;
  /** 原子-管理者公司-狀態 */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司營業地點-是否已刪除 */
  managerCompanyLocationIsDeleted: boolean | null;
}

/** 管理者後台-系統-名單公司-Http-更新公司營業地點-回應模型 */
export interface MbsSysComHttpUpdateCompanyLocationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得多筆關係公司-請求模型 */
export interface MbsSysComHttpGetManyCompanyAffiliateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司-ID */
  managerCompanyID: number;
}

/** 管理者後台-系統-名單公司-Http-取得多筆關係公司-回應模型 */
export interface MbsSysComHttpGetManyCompanyAffiliateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者公司關係-列表 */
  managerCompanyAffiliateList: MbsSysComHttpGetManyCompanyAffiliateRspItemMdl[];
}

/** 管理者後台-系統-名單公司-Http-取得多筆關係公司-回應模型-項目 */
export interface MbsSysComHttpGetManyCompanyAffiliateRspItemMdl {
  /** 管理者-公司關係-ID */
  managerCompanyAffiliateID: number;
  /** 管理者-公司關係-統一編號 */
  managerCompanyAffiliateUnifiedNumber: string;
  /** 管理者-公司關係-名稱 */
  managerCompanyAffiliateName: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-取得單筆關係公司-請求模型 */
export interface MbsSysComHttpGetCompanyAffiliateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司關係-ID */
  managerCompanyAffiliateID: number;
}

/** 管理者後台-系統-名單公司-Http-取得單筆關係公司-回應模型 */
export interface MbsSysComHttpGetCompanyAffiliateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者公司關係-ID */
  managerCompanyAffiliateID: number;
  /** 管理者公司關係-統一編號 */
  managerCompanyAffiliateUnifiedNumber: string | null;
  /** 管理者公司關係-名稱 */
  managerCompanyAffiliateName: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-新增關係公司-請求模型 */
export interface MbsSysComHttpAddCompanyAffiliateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司-ID */
  managerCompanyID: number;
  /** 管理者公司關係-統一編號 */
  managerCompanyAffiliateUnifiedNumber: string;
  /** 管理者公司關係-名稱 */
  managerCompanyAffiliateName: string;
}

/** 管理者後台-系統-名單公司-Http-新增關係公司-回應模型 */
export interface MbsSysComHttpAddCompanyAffiliateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 新增的管理者公司關係-ID */
  managerCompanyAffiliateID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-Http-更新關係公司-請求模型 */
export interface MbsSysComHttpUpdateCompanyAffiliateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司關係-ID */
  managerCompanyAffiliateID: number;
  /** 管理者公司關係-統一編號 */
  managerCompanyAffiliateUnifiedNumber: string | null;
  /** 管理者公司關係-名稱 */
  managerCompanyAffiliateName: string | null;
  /** 管理者公司關係-是否已刪除 */
  managerCompanyAffiliateIsDeleted: boolean | null;
}

/** 管理者後台-系統-名單公司-Http-更新關係公司-回應模型 */
export interface MbsSysComHttpUpdateCompanyAffiliateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//#endregion
