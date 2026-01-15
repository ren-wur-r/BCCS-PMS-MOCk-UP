import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//#region 管理者後台-工作-工項-Http-取得多筆專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-Http-取得多筆專案里程碑工項-請求模型 */
export interface MbsWrkJobHttpGetManyProjectStoneJobReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案-ID */
  employeeProjectID: number | null;
  /** 員工-專案里程碑-ID */
  employeeProjectStoneID: number | null;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number | null;
  /** 員工-專案里程碑工項-狀態 */
  employeeProjectStoneJobStatus: DbAtomEmployeeProjectStatusEnum | null;
  /** 開始-員工-專案里程碑工項-訖止時間 */
  startEmployeeProjectStoneJobEndTime: string | null;
  /** 結束-員工-專案里程碑工項-訖止時間 */
  endEmployeeProjectStoneJobEndTime: string | null;
  /** 頁數 */
  pageIndex: number;
  /** 每筆頁數*/
  pageSize: number;
}

/** 管理者後台-工作-工項-Http-取得多筆專案里程碑工項-回應模型 */
export interface MbsWrkJobHttpGetManyProjectStoneJobRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工-專案里程碑工項-列表 */
  employeeProjectStoneJobList: MbsWrkJobHttpGetManyProjectStoneJobRspItemJobMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-工作-工項-Http-取得多筆專案里程碑工項-回應項目工項模型 */
export interface MbsWrkJobHttpGetManyProjectStoneJobRspItemJobMdl {
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number;
  /** 員工-專案-名稱 */
  employeeProjectName: string;
  /** >員工-專案里程碑-名稱*/
  employeeProjectStoneName: string;
  /** 員工-專案里程碑工項-名稱 */
  employeeProjectStoneJobName: string;
  /** 員工-專案里程碑工項-狀態 */
  employeeProjectStoneJobStatus: DbAtomEmployeeProjectStatusEnum;
  /** 員工-專案里程碑工項-開始時間 */
  employeeProjectStoneJobStartTime: string;
  /** 員工-專案里程碑工項-結束時間 */
  employeeProjectStoneJobEndTime: string;
  /** 員工-專案里程碑工項-執行者-列表 */
  employeeProjectStoneJobExecutorList: MbsWrkJobHttpGetManyProjectStoneJobRspItemExecutorMdl[];
}

/** 管理者後台-工作-工項-Http-取得多筆專案里程碑工項-回應項目執行者模型 */
export interface MbsWrkJobHttpGetManyProjectStoneJobRspItemExecutorMdl {
  /** 員工-專案里程碑工項-執行者-名稱 */
  employeeProjectStoneJobExecutorName: string;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-Http-取得專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-Http-取得專案里程碑工項-請求模型 */
export interface MbsWrkJobHttpGetProjectStoneJobReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number;
}

/** 管理者後台-工作-工項-Http-取得專案里程碑工項-回應模型 */
export interface MbsWrkJobHttpGetProjectStoneJobRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工-專案-名稱 */
  employeeProjectName: string;
  /** 員工-專案-開始時間 */
  employeeProjectStartTime: string;
  /** 員工-專案-結束時間 */
  employeeProjectEndTime: string;
  /** 員工-專案里程碑-名稱 */
  employeeProjectStoneName: string;
  /** 員工-專案里程碑-開始時間 */
  employeeProjectStoneStartTime: string;
  /** 員工-專案里程碑-結束時間 */
  employeeProjectStoneEndTime: string;
  /** 員工-專案里程碑工項-名稱 */
  employeeProjectStoneJobName: string;
  /** 員工-專案里程碑工項-開始時間 */
  employeeProjectStoneJobStartTime: string;
  /** 員工-專案里程碑工項-結束時間 */
  employeeProjectStoneJobEndTime: string;
  /** 員工-專案里程碑工項-狀態 */
  employeeProjectStoneJobStatus: DbAtomEmployeeProjectStatusEnum;
  /** 員工-專案里程碑工項-備註 */
  employeeProjectStoneJobRemark: string;
  /** 員工-專案里程碑工項執行者-列表 */
  employeeProjectStoneJobExecutorList: MbsWrkJobHttpGetProjectStoneJobRspItemExecutorMdl[];
  /** 員工-專案里程碑工項清單-列表 */
  employeeProjectStoneJobBucketList: MbsWrkJobHttpGetProjectStoneJobRspItemBucketMdl[];
  /** 員工-專案里程碑工項工作-列表 */
  employeeProjectStoneJobWorkList: MbsWrkJobHttpGetProjectStoneJobRspItemWorkMdl[];
  /** 員工-專案里程碑工項工作檔案-列表 */
  employeeProjectStoneJobWorkFileList: MbsWrkJobHttpGetProjectStoneJobRspItemFileMdl[];
}

