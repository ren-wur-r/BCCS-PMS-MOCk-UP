import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-角色-Http-取得多筆-請求模型 */
export interface MbsSysRolHttpGetManyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-角色-名稱 */
  managerRoleName: string | null;
  /** 管理者-角色-是否啟用 */
  managerRoleIsEnable: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** 管理者後台-系統-角色-Http-取得多筆-回應模型 */
export interface MbsSysRolHttpGetManyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-角色-列表 */
  managerRoleList: MbsSysRolHttpGetManyRspItemMdl[] | null;
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-角色-Http-取得多筆-回應項目模型 */
export interface MbsSysRolHttpGetManyRspItemMdl {
  /** 管理者-角色-ID */
  managerRoleID: number;
  /** 管理者-角色-名稱 */
  managerRoleName: string;
  /** 員工-數量 */
  employeeCount: number;
  /** 管理者-角色-是否啟用 */
  managerRoleIsEnable: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-角色-Http-取得單筆-請求模型 */
export interface MbsSysRolHttpGetReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-角色-ID */
  managerRoleID: number;
}

/** 管理者後台-系統-角色-Http-取得單筆-回應模型 */
export interface MbsSysRolHttpGetRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-角色-名稱 */
  managerRoleName: string | null;
  /** 管理者-角色-地區ID */
  managerRegionID: number | null;
  /** 管理者-角色-地區名稱 */
  managerRegionName: string | null;
  /** 管理者-角色-部門ID */
  managerDepartmentID: number | null;
  /** 管理者-角色-部門名稱 */
  managerDepartmentName: string | null;
  /** 管理者-角色-備註 */
  managerRoleRemark: string | null;
  /** 管理者-角色-是否啟用 */
  managerRoleIsEnable: boolean | null;
  /** 員工-人數 */
  employeeCount: number;
  /** 管理者-角色-權限列表 */
  managerRolePermissionList: MbsSysRolHttpGetRspItemMdl[] | null;
}

/** 管理者後台-系統-角色-Http-取得單筆-回應項目模型 */
export interface MbsSysRolHttpGetRspItemMdl {
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
/** 管理者後台-系統-角色-Http-新增-請求模型 */
export interface MbsSysRolHttpAddReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-角色-名稱 */
  managerRoleName: string;
  /** 管理者-角色-地區ID */
  managerRegionID: number;
  /** 管理者-角色-部門ID */
  managerDepartmentID: number;
  /** 管理者-角色-備註 */
  managerRoleRemark: string | null;
  /** 管理者-角色-是否啟用 */
  managerRoleIsEnable: boolean;
  /** 管理者-角色-權限列表 */
  managerRolePermissionList: MbsSysRolHttpAddReqItemMdl[] | null;
}

/** 管理者後台-系統-角色-Http-新增-回應模型 */
export interface MbsSysRolHttpAddRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-角色-ID */
  managerRoleID: number | null;
}

/** 管理者後台-系統-角色-Http-新增-請求項目模型 */
export interface MbsSysRolHttpAddReqItemMdl {
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
/** 管理者後台-系統-角色-Http-更新-請求模型 */
export interface MbsSysRolHttpUpdateReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-角色-ID */
  managerRoleID: number;
  /** 管理者-角色-名稱 */
  managerRoleName: string | null;
  /** 管理者-角色-地區ID */
  managerRoleRegionID: number | null;
  /** 管理者-角色-部門ID */
  managerDepartmentID: number | null;
  /** 管理者-角色-備註 */
  managerRoleRemark: string | null;
  /** 管理者-角色-是否啟用 */
  managerRoleIsEnable: boolean | null;
  /** 管理者-角色-權限列表 */
  managerRolePermissionList: MbsSysRolHttpUpdateReqItemMdl[] | null;
}

/** 管理者後台-系統-角色-Http-更新-回應模型 */
export interface MbsSysRolHttpUpdateRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

/** 管理者後台-系統-角色-Http-更新-請求項目模型 */
export interface MbsSysRolHttpUpdateReqItemMdl {
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
