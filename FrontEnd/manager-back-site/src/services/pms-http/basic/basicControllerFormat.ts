import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomSecurityGradeEnum } from "@/constants/DbAtomSecurityGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";

//#region 登入
/** 管理者後台-基本-控制器-登入-請求模型 */
export interface MbsBscCtlLoginReqMdl {
  /** 員工-帳號 */
  a: string;
  /** 員工-密碼 */
  b: string;
}

/** 管理者後台-基本-控制器-登入-回應模型 */
export interface MbsBscCtlLoginRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 登入令牌 */
  a: string;
}

//#endregion

//#region 登出
/** 管理者後台-基本-控制器-登出-請求模型 */
export interface MbsBscCtlLogoutReqMdl {
  /** 登入令牌 */
  aa: string;
}

/** 管理者後台-基本-控制器-登出-回應模型 */
export interface MbsBscCtlLogoutRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//#endregion

//#region 心跳
/** 管理者後台-基本-控制器-心跳-請求模型 */
export interface MbsBscCtlHeartbeatReqMdl {
  /** 登入令牌 */
  aa: string;
}

/** 管理者後台-基本-控制器-心跳-回應模型 */
export interface MbsBscCtlHeartbeatRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//#endregion

//#region 修改密碼
/** 管理者後台-基本-控制器-修改密碼-請求模型 */
export interface MbsBscCtlUpdatePasswordReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 舊密碼 */
  a: string;
  /** 新密碼 */
  b: string;
}

/** 管理者後台-基本-控制器-修改密碼-回應模型 */
export interface MbsBscCtlUpdatePasswordRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//#endregion

//#region 取得員工資訊
/** 管理者後台-基本-控制器-取得員工資訊(姓名、權限)-請求模型 */
export interface MbsBscCtlGetEmployeeReqMdl {
  /** 登入令牌 */
  aa: string;
}

/** 管理者後台-基本-控制器-取得員工資訊(姓名、權限)-回應模型 */
export interface MbsBscCtlGetEmployeeRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工-名稱 */
  a: string;
  /** 員工-權限列表 */
  b: MbsBscCtlGetEmployeeRspItemMdl[];
  /** 員工-帳號 */
  c: string;
  /** 管理者-角色-ID */
  d: number;
  /** 管理者-角色-名稱 */
  e: string;
  /** 管理者-地區-ID */
  f: number;
  /** 管理者-地區-名稱 */
  g: string;
  /** 管理者-部門-ID */
  h: number;
  /** 管理者-部門-名稱 */
  i: string;
}

/** 管理者後台-基本-控制器-取得員工資訊(姓名、權限)-回應項目模型 */
export interface MbsBscCtlGetEmployeeRspItemMdl {
  /** 原子-目錄 */
  a: DbAtomMenuEnum;
  /** 管理者-角色-地區ID */
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

//#endregion

//#region 取得多筆員工資訊
/** 管理者後台-基本-控制器-取得多筆員工資訊-請求模型 */
export interface MbsBscCtlGetManyEmployeeInfoReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 管理者-角色-ID */
  a: number | null;
  /** 員工-是否啟用 */
  b: boolean;
  /** 頁面索引 */
  c: number;
  /** 管理者-地區-ID */
  d: number | null;
  /** 管理者-部門-ID */
  e: number | null;
  /** 員工-姓名(模糊查詢) */
  f: string | null;
}

/** 管理者後台-基本-控制器-取得多筆員工資訊-回應模型 */
export interface MbsBscCtlGetEmployeeInfoRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者地區-列表 */
  a: MbsBscCtlGetEmployeeInfoRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆員工資訊-回應項目模型 */
export interface MbsBscCtlGetEmployeeInfoRspItemMdl {
  /** 員工-ID */
  a: number;
  /** 員工-名稱 */
  b: string;
  /** 員工-是否啟用 */
  c: boolean;
  /** 管理者-地區-ID */
  d: number;
  /** 管理者-地區-名稱 */
  e: string;
  /** 管理者-部門-ID */
  f: number;
  /** 管理者-部門-名稱 */
  g: string;
}

//#endregion

//#region 取得多筆員工專案成員
/** 管理者後台-基本-控制器-取得多筆員工專案成員-請求模型 */
export interface MbsBscCtlGetManyEmployeeProjectMemberReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 員工專案 ID */
  a: number;
}

