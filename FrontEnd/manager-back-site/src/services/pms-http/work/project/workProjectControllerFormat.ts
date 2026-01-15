import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//#region 管理者後台-工作-專案-控制器-儀表板
//-------------------------------------------
/** 管理者後台-工作-專案-控制器-取得儀表板-請求模型 */
export interface MbsWrkPrjCtlGetDashboardReqMdl {
  /** 員工登入令牌 */
  aa: string;
}

/** 管理者後台-工作-專案-控制器-取得儀表板-回應模型-列表項目 */
export interface MbsWrkPrjCtlGetDashboardRspItemMdl {
  /** 員工專案 ID */
  a: number;
  /** 公司名稱 */
  b: string;
  /** 專案名稱 */
  c: string;
  /** 專案開始時間 */
  d: string | null;
  /** 專案結束時間 */
  e: string | null;
}

/** 管理者後台-工作-專案-控制器-取得儀錶板-回應模型 */
export interface MbsWrkPrjCtlGetDashboardRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 延遲專案列表 */
  a: MbsWrkPrjCtlGetDashboardRspItemMdl[];
  /** 風險專案列表 */
  b: MbsWrkPrjCtlGetDashboardRspItemMdl[];
  /** 未指派專案列表 */
  c: MbsWrkPrjCtlGetDashboardRspItemMdl[];
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-控制器-專案
//-------------------------------------------
/** 管理者後台-工作-專案-控制器-取得多筆專案-請求模型 */
export interface MbsWrkPrjCtlGetManyProjectReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 專案狀態 */
  a: number | null;
  /** 專案名稱 */
  b: string | null;
  /** 頁面索引 */
  c: number;
  /** 頁面筆數 */
  d: number;
}

/** 管理者後台-工作-專案-控制器-取得多筆專案-回應項目模型 */
export interface MbsWrkPrjCtlGetManyProjectRspItemMdl {
  /** 專案 ID */
  a: number;
  /** 專案狀態 */
  b: number;
  /** 專案名稱 */
  c: string;
  /** 公司名稱 */
  d: string;
  /** 專案開始時間 */
  e: string;
  /** 專案結束時間 */
  f: string;
}

/** 管理者後台-工作-專案-控制器-取得多筆專案-回應模型 */
export interface MbsWrkPrjCtlGetManyProjectRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 專案列表 */
  a: MbsWrkPrjCtlGetManyProjectRspItemMdl[];
  /** 總筆數 */
  b: number;
}
//-------------------------------------------
/** 管理者後台-工作-專案-控制器-取得單筆專案-請求模型 */
export interface MbsWrkPrjCtlGetProjectReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 專案 ID */
  a: number;
}

/** 管理者後台-工作-專案-控制器-取得單筆專案-回應項目模型 */
export interface MbsWrkPrjCtlGetProjectRspItemMdl {
  /** 專案成員 ID */
  a: number;
  /** 專案成員角色 */
  b: number;
  /** 區域名稱 */
  c: string;
  /** 部門名稱 */
  d: string;
  /** 員工姓名 */
  e: string;
  /** 員工 ID */
  f: number;
}

