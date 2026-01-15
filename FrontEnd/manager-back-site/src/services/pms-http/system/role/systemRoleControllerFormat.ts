import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-角色-控制器-取得多筆-請求模型 */
export interface MbsSysRolCtlGetManyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-角色-名稱 */
  a: string | null;
  /** 管理者-角色-是否啟用 */
  b: boolean | null;
  /** 頁面索引 */
  c: number;
  /** 頁面筆數 */
  d: number;
}

/** 管理者後台-系統-角色-控制器-取得多筆-回應模型 */
export interface MbsSysRolCtlGetManyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-角色-列表 */
  a: MbsSysRolCtlGetManyRspItemMdl[] | null;
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-角色-控制器-取得多筆-回應項目模型 */
export interface MbsSysRolCtlGetManyRspItemMdl {
  /** 管理者-角色-ID */
  a: number;
  /** 管理者-角色-名稱 */
  b: string;
  /** 員工-數量 */
  c: number;
  /** 管理者-角色-是否啟用 */
  d: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-角色-控制器-取得單筆-請求模型 */
export interface MbsSysRolCtlGetReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-角色-ID */
  a: number;
}

/** 管理者後台-系統-角色-控制器-取得單筆-回應模型 */
export interface MbsSysRolCtlGetRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-角色-名稱 */
  a: string;
  /** 管理者-角色-地區ID */
  b: number;
  /** 管理者-角色-地區名稱 */
  c: string;
  /** 管理者-角色-部門ID */
  d: number;
  /** 管理者-角色-部門名稱 */
  e: string;
  /** 管理者-角色-備註 */
  f: string;
  /** 管理者-角色-是否啟用 */
  g: boolean;
  /** 員工-人數 */
  h: number;
  /** 管理者-角色-權限列表 */
  i: MbsSysRolCtlGetRspItemMdl[];
}

/** 管理者後台-系統-角色-控制器-取得單筆-回應項目模型 */
export interface MbsSysRolCtlGetRspItemMdl {
  /** 原子-目錄 */
  a: DbAtomMenuEnum;
  /** 管理者-區域-ID */
  b: number;
  /** 原子-權限-類型-檢視 */
  c: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-新增 */
  d: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-編輯 */
  e: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-刪除 */
  f: DbAtomPermissionKindEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-角色-控制器-新增-請求模型 */
export interface MbsSysRolCtlAddReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-角色-名稱 */
  a: string;
  /** 管理者-角色-地區ID */
  b: number;
  /** 管理者-角色-部門ID */
  c: number;
  /** 管理者-角色-備註 */
  d: string | null;
  /** 管理者-角色-是否啟用 */
  e: boolean;
  /** 管理者-角色-權限列表 */
  f: MbsSysRolCtlAddReqItemMdl[] | null;
}

/** 管理者後台-系統-角色-控制器-新增-回應模型 */
export interface MbsSysRolCtlAddRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-角色-ID */
  a: number | null;
}

/** 管理者後台-系統-角色-控制器-新增-請求項目模型 */
export interface MbsSysRolCtlAddReqItemMdl {
  /** 原子-目錄 */
  a: DbAtomMenuEnum;
  /** 管理者-區域-ID */
  b: number;
  /** 原子-權限-類型-檢視 */
  c: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-新增 */
  d: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-編輯 */
  e: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-刪除 */
  f: DbAtomPermissionKindEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-角色-控制器-更新-請求模型 */
export interface MbsSysRolCtlUpdateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-角色-ID */
  a: number;
  /** 管理者-角色-名稱 */
  b: string | null;
  /** 管理者-角色-地區ID */
  c: number | null;
  /** 管理者-角色-部門ID */
  d: number | null;
  /** 管理者-角色-備註 */
  e: string | null;
  /** 管理者-角色-是否啟用 */
  f: boolean | null;
  /** 管理者-角色-權限列表 */
  g: MbsSysRolCtlUpdateReqItemMdl[] | null;
}

/** 管理者後台-系統-角色-控制器-更新-回應模型 */
export interface MbsSysRolCtlUpdateRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

/** 管理者後台-系統-角色-控制器-更新-請求項目模型 */
export interface MbsSysRolCtlUpdateReqItemMdl {
  /** 原子-目錄 */
  a: DbAtomMenuEnum;
  /** 管理者-區域-ID */
  b: number;
  /** 原子-權限-類型-檢視 */
  c: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-新增 */
  d: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-編輯 */
  e: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-刪除 */
  f: DbAtomPermissionKindEnum;
}