/** 管理者後台-基本-控制器-取得多筆員工專案成員-回應模型 */
export interface MbsBscCtlGetManyEmployeeProjectMemberRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工專案成員列表 */
  a: MbsBscCtlGetManyEmployeeProjectMemberRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆員工專案成員-回應項目模型 */
export interface MbsBscCtlGetManyEmployeeProjectMemberRspItemMdl {
  /** 員工專案成員角色 */
  a: DbEmployeeProjectMemberRoleEnum;
  /** 員工 ID */
  b: number;
  /** 員工名稱 */
  c: string;
}

//#endregion

//#region 取得多筆員工專案
/** 管理者後台-基本-控制器-取得多筆員工專案-請求模型 */
export interface MbsBscCtlGetManyProjectReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 專案名稱(模糊查詢) */
  a: string | null;
  /** 頁面索引 */
  b: number;
}

/** 管理者後台-基本-控制器-取得多筆員工專案-回應模型 */
export interface MbsBscCtlGetManyProjectRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工專案-列表 */
  a: MbsBscCtlGetManyProjectRspItemMdl[] | null;
}

/** 管理者後台-基本-控制器-取得多筆員工專案-回應項目模型 */
export interface MbsBscCtlGetManyProjectRspItemMdl {
  /** 員工專案-ID */
  a: number;
  /** 員工專案-名稱 */
  b: string | null;
}
//#endregion

//#region 取得多筆專案里程碑
/** 管理者後台-基本-控制器-取得多筆專案里程碑-請求模型 */
export interface MbsBscCtlGetManyProjectStoneReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 員工專案-ID */
  a: number | null;
  /** 專案里程碑-名稱 */
  b: string | null;
  /** 頁面索引 */
  c: number;
}

/** 管理者後台-基本-控制器-取得多筆專案里程碑-回應模型 */
export interface MbsBscCtlGetManyProjectStoneRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 專案里程碑-列表 */
  a: MbsBscCtlGetManyProjectStoneRspItemMdl[] | null;
}

/** 管理者後台-基本-控制器-取得多筆專案里程碑-回應項目模型 */
export interface MbsBscCtlGetManyProjectStoneRspItemMdl {
  /** 員工專案里程碑-ID */
  a: number;
  /** 專案里程碑-名稱 */
  b: string | null;
}
//#endregion

//#region 取得多筆專案里程碑工項
/** 管理者後台-基本-控制器-專案里程碑工項-取得多筆-請求模型 */
export interface MbsBscCtlGetManyProjectStoneJobReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案-ID */
  a: number | null;
  /** 員工專案里程碑-ID */
  b: number | null;
  /** 員工專案里程碑工項-名稱 */
  c: string | null;
  /** 頁面索引 */
  d: number;
}

/** 管理者後台-基本-控制器-專案里程碑工項-取得多筆-回應模型 */
export interface MbsBscCtlGetManyProjectStoneJobRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 專案里程碑工項-列表 */
  a: MbsBscCtlGetManyProjectStoneJobRspItemMdl[] | null;
}

/** 管理者後台-基本-控制器-專案里程碑工項-取得多筆-回應項目模型 */
export interface MbsBscCtlGetManyProjectStoneJobRspItemMdl {
  /** 員工專案里程碑工項-ID */
  a: number;
  /** 員工專案里程碑工項-名稱 */
  b: string | null;
}
//#endregion

//#region 取得多筆管理者部門
/** 管理者後台-基本-控制器-部門-取得多筆-請求模型 */
export interface MbsBscCtlGetManyDepartmentReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 部門-名稱 */
  a: string | null;
  /** 是否啟用 */
  b: boolean | null;
  /** 頁面索引 */
  c: number;
}

/** 管理者後台-基本-控制器-部門-取得多筆-回應模型 */
export interface MbsBscCtlGetManyDepartmentRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者部門-列表 */
  a: MbsBscCtlGetManyDepartmentRspItemMdl[];
}

/** 管理者後台-基本-控制器-部門-取得多筆-回應項目模型 */
export interface MbsBscCtlGetManyDepartmentRspItemMdl {
  /** 管理者部門-ID */
  a: number;
  /** 管理者部門-名稱 */
  b: string;
  /** 管理者部門-是否啟用 */
  c: boolean;
}

//#endregion