/** 管理者後台-工作-工項-Http-取得專案里程碑工項-回應項目執行者模型 */
export interface MbsWrkJobHttpGetProjectStoneJobRspItemExecutorMdl {
  /** 員工-專案里程碑工項-執行者-名稱 */
  employeeProjectStoneJobExecutorName: string;
}

/** 管理者後台-工作-工項-Http-取得專案里程碑工項-回應項目清單模型 */
export interface MbsWrkJobHttpGetProjectStoneJobRspItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  employeeProjectStoneJobBucketID: number;
  /** 員工-專案里程碑工項清單-名稱 */
  employeeProjectStoneJobBucketName: string;
  /** 員工-專案里程碑工項清單-完成狀態 */
  employeeProjectStoneJobBucketIsFinished: boolean;
}

/** 管理者後台-工作-工項-Http-取得專案里程碑工項-回應項目工作模型 */
export interface MbsWrkJobHttpGetProjectStoneJobRspItemWorkMdl {
  /** 員工-專案里程碑工項工作-ID */
  employeeProjectStoneJobWorkID: number;
  /** 員工-專案里程碑工項工作-開始時間 */
  employeeProjectStoneJobWorkStartTime: string;
  /** 員工-專案里程碑工項工作-結束時間 */
  employeeProjectStoneJobWorkEndTime: string;
  /** 員工-專案里程碑工項工作-備註 */
  employeeProjectStoneJobWorkRemark: string;
  /** 員工-姓名 */
  employeeName: string;
}

/** 管理者後台-工作-工項-Http-取得專案里程碑工項-回應項目檔案模型 */
export interface MbsWrkJobHttpGetProjectStoneJobRspItemFileMdl {
  /** 員工-專案里程碑工項工作檔案-網址 */
  employeeProjectStoneJobWorkFileUrl: string;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-Http-更新專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-Http-更新專案里程碑工項-請求模型 */
export interface MbsWrkJobHttpUpdateProjectStoneJobReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number;
  /** 員工-專案里程碑工項-備註 */
  employeeProjectStoneJobRemark: string | null;
  /** 員工-專案里程碑工項清單-列表 */
  employeeProjectStoneJobBucketList: MbsWrkJobHttpUpdateProjectStoneJobReqItemBucketMdl[];
}

/** 管理者後台-工作-工項-Http-更新專案里程碑工項-請求項目檔案模型 */
export interface MbsWrkJobHttpUpdateProjectStoneJobReqItemFileMdl {
  /** 員工-專案里程碑工項工作檔案-網址 */
  employeeProjectStoneJobWorkFileUrl: string;
}

/** 管理者後台-工作-工項-Http-更新專案里程碑工項-請求項目清單模型 */
export interface MbsWrkJobHttpUpdateProjectStoneJobReqItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  employeeProjectStoneJobBucketID: number;
  /** 員工-專案里程碑工項清單-名稱 */
  employeeProjectStoneJobBucketName: string;
  /** 員工-專案里程碑工項清單-完成狀態 */
  employeeProjectStoneJobBucketIsFinished: boolean;
}

/** 管理者後台-工作-工項-Http-更新專案里程碑工項-回應模型 */
export interface MbsWrkJobHttpUpdateProjectStoneJobRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-Http-取得專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-Http-取得專案里程碑工項工作-請求模型 */
export interface MbsWrkJobHttpGetProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobWorkID: number;
}

