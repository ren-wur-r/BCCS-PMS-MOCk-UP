import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbManagerProductKindEnum } from "@/constants/DbManagerProductKindEnum";

// #region 管理者後台-系統-產品
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-取得多筆產品-請求模型 */
export interface MbsSysPrdHttpGetManyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number | null;
  /** 管理者-產品子分類-ID */
  managerProductSubKindID: number | null;
  /** 管理者-產品-是否為主力產品 */
  managerProductIsKey: boolean | null;
  /** 管理者-產品-名稱 */
  managerProductName: string | null;
  /** 產品規格-名稱 */
  managerProductSpecificationName: string | null;
  /** 產品規格-是否啟用 */
  managerProductSpecificationIsEnable: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** 管理者後台-系統-產品-Http-取得多筆產品-回應模型 */
export interface MbsSysPrdHttpGetManyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品-列表 */
  managerProductList: MbsSysPrdHttpGetManyRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-產品-Http-取得多筆產品-回應項目模型 */
export interface MbsSysPrdHttpGetManyRspItemMdl {
  /** 管理者-產品-ID */
  managerProductID: number;
  /** 管理者-產品-名稱 */
  managerProductName: string;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品子分類-名稱 */
  managerProductSubKindName: string;
  /** 管理者-產品-是否為主力產品 */
  managerProductIsKey: boolean;
  /** 管理者-產品規格-名稱 */
  managerProductSpecificationName: string;
  /** 管理者-產品規格-售價 */
  managerProductSpecificationSellPrice: number;
  /** 管理者-產品規格-成本 */
  managerProductSpecificationCostPrice: number;
  /** 管理者-產品規格-是否啟用 */
  managerProductSpecificationIsEnable: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-取得單筆產品-請求模型 */
export interface MbsSysPrdHttpGetReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品-ID */
  managerProductID: number;
}

/** 管理者後台-系統-產品-Http-取得單筆產品-回應模型 */
export interface MbsSysPrdHttpGetRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品-名稱 */
  managerProductName: string;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品子分類-ID */
  managerProductSubKindID: number;
  /** 管理者-產品子分類-名稱 */
  managerProductSubKindName: string;
  /** 管理者-產品種類列舉 */
  managerProductKind: DbManagerProductKindEnum;
  /** 管理者-產品-是否為主力產品 */
  managerProductIsKey: boolean;
  /** 管理者-產品-備註 */
  managerProductRemark: string;
  /** 管理者-產品-是否啟用 */
  managerProductIsEnable: boolean;
  /** 管理者-產品規格-列表 */
  managerProductSpecificationList: MbsSysPrdHttpGetSpecificationRspItemMdl[];
}

/** 管理者後台-系統-產品-Http-取得單筆產品-規格-回應項目模型 */
export interface MbsSysPrdHttpGetSpecificationRspItemMdl {
  /** 管理者-產品規格-ID */
  managerProductSpecificationID: number;
  /** 管理者-產品規格-名稱 */
  managerProductSpecificationName: string;
  /** 管理者-產品規格-售價 */
  managerProductSpecificationSellPrice: number;
  /** 管理者-產品規格-成本 */
  managerProductSpecificationCostPrice: number;
  /** 管理者-產品規格-是否啟用 */
  managerProductSpecificationIsEnable: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-新增產品-請求模型 */
export interface MbsSysPrdHttpAddReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品-名稱 */
  managerProductName: string;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品子分類-ID */
  managerProductSubKindID: number;
  /** 管理者-產品種類列舉 */
  managerProductKind: DbManagerProductKindEnum;
  /** 管理者-產品-是否為主力產品 */
  managerProductIsKey: boolean;
  /** 管理者-產品-備註 */
  managerProductRemark: string | null;
  /** 管理者-產品-是否啟用 */
  managerProductIsEnable: boolean;
  /** 管理者-產品規格-列表 */
  managerProductSpecificationList: MbsSysPrdHttpAddSpecificationReqItemMdl[];
}