//#region 取得全部管理者部門
/** 管理者後台-基本-控制器-取得全部管理者部門-請求模型 */
export interface MbsBscCtlGetAllDepartmentReqMdl {
  /** 登入令牌 */
  aa: string;
}

/** 管理者後台-基本-控制器-取得全部管理者部門-回應模型 */
export interface MbsBscCtlGetAllDepartmentRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者部門-列表 */
  a: MbsBscCtlGetAllDepartmentRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得全部管理者部門-回應項目模型 */
export interface MbsBscCtlGetAllDepartmentRspItemMdl {
  /** 管理者部門-ID */
  a: number;
  /** 管理者部門-名稱 */
  b: string;
}

//#endregion

//#region 取得多筆管理者地區
/** 管理者後台-基本-控制器-地區-取得多筆-請求模型 */
export interface MbsBscCtlGetManyRegionReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 地區-名稱 */
  a: string | null;
  /** 是否啟用 */
  b: boolean | null;
  /** 頁面索引 */
  c: number;
}

/** 管理者後台-基本-控制器-地區-取得多筆-回應模型 */
export interface MbsBscCtlGetManyRegionRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者地區-列表 */
  a: MbsBscCtlGetManyRegionRspItemMdl[];
}

/** 管理者後台-基本-控制器-地區-取得多筆-回應項目模型 */
export interface MbsBscCtlGetManyRegionRspItemMdl {
  /** 管理者地區-ID */
  a: number;
  /** 管理者地區-名稱 */
  b: string;
  /** 管理者地區-是否啟用 */
  c: boolean;
}

//#endregion

//#region 取得全部管理者地區
/** 管理者後台-基本-控制器-取得全部管理者地區-請求模型 */
export interface MbsBscCtlGetAllRegionReqMdl {
  /** 登入令牌 */
  aa: string;
}

/** 管理者後台-基本-控制器-取得全部管理者地區-回應模型 */
export interface MbsBscCtlGetAllRegionRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者地區-列表 */
  a: MbsBscCtlGetAllRegionRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得全部管理者地區-回應項目模型 */
export interface MbsBscCtlGetAllRegionRspItemMdl {
  /** 管理者地區-ID */
  a: number;
  /** 管理者地區-名稱 */
  b: string;
}

//#endregion

//#region 取得多筆管理者角色
/** 管理者後台-基本-控制器-角色-取得多筆-請求模型 */
export interface MbsBscCtlGetManyRoleReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 角色-名稱 */
  a: string | null;
  /** 是否啟用 */
  b: boolean | null;
  /** 頁面索引 */
  c: number;
}

/** 管理者後台-基本-控制器-角色-取得多筆-回應模型 */
export interface MbsBscCtlGetManyRoleRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者角色-列表 */
  a: MbsBscCtlGetManyRoleRspItemMdl[];
}

/** 管理者後台-基本-控制器-角色-取得多筆-回應項目模型 */
export interface MbsBscCtlGetManyRoleRspItemMdl {
  /** 管理者角色-ID */
  a: number;
  /** 管理者角色-名稱 */
  b: string;
  /** 管理者角色-地區ID */
  c: number;
  /** 管理者角色-地區名稱 */
  d: string;
  /** 管理者角色-部門名稱 */
  e: string;
  /** 管理者角色-是否啟用 */
  f: boolean;
}
//#endregion

//#region 取得多筆角色權限從[角色ID]
/** 管理者後台-基本-控制器-取得多筆角色權限從[角色ID]-請求模型 */
export interface MbsBscCtlGetManyRolePermissionFromRoleIdReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 角色-ID */
  a: number;
}

/** 管理者後台-基本-控制器-取得多筆角色權限從[角色ID]-回應模型 */
export interface MbsBscCtlGetManyRolePermissionFromRoleIdRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 角色權限-列表 */
  a: MbsBscCtlGetManyRolePermissionFromRoleIdRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆角色權限從[角色ID]-回應項目模型 */
export interface MbsBscCtlGetManyRolePermissionFromRoleIdRspItemMdl {
  /** 原子-目錄 */
  a: DbAtomMenuEnum;
  /** 管理者-角色-地區ID */
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

//#endregion

//#region 取得多筆產品主分類
/** 管理者後台-基本-控制器-取得多筆產品主分類-請求模型 */
export interface MbsBscCtlGetManyProductMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品主分類-名稱*/
  a: string | null;
  /** 管理者-產品主分類-是否啟用*/
  b: boolean | null;
  /** 頁面索引 */
  c: number;
}