/** 管理者後台-工作-工項-Http-取得專案里程碑工項工作-回應模型 */
export interface MbsWrkJobHttpGetProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number;
  /** 員工-專案里程碑工項-備註 */
  employeeProjectStoneJobRemark: string;
  /** 員工-專案里程碑工項清單-列表 */
  employeeProjectStoneJobBucketList: MbsWrkJobHttpGetProjectStoneJobWorkRspItemBucketMdl[];
  /** 員工-專案里程碑工項-開始時間 */
  employeeProjectStoneJobWorkStartTime: string;
  /** 員工-專案里程碑工項-結束時間 */
  employeeProjectStoneJobWorkEndTime: string;
  /** 員工-專案里程碑工項工作-備註 */
  employeeProjectStoneJobWorkRemark: string;
  /** 員工-專案里程碑工項工作檔案-列表 */
  employeeProjectStoneJobWorkFileList: MbsWrkJobHttpGetProjectStoneJobWorkRspItemFileMdl[];
  /** 員工-專案-名稱 */
  employeeProjectName: string;
  /** 員工-專案-開始時間 */
  employeeProjectStartTime: string;
  /** 員工-專案-結束時間 */
  employeeProjectEndTime: string;
  /** 員工-專案里程碑-名稱 */
  employeeProjectStoneName: string;
  /** 員工-專案里程碑-開始時間 */
  employeeProjectStoneStartTime: string;
  /** 員工-專案里程碑-結束時間 */
  employeeProjectStoneEndTime: string;
  /** 員工-專案里程碑工項-名稱 */
  employeeProjectStoneJobName: string;
  /** 員工-專案里程碑工項-開始時間 */
  employeeProjectStoneJobStartTime: string;
  /** 員工-專案里程碑工項-結束時間 */
  employeeProjectStoneJobEndTime: string;
}

/** 管理者後台-工作-工項-Http-取得專案里程碑工項工作-回應項目清單模型 */
export interface MbsWrkJobHttpGetProjectStoneJobWorkRspItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  employeeProjectStoneJobBucketID: number;
  /** 員工-專案里程碑工項清單-名稱 */
  employeeProjectStoneJobBucketName: string;
  /** 員工-專案里程碑工項清單-完成狀態 */
  employeeProjectStoneJobBucketIsFinished: boolean;
}

/** 管理者後台-工作-工項-Http-取得專案里程碑工項工作-回應項目檔案模型 */
export interface MbsWrkJobHttpGetProjectStoneJobWorkRspItemFileMdl {
  /**員工-專案里程碑工項工作檔案-ID  */
  employeeProjectStoneJobWorkFileID: number;
  /** 員工-專案里程碑工項工作檔案-網址 */
  employeeProjectStoneJobWorkFileUrl: string;
}
//#endregion

//#region 管理者後台-工作-工項-Http-新增專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-Http-新增專案里程碑工項工作-請求模型 */
export interface MbsWrkJobHttpAddProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number | null;
  /** 員工-專案里程碑工項-備註 */
  employeeProjectStoneJobRemark: string | null;
  /** 員工-專案里程碑工項清單-列表 */
  employeeProjectStoneJobBucketList: MbsWrkJobHttpAddProjectStoneJobWorkReqItemBucketMdl[] | null;
  /** 員工-專案里程碑工項工作-開始時間 */
  employeeProjectStoneJobWorkStartTime: string;
  /** 員工-專案里程碑工項工作-結束時間 */
  employeeProjectStoneJobWorkEndTime: string;
  /** 員工-專案里程碑工項工作-備註 */
  employeeProjectStoneJobWorkRemark: string | null;
  /** 員工-專案里程碑工項工作檔案-列表 */
  employeeProjectStoneJobWorkFileList: MbsWrkJobHttpAddProjectStoneJobWorkReqItemFileMdl[];
}

/** 管理者後台-工作-工項-Http-新增專案里程碑工項工作-請求項目檔案模型 */
export interface MbsWrkJobHttpAddProjectStoneJobWorkReqItemFileMdl {
  /** 員工-專案里程碑工項工作檔案-網址 */
  employeeProjectStoneJobWorkFileUrl: string;
}

/** 管理者後台-工作-工項-Http-新增專案里程碑工項工作-請求項目清單模型 */
export interface MbsWrkJobHttpAddProjectStoneJobWorkReqItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  employeeProjectStoneJobBucketID: number;
  /** 員工-專案里程碑工項清單-完成狀態 */
  employeeProjectStoneJobBucketIsFinished: boolean;
}