/** 管理者後台-系統-產品-Http-新增產品-規格項目模型 */
export interface MbsSysPrdHttpAddSpecificationReqItemMdl {
  /** 管理者-產品規格-名稱 */
  managerProductSpecificationName: string;
  /** 管理者-產品規格-售價 */
  managerProductSpecificationSellPrice: number;
  /** 管理者-產品規格-成本 */
  managerProductSpecificationCostPrice: number;
  /** 管理者-產品規格-是否啟用 */
  managerProductSpecificationIsEnable: boolean;
}

/** 管理者後台-系統-產品-Http-新增產品-回應模型 */
export interface MbsSysPrdHttpAddRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品-ID */
  managerProductID: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-更新產品-請求模型 */
export interface MbsSysPrdHttpUpdateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品-ID */
  managerProductID: number;
  /** 管理者-產品-名稱 */
  managerProductName: string | null;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number | null;
  /** 管理者-產品子分類-ID */
  managerProductSubKindID: number | null;
  /** 管理者-產品種類列舉 */
  managerProductKind: DbManagerProductKindEnum | null;
  /** 管理者-產品-是否為主力產品 */
  managerProductIsKey: boolean | null;
  /** 管理者-產品-備註 */
  managerProductRemark: string | null;
  /** 管理者-產品-是否啟用 */
  managerProductIsEnable: boolean | null;
}

/** 管理者後台-系統-產品-Http-更新產品-回應模型 */
export interface MbsSysPrdHttpUpdateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
//#endregion

//#region 管理者後台-系統-產品-Http-產品規格
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-新增產品規格-請求模型 */
export interface MbsSysPrdHttpAddSpecificationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品-ID */
  managerProductID: number;
  /** 管理者-產品規格-名稱 */
  managerProductSpecificationName: string;
  /** 管理者-產品規格-售價 */
  managerProductSpecificationSellPrice: number;
  /** 管理者-產品規格-成本 */
  managerProductSpecificationCostPrice: number;
  /** 管理者-產品規格-是否啟用 */
  managerProductSpecificationIsEnable: boolean;
}

/** 管理者後台-系統-產品-Http-新增產品規格-回應模型 */
export interface MbsSysPrdHttpAddSpecificationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品規格-ID */
  managerProductSpecificationID: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-更新產品規格-請求模型 */
export interface MbsSysPrdHttpUpdateSpecificationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品-ID */
  managerProductID: number;
  /** 管理者-產品規格-ID */
  managerProductSpecificationID: number;
  /** 管理者-產品規格-名稱 */
  managerProductSpecificationName: string | null;
  /** 管理者-產品規格-售價 */
  managerProductSpecificationSellPrice: number;
  /** 管理者-產品規格-成本 */
  managerProductSpecificationCostPrice: number;
  /** 管理者-產品規格-是否啟用 */
  managerProductSpecificationIsEnable: boolean;
}

/** 管理者後台-系統-產品-Http-更新產品規格-回應模型 */
export interface MbsSysPrdHttpUpdateSpecificationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
//#endregion

//#region 管理者後台-系統-產品-Http-產品主分類
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-取得多筆產品主分類-請求模型 */
export interface MbsSysPrdHttpGetManyMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string | null;
  /** 管理者-產品主分類-是否啟用 */
  managerProductMainKindIsEnable: boolean | null;
  /** 頁數 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 管理者後台-系統-產品-Http-取得多筆產品主分類-回應模型 */
export interface MbsSysPrdHttpGetManyMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品主分類-列表 */
  managerProductMainKindList: MbsSysPrdHttpGetManyMainKindRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-產品-Http-取得多筆產品主分類-回應項目模型 */
export interface MbsSysPrdHttpGetManyMainKindRspItemMdl {
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品主分類-業務毛利率 */
  managerProductMainKindCommissionRate: number;
  /** 管理者-產品主分類-是否啟用 */
  managerProductMainKindIsEnable: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-取得單筆產品主分類-請求模型 */
export interface MbsSysPrdHttpGetMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
}

