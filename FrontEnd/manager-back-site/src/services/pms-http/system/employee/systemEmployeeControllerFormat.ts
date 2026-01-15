import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-員工-控制器-取得多筆-請求模型 */
export interface MbsSysEmpCtlGetManyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-部門-ID*/
  a: number | null;
  /** 管理者-角色-ID */
  b: number | null;
  /** 員工-是否啟用 */
  c: boolean | null;
  /** 員工-帳號 */
  d: string | null;
  /** 頁面索引 */
  e: number;
  /** 頁面筆數 */
  f: number;
}

/** 管理者後台-系統-員工-控制器-取得多筆-回應模型 */
export interface MbsSysEmpCtlGetManyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工-列表 */
  a: MbsSysEmpCtlGetManyRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-員工-控制器-取得多筆-回應項目模型 */
export interface MbsSysEmpCtlGetManyRspItemMdl {
  /** 管理者-部門-名稱 */
  a: string;
  /** 管理者-角色-名稱 */
  b: string;
  /** 員工-ID */
  c: number;
  /** 員工-姓名 */
  d: string;
  /** 員工-帳號 */
  e: string;
  /** 員工-是否啟用 */
  f: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-員工-控制器-新增-請求模型 */
export interface MbsSysEmpCtlAddReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-帳號 */
  a: string;
  /** 員工-密碼 */
  b: string;
  /** 員工-姓名 */
  c: string;
  /** 管理者-角色-ID */
  d: number;
  /** 員工-是否啟用 */
  e: boolean;
  /** 員工-權限列表 */
  f: MbsSysEmpCtlAddReqItemMdl[] | null;
}

/** 管理者後台-系統-員工-控制器-新增-回應模型 */
export interface MbsSysEmpCtlAddRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工-ID */
  a: number;
}

/** 管理者後台-系統-員工-控制器-新增-請求-請求項目模型 */
export interface MbsSysEmpCtlAddReqItemMdl {
  /** 原子-目錄 */
  a: DbAtomMenuEnum;
  /** 管理者-區域-ID (-1=全區, 其他=指定地區)*/
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
/** 管理者後台-系統-員工-控制器-更新-請求模型 */
export interface MbsSysEmpCtlUpdateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-ID */
  a: number;
  /** 員工-密碼 */
  b: string | null;
  /** 員工-名稱 */
  c: string | null;
  /** 管理者-角色-ID */
  d: number | null;
  /** 員工-是否啟用 */
  e: boolean | null;
  /** 員工-權限列表 */
  f: MbsSysEmpCtlUpdateReqItemMdl[] | null;
}

/** 管理者後台-系統-員工-控制器-更新-回應模型 */
export interface MbsSysEmpCtlUpdateRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

/** 管理者後台-系統-員工-控制器-更新-請求項目模型 */
export interface MbsSysEmpCtlUpdateReqItemMdl {
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
/** 管理者後台-系統-員工-控制器-取得單筆-請求模型 */
export interface MbsSysEmpCtlGetReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-ID */
  a: number;
}

/** 管理者後台-系統-員工-控制器-取得單筆-回應模型 */
export interface MbsSysEmpCtlGetRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工-帳號 */
  a: string;
  /** 員工-信箱 */
  b: string;
  /** 員工-姓名 */
  c: string;
  /** 管理者-角色-ID */
  d: number;
  /** 管理者-角色-名稱 */
  e: string;
  /** 管理者-地區-ID */
  f: number;
  /** 管理者-地區-名稱 */
  g: string;
  /** 管理者-部門-名稱 */
  h: string;
  /** 員工-是否啟用 */
  i: boolean;
  /** 員工-權限列表 */
  j: MbsSysEmpCtlGetRspItemMdl[];
}

/** 管理者後台-系統-員工-控制器-取得單筆-回應項目模型 */
export interface MbsSysEmpCtlGetRspItemMdl {
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