/** 管理者後台-基本-控制器-取得多筆產品主分類-回應模型 */
export interface MbsBscCtlGetManyProductMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品主分類-列表 */
  a: MbsBscCtlGetManyProductMainKindRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆產品主分類-回應項目模型 */
export interface MbsBscCtlGetManyProductMainKindRspItemMdl {
  /** 管理者-產品主分類-ID */
  a: number;
  /** 管理者-產品主分類-名稱 */
  b: string;
  /** 管理者-產品主分類-是否啟用 */
  c: boolean;
}

//#endregion

//#region 取得多筆產品子分類
/** 管理者後台-基本-控制器-產品子分類-取得多筆-請求模型 */
export interface MbsBscCtlGetManyProductSubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 產品主分類ID */
  a: number | null;
  /** 產品子分類-名稱 */
  b: string | null;
  /** 是否啟用 */
  c: boolean;
  /** 頁面索引 */
  d: number;
}

/** 管理者後台-基本-控制器-產品子分類-取得多筆-回應模型 */
export interface MbsBscCtlGetManyProductSubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 產品子分類-列表 */
  a: MbsBscCtlGetManyProductSubKindRspItemMdl[];
}

/** 管理者後台-基本-控制器-產品子分類-取得多筆-回應項目模型 */
export interface MbsBscCtlGetManyProductSubKindRspItemMdl {
  /** 產品子分類-ID */
  a: number;
  /** 產品子分類-名稱 */
  b: string;
  /** 產品子分類-是否啟用 */
  c: boolean;
}
//#endregion

//#region 取得多筆產品規格
/** 管理者後台-基本-控制器-取得多筆產品規格-請求模型 */
export interface MbsBscCtlGetManyProductSpecificationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 產品ID */
  a: number;
  /** 產品規格-名稱 */
  b: string | null;
  /** 是否啟用 */
  c: boolean;
  /** 頁面索引 */
  d: number;
}

/** 管理者後台-基本-控制器-取得多筆產品規格-回應模型 */
export interface MbsBscCtlGetManyProductSpecificationRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 產品規格-列表 */
  a: MbsBscCtlGetManyProductSpecificationRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆產品規格-回應項目模型 */
export interface MbsBscCtlGetManyProductSpecificationRspItemMdl {
  /** 管理者-產品規格-ID */
  a: number;
  /** 管理者-產品規格-名稱 */
  b: string;
  /** 管理者-產品規格-產品售價 */
  c: number;
  /** 管理者-產品規格-產品成本 */
  d: number;
  /** 管理者-產品規格-是否啟用 */
  e: boolean;
}
//#endregion

//#region 取得多筆窗口開發評等原因
/** 管理者後台-基本-控制器-取得多筆窗口開發評等原因-請求模型 */
export interface MbsBscCtlGetManyContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 窗口開發評等原因名稱(模糊查詢) */
  a: string | null;
  /** 窗口開發評等原因-是否啟用 */
  b: boolean | null;
  /** 頁面索引 */
  c: number;
}

/** 管理者後台-基本-控制器-取得多筆窗口開發評等原因-回應模型 */
export interface MbsBscCtlGetManyContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 窗口開發評等原因-列表 */
  a: MbsBscCtlGetManyContacterRatingReasonRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆窗口開發評等原因-回應項目模型 */
export interface MbsBscCtlGetManyContacterRatingReasonRspItemMdl {
  /** 窗口開發評等原因-ID */
  a: number;
  /** 窗口開發評等原因-名稱 */
  b: string;
  /** 窗口開發評等原因-狀態 */
  c: boolean;
}

//#endregion

//#region 取得多筆公司主分類
/** 管理者後台-基本-控制器-取得多筆公司主分類-請求模型 */
export interface MbsBscCtlGetManyCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司主分類名稱*/
  a: string | null;
  /** 頁面索引 */
  b: number;
}

/** 管理者後台-基本-控制器-取得多筆公司主分類-回應模型 */
export interface MbsBscCtlGetManyCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司主分類-列表 */
  a: MbsBscCtlGetManyCompanyMainKindRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆公司主分類-回應項目模型 */
export interface MbsBscCtlGetManyCompanyMainKindRspItemMdl {
  /** 管理者-公司主分類-ID */
  a: number;
  /** 管理者-公司主分類-名稱 */
  b: string;
  /** 管理者-公司主分類-是否啟用 */
  c: boolean;
}

