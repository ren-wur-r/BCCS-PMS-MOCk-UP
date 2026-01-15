import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-地區-控制器-取得多筆-請求模型 */
export interface MbsSysRgnCtlGetManyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-地區-名稱 */
  a: string | null;
  /** 管理者-地區-是否啟用 */
  b: boolean | null;
  /** 頁數 */
  c: number;
  /** 每頁筆數 */
  d: number;
}

/** 管理者後台-系統-地區-控制器-取得多筆-回應模型 */
export interface MbsSysRgnCtlGetManyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-地區列表 */
  a: MbsSysRgnCtlGetManyRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-地區-控制器-取得多筆-回應-列表項目 */
export interface MbsSysRgnCtlGetManyRspItemMdl {
  /** 管理者-地區-ID */
  a: number;
  /** 管理者-地區-名稱 */
  b: string;
  /** 管理者-地區-是否啟用 */
  c: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-地區-控制器-新增-請求模型 */
export interface MbsSysRgnCtlAddReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-地區-名稱  */
  a: string;
  /** 管理者-地區-是否啟用 */
  b: boolean | null;
}

/** 管理者後台-系統-地區-控制器-新增-回應模型 */
export interface MbsSysRgnCtlAddRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-地區-控制器-更新-請求模型 */
export interface MbsSysRgnCtlUpdateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-地區-ID  */
  a: number;
  /** 管理者-地區-名稱 */
  b: string | null;
  /** 管理者-地區-是否啟用 */
  c: boolean | null;
}

/** 管理者後台-系統-地區-控制器-更新-回應模型 */
export interface MbsSysRgnCtlUpdateRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-地區-控制器-取得單筆-請求模型 */
export interface MbsSysRgnCtlGetReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-地區-ID */
  a: number;
}

/** 管理者後台-系統-地區-控制器-取得單筆-回應模型 */
export interface MbsSysRgnCtlGetRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-地區-名稱 */
  a: string;
  /** 管理者-地區-是否啟用 */
  b: boolean;
}