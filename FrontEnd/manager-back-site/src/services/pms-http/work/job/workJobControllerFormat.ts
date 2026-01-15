import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//#region 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項-請求模型 */
export interface MbsWrkJobCtlGetManyProjectStoneJobReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案-ID */
  a: number | null;
  /** 員工-專案里程碑-ID */
  b: number | null;
  /** 員工-專案里程碑工項-ID */
  c: number | null;
  /** 員工-專案里程碑工項-狀態 */
  d: DbAtomEmployeeProjectStatusEnum | null;
  /** 開始-員工-專案里程碑工項-訖止時間 */
  e: string | null;
  /** 結束-員工-專案里程碑工項-訖止時間 */
  f: string | null;
  /** 頁數 */
  g: number;
  /** 每頁筆數 */
  h: number;
}

/** 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項-回應模型 */
export interface MbsWrkJobCtlGetManyProjectStoneJobRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工-專案里程碑工項-列表 */
  a: MbsWrkJobCtlGetManyProjectStoneJobRspItemJobMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項-回應項目工項模型 */
export interface MbsWrkJobCtlGetManyProjectStoneJobRspItemJobMdl {
  /** 員工-專案里程碑工項-ID */
  a: number;
  /** 員工-專案-名稱 */
  b: string;
  /** >員工-專案里程碑-名稱*/
  c: string;
  /** 員工-專案里程碑工項-名稱 */
  d: string;
  /** 員工-專案里程碑工項-狀態 */
  e: DbAtomEmployeeProjectStatusEnum;
  /** 員工-專案里程碑工項-開始時間 */
  f: string;
  /** 員工-專案里程碑工項-結束時間 */
  g: string;
  /** 員工-專案里程碑工項-執行者-列表 */
  h: MbsWrkJobCtlGetManyProjectStoneJobRspItemExecutorMdl[];
}

/** 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項-回應項目執行者模型 */
export interface MbsWrkJobCtlGetManyProjectStoneJobRspItemExecutorMdl {
  /** 員工-專案里程碑工項-執行者-名稱 */
  a: string;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-控制器-取得專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-控制器-取得專案里程碑工項-請求模型 */
export interface MbsWrkJobCtlGetProjectStoneJobReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-專案里程碑工項-ID */
  a: number;
}

/** 管理者後台-工作-工項-控制器-取得專案里程碑工項-回應模型 */
export interface MbsWrkJobCtlGetProjectStoneJobRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工-專案-名稱 */
  a: string;
  /** 員工-專案-開始時間 */
  b: string;
  /** 員工-專案-結束時間 */
  c: string;
  /** 員工-專案里程碑-名稱 */
  d: string;
  /** 員工-專案里程碑-開始時間 */
  e: string;
  /** 員工-專案里程碑-結束時間 */
  f: string;
  /** 員工-專案里程碑工項-名稱 */
  g: string;
  /** 員工-專案里程碑工項-開始時間 */
  h: string;
  /** 員工-專案里程碑工項-結束時間 */
  i: string;
  /** 員工-專案里程碑工項-狀態 */
  j: DbAtomEmployeeProjectStatusEnum;
  /** 員工-專案里程碑工項-備註 */
  k: string;
  /** 員工-專案里程碑工項執行者-列表 */
  l: MbsWrkJobCtlGetProjectStoneJobRspItemExecutorMdl[];
  /** 員工-專案里程碑工項清單-列表 */
  m: MbsWrkJobCtlGetProjectStoneJobRspItemBucketMdl[];
  /** 員工-專案里程碑工項工作-列表 */
  n: MbsWrkJobCtlGetProjectStoneJobRspItemWorkMdl[];
  /** 員工-專案里程碑工項工作檔案-列表 */
  o: MbsWrkJobCtlGetProjectStoneJobRspItemFileMdl[];
}

/** 管理者後台-工作-工項-控制器-取得專案里程碑工項-回應項目執行者模型 */
export interface MbsWrkJobCtlGetProjectStoneJobRspItemExecutorMdl {
  /** 員工-專案里程碑工項-執行者-名稱 */
  a: string;
}

/** 管理者後台-工作-工項-控制器-取得專案里程碑工項-回應項目清單模型 */
export interface MbsWrkJobCtlGetProjectStoneJobRspItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  a: number;
  /** 員工-專案里程碑工項清單-名稱 */
  b: string;
  /** 員工-專案里程碑工項清單-完成狀態 */
  c: boolean;
}

/** 管理者後台-工作-工項-控制器-取得專案里程碑工項-回應項目工作模型 */
export interface MbsWrkJobCtlGetProjectStoneJobRspItemWorkMdl {
  /** 員工-專案里程碑工項工作-ID */
  a: number;
  /** 員工-專案里程碑工項工作-開始時間 */
  b: string;
  /** 員工-專案里程碑工項工作-結束時間 */
  c: string;
  /** 員工-專案里程碑工項工作-備註 */
  d: string;
  /** 員工-姓名 */
  e: string;
}

