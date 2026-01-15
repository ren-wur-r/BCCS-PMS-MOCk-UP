import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-部門-Http-取得多筆-請求模型 */
export interface MbsSysDptHttpGetManyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-部門-名稱 */
  managerDepartmentName: string | null;
  /** 管理者-部門-是否啟用*/
  managerDepartmentIsEnable: boolean | null;
  /** 頁數 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 管理者後台-系統-部門-Http-取得多筆-回應模型 */
export interface MbsSysDptHttpGetManyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-部門列表 */
  managerDepartmentList: MbsSysDptHttpGetManyRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-部門-Http-取得多筆-回應-列表 */
export interface MbsSysDptHttpGetManyRspItemMdl {
  /** 管理者-部門-ID */
  managerDepartmentID: number;
  /** 管理者-部門-名稱 */
  managerDepartmentName: string;
  /** 管理者-部門-是否啟用 */
  managerDepartmentIsEnable: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-部門-Http-新增-請求模型 */
export interface MbsSysDptHttpAddReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-部門-名稱 */
  managerDepartmentName: string;
  /** 管理者-部門-是否啟用 */
  managerDepartmentIsEnable: boolean;
}

/** 管理者後台-系統-部門-Http-新增-回應模型 */
export interface MbsSysDptHttpAddRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-部門-Http-更新-請求模型 */
export interface MbsSysDptHttpUpdateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-部門-ID */
  managerDepartmentID: number;
  /** 管理者-部門-名稱 (可選) */
  managerDepartmentName: string | null;
  /** 管理者-部門-是否啟用 */
  managerDepartmentIsEnable: boolean;
}

/** 管理者後台-系統-部門-Http-更新-回應模型 */
export interface MbsSysDptHttpUpdateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-部門-Http-取得單筆-請求模型 */
export interface MbsSysDptHttpGetReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-部門-ID */
  managerDepartmentID: number;
}

/** 管理者後台-系統-部門-Http-取得單筆-回應模型 */
export interface MbsSysDptHttpGetRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-部門-名稱 */
  managerDepartmentName: string;
  /** 管理者-部門-是否啟用 */
  managerDepartmentIsEnable: boolean;
}
