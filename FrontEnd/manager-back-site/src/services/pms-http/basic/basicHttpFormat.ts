import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomSecurityGradeEnum } from "@/constants/DbAtomSecurityGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";

//#region 登入
/** 管理者後台-基本-Http-登入-請求模型 */
export interface MbsBscHttpLoginReqMdl {
  /** 員工-帳號 */
  employeeAccount: string;
  /** 員工-密碼 */
  employeePassword: string;
}

/** 管理者後台-基本-Http-登入-回應模型 */
export interface MbsBscHttpLoginRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 登入令牌 */
  loginToken: string;
}

//#endregion

//#region 登出
/** 管理者後台-基本-Http-登出-請求模型 */
export interface MbsBscHttpLogoutReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
}

/** 管理者後台-基本-Http-登出-回應模型 */
export interface MbsBscHttpLogoutRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//#endregion

//#region 心跳
/** 管理者後台-基本-Http-心跳-請求模型 */
export interface MbsBscHttpHeartbeatReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
}

/** 管理者後台-基本-Http-心跳-回應模型 */
export interface MbsBscHttpHeartbeatRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//#endregion

//#region 修改密碼
/** 管理者後台-基本-Http-修改密碼-請求模型 */
export interface MbsBscHttpUpdatePasswordReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 舊密碼 */
  oldPassword: string;
  /** 新密碼 */
  newPassword: string;
}

/** 管理者後台-基本-Http-修改密碼-回應模型 */
export interface MbsBscHttpUpdatePasswordRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//#endregion

//#region 取得員工資訊
/** 管理者後台-基本-Http-取得員工資訊(姓名、權限)-請求模型 */
export interface MbsBscHttpGetEmployeeReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
}

/** 管理者後台-基本-Http-取得員工資訊(姓名、權限)-回應模型 */
export interface MbsBscHttpGetEmployeeRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工-名稱 */
  employeeName: string;
  /** 員工-權限列表 */
  managerBackSiteMenuPermissionList: MbsBscHttpGetEmployeeRspItemMdl[];
  /** 員工-帳號 */
  employeeAccount: string;
  /** 管理者-角色-ID */
  managerRoleID: number;
  /** 管理者-角色-名稱 */
  managerRoleName: string;
  /** 管理者-地區-ID */
  managerRegionID: number;
  /** 管理者-地區-名稱 */
  managerRegionName: string;
  /** 管理者-部門-ID */
  managerDepartmentID: number;
  /** 管理者-部門-名稱 */
  managerDepartmentName: string;
}

/** 管理者後台-基本-Http-取得員工資訊(姓名、權限)-回應項目模型 */
export interface MbsBscHttpGetEmployeeRspItemMdl {
  /** 原子-目錄 */
  atomMenu: DbAtomMenuEnum;
  /** 管理者-角色-地區ID */
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

//#endregion

//#region 取得多筆員工資訊
/** 管理者後台-基本-Http-取得多筆員工資訊-請求模型 */
export interface MbsBscHttpGetManyEmployeeInfoReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 管理者-角色-ID */
  managerRoleID: number | null;
  /** 員工-是否啟用 */
  employeeIsEnable: boolean;
  /** 頁面索引 */
  pageIndex: number;
  /** 管理者-地區-ID */
  managerRegionID: number | null;
  /** 管理者-部門-ID */
  managerDepartmentID: number | null;
  /** 員工-姓名(模糊查詢) */
  employeeName: string | null;
}

/** 管理者後台-基本-Http-取得多筆員工資訊-回應模型 */
export interface MbsBscHttpGetEmployeeInfoRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者地區-列表 */
  employeeList: MbsBscHttpGetEmployeeInfoRspItemMdl[];
}