/** 管理者後台-工作-工項-控制器-取得專案里程碑工項-回應項目檔案模型 */
export interface MbsWrkJobCtlGetProjectStoneJobRspItemFileMdl {
  /** 員工-專案里程碑工項工作檔案-網址 */
  a: string;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-控制器-更新專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-控制器-更新專案里程碑工項-請求模型 */
export interface MbsWrkJobCtlUpdateProjectStoneJobReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-專案里程碑工項-ID */
  a: number;
  /** 員工-專案里程碑工項-備註 */
  b: string | null;
  /** 員工-專案里程碑工項清單-列表 */
  c: MbsWrkJobCtlUpdateProjectStoneJobReqItemBucketMdl[];
}

/** 管理者後台-工作-工項-控制器-更新專案里程碑工項-請求項目檔案模型 */
export interface MbsWrkJobCtlUpdateProjectStoneJobReqItemFileMdl {
  /** 員工-專案里程碑工項工作檔案-網址 */
  a: string;
}

/** 管理者後台-工作-工項-控制器-更新專案里程碑工項-請求項目清單模型 */
export interface MbsWrkJobCtlUpdateProjectStoneJobReqItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  a: number;
  /** 員工-專案里程碑工項清單-名稱 */
  b: string;
  /** 員工-專案里程碑工項清單-完成狀態 */
  c: boolean;
}

/** 管理者後台-工作-工項-控制器-更新專案里程碑工項-回應模型 */
export interface MbsWrkJobCtlUpdateProjectStoneJobRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-控制器-取得專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-控制器-取得專案里程碑工項工作-請求模型 */
export interface MbsWrkJobCtlGetProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-專案里程碑工項-ID */
  a: number;
}

/** 管理者後台-工作-工項-控制器-取得專案里程碑工項工作-回應模型 */
export interface MbsWrkJobCtlGetProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工-專案里程碑工項-ID */
  a: number;
  /** 員工-專案里程碑工項-備註 */
  b: string;
  /** 員工-專案里程碑工項清單-列表 */
  c: MbsWrkJobCtlGetProjectStoneJobWorkRspItemBucketMdl[];
  /** 員工-專案里程碑工項工作-開始時間 */
  d: string;
  /** 員工-專案里程碑工項工作-結束時間 */
  e: string;
  /** 員工-專案里程碑工項工作-備註 */
  f: string;
  /** 員工-專案里程碑工項工作檔案-列表 */
  g: MbsWrkJobCtlGetProjectStoneJobWorkRspItemFileMdl[];
  /** 員工-專案-名稱 */
  h: string;
  /** 員工-專案-開始時間 */
  i: string;
  /** 員工-專案-結束時間 */
  j: string;
  /** 員工-專案里程碑-名稱 */
  k: string;
  /** 員工-專案里程碑-開始時間 */
  l: string;
  /** 員工-專案里程碑-結束時間 */
  m: string;
  /** 員工-專案里程碑工項-名稱 */
  n: string;
  /** 員工-專案里程碑工項-開始時間 */
  o: string;
  /** 員工-專案里程碑工項-結束時間 */
  p: string;
}

/** 管理者後台-工作-工項-控制器-取得專案里程碑工項工作-回應項目清單模型 */
export interface MbsWrkJobCtlGetProjectStoneJobWorkRspItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  a: number;
  /** 員工-專案里程碑工項清單-名稱 */
  b: string;
  /** 員工-專案里程碑工項清單-完成狀態 */
  c: boolean;
}

/** 管理者後台-工作-工項-控制器-取得專案里程碑工項工作-回應項目檔案模型 */
export interface MbsWrkJobCtlGetProjectStoneJobWorkRspItemFileMdl {
  /**員工-專案里程碑工項工作檔案-ID  */
  a: number;
  /** 員工-專案里程碑工項工作檔案-網址 */
  b: string;
}
//#endregion

//#region 管理者後台-工作-工項-控制器-新增專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-控制器-新增專案里程碑工項工作-請求模型 */
export interface MbsWrkJobCtlAddProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-專案里程碑工項-ID */
  a: number | null;
  /** 員工-專案里程碑工項-備註 */
  b: string | null;
  /** 員工-專案里程碑工項清單-列表 */
  c: MbsWrkJobCtlAddProjectStoneJobWorkReqItemBucketMdl[] | null;
  /** 員工-專案里程碑工項工作-開始時間 */
  d: string;
  /** 員工-專案里程碑工項工作-結束時間 */
  e: string;
  /** 員工-專案里程碑工項工作-備註 */
  f: string | null;
  /** 員工-專案里程碑工項工作檔案-列表 */
  g: MbsWrkJobCtlAddProjectStoneJobWorkReqItemFileMdl[];
}