/** 管理者後台-工作-專案-控制器-取得單筆專案-回應模型 */
export interface MbsWrkPrjCtlGetProjectRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 專案狀態 */
  a: DbAtomEmployeeProjectStatusEnum;
  /** 員工專案代碼 */
  b: string;
  /** 員工專案合約建立時間 */
  c: string;
  /** 員工專案合約網址 */
  d: string | null;
  /** 員工專案工作計劃書建立時間 */
  e: string;
  /** 員工專案工作計劃書網址 */
  f: string | null;
  /** 專案成員列表 */
  g: MbsWrkPrjCtlGetProjectRspItemMdl[];
  /** 專案名稱 */
  h: string;
  /** 起始時間 */
  i: string;
  /** 迄止時間 */
  j: string;
  /** 管理者-公司-名稱 */
  k: string;
}
//-------------------------------------------
/** 管理者後台-工作-專案-控制器-新增專案-請求模型 */
export interface MbsWrkPrjCtlAddProjectReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 公司 ID */
  a: number;
  /** 專案名稱 */
  b: string;
  /** 專案代碼 */
  c: string;
  /** 專案開始時間 */
  d: string;
  /** 專案結束時間 */
  e: string | null;
  /** 合約網址 */
  f: string | null;
  /** 工作計畫書網址 */
  g: string | null;
  /** 專案成員列表 */
  h: MbsWrkPrjCtlAddProjectReqItemMdl[];
}
/** 管理者後台-工作-專案-控制器-新增專案-請求項目模型 */
export interface MbsWrkPrjCtlAddProjectReqItemMdl {
  /** 專案角色 */
  a: DbEmployeeProjectMemberRoleEnum | null;
  /** 員工 ID */
  b: number;
}
/** 管理者後台-工作-專案-控制器-新增專案-回應模型 */
export interface MbsWrkPrjCtlAddProjectRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//-------------------------------------------
/** 管理者後台-工作-專案-控制器-更新專案-請求模型 */
export interface MbsWrkPrjCtlUpdateProjectReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 專案 ID */
  a: number;
  /** 專案代碼 */
  b: string;
  /** 專案名稱 */
  c: string;
}

/** 管理者後台-工作-專案-控制器-更新專案-回應模型 */
export interface MbsWrkPrjCtlUpdateProjectRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//#endregion

//#region 管理者後台-工作-專案-控制器-合約
// -------------------------------------------
/** 管理者後台-工作-專案-控制器-新增專案合約-請求模型 */
export interface MbsWrkPrjCtlAddContractReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 專案 ID */
  a: number;
  /** 合約檔案網址 */
  b: string | null;
}
/** 管理者後台-工作-專案-控制器-新增專案合約-回應模型 */
export interface MbsWrkPrjCtlAddContractRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
// -------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-控制器-工作計劃書
//-------------------------------------------
/** 管理者後台-工作-專案-控制器-新增工作計劃書-請求模型 */
export interface MbsWrkPrjCtlAddWorkReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 專案 ID */
  a: number;
  /** 工作表網址 */
  b: string;
}
/** 管理者後台-工作-專案-控制器-新增工作計劃書-回應模型 */
export interface MbsWrkPrjCtlAddWorkRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
// -------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-控制器-成員
//-------------------------------------------
/** 管理者後台-工作-專案-控制器-新增專案成員-請求模型 */
export interface MbsWrkPrjCtlAddMemberReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 專案 ID */
  a: number;
  /** 加入的員工 ID */
  b: number;
  /** 成員角色 */
  c: DbEmployeeProjectMemberRoleEnum | null;
}
/** 管理者後台-工作-專案-控制器-新增專案成員-回應模型 */
export interface MbsWrkPrjCtlAddMemberRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
// -------------------------------------------
/** 管理者後台-工作-專案-控制器-移除專案成員-請求模型 */
export interface MbsWrkPrjCtlRemoveMemberReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 專案成員 ID（要刪除的對象） */
  a: number;
}
/** 管理者後台-工作-專案-控制器-移除專案成員-回應模型 */
export interface MbsWrkPrjCtlRemoveMemberRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//-------------------------------------------
/** 管理者後台-工作-專案-控制器-取得多筆成員角色-請求模型 */
export interface MbsWrkPrjCtlGetManyMemberRoleReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 專案 ID */
  a: number;
}

/** 管理者後台-工作-專案-控制器-取得多筆成員角色-回應模型 */
export interface MbsWrkPrjCtlGetManyMemberRoleRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 成員角色列表 */
  a: MbsWrkPrjCtlGetManyMemberRoleRspItemMdl[];
}

/** 管理者後台-工作-專案-控制器-取得多筆成員角色-回應模型 */
export interface MbsWrkPrjCtlGetManyMemberRoleRspItemMdl {
  /** 成員角色 */
  a: DbEmployeeProjectMemberRoleEnum;
}
// -------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-控制器-里程碑
//-------------------------------------------
/** 管理後台-工作-專案-控制器-取得多筆專案里程碑-請求模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案 ID */
  a: number;
}