/** 管理者後台-工作-工項-Http-新增專案里程碑工項工作-回應模型 */
export interface MbsWrkJobHttpAddProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-Http-更新專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-Http-更新專案里程碑工項工作-請求模型 */
export interface MbsWrkJobHttpUpdateProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number|null;
  /** 員工-專案里程碑工項-備註 */
  employeeProjectStoneJobRemark: string | null;
  /** 員工-專案里程碑工項清單-列表 */
  employeeProjectStoneJobBucketList: MbsWrkJobHttpUpdateProjectStoneJobWorkReqItemBucketMdl[];
  /** 員工-專案里程碑工項工作-ID */
  employeeProjectStoneJobWorkID: number;
  /** 員工-專案里程碑工項工作-開始時間 */
  employeeProjectStoneJobWorkStartTime: string | null;
  /** 員工-專案里程碑工項工作-結束時間 */
  employeeProjectStoneJobWorkEndTime: string | null;
  /** 員工-專案里程碑工項工作-備註 */
  employeeProjectStoneJobWorkRemark: string | null;
  /** 員工-專案里程碑工項工作檔案-列表 */
  employeeProjectStoneJobWorkFileList: MbsWrkJobHttpUpdateProjectStoneJobWorkReqItemFileMdl[];
}

/** 管理者後台-工作-工項-Http-更新專案里程碑工項-請求項目檔案模型 */
export interface MbsWrkJobHttpUpdateProjectStoneJobWorkReqItemFileMdl {
  /** 員工-專案里程碑工項工作檔案-ID */
  employeeProjectStoneJobWorkFileID: number;
  /** 員工-專案里程碑工項工作檔案-網址 */
  employeeProjectStoneJobWorkFileUrl: string;
}

/** 管理者後台-工作-工項-Http-更新專案里程碑工項-請求項目清單模型 */
export interface MbsWrkJobHttpUpdateProjectStoneJobWorkReqItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  employeeProjectStoneJobBucketID: number;
  /** 員工-專案里程碑工項清單-名稱 */
  employeeProjectStoneJobBucketName: string;
  /** 員工-專案里程碑工項清單-完成狀態 */
  employeeProjectStoneJobBucketIsFinished: boolean;
}

/** 管理者後台-工作-工項-Http-更新專案里程碑工項-回應模型 */
export interface MbsWrkJobHttpUpdateProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-Http-取得多筆專案里程碑工項工作
/** 管理者後台-工作-工項-Http-取得多筆專案里程碑工項工作-請求模型 */
export interface MbsWrkJobHttpGetManyProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工專案-ID */
  employeeProjectID: number | null;
  /** 員工-專案里程碑-ID */
  employeeProjectStoneID: number | null;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number | null;
  /** 員工-ID */
  employeeID: number | null;
  /** 員工-專案里程碑工項工作-開始時間 */
  employeeProjectStoneJobWorkStartTime: string | null;
  /** 員工-專案里程碑工項工作-結束時間 */
  employeeProjectStoneJobWorkEndTime: string | null;
  /** 頁數 */
  pageIndex: number;
  /** 每筆頁數*/
  pageSize: number;
}
/** 管理者後台-工作-工項-Http-取得多筆專案里程碑工項工作-回應模型 */
export interface MbsWrkJobHttpGetManyProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 員工-專案里程碑工項工作-列表 */
  employeeProjectStoneJobWorkList: MbsWrkJobHttpGetManyProjectStoneJobWorkRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-工作-工項-Http-取得多筆專案里程碑工項工作-回應項目模型 */
export interface MbsWrkJobHttpGetManyProjectStoneJobWorkRspItemMdl {
  /** 員工-專案里程碑工項工作-ID */
  employeeProjectStoneJobWorkID: number;
  /** 員工-專案里程碑工項工作-開始時間  */
  employeeProjectStoneJobWorkStartTime: string;
  /** 員工-專案里程碑工項工作-結束時間 */
  employeeProjectStoneJobWorkEndTime: string;
  /** 員工-專案-名稱 */
  employeeProjectName: string;
  /** 員工-專案里程碑-名稱 */
  employeeProjectStoneName: string;
  /** 員工-專案里程碑工項-名稱 */
  employeeProjectStoneJobName: string;
  /** 員工-姓名 */
  employeeName: string;
  /** 員工-專案里程碑工項工作-備註 */
  employeeProjectStoneJobWorkRemark: string;
  /** 員工-專案里程碑工項-ID */
  employeeProjectStoneJobID: number ;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-Http-移除專案里程碑工項工作
/** 管理者後台-工作-工項-Http-移除專案里程碑工項工作-請求模型 */
export interface MbsWrkJobHttpRemoveProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 員工-專案里程碑工項工作-ID */
  employeeProjectStoneJobWorkID: number;
}

/** 管理者後台-工作-工項-Http-移除專案里程碑工項工作-回應模型 */
export interface MbsWrkJobHttpRemoveProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//#endregion
