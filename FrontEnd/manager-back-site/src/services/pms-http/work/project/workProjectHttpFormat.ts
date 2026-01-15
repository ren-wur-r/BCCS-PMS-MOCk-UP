import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//#region 管理者後台-工作-專案-Http-儀表板
//-------------------------------------------
/** 管理者後台-工作-專案-Http-取得儀表板-請求模型 */
export interface MbsWrkPrjHttpGetDashboardReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
}

/** 管理者後台-工作-專案-Http-取得儀表板-回應模型-項目 */
export interface MbsWrkPrjHttpGetDashboardRspItemMdl {
  /** 員工專案 ID */
  employeeProjectID: number;
  /** 公司名稱 */
  managerCompanyName: string;
  /** 專案名稱 */
  employeeProjectName: string;
  /** 專案開始時間 */
  employeeProjectStartTime: string | null;
  /** 專案結束時間 */
  employeeProjectEndTime: string | null;
}

/** 管理者後台-工作-專案-Http-取得儀表板-回應模型 */
export interface MbsWrkPrjHttpGetDashboardRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 延遲專案列表 */
  delayedEmployeeProjectList: MbsWrkPrjHttpGetDashboardRspItemMdl[];
  /** 風險專案列表 */
  atRiskEmployeeProjectList: MbsWrkPrjHttpGetDashboardRspItemMdl[];
  /** 未指派專案列表 */
  notAssignedEmployeeProjectList: MbsWrkPrjHttpGetDashboardRspItemMdl[];
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-Http-專案
//-------------------------------------------
/** 管理者後台-工作-專案-Http-取得多筆專案-請求模型 */
export interface MbsWrkPrjHttpGetManyProjectReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案狀態（列舉） */
  atomEmployeeProjectStatus: number | null;
  /** 專案名稱 */
  employeeProjectName: string | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** 管理者後台-工作-專案-Http-取得多筆專案-回應項目模型 */
export interface MbsWrkPrjHttpGetManyProjectRspItemMdl {
  /** 專案 ID */
  employeeProjectID: number;
  /** 專案狀態 */
  atomEmployeeProjectStatus: number;
  /** 專案名稱 */
  employeeProjectName: string;
  /** 公司名稱 */
  managerCompanyName: string;
  /** 專案開始時間 */
  employeeProjectStartTime: string;
  /** 專案結束時間 */
  employeeProjectEndTime: string;
}

/** 管理者後台-工作-專案-Http-取得多筆專案-回應模型 */
export interface MbsWrkPrjHttpGetManyProjectRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 專案列表 */
  employeeProjectList: MbsWrkPrjHttpGetManyProjectRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}
//--------------------------------------------
/** 管理者後台-工作-專案-Http-取得單筆專案-請求模型 */
export interface MbsWrkPrjHttpGetProjectReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案 ID */
  employeeProjectID: number;
}

/** 管理者後台-工作-專案-Http-取得單筆專案-回應項目模型 */
export interface MbsWrkPrjHttpGetProjectRspItemMdl {
  /** 專案成員 ID */
  employeeProjectMemberID: number;
  /** 專案成員角色 */
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum;
  /** 區域名稱 */
  managerRegionName: string;
  /** 部門名稱 */
  managerDepartmentName: string;
  /** 員工姓名 */
  employeeName: string;
  /** 員工 ID */
  employeeID: number;
}

