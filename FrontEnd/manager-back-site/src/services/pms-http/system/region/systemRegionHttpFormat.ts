import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-地區-Http-取得多筆-請求模型 */
export interface MbsSysRgnHttpGetManyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-地區-名稱 */
  managerRegionName: string | null;
  /** 管理者-地區-是否啟用 */
  managerRegionIsEnable: boolean | null;
  /** 頁數 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 管理者後台-系統-地區-Http-取得多筆-回應模型 */
export interface MbsSysRgnHttpGetManyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-地區列表 */
  managerRegionList: MbsSysRgnHttpGetManyRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-地區-Http-取得多筆-回應-列表項目 */
export interface MbsSysRgnHttpGetManyRspItemMdl {
  /** 管理者-地區-ID */
  managerRegionID: number;
  /** 管理者-地區-名稱 */
  managerRegionName: string;
  /** 管理者-地區-是否啟用 */
  managerRegionIsEnable: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-地區-Http-新增-請求模型 */
export interface MbsSysRgnHttpAddReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-地區-名稱  */
  managerRegionName: string;
  /** 管理者-地區-是否啟用 */
  managerRegionIsEnable: boolean | null;
}

/** 管理者後台-系統-地區-Http-新增-回應模型 */
export interface MbsSysRgnHttpAddRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-地區-Http-更新-請求模型 */
export interface MbsSysRgnHttpUpdateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-地區-ID  */
  managerRegionID: number;
  /** 管理者-地區-名稱 */
  managerRegionName: string | null;
  /** 管理者-地區-是否啟用 */
  managerRegionIsEnable: boolean | null;
}

/** 管理者後台-系統-地區-Http-更新-回應模型 */
export interface MbsSysRgnHttpUpdateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-地區-Http-取得單筆-請求模型 */
export interface MbsSysRgnHttpGetReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-地區-ID */
  managerRegionID: number;
}

/** 管理者後台-系統-地區-Http-取得單筆-回應模型 */
export interface MbsSysRgnHttpGetRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-地區-名稱 */
  managerRegionName: string;
  /** 管理者-地區-是否啟用 */
  managerRegionIsEnable: boolean;
}