/** 管理者後台-基本-Http-取得多筆員工資訊-回應項目模型 */
export interface MbsBscHttpGetEmployeeInfoRspItemMdl {
  /** 員工-ID */
  employeeID: number;
  /** 員工-名稱 */
  employeeName: string;
  /** 員工-是否啟用 */
  employeeIsEnable: boolean;
  /** 管理者-地區-ID */
  managerRegionID: number;
  /** 管理者-地區-名稱 */
  managerRegionName: string;
  /** 管理者-部門-ID */
  managerDepartmentID: number;
  /** 管理者-部門-名稱 */
  managerDepartmentName: string;
}

//#endregion

//#region 取得多筆員工專案成員

/** 管理者後台-基本-Http-取得多筆員工專案成員-請求模型 */
export interface MbsBscHttpGetManyEmployeeProjectMemberReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 員工專案 ID */
  employeeProjectID: number;
}

/** 管理者後台-基本-Http-取得多筆員工專案成員-回應模型 */
export interface MbsBscHttpGetManyEmployeeProjectMemberRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工專案成員列表 */
  employeeProjectMemberList: MbsBscHttpGetManyEmployeeProjectMemberRspItemMdl[];
}

/** 管理者後台-基本-Http-取得多筆員工專案成員-回應項目模型 */
export interface MbsBscHttpGetManyEmployeeProjectMemberRspItemMdl {
  /** 員工專案成員角色 */
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum;
  /** 員工 ID */
  employeeID: number;
  /** 員工名稱 */
  employeeName: string;
}

//#endregion

//#region 取得多筆員工專案
/** 管理者後台-基本-Http-取得多筆員工專案-請求模型 */
export interface MbsBscHttpGetManyProjectReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案名稱(模糊查詢) */
  projectName: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-取得多筆員工專案-回應模型 */
export interface MbsBscHttpGetManyProjectRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工專案-列表 */
  employeeProjectList: MbsBscHttpGetManyProjectRspItemMdl[] | null;
}

/** 管理者後台-基本-Http-取得多筆員工專案-回應項目模型 */
export interface MbsBscHttpGetManyProjectRspItemMdl {
  /** 員工專案-ID */
  employeeProjectID: number;
  /** 員工專案-名稱 */
  employeeProjectName: string | null;
}

//#endregion

//#region 取得多筆專案里程碑
/** 管理者後台-基本-Http-取得多筆專案里程碑-請求模型 */
export interface MbsBscHttpGetManyProjectStoneReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 員工專案-ID */
  employeeProjectID: number | null;
  /** 專案里程碑-名稱 */
  employeeProjectStoneName: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-取得多筆專案里程碑-回應模型 */
export interface MbsBscHttpGetManyProjectStoneRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 專案里程碑-列表 */
  employeeProjectStoneList: MbsBscHttpGetManyProjectStoneRspItemMdl[] | null;
}

/** 管理者後台-基本-Http-取得多筆專案里程碑-回應項目模型 */
export interface MbsBscHttpGetManyProjectStoneRspItemMdl {
  /** 員工專案里程碑-ID */
  employeeProjectStoneID: number;
  /** 員工專案里程碑-名稱 */
  employeeProjectStoneName: string | null;
}
//#endregion

//#region 取得多筆專案里程碑工項
/** 管理者後台-基本-Http-取得多筆專案里程碑工項-請求模型 */
export interface MbsBscHttpGetManyProjectStoneJobReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案-ID (nullable) */
  employeeProjectID: number | null;
  /** 員工專案里程碑-ID (nullable) */
  employeeProjectStoneID: number | null;
  /** 員工專案里程碑工項-名稱 (nullable) */
  employeeProjectStoneJobName: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-取得多筆專案里程碑工項-回應模型 */
export interface MbsBscHttpGetManyProjectStoneJobRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 專案里程碑工項列表 */
  projectStoneJobList: MbsBscHttpGetManyProjectStoneJobRspItemMdl[];
}

/** 管理者後台-基本-Http-取得多筆專案里程碑工項-回應項目模型 */
export interface MbsBscHttpGetManyProjectStoneJobRspItemMdl {
  /** 員工專案里程碑工項-ID */
  employeeProjectStoneJobID: number;
  /** 員工專案里程碑工項-名稱 */
  employeeProjectStoneJobName: string | null;
}