/** 管理者後台-工作-專案-Http-取得單筆專案-回應模型 */
export interface MbsWrkPrjHttpGetProjectRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 專案代號 */
  employeeProjectCode: string;
  /** 員工專案合約建立時間 */
  employeeProjectContractCreateTime: string;
  /** 員工專案合約網址 */
  employeeProjectContractUrl: string | null;
  /** 員工專案工作計劃書建立時間 */
  employeeProjectWorkCreateTime: string;
  /** 員工專案工作計劃書網址 */
  employeeProjectWorkUrl: string | null;
  /** 專案成員列表 */
  employeeProjectMemberList: MbsWrkPrjHttpGetProjectRspItemMdl[];
  /** 專案名稱 */
  employeeProjectName: string;
  /** 起始時間 */
  employeeProjectStartTime: string;
  /** 迄止時間 */
  employeeProjectEndTime: string;
  /** 管理者-公司-名稱 */
  managerCompanyName: string;
}
//-------------------------------------------
/** 管理者後台-工作-專案-Http-新增專案-請求模型 */
export interface MbsWrkPrjHttpAddProjectReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 公司 ID */
  managerCompanyID: number;
  /** 專案名稱 */
  employeeProjectName: string;
  /** 專案代碼 */
  employeeProjectCode: string;
  /** 專案開始時間 (ISO 字串) */
  employeeProjectStartTime: string;
  /** 專案結束時間 (ISO 字串) */
  employeeProjectEndTime: string | null;
  /** 合約檔案網址 */
  employeeProjectContractUrl: string | null;
  /** 工作表網址 */
  employeeProjectWorkUrl: string | null;
  /** 專案成員列表 */
  employeeProjectMemberList: MbsWrkPrjHttpAddProjectReqItemMdl[];
}

/** 管理者後台-工作-專案-Http-新增專案-請求項目模型 */
export interface MbsWrkPrjHttpAddProjectReqItemMdl {
  /** 成員角色 */
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
  /** 成員員工 ID */
  employeeID: number;
}

/** 管理者後台-工作-專案-Http-新增專案-回應模型 */
export interface MbsWrkPrjHttpAddProjectRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//-------------------------------------------
/** 管理者後台-工作-專案-Http-更新專案-請求模型 */
export interface MbsWrkPrjHttpUpdateProjectReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
   /** 專案 ID */
  employeeProjectID: number;
  /** 專案代碼 */
  employeeProjectCode: string;
 /** 專案名稱 */
  employeeProjectName: string;
}

/** 管理者後台-工作-專案-Http-更新專案-回應模型 */
export interface MbsWrkPrjHttpUpdateProjectRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//#endregion

//#region 管理者後台-工作-專案-Http-合約
// -------------------------------------------
/** 管理者後台-工作-專案-Http-新增專案合約-請求模型 */
export interface MbsWrkPrjHttpAddContractReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案 ID */
  employeeProjectID: number;
  /** 合約檔案網址 */
  employeeProjectContractUrl: string | null;
}

/** 管理者後台-工作-專案-Http-新增專案合約-回應模型 */
export interface MbsWrkPrjHttpAddContractRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
// -------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-Http-工作計劃書
//-------------------------------------------
/** 管理者後台-工作-專案-Http-新增工作計劃書-請求模型 */
export interface MbsWrkPrjHttpAddWorkReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案 ID */
  employeeProjectID: number;
  /** 工作表網址 */
  employeeProjectWorkUrl: string;
}
/** 管理者後台-工作-專案-Http-新增工作計劃書-回應模型 */
export interface MbsWrkPrjHttpAddWorkRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-Http-成員
//-------------------------------------------
/** 管理者後台-工作-專案-Http-新增專案成員-請求模型 */
export interface MbsWrkPrjHttpAddMemberReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案 ID */
  employeeProjectID: number;
  /** 加入的員工 ID */
  employeeID: number;
  /** 成員角色 */
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
}
/** 管理者後台-工作-專案-Http-新增專案成員-回應模型 */
export interface MbsWrkPrjHttpAddMemberRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//-------------------------------------------
/** 管理者後台-工作-專案-Http-移除專案成員-請求模型 */
export interface MbsWrkPrjHttpRemoveMemberReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案成員 ID（要刪除的對象） */
  employeeProjectMemberID: number;
}
/** 管理者後台-工作-專案-Http-移除專案成員-回應模型 */
export interface MbsWrkPrjHttpRemoveMemberRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//-------------------------------------------
/** 管理者後台-工作-專案-Http-取得多筆成員角色-請求模型 */
export interface MbsWrkPrjHttpGetManyMemberRoleReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案 ID */
  employeeProjectID: number;
}

/** 管理者後台-工作-專案-Http-取得多筆成員角色-回應模型 */
export interface MbsWrkPrjHttpGetManyMemberRoleRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 成員角色列表 */
  employeeProjectMemberList: MbsWrkPrjHttpGetManyMemberRoleRspItemMdl[];
}