/** 管理者後台-工作-工項-控制器-新增專案里程碑工項工作-回應模型 */
export interface MbsWrkJobCtlAddProjectStoneJobWorkReqItemFileMdl {
  /** 員工-專案里程碑工項工作檔案-網址 */
  a: string;
}

/** 管理者後台-工作-工項-控制器-新增專案里程碑工項工作-請求項目清單模型 */
export interface MbsWrkJobCtlAddProjectStoneJobWorkReqItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  a: number;
  /** 員工-專案里程碑工項清單-完成狀態 */
  b: boolean;
}

/** 管理者後台-工作-工項-控制器-新增專案里程碑工項工作-回應模型 */
export interface MbsWrkJobCtlAddProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-控制器-更新專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-控制器-更新專案里程碑工項工作-請求模型 */
export interface MbsWrkJobCtlUpdateProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-專案里程碑工項-ID */
  a: number | null;
  /** 員工-專案里程碑工項-備註 */
  b: string | null;
  /** 員工-專案里程碑工項清單-列表 */
  c: MbsWrkJobCtlUpdateProjectStoneJobWorkReqItemBucketMdl[];
  /** 員工-專案里程碑工項工作-ID */
  d: number;
  /** 員工-專案里程碑工項工作-開始時間 */
  e: string | null;
  /** 員工-專案里程碑工項工作-結束時間 */
  f: string | null;
  /** 員工-專案里程碑工項工作-備註 */
  g: string | null;
  /** 員工-專案里程碑工項工作檔案-列表 */
  h: MbsWrkJobCtlUpdateProjectStoneJobWorkReqItemFileMdl[];
}

/** 管理者後台-工作-工項-控制器-更新專案里程碑工項工作-請求項目檔案模型 */
export interface MbsWrkJobCtlUpdateProjectStoneJobWorkReqItemFileMdl {
  /** 員工-專案里程碑工項工作檔案-ID */
  a: number;
  /** 員工-專案里程碑工項工作檔案-網址 */
  b: string;
}

/** 管理者後台-工作-工項-控制器-更新專案里程碑工項工作-請求項目清單模型 */
export interface MbsWrkJobCtlUpdateProjectStoneJobWorkReqItemBucketMdl {
  /** 員工-專案里程碑工項清單-ID */
  a: number;
  /** 員工-專案里程碑工項清單-名稱 */
  b: string;
  /** 員工-專案里程碑工項清單-完成狀態 */
  c: boolean;
}

/** 管理者後台-工作-工項-控制器-更新專案里程碑工項工作-回應模型 */
export interface MbsWrkJobCtlUpdateProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項工作
/** 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項工作-請求模型 */
export interface MbsWrkJobCtlGetManyProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工專案-ID */
  a: number | null;
  /** 員工-專案里程碑-ID */
  b: number | null;
  /** 員工-專案里程碑工項-ID */
  c: number | null;
  /** 員工-ID */
  d: number | null;
  /** 員工-專案里程碑工項工作-開始時間 */
  e: string | null;
  /** 員工-專案里程碑工項工作-結束時間 */
  f: string | null;
  /** 頁數 */
  g: number;
  /** 每筆頁數*/
  h: number;
}
/** 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項工作-回應模型 */
export interface MbsWrkJobCtlGetManyProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 員工-專案里程碑工項工作-列表 */
  a: MbsWrkJobCtlGetManyProjectStoneJobWorkRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-工作-工項-控制器-取得多筆專案里程碑工項工作-回應項目模型 */
export interface MbsWrkJobCtlGetManyProjectStoneJobWorkRspItemMdl {
  /** 員工-專案里程碑工項工作-ID */
  a: number;
  /** 員工-專案里程碑工項工作-開始時間  */
  b: string;
  /** 員工-專案里程碑工項工作-結束時間 */
  c: string;
  /** 員工-專案-名稱 */
  d: string;
  /** 員工-專案里程碑-名稱 */
  e: string;
  /** 員工-專案里程碑工項-名稱 */
  f: string;
  /** 員工-姓名 */
  g: string;
  /** 員工-專案里程碑工項工作-備註 */
  h: string;
  /** 員工-專案里程碑工項-ID */
  i: number;
}
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-控制器-移除專案里程碑工項工作
/** 管理者後台-工作-工項-控制器-移除專案里程碑工項工作-請求模型 */
export interface MbsWrkJobCtlRemoveProjectStoneJobWorkReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 員工-專案里程碑工項工作-ID */
  a: number;
}

/** 管理者後台-工作-工項-控制器-移除專案里程碑工項工作-回應模型 */
export interface MbsWrkJobCtlRemoveProjectStoneJobWorkRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//#endregion