/** 管理者後台-系統-產品-Http-取得單筆產品主分類-回應模型 */
export interface MbsSysPrdHttpGetMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品主分類-業務毛利率 */
  managerProductMainKindCommissionRate: number;
  /** 管理者-產品主分類-是否啟用 */
  managerProductMainKindIsEnable: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-新增產品主分類-請求模型 */
export interface MbsSysPrdHttpAddMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品主分類-業務毛利率 */
  managerProductMainKindCommissionRate: number;
  /** 管理者-產品主分類-是否啟用 */
  managerProductMainKindIsEnable: boolean;
}

/** 管理者後台-系統-產品-Http-新增產品主分類-回應模型 */
export interface MbsSysPrdHttpAddMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-更新產品主分類-請求模型 */
export interface MbsSysPrdHttpUpdateMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string | null;
  /** 管理者-產品主分類-業務毛利率 */
  managerProductMainKindCommissionRate: number | null;
  /** 管理者-產品主分類-是否啟用 */
  managerProductMainKindIsEnable: boolean | null;
}

/** 管理者後台-系統-產品-Http-更新產品主分類-回應模型 */
export interface MbsSysPrdHttpUpdateMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
//#endregion

// #region 管理者後台-系統-產品-Http-產品子分類
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-取得多筆產品子分類-請求模型 */
export interface MbsSysPrdHttpGetManySubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number | null;
  /** 管理者-產品子分類-名稱 */
  managerProductSubKindName: string | null;
  /** 管理者-產品子分類-是否啟用 */
  managerProductSubKindIsEnable: boolean | null;
  /** 頁數 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 管理者後台-系統-產品-Http-取得多筆產品子分類-回應模型 */
export interface MbsSysPrdHttpGetManySubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品子分類-列表 */
  managerProductSubKindList: MbsSysPrdHttpGetManySubKindRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-產品-Http-取得多筆產品子分類-回應項目模型 */
export interface MbsSysPrdHttpGetManySubKindRspItemMdl {
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品子分類-ID */
  managerProductSubKindID: number;
  /** 管理者-產品子分類-名稱 */
  managerProductSubKindName: string;
  /** 管理者-產品子分類-業務毛利率 */
  managerProductSubKindCommissionRate: number;
  /** 管理者-產品子分類-是否啟用 */
  managerProductSubKindIsEnable: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-取得單筆產品子分類-請求模型 */
export interface MbsSysPrdHttpGetSubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品子分類-ID */
  managerProductSubKindID: number;
}

/** 管理者後台-系統-產品-Http-取得單筆產品子分類-回應模型 */
export interface MbsSysPrdHttpGetSubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品子分類-名稱 */
  managerProductSubKindName: string;
  /** 管理者-產品子分類-業務毛利率 */
  managerProductSubKindCommissionRate: number;
  /** 管理者-產品子分類-是否啟用 */
  managerProductSubKindIsEnable: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-新增產品子分類-請求模型 */
export interface MbsSysPrdHttpAddSubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品子分類-名稱 */
  managerProductSubKindName: string;
  /** 管理者-產品子分類-業務毛利率 */
  managerProductSubKindCommissionRate: number;
  /** 管理者-產品子分類-是否啟用 */
  managerProductSubKindIsEnable: boolean;
}

/** 管理者後台-系統-產品-Http-新增產品子分類-回應模型 */
export interface MbsSysPrdHttpAddSubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品子分類-ID */
  managerProductSubKindID: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-Http-更新產品子分類-請求模型 */
export interface MbsSysPrdHttpUpdateSubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品子分類-ID */
  managerProductSubKindID: number;
  /** 管理者-產品子分類-名稱 */
  managerProductSubKindName: string | null;
  /** 管理者-產品子分類-業務毛利率 */
  managerProductSubKindCommissionRate: number | null;
  /** 管理者-產品子分類-是否啟用 */
  managerProductSubKindIsEnable: boolean | null;
}

/** 管理者後台-系統-產品-Http-更新產品子分類-回應模型 */
export interface MbsSysPrdHttpUpdateSubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
//#endregion