/** 管理者後台-工作-專案-Http-取得多筆成員角色-回應模型 */
export interface MbsWrkPrjHttpGetManyMemberRoleRspItemMdl {
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum;
}
// -------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-Http-里程碑
//-------------------------------------------
/** 管理後台-工作-專案-Http-取得多筆專案里程碑-請求模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案 ID */
  employeeProjectID: number;
}

/** 管理後台-工作-專案-Http-取得多筆專案里程碑-回應模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 里程碑列表 */
  employeeProjectStoneList: MbsWrkPrjHttpGetManyProjectStoneRspItemStoneMdl[];
}

/** 管理後台-工作-專案-Http-取得多筆專案里程碑-回應項目里程碑模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneRspItemStoneMdl {
  /** 里程碑 ID */
  employeeProjectStoneID: number;
  /** 里程碑名稱 */
  employeeProjectStoneName: string;
  /** 里程碑開始時間 */
  employeeProjectStoneStartTime: string;
  /** 里程碑結束時間 */
  employeeProjectStoneEndTime: string;
  /** 里程碑前 N 日通知 */
  employeeProjectStonePreNotifyDay: number;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項列表 */
  employeeProjectStoneTaskList: MbsWrkPrjHttpGetManyProjectStoneRspItemTaskMdl[];
}

/** 管理後台-工作-專案-Http-取得多筆專案里程碑-回應項目工項模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneRspItemTaskMdl {
  /** 里程碑工項 ID */
  employeeProjectStoneTaskID: number;
  /** 里程碑工項名稱 */
  employeeProjectStoneTaskName: string;
  /** 里程碑工項開始時間 */
  employeeProjectStoneTaskStartTime: string;
  /** 里程碑工項結束時間 */
  employeeProjectStoneTaskEndTime: string;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項執行者筆數 */
  employeeProjectStoneTaskExecutorCount: number;
}
//-----------------------------------------------------------------
/** 管理後台-工作-專案-Http-取得專案里程碑-請求模型 */
export interface MbsWrkPrjHttpGetProjectStoneReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-專案里程碑-ID */
  employeeProjectStoneID: number;
}
/** 管理後台-工作-專案-Http-取得專案里程碑-回應模型 */
export interface MbsWrkPrjHttpGetProjectStoneRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 里程碑名稱 */
  employeeProjectStoneName: string;
  /** 里程碑開始時間 */
  employeeProjectStoneStartTime: string;
  /** 里程碑結束時間 */
  employeeProjectStoneEndTime: string;
  /** 前 N 日通知 */
  employeeProjectStonePreNotifyDay: number;
  /** 工項列表 */
  employeeProjectStoneTaskList: MbsWrkPrjHttpGetProjectStoneRspItemTaskMdl[];
}

/** 管理後台-工作-專案-Http-取得專案里程碑-回應項目工項模型 */
export interface MbsWrkPrjHttpGetProjectStoneRspItemTaskMdl {
  /** 里程碑工項 ID */
  employeeProjectStoneTaskID: number;
  /** 里程碑工項名稱 */
  employeeProjectStoneTaskName: string;
  /** 里程碑工項開始時間 */
  employeeProjectStoneTaskStartTime: string;
  /** 里程碑工項結束時間 */
  employeeProjectStoneTaskEndTime: string;
  /** 工項時數 */
  employeeProjectStoneTaskWorkHour: number;
  /** 里程碑工項執行者列表 */
  employeeProjectStoneTaskExecutorList: MbsWrkPrjHttpGetProjectStoneRspItemExecutorMdl[];
}