/** 管理後台-工作-專案-控制器-取得多筆專案里程碑-回應模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 里程碑列表 */
  a: MbsWrkPrjCtlGetManyProjectStoneRspItemStoneMdl[];
}

/** 管理後台-工作-專案-控制器-取得多筆專案里程碑-回應項目里程碑模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneRspItemStoneMdl {
  /** 里程碑 ID */
  a: number;
  /** 里程碑名稱 */
  b: string;
  /** 里程碑開始時間 */
  c: string;
  /** 里程碑結束時間 */
  d: string;
  /** 里程碑前 N 日通知 */
  e: number;
  /** 專案狀態 */
  f: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項列表 */
  g: MbsWrkPrjCtlGetManyProjectStoneRspItemTaskMdl[];
}

/** 管理後台-工作-專案-控制器-取得多筆專案里程碑-回應項目工項模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneRspItemTaskMdl {
  /** 里程碑工項 ID */
  a: number;
  /** 里程碑工項名稱 */
  b: string;
  /** 里程碑工項開始時間 */
  c: string;
  /** 里程碑工項結束時間 */
  d: string;
  /** 專案狀態 */
  e: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項執行者筆數 */
  f: number;
}
//--------------------------------------------------------------------
/** 管理後台-工作-專案-控制器-取得專案里程碑-請求模型 */
export interface MbsWrkPrjCtlGetProjectStoneReqMdl {
  /** 登入令牌 */
  aa: string;
  /** 里程碑 ID */
  a: number;
}

/** 管理後台-工作-專案-控制器-取得專案里程碑-回應模型 */
export interface MbsWrkPrjCtlGetProjectStoneRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 里程碑名稱 */
  a: string;
  /** 里程碑開始時間 */
  b: string;
  /** 里程碑結束時間 */
  c: string;
  /** 前 N 日通知 */
  d: number;
  /** 里程碑工項列表 */
  e: MbsWrkPrjCtlGetProjectStoneRspItemTaskMdl[];
}

/** 管理後台-工作-專案-控制器-取得專案里程碑-回應項目工項模型 */
export interface MbsWrkPrjCtlGetProjectStoneRspItemTaskMdl {
  /** 里程碑工項 ID */
  a: number;
  /** 里程碑工項名稱 */
  b: string;
  /** 里程碑工項開始時間 */
  c: string;
  /** 里程碑工項結束時間 */
  d: string;
  /** 工項時數 */
  e: number;
  /** 工項執行者列表 */
  f: MbsWrkPrjCtlGetProjectStoneRspItemExecutorMdl[];
}

/** 管理後台-工作-專案-控制器-取得專案里程碑-回應項目工項執行者模型 */
export interface MbsWrkPrjCtlGetProjectStoneRspItemExecutorMdl {
  /** 執行者 ID */
  a: number;
  /** 執行者名稱 */
  b: string;
}

//---------------------------------------------------------------------
/** 管理後台-工作-專案-控制器-新增專案里程碑-請求模型 */
export interface MbsWrkPrjCtlAddProjectStoneReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案-ID */
  a: number;
  /** 員工-專案里程碑-名稱 */
  b: string;
  /** 員工-專案里程碑-前N日通知 */
  c: number;
  /** 員工-專案里程碑-開始時間 */
  d: string;
  /** 員工-專案里程碑-結束時間 */
  e: string;
  /** 員工-專案里程碑工項-列表  */
  f: MbsWrkPrjCtlAddProjectStoneTaskReqItemMdl[] | null;
}

/** 管理者後台-工作-專案-控制器-新增里程碑-請求項目執行者模型 */
export interface MbsWrkPrjCtlAddProjectStoneTaskExecutorReqMdl {
  /** 員工-ID  */
  a: number;
}