//#endregion

//#region 取得多筆公司子分類
/** 管理者後台-基本-控制器-取得多筆公司子分類-請求模型 */
export interface MbsBscCtlGetManyCompanySubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 公司主分類ID */
  a: number | null;
  /** 公司子分類名稱 */
  b: string | null;
  /** 公司子分類是否啟用 */
  c: boolean | null;
  /** 頁面索引 */
  d: number;
}

/** 管理者後台-基本-控制器-取得多筆公司子分類-回應模型 */
export interface MbsBscCtlGetManyCompanySubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 公司子分類列表 */
  a: MbsBscCtlGetManyCompanySubKindRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆公司子分類-回應項目模型 */
export interface MbsBscCtlGetManyCompanySubKindRspItemMdl {
  /** 公司子分類ID */
  a: number;
  /** 公司子分類名稱 */
  b: string;
  /** 公司子分類是否啟用 */
  c: boolean;
}
//#endregion

//#region 取得多筆窗口
/** 管理者後台-基本-控制器-取得多筆窗口-請求模型 */
export interface MbsBscCtlGetManyContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口-名稱 */
  a: string | null;
  /** 管理者-窗口-公司ID */
  b: number;
  /** 管理者-窗口-電子郵件 */
  c: string | null;
  /** 頁面索引 */
  d: number;
}

/** 管理者後台-基本-控制器-取得多筆窗口-回應模型 */
export interface MbsBscCtlGetManyContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口-列表 */
  a: MbsBscCtlGetManyContacterRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆窗口-回應項目模型 */
export interface MbsBscCtlGetManyContacterRspItemMdl {
  /** 管理者-窗口-ID */
  a: number;
  /** 管理者-窗口-名稱 */
  b: string;
  /** 管理者-窗口-電子郵件 */
  c: string;
}
//#endregion

//#region 取得管理者窗口(用在名單)
/** 管理者後台-系統-名單窗口-控制器-取得管理者窗口-請求模型 */
export interface MbsSysCttCtlGetManagerContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口-ID */
  a: number;
}

/** 管理者後台-系統-名單窗口-控制器-取得管理者窗口-回應項目模型 */
export interface MbsSysCttCtlGetManagerContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口-ID */
  a: number;
  /** 管理者-公司-ID */
  b: number;
  /** 管理者-窗口-姓名 */
  c: string | null;
  /** 管理者-窗口-Email */
  d: string;
  /** 管理者-窗口-電話 */
  e: string | null;
  /** 管理者-窗口-部門 */
  f: string | null;
  /** 管理者-窗口-職稱 */
  g: string | null;
  /** 管理者-窗口-電話(市話) */
  h: string | null;
  /** 原子-管理者窗口狀態 */
  i: DbAtomManagerContacterStatusEnum;
  /** 管理者-窗口-是否同意問卷 */
  j: boolean;
  /** 原子-窗口評等類型 */
  k: DbAtomManagerContacterRatingKindEnum;
  /**管理者-窗口-建立者員工ID */
  l: number;
  /** 管理者-窗口-備註  */
  m: string | null;
}
//#endregion

//#region 取得多筆管理者公司
/** 管理者後台-基本-控制器-取得多筆管理者公司-請求模型 */
export interface MbsBscCtlGetManyCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者公司-名稱 */
  a: string | null;
  /** 頁面索引 */
  b: number;
}

/** 管理者後台-基本-控制器-取得多筆管理者公司-回應模型 */
export interface MbsBscCtlGetManyCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者公司-列表 */
  a: MbsBscCtlGetManyCompanyRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆管理者公司-回應項目模型 */
export interface MbsBscCtlGetManyCompanyRspItemMdl {
  /** 管理者公司-編號 */
  a: number;
  /** 管理者公司-名稱 */
  b: string;
}
//#endregion

//#region 取得多筆公司名稱從[商機原始]
/** 管理者後台-基本-控制器-取得多筆公司名稱從[商機原始]-請求模型 */
export interface MbsBscCtlGetManyCompanyNameFromPipelineOriginalReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者公司-名稱 */
  a: string | null;
  /** 頁面索引 */
  b: number;
}

/** 管理者後台-基本-控制器-取得多筆公司名稱從[商機原始]-回應模型 */
export interface MbsBscCtlGetManyCompanyNameFromPipelineOriginalRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者公司名稱-列表 */
  a: string[];
}
//#endregion