/** 管理後台-工作-專案-Http-取得專案里程碑-回應項目工項執行者模型 */
export interface MbsWrkPrjHttpGetProjectStoneRspItemExecutorMdl {
  /** 執行者 ID */
  employeeProjectStoneTaskExecutorEmployeeID: number;
  /** 執行者名稱 */
  employeeProjectStoneTaskExecutorEmployeeName: string;
}
//-------------------------------------------
/** 管理後台-工作-專案-Http-新增專案里程碑-請求模型 */
export interface MbsWrkPrjHttpAddProjectStoneReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案-ID */
  employeeProjectID: number;
  /** 員工-專案里程碑-名稱 */
  employeeProjectStoneName: string;
  /** 員工-專案里程碑-前N日通知 */
  employeeProjectStonePreNotifyDay: number;
  /** 員工-專案里程碑-開始時間 */
  employeeProjectStoneStartTime: string;
  /** 員工-專案里程碑-結束時間 */
  employeeProjectStoneEndTime: string;
  /** 員工-專案里程碑工項-列表  */
  employeeProjectStoneTaskList: MbsWrkPrjHttpAddProjectStoneTaskReqItemMdl[] | null;
}

/** 管理者後台-工作-專案-Http-新增里程碑-請求項目執行者模型 */
export interface MbsWrkPrjHttpAddProjectStoneTaskExecutorReqMdl {
  /** 員工-ID  */
  employeeID: number;
}

/** 管理後台-工作-專案-Http-新增專案里程碑-請求項目工項模型 */
export interface MbsWrkPrjHttpAddProjectStoneTaskReqItemMdl {
  /** 員工-專案里程碑工項-名稱 */
  employeeProjectStoneTaskName: string;
  /** 員工-專案里程碑工項-開始時間 */
  employeeProjectStoneTaskStartTime: string;
  /** 員工-專案里程碑工項-結束時間 */
  employeeProjectStoneTaskEndTime: string;
  /**員工-專案里程碑工項-工時 */
  employeeProjectStoneTaskWorkHour: number;
  /** 員工-專案里程碑工項-備註 */
  employeeProjectStoneTaskRemark: string | null;
  /** 員工-專案里程碑工項執行者-列表 */
  employeeProjectStoneTaskExecutorList: MbsWrkPrjHttpAddProjectStoneTaskExecutorReqMdl[] | null;
}

/** 管理後台-工作-專案-Http-新增專案里程碑-回應模型 */
export interface MbsWrkPrjHttpAddProjectStoneRspMdl {
  errorCode: MbsErrorCodeEnum;
}

/** 管理後台-工作-專案-Http-更新專案里程碑-請求模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 里程碑 ID */
  employeeProjectStoneID: number;
  /** 里程碑名稱 */
  employeeProjectStoneName: string;
  /** 里程碑開始時間 */
  employeeProjectStoneStartTime: string | null;
  /** 里程碑結束時間 */
  employeeProjectStoneEndTime: string | null;
  /** 里程碑前 N 日通知 */
  employeeProjectStonePreNotifyDay: number | null;
  /** 里程碑工項列表 */
  employeeProjectStoneTaskList: MbsWrkPrjHttpUpdateProjectStoneTaskReqItemMdl[] | null;
}

/** 管理後台-工作-專案-Http-更新專案里程碑-請求項目工項模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneTaskReqItemMdl {
  /** 里程碑工項 ID */
  employeeProjectStoneTaskID: number;
  /** 里程碑工項名稱 */
  employeeProjectStoneTaskName: string;
  /** 里程碑工項開始時間 */
  employeeProjectStoneTaskStartTime: string;
  /** 里程碑工項結束時間 */
  employeeProjectStoneTaskEndTime: string;
  /** 里程碑工項工時 */
  employeeProjectStoneTaskWorkHour: number;
  /** 里程碑工項執行者列表 */
  employeeProjectStoneTaskExecutorList: MbsWrkPrjHttpUpdateProjectStoneTaskExecutorReqMdl[];
}

/** 管理後台-工作-專案-Http-更新專案里程碑-請求項目執行者模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneTaskExecutorReqMdl {
  /** 員工 ID */
  employeeID: number;
}

/** 管理後台-工作-專案-Http-更新專案里程碑-回應模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

/** 管理後台-工作-專案-Http-移除專案里程碑-請求模型 */
export interface MbsWrkPrjHttpRemoveProjectStoneReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 里程碑 ID */
  employeeProjectStoneID: number;
}

/** 管理後台-工作-專案-Http-移除專案里程碑-回應模型 */
export interface MbsWrkPrjHttpRemoveProjectStoneRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//-------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-Http-工項
//-------------------------------------------
/** 管理後台-工作-專案-Http-取得多筆專案里程碑工項-請求模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneTaskReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案 ID */
  employeeProjectID: number;
}