//#endregion

//#region 取得多筆管理者部門
/** 管理者後台-基本-Http-部門-取得多筆-請求模型 */
export interface MbsBscHttpGetManyDepartmentReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 部門-名稱 */
  managerDepartmentName: string | null;
  /** 是否啟用 */
  managerDepartmentIsEnable: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-部門-取得多筆-回應模型 */
export interface MbsBscHttpGetManyDepartmentRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者部門-列表 */
  managerDepartmentList: MbsBscHttpGetManyDepartmentRspItemMdl[];
}

/** 管理者後台-基本-Http-部門-取得多筆-回應項目模型 */
export interface MbsBscHttpGetManyDepartmentRspItemMdl {
  /** 管理者部門-ID */
  managerDepartmentID: number;
  /** 管理者部門-名稱 */
  managerDepartmentName: string;
  /** 管理者部門-是否啟用 */
  managerDepartmentIsEnable: boolean;
}

//#endregion

//#region 取得全部管理者部門
/** 管理者後台-基本-Http-取得全部管理者部門-請求模型 */
export interface MbsBscHttpGetAllDepartmentReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
}

/** 管理者後台-基本-Http-取得全部管理者部門-回應模型 */
export interface MbsBscHttpGetAllDepartmentRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者部門-列表 */
  managerDepartmentList: MbsBscHttpGetAllDepartmentRspItemMdl[];
}

/** 管理者後台-基本-Http-取得全部管理者部門-回應項目模型 */
export interface MbsBscHttpGetAllDepartmentRspItemMdl {
  /** 管理者部門-ID */
  managerDepartmentID: number;
  /** 管理者部門-名稱 */
  managerDepartmentName: string;
}

//#endregion

//#region 取得多筆管理者地區
/** 管理者後台-基本-Http-地區-取得多筆-請求模型 */
export interface MbsBscHttpGetManyRegionReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 地區-名稱 */
  managerRegionName: string | null;
  /** 是否啟用 */
  managerRegionIsEnable: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-地區-取得多筆-回應模型 */
export interface MbsBscHttpGetManyRegionRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者地區-列表 */
  managerRegionList: MbsBscHttpGetManyRegionRspItemMdl[];
}

/** 管理者後台-基本-Http-地區-取得多筆-回應項目模型 */
export interface MbsBscHttpGetManyRegionRspItemMdl {
  /** 管理者地區-ID */
  managerRegionID: number;
  /** 管理者地區-名稱 */
  managerRegionName: string;
  /** 管理者地區-是否啟用 */
  managerRegionIsEnable: boolean;
}

//#endregion

//#region 取得全部管理者地區
/** 管理者後台-基本-Http-取得全部管理者地區-請求模型 */
export interface MbsBscHttpGetAllRegionReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
}

/** 管理者後台-基本-Http-取得全部管理者地區-回應模型 */
export interface MbsBscHttpGetAllRegionRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者地區-列表 */
  managerRegionList: MbsBscHttpGetAllRegionRspItemMdl[];
}

/** 管理者後台-基本-Http-取得全部管理者地區-回應項目模型 */
export interface MbsBscHttpGetAllRegionRspItemMdl {
  /** 管理者地區-ID */
  managerRegionID: number;
  /** 管理者地區-名稱 */
  managerRegionName: string;
}

//#endregion

//#region 取得多筆管理者角色
/** 管理者後台-基本-Http-角色-取得多筆-請求模型 */
export interface MbsBscHttpGetManyRoleReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 角色-名稱 */
  managerRoleName: string | null;
  /** 是否啟用 */
  managerRoleIsEnable: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-角色-取得多筆-回應模型 */
export interface MbsBscHttpGetManyRoleRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者角色-列表 */
  managerRoleList: MbsBscHttpGetManyRoleRspItemMdl[];
}

