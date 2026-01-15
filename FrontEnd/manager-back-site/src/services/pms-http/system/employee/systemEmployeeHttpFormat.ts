import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-員工-Http-取得多筆-請求模型 */
export interface MbsSysEmpHttpGetManyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-部門-ID */
  managerDepartmentID: number | null;
  /** 管理者-角色-ID*/
  managerRoleID: number | null;
  /** 員工-是否啟用 */
  employeeIsEnable: boolean | null;
  /** 員工-帳號 */
  employeeAccount: string | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** 管理者後台-系統-員工-Http-取得多筆-回應模型 */
export interface MbsSysEmpHttpGetManyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工-列表 */
  employeeList: MbsSysEmpHttpGetManyRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-員工-Http-取得多筆-回應項目模型 */
export interface MbsSysEmpHttpGetManyRspItemMdl {
  /** 管理者-部門-名稱 */
  managerDepartmentName: string;
  /** 管理者-角色-名稱 */
  managerRoleName: string;
  /** 員工-ID */
  employeeID: number;
  /** 員工-姓名 */
  employeeName: string;
  /** 員工-帳號 */
  employeeAccount: string;
  /** 員工-是否啟用 */
  employeeIsEnable: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-員工-Http-新增-請求模型 */
export interface MbsSysEmpHttpAddReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-帳號 */
  employeeAccount: string;
  /** 員工-密碼 */
  employeePassword: string;
  /** 員工-姓名 */
  employeeName: string;
  /** 管理者-角色-ID*/
  managerRoleID: number;
  /** 員工-是否啟用 */
  employeeIsEnable: boolean;
  /** 員工-權限列表 */
  employeePermissionList: MbsSysEmpHttpAddReqItemMdl[] | null;
}

/** 管理者後台-系統-員工-Http-新增-回應模型 */
export interface MbsSysEmpHttpAddRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工-ID */
  employeeID: number;
}

/** 管理者後台-系統-員工-Http-新增-請求項目模型 */
export interface MbsSysEmpHttpAddReqItemMdl {
  /** 原子-目錄 */
  atomMenu: DbAtomMenuEnum;
  /** 管理者-區域-ID (-1=全區, 其他=指定地區)*/
  managerRegionID: number;
  /** 原子-權限-類型-檢視 */
  atomPermissionKindIdView: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-新增 */
  atomPermissionKindIdCreate: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-編輯 */
  atomPermissionKindIdEdit: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-刪除 */
  atomPermissionKindIdDelete: DbAtomPermissionKindEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-員工-Http-更新-請求模型 */
export interface MbsSysEmpHttpUpdateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-ID */
  employeeID: number;
  /** 員工-密碼 */
  employeePassword: string | null;
  /** 員工-名稱 */
  employeeName: string | null;
  /** 管理者-角色-ID*/
  managerRoleID: number | null;
  /** 員工-是否啟用 */
  employeeIsEnable: boolean | null;
  /** 員工-權限列表 */
  employeePermissionList: MbsSysEmpHttpUpdateReqItemMdl[] | null;
}

/** 管理者後台-系統-員工-Http-更新-回應模型 */
export interface MbsSysEmpHttpUpdateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

/** 管理者後台-系統-員工-Http-更新-請求項目模型 */
export interface MbsSysEmpHttpUpdateReqItemMdl {
  /** 原子-目錄 */
  atomMenu: DbAtomMenuEnum;
  /** 管理者-區域-ID */
  managerRegionID: number;
  /** 原子-權限-類型-檢視 */
  atomPermissionKindIdView: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-新增 */
  atomPermissionKindIdCreate: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-編輯 */
  atomPermissionKindIdEdit: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-刪除 */
  atomPermissionKindIdDelete: DbAtomPermissionKindEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-員工-Http-取得單筆-請求模型 */
export interface MbsSysEmpHttpGetReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-ID */
  employeeID: number;
}

/** 管理者後台-系統-員工-Http-取得單筆-回應模型 */
export interface MbsSysEmpHttpGetRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工-帳號 */
  employeeAccount: string;
  /** 員工-信箱 */
  employeeEmail: string;
  /** 員工-姓名 */
  employeeName: string;
  /** 管理者-角色-ID */
  managerRoleID: number;
  /** 管理者-角色-名稱 */
  managerRoleName: string;
  /** 管理者-地區-ID */
  managerRegionID: number;
  /** 管理者-地區-名稱 */
  managerRegionName: string;
  /** 管理者-部門-名稱 */
  managerDepartmentName: string;
  /** 員工-是否啟用 */
  employeeIsEnable: boolean;
  /** 員工-權限列表 */
  employeePermissionList: MbsSysEmpHttpGetRspItemMdl[];
}

/** 管理者後台-系統-員工-Http-取得單筆-回應項目模型 */
export interface MbsSysEmpHttpGetRspItemMdl {
  /** 原子-目錄 */
  atomMenu: DbAtomMenuEnum;
  /** 管理者-區域-ID */
  managerRegionID: number;
  /** 原子-權限-類型-檢視 */
  atomPermissionKindIdView: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-新增 */
  atomPermissionKindIdCreate: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-編輯 */
  atomPermissionKindIdEdit: DbAtomPermissionKindEnum;
  /** 原子-權限-類型-刪除 */
  atomPermissionKindIdDelete: DbAtomPermissionKindEnum;
}