/** 管理後台-工作-專案-Http-取得多筆專案里程碑工項-回應模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneTaskRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 里程碑列表 */
  employeeProjectStoneList: MbsWrkPrjHttpGetManyProjectStoneTaskRspStoneItemMdl[];
}

/** 管理後台-工作-專案-Http-取得多筆專案里程碑工項-回應項目里程碑模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneTaskRspStoneItemMdl {
  /** 里程碑 ID */
  employeeProjectStoneID: number;
  /** 里程碑名稱 */
  employeeProjectStoneName: string;
  /** 里程碑前 N 日通知 */
  employeeProjectStonePreNotifyDay: number;
  /** 里程碑開始時間 */
  employeeProjectStoneStartTime: string;
  /** 里程碑結束時間 */
  employeeProjectStoneEndTime: string;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項列表 */
  employeeProjectStoneTaskList: MbsWrkPrjHttpGetManyProjectStoneTaskRspTaskItemMdl[];
}

/** 管理後台-工作-專案-Http-取得多筆專案里程碑工項-回應項目工項模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneTaskRspTaskItemMdl {
  /** 工項 ID */
  employeeProjectStoneTaskID: number;
  /** 工項名稱 */
  employeeProjectStoneTaskName: string;
  /** 工項開始時間 */
  employeeProjectStoneTaskStartTime: string;
  /** 工項結束時間 */
  employeeProjectStoneTaskEndTime: string;
  /** 工項工時 */
  employeeProjectStoneTaskWorkHour: number;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 工項執行者筆數 */
  employeeProjectStoneTaskExecutorCount: number;
  /** 工項清單列表 */
  employeeProjectStoneTaskBucketList: MbsWrkPrjHttpGetManyProjectStoneTaskRspBucketItemMdl[];
}

/** 管理後台-工作-專案-Http-取得多筆專案里程碑工項-回應項目清單模型 */
export interface MbsWrkPrjHttpGetManyProjectStoneTaskRspBucketItemMdl {
  /** 清單 ID */
  employeeProjectStoneTaskBucketID: number;
  /** 清單名稱 */
  employeeProjectStoneTaskBucketName: string;
  /** 是否完成 */
  employeeProjectStoneTaskBucketIsFinish: boolean;
}

/** 管理後台-工作-專案-Http-取得專案里程碑工項-請求模型 */
export interface MbsWrkPrjHttpGetProjectStoneTaskReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 里程碑工項 ID */
  employeeProjectStoneTaskID: number;
}

/** 管理後台-工作-專案-Http-取得專案里程碑工項-回應模型 */
export interface MbsWrkPrjHttpGetProjectStoneTaskRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 里程碑 ID */
  employeeProjectStoneID: number;
  /** 里程碑名稱 */
  employeeProjectStoneName: string;
  /** 里程碑開始時間 */
  employeeProjectStoneStartTime: string;
  /** 里程碑結束時間 */
  employeeProjectStoneEndTime: string;
  // /** 里程碑工項 ID */
  // employeeProjectStoneTaskID: number;
  /** 里程碑工項名稱 */
  employeeProjectStoneTaskName: string;
  /** 里程碑工項開始時間 */
  employeeProjectStoneTaskStartTime: string;
  /** 里程碑工項結束時間 */
  employeeProjectStoneTaskEndTime: string;
  /** 里程碑工項工時 */
  employeeProjectStoneTaskWorkHour: number;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項備註 */
  employeeProjectStoneTaskRemark: string | null;
  /** 里程碑工項執行者列表 */
  employeeProjectStoneTaskExecutorList: MbsWrkPrjHttpGetProjectStoneTaskRspExecutorItemMdl[];
  /** 里程碑工項清單列表 */
  employeeProjectStoneTaskBucketList: MbsWrkPrjHttpGetProjectStoneTaskRspBucketItemMdl[];
}