/** 管理者後台-基本-Http-角色-取得多筆-回應項目模型 */
export interface MbsBscHttpGetManyRoleRspItemMdl {
  /** 管理者角色-ID */
  managerRoleID: number;
  /** 管理者角色-名稱 */
  managerRoleName: string;
  /** 管理者角色-地區ID */
  managerRegionID: number;
  /** 管理者角色-地區名稱 */
  managerRegionName: string;
  /** 管理者角色-部門名稱 */
  managerDepartmentName: string;
  /** 管理者角色-是否啟用 */
  managerRoleIsEnable: boolean;
}

//#endregion

//#region 取得多筆角色權限從[角色ID]
/** 管理者後台-基本-Http-取得多筆角色權限從[角色ID]-請求模型 */
export interface MbsBscHttpGetManyRolePermissionFromRoleIdReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 角色-ID */
  managerRoleID: number;
}

/** 管理者後台-基本-Http-取得多筆角色權限從[角色ID]-回應模型 */
export interface MbsBscHttpGetManyRolePermissionFromRoleIdRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 角色權限-列表 */
  rolePermissionList: MbsBscHttpGetManyRolePermissionFromRoleIdRspItemMdl[];
}

/** 管理者後台-基本-Http-取得多筆角色權限從[角色ID]-回應項目模型 */
export interface MbsBscHttpGetManyRolePermissionFromRoleIdRspItemMdl {
  /** 原子-目錄 */
  atomMenu: DbAtomMenuEnum;
  /** 管理者-角色-地區ID */
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

//#endregion

//#region 取得多筆產品主分類
/** 管理者後台-基本-Http-取得多筆產品主分類-請求模型 */
export interface MbsBscHttpGetManyProductMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品主分類-名稱*/
  managerProductMainKindName: string | null;
  /** 管理者-產品主分類-是否啟用*/
  managerProductMainKindIsEnable: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-取得多筆產品主分類-回應模型 */
export interface MbsBscHttpGetManyProductMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-產品主分類-列表 */
  managerProductMainKindList: MbsBscHttpGetManyProductMainKindRspItemMdl[];
}

/** 管理者後台-基本-Http-取得多筆產品主分類-回應項目模型 */
export interface MbsBscHttpGetManyProductMainKindRspItemMdl {
  /** 管理者-產品主分類-ID */
  managerProductMainKindID: number;
  /** 管理者-產品主分類-名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品主分類-是否啟用 */
  managerProductMainKindIsEnable: boolean;
}

//#endregion

//#region 取得多筆產品子分類
/** 管理者後台-基本-HTTP-取得多筆產品子分類-請求模型 */
export interface MbsBscHttpGetManyProductSubKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 產品主分類ID */
  productMainKindID: number | null;
  /** 產品子分類-名稱 */
  productSubKindName: string | null;
  /** 是否啟用 */
  productSubKindIsEnable: boolean;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-HTTP-取得多筆產品子分類-回應模型 */
export interface MbsBscHttpGetManyProductSubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 產品子分類-列表 */
  productSubKindList: MbsBscHttpGetManyProductSubKindRspItemMdl[];
}

/** 管理者後台-基本-HTTP-取得多筆產品子分類-回應項目模型 */
export interface MbsBscHttpGetManyProductSubKindRspItemMdl {
  /** 產品子分類-ID */
  productSubKindID: number;
  /** 產品子分類-名稱 */
  productSubKindName: string;
  /** 產品子分類-是否啟用 */
  productSubKindIsEnable: boolean;
}
//#endregion

//#region 取得多筆產品規格
/** 管理者後台-基本-HTTP-取得多筆產品規格-請求模型 */
export interface MbsBscHttpGetManyProductSpecificationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 產品ID */
  managerProductID: number;
  /** 產品規格-名稱 */
  productSpecificationName: string | null;
  /** 是否啟用 */
  productSpecificationIsEnable: boolean;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-HTTP-取得多筆產品規格-回應模型 */
export interface MbsBscHttpGetManyProductSpecificationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 產品規格-列表 */
  productSpecificationList: MbsBscHttpGetManyProductSpecificationRspItemMdl[];
}

