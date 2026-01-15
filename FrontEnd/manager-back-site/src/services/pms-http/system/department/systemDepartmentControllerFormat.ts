import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-部門-控制器-取得多筆-請求模型 */
export interface MbsSysDptCtlGetManyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-部門-名稱 */
  a: string | null;
  /** 管理者-部門-是否啟用*/
  b: boolean | null;
  /** 頁數 */
  c: number;
  /** 每頁筆數 */
  d: number;
}

/** 管理者後台-系統-部門-控制器-取得多筆-回應模型 */
export interface MbsSysDptCtlGetManyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-部門列表 */
  a: MbsSysDptCtlGetManyRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-部門-控制器-取得多筆-回應-列表 */
export interface MbsSysDptCtlGetManyRspItemMdl {
  /** 管理者-部門-ID */
  a: number;
  /** 管理者-部門-名稱 */
  b: string;
  /** 管理者-部門-是否啟用 */
  c: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-部門-控制器-新增-請求模型 */
export interface MbsSysDptCtlAddReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-部門-名稱 */
  a: string;
  /** 管理者-部門-是否啟用 */
  b: boolean;
}

/** 管理者後台-系統-部門-控制器-新增-回應模型 */
export interface MbsSysDptCtlAddRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-部門-控制器-更新-請求模型 */
export interface MbsSysDptCtlUpdateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-部門-ID */
  a: number;
  /** 管理者-部門-名稱 (可選) */
  b: string | null;
  /** 管理者-部門-是否啟用 */
  c: boolean;
}

/** 管理者後台-系統-部門-控制器-更新-回應模型 */
export interface MbsSysDptCtlUpdateRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-部門-控制器-取得單筆-請求模型 */
export interface MbsSysDptCtlGetReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-部門-ID */
  a: number;
}

/** 管理者後台-系統-部門-控制器-取得單筆-回應模型 */
export interface MbsSysDptCtlGetRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-部門-名稱 */
  a: string;
  /** 管理者-部門-是否啟用 */
  b: boolean;
}