/** 管理後台-工作-專案-Http-取得專案里程碑工項-回應執行者項目模型 */
export interface MbsWrkPrjHttpGetProjectStoneTaskRspExecutorItemMdl {
  /** 執行者員工 ID */
  employeeProjectStoneTaskExecutorEmployeeID: number;
  /** 執行者員工名稱 */
  employeeProjectStoneTaskExecutorEmployeeName: string;
}

/** 管理後台-工作-專案-Http-取得專案里程碑工項-回應清單項目模型 */
export interface MbsWrkPrjHttpGetProjectStoneTaskRspBucketItemMdl {
  /** 清單 ID */
  employeeProjectStoneTaskBucketID: number;
  /** 清單名稱 */
  employeeProjectStoneTaskBucketName: string;
  /** 是否完成 */
  employeeProjectStoneTaskBucketIsFinish: boolean;
}

/** 管理後台-工作-專案-Http-更新專案里程碑工項-請求模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneTaskReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 里程碑工項 ID */
  employeeProjectStoneTaskID: number;
  /** 里程碑工項備註 */
  employeeProjectStoneTaskRemark: string | null;
  /** 里程碑工項執行者 ID 列表 */
  employeeProjectStoneTaskExecutorIdList: number[];
  /** 里程碑工項清單列表 */
  employeeProjectStoneTaskBucketList: MbsWrkPrjHttpUpdateProjectStoneTaskBucketItemReqMdl[];
}

/** 管理後台-工作-專案-Http-更新專案里程碑工項-請求項目清單模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneTaskBucketItemReqMdl {
  /** 清單 ID，>0 更新，-1 新增 */
  employeeProjectStoneTaskBucketID: number;
  /** 清單名稱 */
  employeeProjectStoneTaskBucketName: string;
  /** 是否完成 */
  employeeProjectStoneTaskBucketIsFinish: boolean;
}

/** 管理後台-工作-專案-Http-更新專案里程碑工項-回應模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneTaskRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

/** 管理後台-工作-專案-Http-更新專案里程碑工項清單-請求模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneTaskBucketReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 清單 ID */
  employeeProjectStoneTaskBucketID: number;
  /** 是否完成 */
  employeeProjectStoneTaskBucketIsFinish: boolean;
}

/** 管理後台-工作-專案-Http-更新專案里程碑工項清單-回應模型 */
export interface MbsWrkPrjHttpUpdateProjectStoneTaskBucketRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-Http-收支
//-------------------------------------------
/** 管理者後台-工作-專案-Http-取得專案支出-請求模型 */
export interface MbsWrkPrjHttpGetExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案支出 ID */
  employeeProjectExpenseID: number;
}

/** 管理者後台-工作-專案-Http-取得專案支出-回應模型 */
export interface MbsWrkPrjHttpGetExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工專案 ID */
  employeeProjectID: number;
  /** 員工專案支出名稱 */
  employeeProjectExpenseName: string;
  /** 員工專案支出預估金額 */
  employeeProjectExpensePredictAmount: number;
  /** 員工專案支出實際金額 */
  employeeProjectExpenseActualAmount: number | null;
  /** 員工專案支出發票號碼 */
  employeeProjectExpenseBillNumber: string | null;
  /** 員工專案支出發票日期 */
  employeeProjectExpenseBillTime: string | null;
  /** 員工專案支出備註 */
  employeeProjectExpenseRemark: string | null;
}

/** 管理者後台-工作-專案-Http-取得多筆專案支出-請求模型 */
export interface MbsWrkPrjHttpGetManyExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 專案 ID */
  employeeProjectID: number;
}

/** 管理者後台-工作-專案-Http-取得多筆專案支出-回應模型 */
export interface MbsWrkPrjHttpGetManyExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工專案支出列表 */
  employeeProjectExpenseList: MbsWrkPrjHttpGetManyExpenseRspItemMdl[];
  /** Eip專案支出列表 */
  eipProjectExpenseList: MbsWrkPrjHttpGetEipExpenseRspItemMdl[];
  /** Eip專案旅費列表 */
  eipProjectTravelExpenseList: MbsWrkPrjHttpGetEipTravelExpenseRspItemMdl[];
}