/** 管理後台-工作-專案-控制器-新增專案里程碑-請求項目工項模型 */
export interface MbsWrkPrjCtlAddProjectStoneTaskReqItemMdl {
  /** 員工-專案里程碑工項-名稱 */
  a: string;
  /** 員工-專案里程碑工項-開始時間 */
  b: string;
  /** 員工-專案里程碑工項-結束時間 */
  c: string;
  /**員工-專案里程碑工項-工時 */
  d: number;
  /** 員工-專案里程碑工項-備註 */
  e: string | null;
  /** 員工-專案里程碑工項執行者-列表 */
  f: MbsWrkPrjCtlAddProjectStoneTaskExecutorReqMdl[];
}

/** 管理後台-工作-專案-控制器-新增專案里程碑-回應模型 */
export interface MbsWrkPrjCtlAddProjectStoneRspMdl {
  aa: MbsErrorCodeEnum;
}
//---------------------------------------------------------------------
/** 管理後台-工作-專案-控制器-更新專案里程碑-請求模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-專案里程碑-ID */
  a: number;
  /** 員工-專案里程碑-名稱 */
  b: string;
  /** 員工-專案里程碑-開始時間 */
  c: string | null;
  /** 員工-專案里程碑-結束時間 */
  d: string | null;
  /** 員工-專案里程碑-前N日通知 */
  e: number | null;
  /** 員工-專案里程碑工項-列表 */
  f: MbsWrkPrjCtlUpdateProjectStoneReqItemTaskMdl[] | null;
}

/** 管理後台-工作-專案-控制器-更新專案里程碑-回應模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

/** 管理後台-工作-專案-控制器-更新專案里程碑-請求項目工項模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneReqItemTaskMdl {
  /** 里程碑工項 ID */
  a: number;
  /** 里程碑工項名稱 */
  b: string;
  /** 里程碑工項開始時間 */
  c: string;
  /** 里程碑工項結束時間 */
  d: string;
  /** 里程碑工項工時 */
  e: number;
  /** 里程碑工項執行者列表 */
  f: MbsWrkPrjCtlUpdateProjectStoneReqItemExecutorMdl[];
}

/** 管理後台-工作-專案-控制器-更新專案里程碑-請求項目執行者模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneReqItemExecutorMdl {
  /** 員工 ID */
  a: number;
}
//---------------------------------------------------------------------
/** 管理後台-工作-專案-控制器-移除專案里程碑-請求模型 */
export interface MbsWrkPrjCtlRemoveProjectStoneReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 里程碑 ID */
  a: number;
}

/** 管理後台-工作-專案-控制器-移除專案里程碑-回應模型 */
export interface MbsWrkPrjCtlRemoveProjectStoneRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-控制器-工項
//-------------------------------------------
/** 管理後台-工作-專案-控制器-取得多筆專案里程碑工項-請求模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneTaskReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案 ID */
  a: number;
}

/** 管理後台-工作-專案-控制器-取得多筆專案里程碑工項-回應模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneTaskRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 里程碑列表 */
  a: MbsWrkPrjCtlGetManyProjectStoneTaskRspStoneItemMdl[];
}

/** 管理後台-工作-專案-控制器-取得多筆專案里程碑工項-回應項目里程碑模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneTaskRspStoneItemMdl {
  /** 里程碑 ID */
  a: number;
  /** 里程碑名稱 */
  b: string;
  /** 里程碑前 N 日通知 */
  c: number;
  /** 里程碑開始時間 */
  d: string;
  /** 里程碑結束時間 */
  e: string;
  /** 專案狀態 */
  f: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項列表 */
  g: MbsWrkPrjCtlGetManyProjectStoneTaskRspTaskItemMdl[];
}

/** 管理後台-工作-專案-控制器-取得多筆專案里程碑工項-回應項目工項模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneTaskRspTaskItemMdl {
  /** 里程碑工項 ID */
  a: number;
  /** 里程碑工項名稱 */
  b: string;
  /** 里程碑工項開始時間 */
  c: string;
  /** 里程碑工項結束時間 */
  d: string;
  /** 里程碑工項工時 */
  e: number;
  /** 專案狀態 */
  f: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項執行者筆數 */
  g: number;
  /** 里程碑工項清單 */
  h: MbsWrkPrjCtlGetManyProjectStoneTaskRspBucketItemMdl[];
}