/** 管理者後台-基本-HTTP-取得多筆產品規格-回應項目模型 */
export interface MbsBscHttpGetManyProductSpecificationRspItemMdl {
  /** 管理者-產品規格-ID */
  ManagerProductSpecificationID: number;
  /** 管理者-產品規格-名稱 */
  ManagerProductSpecificationName: string;
  /** 管理者-產品規格-產品售價 */
  ManagerProductSpecificationSellPrice: number;
  /** 管理者-產品規格-產品成本 */
  ManagerProductSpecificationCostPrice: number;
  /** 管理者-產品規格-是否啟用 */
  ManagerProductSpecificationIsEnable: boolean;
}
//#endregion

//#region 取得多筆窗口開發評等原因
/** 管理者後台-基本-HTTP-取得多筆窗口開發評等原因-請求模型 */
export interface MbsBscHttpGetManyContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 窗口開發評等原因名稱(模糊查詢) */
  managerContacterRatingReasonName: string | null;
  /** 窗口開發評等原因-是否啟用 */
  managerContacterRatingReasonStatus: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-HTTP-取得多筆窗口開發評等原因-回應模型 */
export interface MbsBscHttpGetManyContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 窗口開發評等原因-列表 */
  managerContacterRatingReasonList: MbsBscHttpGetManyContacterRatingReasonRspItemMdl[];
}

/** 管理者後台-基本-HTTP-取得多筆窗口開發評等原因-回應項目模型 */
export interface MbsBscHttpGetManyContacterRatingReasonRspItemMdl {
  /** 窗口開發評等原因-ID */
  managerContacterRatingReasonID: number;
  /** 窗口開發評等原因-名稱 */
  managerContacterRatingReasonName: string;
  /** 窗口開發評等原因-狀態 */
  managerContacterRatingReasonStatus: boolean;
}

//#endregion

//#region 取得多筆公司主分類
/** 管理者後台-基本-Http-取得多筆公司主分類-請求模型 */
export interface MbsBscHttpGetManyCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司主分類名稱*/
  managerProductMainKindName: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-取得多筆公司主分類-回應模型 */
export interface MbsBscHttpGetManyCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-公司主分類-列表 */
  managerCompanyMainKindList: MbsBscHttpGetManyCompanyMainKindRspItemMdl[];
}

/** 管理者後台-基本-Http-取得多筆公司主分類-回應項目模型 */
export interface MbsBscHttpGetManyCompanyMainKindRspItemMdl {
  /** 管理者-公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者-公司主分類-名稱 */
  managerCompanyMainKindName: string;
  /** 管理者-公司主分類-是否啟用 */
  managerCompanyMainKindIsEnable: boolean;
}

//#endregion

//#region 取得多筆公司子分類
/** 管理者後台-基本-HTTP-取得多筆公司子分類-請求模型 */
export interface MbsBscHttpGetManyCompanySubKindReqMdl {
  /** 登入令牌 */
  employeeLoginToken: string;
  /** 公司主分類ID */
  companyMainKindID: number | null;
  /** 公司子分類名稱 */
  companySubKindName: string | null;
  /** 公司子分類是否啟用 */
  companySubKindIsEnable: boolean | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-HTTP-取得多筆公司子分類-回應模型 */
export interface MbsBscHttpGetManyCompanySubKindRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 公司子分類列表 */
  companySubKindList: MbsBscHttpGetManyCompanySubKindRspItemMdl[];
}

/** 管理者後台-基本-HTTP-取得多筆公司子分類-回應項目模型 */
export interface MbsBscHttpGetManyCompanySubKindRspItemMdl {
  /** 公司子分類ID */
  companySubKindID: number;
  /** 公司子分類名稱 */
  companySubKindName: string;
  /** 公司子分類是否啟用 */
  companySubKindIsEnable: boolean;
}

//#endregion

