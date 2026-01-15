import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口-請求模型 */
export interface MbsSysCttCtlGetManyContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-ID */
  a: number | null;
  /** 原子-窗口評等類型 */
  b: DbAtomManagerContacterRatingKindEnum | null;
  /** 管理者-窗口-姓名 */
  c: string | null;
  /** 管理者-窗口-Email */
  d: string | null;
  /** 頁面索引 */
  e: number;
  /** 每頁筆數 */
  f: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口-回應模型 */
export interface MbsSysCttCtlGetManyContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口-列表 */
  a: MbsSysCttCtlGetManyContacterRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口-回應項目模型 */
export interface MbsSysCttCtlGetManyContacterRspItemMdl {
  /** 管理者-窗口-ID */
  a: number;
  /** 管理者-公司-名稱 */
  b: string | null;
  /** 管理者-窗口-姓名 */
  c: string | null;
  /** 管理者-窗口-Email */
  d: string | null;
  /** 管理者-窗口-部門 */
  e: string | null;
  /** 管理者-窗口-職稱 */
  f: string | null;
  /** 管理者-窗口-電話 */
  g: string | null;
  /** 原子-窗口評等類型 */
  h: DbAtomManagerContacterRatingKindEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-取得單筆窗口-請求模型 */
export interface MbsSysCttCtlGetContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口-ID */
  a: number;
}

/** 管理者後台-系統-名單窗口-控制器-取得單筆窗口-回應模型 */
export interface MbsSysCttCtlGetContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口-ID */
  a: number;
  /** 管理者-公司-ID */
  b: number;
  /** 管理者-窗口-姓名 */
  c: string;
  /** 管理者-窗口-Email */
  d: string;
  /** 管理者-窗口-電話 */
  e: string;
  /** 管理者-窗口-部門 */
  f: string;
  /** 管理者-窗口-職稱 */
  g: string;
  /** 管理者-窗口-電話(市話) */
  h: string;
  /** 原子-管理者窗口狀態 */
  i: DbAtomManagerContacterStatusEnum;
  /** 管理者-窗口-是否同意問卷 */
  j: boolean;
  /** 原子-窗口評等類型 */
  k: DbAtomManagerContacterRatingKindEnum;
  /** 管理者-窗口-備註  */
  m: string | null;
  /** 管理者-公司-名稱 */
  n: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-新增窗口-請求模型 */
export interface MbsSysCttCtlAddContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-ID */
  a: number;
  /** 管理者-窗口-姓名 */
  b: string | null;
  /** 管理者-窗口-Email */
  c: string | null;
  /** 管理者-窗口-手機 */
  d: string | null;
  /** 管理者-窗口-部門 */
  e: string | null;
  /** 管理者-窗口-職稱 */
  f: string | null;
  /** 管理者-窗口-電話(市話) */
  g: string | null;
  /** 原子-管理者窗口狀態 */
  h: DbAtomManagerContacterStatusEnum;
  /** 管理者-窗口-是否同意問卷 */
  i: boolean;
  /** 原子-窗口評等類型 */
  j: DbAtomManagerContacterRatingKindEnum;
  /** 管理者-窗口-評等原因列表 */
  k: MbsSysCttCtlAddContacterReqRatingItemMdl[];
}

/** 管理者後台-系統-名單窗口-控制器-新增窗口-回應模型 */
export interface MbsSysCttCtlAddContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口-ID */
  a: number;
}

/** 管理者後台-系統-名單窗口-控制器-新增-窗口評等項目模型 */
export interface MbsSysCttCtlAddContacterReqRatingItemMdl {
  /** 管理者-窗口-評等原因-ID */
  a: number;
  /** 管理者-窗口-評等原因-備註 */
  b: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-更新窗口-請求模型 */
export interface MbsSysCttCtlUpdateContacterReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口-ID */
  a: number;
  /** 管理者-窗口-姓名 */
  b: string | null;
  /** 管理者-窗口-Email */
  c: string | null;
  /** 管理者-窗口-手機 */
  d: string | null;
  /** 管理者-窗口-部門 */
  e: string | null;
  /** 管理者-窗口-職稱 */
  f: string | null;
  /** 管理者-窗口-電話(市話) */
  g: string | null;
  /** 原子-管理者窗口狀態 */
  h: DbAtomManagerContacterStatusEnum;
  /** 管理者-窗口-是否同意問卷 */
  i: boolean | null;
  /** 原子-窗口評等類型 */
  j: DbAtomManagerContacterRatingKindEnum;
  /** 管理者-窗口-備註  */
  k: string | null;
  /** 管理者-公司-ID */
  l: number | null;
}