/** 管理者後台-工作-專案-Http-取得多筆專案支出-回應項目模型 */
export interface MbsWrkPrjHttpGetManyExpenseRspItemMdl {
  /** 員工專案支出ID */
  employeeProjectExpenseID: number;
  /** 員工專案支出名稱 */
  employeeProjectExpenseName: string;
  /** 員工專案支出預估金額 */
  employeeProjectExpensePredictAmount: number;
  /** 員工專案支出實際金額 */
  employeeProjectExpenseActualAmount: number | null;
  /** 員工專案支出發票號碼 */
  employeeProjectExpenseBillNumber: string | null;
  /** 員工專案支出發票日期 */
  employeeProjectExpenseBillTime: string | null;
  /** 員工專案支出備註 */
  employeeProjectExpenseRemark: string | null;
}

/** 管理者後台-工作-專案-Http-取得Eip專案支出-回應項目模型 */
export interface MbsWrkPrjHttpGetEipExpenseRspItemMdl {
  /** 簽核狀態 */
  projectExpenseApprovalStatus: string;
  /** 申請人員 */
  projectExpenseApplicant: string;
  /** 日期 */
  projectExpenseDate: string;
  /** 事由 */
  projectExpenseReason: string;
  /** 參與人員 */
  projectExpenseParticipants: string;
  /** 金額 */
  projectExpenseAmount: number;
}

/** 管理者後台-工作-專案-Http-取得專案旅費-回應項目模型 */
export interface MbsWrkPrjHttpGetEipTravelExpenseRspItemMdl {
  /** 簽核狀態 */
  projectTravelExpenseApprovalStatus: string;
  /** 申請人員 */
  projectTravelExpenseApplicant: string;
  /** 日期 */
  projectTravelExpenseTravelDate: string;
  /** 起訖地點 */
  projectTravelExpenseTravelRoute: string;
  /** 工作記要 */
  projectTravelExpenseWorkDescription: string;
  /** 交通工具 */
  projectTravelExpenseTransportationTool: string;
  /** 交通費金額 */
  projectTravelExpenseTransportationAmount: number;
  /** 里程 */
  projectTravelExpenseMileage: number;
  /** 膳宿費 */
  projectTravelExpenseAccommodationAmount: number;
  /** 特別費事由 */
  projectTravelExpenseSpecialExpenseReason: string;
  /** 特別費金額 */
  projectTravelExpenseSpecialExpenseAmount: number;
  /** 合計 */
  projectTravelExpenseTotalAmount: number;
}

/** 管理者後台-工作-專案-Http-新增專案支出-請求模型 */
export interface MbsWrkPrjHttpAddExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案 ID */
  employeeProjectID: number;
  /** 員工專案支出名稱 */
  employeeProjectExpenseName: string;
  /** 員工專案支出預估金額 */
  employeeProjectExpensePredictAmount: number;
  /** 員工專案支出實際金額 */
  employeeProjectExpenseActualAmount: number | null;
  /** 員工專案支出發票號碼 */
  employeeProjectExpenseBillNumber: string | null;
  /** 員工專案支出發票日期 */
  employeeProjectExpenseBillTime: string | null;
  /** 員工專案支出備註 */
  employeeProjectExpenseRemark: string | null;
}

/** 管理者後台-工作-專案-Http-新增專案支出-回應模型 */
export interface MbsWrkPrjHttpAddExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

/** 管理者後台-工作-專案-Http-更新專案支出-請求模型 */
export interface MbsWrkPrjHttpUpdateExpenseReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案支出 ID */
  employeeProjectExpenseID: number;
  /** 員工專案支出名稱 */
  employeeProjectExpenseName: string;
  /** 員工專案支出預估金額 */
  employeeProjectExpensePredictAmount: number;
  /** 員工專案支出實際金額 */
  employeeProjectExpenseActualAmount: number | null;
  /** 員工專案支出發票號碼 */
  employeeProjectExpenseBillNumber: string | null;
  /** 員工專案支出發票日期 */
  employeeProjectExpenseBillTime: string | null;
  /** 員工專案支出備註 */
  employeeProjectExpenseRemark: string | null;
}

/** 管理者後台-工作-專案-Http-更新專案支出-回應模型 */
export interface MbsWrkPrjHttpUpdateExpenseRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}