/** 管理後台-工作-專案-控制器-取得多筆專案里程碑工項-回應項目清單模型 */
export interface MbsWrkPrjCtlGetManyProjectStoneTaskRspBucketItemMdl {
  /** 清單 ID */
  a: number;
  /** 清單名稱 */
  b: string;
  /** 是否完成 */
  c: boolean;
}
//-------------------------------------------
/** 管理後台-工作-專案-控制器-取得專案里程碑工項-請求模型 */
export interface MbsWrkPrjCtlGetProjectStoneTaskReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 里程碑工項 ID */
  a: number;
}

/** 管理後台-工作-專案-控制器-取得專案里程碑工項-回應模型 */
export interface MbsWrkPrjCtlGetProjectStoneTaskRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 里程碑 ID */
  a: number;
  /** 里程碑名稱 */
  b: string;
  /** 里程碑開始時間 */
  c: string;
  /** 里程碑結束時間 */
  d: string;
  /** 里程碑工項名稱 */
  e: string;
  /** 里程碑工項開始時間 */
  f: string;
  /** 里程碑工項結束時間 */
  g: string;
  /** 里程碑工項工時 */
  h: number;
  /** 專案狀態 */
  i: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項備註 */
  j: string | null;
  /** 里程碑工項執行者列表 */
  k: MbsWrkPrjCtlGetProjectStoneTaskRspExecutorItemMdl[];
  /** 里程碑工項清單列表 */
  l: MbsWrkPrjCtlGetProjectStoneTaskRspBucketItemMdl[];
}

/** 管理後台-工作-專案-控制器-取得專案里程碑工項-回應執行者項目模型 */
export interface MbsWrkPrjCtlGetProjectStoneTaskRspExecutorItemMdl {
  /** 執行者員工 ID */
  a: number;
  /** 執行者員工名稱 */
  b: string;
}

/** 管理後台-工作-專案-控制器-取得專案里程碑工項-回應清單項目模型 */
export interface MbsWrkPrjCtlGetProjectStoneTaskRspBucketItemMdl {
  /** 清單 ID */
  a: number;
  /** 清單名稱 */
  b: string;
  /** 是否完成 */
  c: boolean;
}
/** 管理後台-工作-專案-控制器-更新專案里程碑工項-請求模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneTaskReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 里程碑工項 ID */
  a: number;
  /** 里程碑工項備註 */
  b: string | null;
  /** 里程碑工項執行者 ID 列表 */
  c: number[];
  /** 里程碑工項清單列表 */
  d: MbsWrkPrjCtlUpdateProjectStoneTaskReqBucketItemMdl[];
}

/** 管理後台-工作-專案-控制器-更新專案里程碑工項-請求項目清單模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneTaskReqBucketItemMdl {
  /** 清單 ID，>0 更新，-1 新增 */
  a: number;
  /** 清單名稱 */
  b: string;
  /** 是否完成 */
  c: boolean;
}

/** 管理後台-工作-專案-控制器-更新專案里程碑工項-回應模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneTaskRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

/** 管理後台-工作-專案-控制器-更新專案里程碑工項清單-請求模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneTaskBucketReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 清單 ID */
  a: number;
  /** 是否完成 */
  b: boolean;
}

/** 管理後台-工作-專案-控制器-更新專案里程碑工項清單-回應模型 */
export interface MbsWrkPrjCtlUpdateProjectStoneTaskBucketRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-控制器-收支
//-------------------------------------------
/** 管理後台-工作-專案-控制器-取得專案支出-請求模型 */
export interface MbsWrkPrjCtlGetExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案支出 ID */
  a: number;
}