//#region 取得多筆窗口
/** 管理者後台-基本-HTTP-取得多筆窗口-請求模型 */
export interface MbsBscHttpGetManyContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口-名稱 */
  managerContacterName: string | null;
  /** 管理者-窗口-公司ID */
  managerContacterCompanyID: number;
  /** 管理者-窗口-電子郵件 */
  managerContacterEmail: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-HTTP-取得多筆窗口-回應模型 */
export interface MbsBscHttpGetManyContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口-列表 */
  managerContacterList: MbsBscHttpGetManyContacterRspItemMdl[];
}

/** 管理者後台-基本-HTTP-取得多筆窗口-回應項目模型 */
export interface MbsBscHttpGetManyContacterRspItemMdl {
  /** 管理者-窗口-ID */
  managerContacterID: number;
  /** 管理者-窗口-名稱 */
  managerContacterName: string;
  /** 管理者-窗口-電子郵件 */
  managerContacterEmail: string;
}
//#endregion

//#region 取得管理者窗口(用在名單)
/** 管理者後台-系統-名單窗口-Http-取得管理者窗口-請求模型 */
export interface MbsSysCttHttpGetManagerContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口-ID */
  managerContacterID: number;
}

/** 管理者後台-系統-名單窗口-Http-取得管理者窗口-回應項目模型 */
export interface MbsSysCttHttpGetManagerContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口-ID */
  managerContacterID: number;
  /** 管理者-公司-ID */
  managerCompanyID: number;
  /** 管理者-窗口-姓名 */
  managerContacterName: string | null;
  /** 管理者-窗口-Email */
  managerContacterEmail: string;
  /** 管理者-窗口-電話 */
  managerContacterPhone: string | null;
  /** 管理者-窗口-部門 */
  managerContacterDepartment: string | null;
  /** 管理者-窗口-職稱 */
  managerContacterJobTitle: string | null;
  /** 管理者-窗口-電話(市話) */
  managerContacterTelephone: string | null;
  /** 原子-管理者窗口狀態 */
  atomManagerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 管理者-窗口-是否同意問卷 */
  managerContacterIsAgreeSurvey: boolean;
  /** 原子-窗口評等類型 */
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum;
  /**管理者-窗口-建立者員工ID */
  managerContacterEmployeeID: number;
  /** 管理者-窗口-備註  */
  managerContacterRemark: string | null;
}
//#endregion

//#region 取得多筆管理者公司
/** 管理者後台-基本-HTTP-取得多筆管理者公司-請求模型 */
export interface MbsBscHttpGetManyCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司-名稱 */
  companyName: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-HTTP-取得多筆管理者公司-回應模型 */
export interface MbsBscHttpGetManyCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者公司-列表 */
  companyList: MbsBscHttpGetManyCompanyRspItemMdl[];
}

/** 管理者後台-基本-HTTP-取得多筆管理者公司-回應項目模型 */
export interface MbsBscHttpGetManyCompanyRspItemMdl {
  /** 管理者公司-ID */
  companyID: number;
  /** 管理者公司-名稱 */
  companyName: string;
}

//#endregion

//#region 取得多筆公司名稱從[商機原始]
/** 管理者後台-基本-Http-取得多筆公司名稱從[商機原始]-請求模型 */
export interface MbsBscHttpGetManyCompanyNameFromPipelineOriginalReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司-名稱 */
  managerCompanyName: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-取得多筆公司名稱從[商機原始]-回應模型 */
export interface MbsBscHttpGetManyCompanyNameFromPipelineOriginalRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者公司名稱-列表 */
  managerCompanyNameList: string[];
}
//#endregion

//#region 取得多筆產品
/** 管理者後台-基本-Http-產品-取得多筆-請求模型 */
export interface MbsBscHttpGetManyProductReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-產品-主類別ID */
  managerProductMainKindID: number | null;
  /** 管理者-產品-子類別ID */
  managerProductSubKindID: number | null;
  /** 管理者-產品-名稱 */
  managerProductName: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-產品-取得多筆-回應模型 */
export interface MbsBscHttpGetManyProductRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 產品-列表 */
  productList: MbsBscHttpGetManyProductRspItemMdl[];
}