//#region 取得多筆產品
/** 管理者後台-基本-控制器-產品-取得多筆-請求模型 */
export interface MbsBscCtlGetManyProductReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品-主類別ID */
  a: number | null;
  /** 管理者-產品-子類別ID */
  b: number | null;
  /** 管理者-產品-名稱 */
  c: string | null;
  /** 頁面索引 */
  d: number;
}

/** 管理者後台-基本-控制器-產品-取得多筆-回應模型 */
export interface MbsBscCtlGetManyProductRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 產品-列表 */
  a: MbsBscCtlGetManyProductRspItemMdl[];
}

/** 管理者後台-基本-控制器-產品-取得多筆-回應項目模型 */
export interface MbsBscCtlGetManyProductRspItemMdl {
  /** 管理者-產品-ID */
  a: number;
  /** 管理者-產品-名稱 */
  b: string;
  /** 管理者-產品-是否啟用 */
  c: boolean;
  /** 管理者-產品-主類別ID */
  d: number;
  /** 管理者-產品-主類別名稱 */
  e: string;
  /** 管理者-產品-子類別ID */
  f: number;
  /** 管理者-產品-子類別名稱 */
  g: string;
  /** 管理者-產品-主分類-業務毛利率 */
  h: number;
}
//#endregion

//#region 取得管理者公司
/** 管理者後台-基本-控制器-取得管理者公司-請求模型 */
export interface MbsBscCtlGetManagerCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者公司-ID */
  a: number;
}

/** 管理者後台-基本-控制器-取得管理者公司-回應模型 */
export interface MbsBscCtlGetManagerCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者公司-ID */
  a: number;
  /** 統一編號 */
  b: string | null;
  /** 管理者公司名稱 */
  c: string | null;
  /** 管理者公司狀態 */
  d: DbAtomManagerCompanyStatusEnum | null;
  /** 管理者公司主分類-ID */
  e: number;
  /** 管理者公司主分類-名稱 */
  f: string;
  /** 管理者公司子分類-ID */
  g: number;
  /** 管理者公司子分類-名稱 */
  h: string;
  /** 客戶等級 */
  i: DbAtomCustomerGradeEnum | null;
  /** 安全等級 */
  j: DbAtomSecurityGradeEnum | null;
  /** 員工範圍 */
  k: DbAtomEmployeeRangeEnum | null;
}
//#endregion

//#region 取得多筆公司營業地點
/** 管理者後台-基本-控制器-取得多筆公司營業地點-請求模型 */
export interface MbsBscCtlGetManyCompanyLocationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者公司ID */
  a: number;
  /** 管理者公司營業地點名稱（模糊搜尋） */
  b: string | null;
  /** 頁面索引 */
  c: number;
}

/** 管理者後台-基本-控制器-取得多筆公司營業地點-回應模型 */
export interface MbsBscCtlGetManyCompanyLocationRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者公司營業地點清單 */
  a: MbsBscCtlGetManyCompanyLocationRspItemMdl[];
}

/** 管理者後台-基本-控制器-取得多筆公司營業地點-項目-回應模型 */
export interface MbsBscCtlGetManyCompanyLocationRspItemMdl {
  /** 管理者公司營業地點ID */
  a: number;
  /** 管理者公司營業地點名稱 */
  b: string;
  /** 管理者公司營業地點是否已刪除 */
  c: boolean;
}
//#endregion

//#region 取得公司營業地點
/** 管理者後台-基本-控制器-取得公司營業地點-請求模型 */
export interface MbsBscCtlGetManagerCompanyLocationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者公司營業地點-ID */
  a: number;
  /** 管理者公司營業地點-是否已刪除 */
  b: boolean | null;
}

/** 管理者後台-基本-控制器-取得公司營業地點-回應模型 */
export interface MbsBscCtlGetManagerCompanyLocationRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者公司營業地點-ID */
  a: number;
  /** 管理者公司-ID */
  b: number;
  /** 管理者公司營業地點-名稱 */
  c: string | null;
  /** 原子-縣市 */
  d: DbAtomCityEnum;
  /** 地址 */
  e: string | null;
  /** 電話 */
  f: string | null;
  /** 管理者公司狀態 */
  g: DbAtomManagerCompanyStatusEnum;
}
//#endregion