/** 管理者後台-系統-名單窗口-控制器-更新窗口-回應模型 */
export interface MbsSysCttCtlUpdateContacterRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口-ID（如果修改了公司，回傳新的窗口 ID） */
  a: number | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口開發評等原因-請求模型 */
export interface MbsSysCttCtlGetManyContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口開發評等原因-名稱 */
  a: string | null;
  /** 管理者-窗口開發評等原因-狀態 */
  b: boolean | null;
  /** 頁碼 */
  c: number;
  /** 每頁筆數 */
  d: number;
}

/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口開發評等原因-回應模型 */
export interface MbsSysCttCtlGetManyContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等原因-列表 */
  a: MbsSysCttCtlGetManyContacterRatingReasonRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口開發評等原因-回應項目模型 */
export interface MbsSysCttCtlGetManyContacterRatingReasonRspItemMdl {
  /** 管理者-窗口開發評等原因-ID */
  a: number;
  /** 管理者-窗口開發評等原因-名稱 */
  b: string | null;
  /** 管理者-窗口開發評等原因-狀態 */
  c: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-取得單筆窗口開發評等原因-請求模型 */
export interface MbsSysCttCtlGetContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口開發評等原因-ID */
  a: number;
}

/** 管理者後台-系統-名單窗口-控制器-取得單筆窗口開發評等原因-回應模型 */
export interface MbsSysCttCtlGetContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等原因-名稱 */
  a: string;
  /** 管理者-窗口開發評等原因-狀態 */
  b: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-新增窗口開發評等原因-請求模型 */
export interface MbsSysCttCtlAddContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口開發評等原因-名稱 */
  a: string;
  /** 管理者-窗口開發評等原因-狀態 */
  b: boolean;
}

/** 管理者後台-系統-名單窗口-控制器-新增窗口開發評等原因-回應模型 */
export interface MbsSysCttCtlAddContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等原因-ID */
  a: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-更新窗口開發評等原因-請求模型 */
export interface MbsSysCttCtlUpdateContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口開發評等原因-ID */
  a: number;
  /** 管理者-窗口開發評等原因-名稱 */
  b: string | null;
  /** 管理者-窗口開發評等原因-狀態 */
  c: boolean | null;
}

/** 管理者後台-系統-名單窗口-控制器-更新窗口開發評等原因-回應模型 */
export interface MbsSysCttCtlUpdateContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口開發評等-請求模型 */
export interface MbsSysCttCtlGetManyContacterRatingReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口開發評等原因-ID */
  a: number;
}

/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口開發評等-回應模型 */
export interface MbsSysCttCtlGetManyContacterRatingRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等列表 */
  a: MbsSysCttCtlGetManyContacterRatingRspItemMdl[];
}

/** 管理者後台-系統-名單窗口-控制器-取得多筆窗口開發評等-回應項目模型 */
export interface MbsSysCttCtlGetManyContacterRatingRspItemMdl {
  /** 管理者-窗口開發評等-ID */
  a: number;
  /** 管理者-窗口開發評等原因名稱 */
  b: string;
  /** 管理者-窗口開發評等-備註 */
  c: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-取得單筆窗口開發評等-請求模型 */
export interface MbsSysCttCtlGetContacterRatingReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口開發評等-ID */
  a: number;
}

/** 管理者後台-系統-名單窗口-控制器-取得單筆窗口開發評等-回應模型 */
export interface MbsSysCttCtlGetContacterRatingRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等原因-ID */
  a: number;
  /** 管理者-窗口開發評等-備註 */
  b: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-新增窗口開發評等-請求模型 */
export interface MbsSysCttCtlAddContacterRatingReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口-ID */
  a: number;
  /** 管理者-窗口開發評等原因-ID */
  b: number;
  /** 管理者-窗口開發評等-備註 */
  c: string | null;
}

/** 管理者後台-系統-名單窗口-控制器-新增窗口開發評等-回應模型 */
export interface MbsSysCttCtlAddContacterRatingRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等-ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-更新窗口開發評等-請求模型 */
export interface MbsSysCttCtlUpdateContacterRatingReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口開發評等-ID */
  a: number;
  /** 管理者-窗口開發評等原因-ID */
  b: number | null;
  /** 管理者-窗口開發評等-備註 */
  c: string | null;
}

/** 管理者後台-系統-名單窗口-控制器-更新窗口開發評等-回應模型 */
export interface MbsSysCttCtlUpdateContacterRatingRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-控制器-移除窗口開發評等-請求模型 */
export interface MbsSysCttCtlRemoveContacterRatingReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-窗口開發評等-ID */
  a: number;
}

/** 管理者後台-系統-名單窗口-控制器-移除窗口開發評等-回應模型 */
export interface MbsSysCttCtlRemoveContacterRatingRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