/** 管理者後台-基本-Http-產品-取得多筆-回應項目模型 */
export interface MbsBscHttpGetManyProductRspItemMdl {
  /** 管理者-產品-ID */
  managerProductID: number;
  /** 管理者-產品-名稱 */
  managerProductName: string;
  /** 管理者-產品-是否啟用 */
  managerProductIsEnable: boolean;
  /** 管理者-產品-主類別ID */
  managerProductMainKindID: number;
  /** 管理者-產品-主類別名稱 */
  managerProductMainKindName: string;
  /** 管理者-產品-子類別ID */
  managerProductSubKindID: number;
  /** 管理者-產品-子類別名稱 */
  managerProductSubKindName: string;
  /** 管理者-產品-主分類-業務毛利率 */
  managerProductMainKindCommissionRate: number;
}
//#endregion

//#region 取得管理者公司
/** 管理者後台-基本-Http-取得管理者公司-請求模型 */
export interface MbsBscHttpGetManagerCompanyReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司-ID */
  managerCompanyID: number;
}

/** 管理者後台-基本-Http-取得管理者公司-回應模型 */
export interface MbsBscHttpGetManagerCompanyRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者公司-ID */
  managerCompanyID: number;
  /** 統一編號 */
  managerCompanyUnifiedNumber: string | null;
  /** 管理者公司名稱 */
  managerCompanyName: string | null;
  /** 管理者公司狀態 */
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
  /** 管理者公司主分類-ID */
  managerCompanyMainKindID: number;
  /** 管理者公司主分類-名稱 */
  managerCompanyMainKindName: string;
  /** 管理者公司子分類-ID */
  managerCompanySubKindID: number;
  /** 管理者公司子分類-名稱 */
  managerCompanySubKindName: string;
  /** 客戶等級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  /** 安全等級 */
  atomSecurityGrade: DbAtomSecurityGradeEnum | null;
  /** 員工範圍 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
}
//#endregion

//#region 取得多筆公司營業地點
/** 管理者後台-基本-Http-取得多筆公司營業地點-請求模型 */
export interface MbsBscHttpGetManyCompanyLocationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司ID */
  managerCompanyID: number;
  /** 管理者公司營業地點名稱（模糊搜尋） */
  managerCompanyLocationName: string | null;
  /** 頁面索引 */
  pageIndex: number;
}

/** 管理者後台-基本-Http-取得多筆公司營業地點-回應模型 */
export interface MbsBscHttpGetManyCompanyLocationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者公司營業地點清單 */
  managerCompanyLocationList: MbsBscHttpGetManyCompanyLocationRspItemMdl[];
}

/** 管理者後台-基本-Http-取得多筆公司營業地點-回應項目模型 */
export interface MbsBscHttpGetManyCompanyLocationRspItemMdl {
  /** 管理者公司營業地點ID */
  managerCompanyLocationID: number;
  /** 管理者公司營業地點名稱 */
  managerCompanyLocationName: string;
  /** 管理者公司營業地點是否已刪除 */
  managerCompanyLocationIsDeleted: boolean;
}
//#endregion

//#region 取得公司營業地點
/** 管理者後台-基本-HTTP-取得公司營業地點-請求模型 */
export interface MbsBscHttpGetManagerCompanyLocationReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者公司營業地點-ID */
  managerCompanyLocationID: number;
  /** 是否已刪除 */
  managerCompanyLocationIsDeleted: boolean | null;
}

/** 管理者後台-基本-HTTP-取得公司營業地點-回應模型 */
export interface MbsBscHttpGetManagerCompanyLocationRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者公司營業地點-ID */
  managerCompanyLocationID: number;
  /** 管理者公司-ID */
  managerCompanyID: number;
  /** 管理者公司營業地點-名稱 */
  managerCompanyLocationName: string | null;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum;
  /** 地址 */
  managerCompanyLocationAddress: string | null;
  /** 電話 */
  managerCompanyLocationTelephone: string | null;
  /** 原子-管理者公司-狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum;
}
//#endregion