/** 管理後台-工作-專案-控制器-取得專案支出-回應模型 */
export interface MbsWrkPrjCtlGetExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工專案 ID */
  a: number;
  /** 員工專案支出名稱 */
  b: string;
  /** 員工專案支出預估金額 */
  c: number;
  /** 員工專案支出實際金額 */
  d: number | null;
  /** 員工專案支出發票號碼 */
  e: string | null;
  /** 員工專案支出發票日期 */
  f: string | null;
  /** 員工專案支出備註 */
  g: string | null;
}

/** 管理後台-工作-專案-控制器-取得多筆專案收支-請求模型 */
export interface MbsWrkPrjCtlGetManyExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案 ID */
  a: number;
}

/** 管理後台-工作-專案-控制器-取得多筆專案收支-回應模型 */
export interface MbsWrkPrjCtlGetManyExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工專案支出列表 */
  a: MbsWrkPrjCtlGetManyExpenseRspItemMdl[];
  /** Eip專案支出列表 */
  b: MbsWrkPrjCtlGetEipExpenseRspItemMdl[];
  /** Eip專案旅費列表 */
  c: MbsWrkPrjCtlGetEipTravelExpenseRspItemMdl[];
}

/** 管理後台-工作-專案-控制器-取得多筆專案支出-回應項目模型 */
export interface MbsWrkPrjCtlGetManyExpenseRspItemMdl {
  /** 員工專案支出ID */
  a: number;
  /** 員工專案支出名稱 */
  b: string;
  /** 員工專案支出預估金額 */
  c: number;
  /** 員工專案支出實際金額 */
  d: number;
  /** 員工專案支出發票號碼 */
  e: string;
  /** 員工專案支出發票日期 */
  f: string;
  /** 員工專案支出備註 */
  g: string;
}

/** 管理後台-工作-專案-控制器-取得Eip專案支出-回應項目模型 */
export interface MbsWrkPrjCtlGetEipExpenseRspItemMdl {
  /** 簽核狀態 */
  a: string;
  /** 申請人員 */
  b: string;
  /** 日期 */
  c: string;
  /** 事由 */
  d: string;
  /** 參與人員 */
  e: string;
  /** 金額 */
  f: number;
}

/** 管理後台-工作-專案-控制器-取得Eip專案旅費-回應項目模型 */
export interface MbsWrkPrjCtlGetEipTravelExpenseRspItemMdl {
  /** 簽核狀態 */
  a: string;
  /** 申請人員 */
  b: string;
  /** 日期 */
  c: string;
  /** 起訖地點 */
  d: string;
  /** 工作記要 */
  e: string;
  /** 交通工具 */
  f: string;
  /** 交通費金額 */
  g: number;
  /** 里程 */
  h: number;
  /** 膳宿費 */
  i: number;
  /** 特別費事由 */
  j: string;
  /** 特別費金額 */
  k: number;
  /** 合計 */
  l: number;
}

/** 管理後台-工作-專案-控制器-新增專案支出-請求模型 */
export interface MbsWrkPrjCtlAddExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案 ID */
  a: number;
  /** 員工專案支出名稱 */
  b: string;
  /** 員工專案支出預估金額 */
  c: number;
  /** 員工專案支出實際金額 */
  d: number | null;
  /** 員工專案支出發票號碼 */
  e: string | null;
  /** 員工專案支出發票日期 */
  f: string | null;
  /** 員工專案支出備註 */
  g: string | null;
}

/** 管理後台-工作-專案-控制器-新增專案支出-回應模型 */
export interface MbsWrkPrjCtlAddExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

/** 管理後台-工作-專案-控制器-更新專案支出-請求模型 */
export interface MbsWrkPrjCtlUpdateExpenseReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案支出 ID */
  a: number;
  /** 員工專案支出名稱 */
  b: string;
  /** 員工專案支出預估金額 */
  c: number;
  /** 員工專案支出實際金額 */
  d: number | null;
  /** 員工專案支出發票號碼 */
  e: string | null;
  /** 員工專案支出發票日期 */
  f: string | null;
  /** 員工專案支出備註 */
  g: string | null;
}

/** 管理後台-工作-專案-控制器-更新專案支出-回應模型 */
export interface MbsWrkPrjCtlUpdateExpenseRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//#endregion
