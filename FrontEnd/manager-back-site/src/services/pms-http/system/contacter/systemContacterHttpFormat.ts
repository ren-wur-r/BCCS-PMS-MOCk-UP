import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-取得多筆窗口-請求模型 */
export interface MbsSysCttHttpGetManyContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司-ID */
  managerCompanyID: number | null;
  /** 原子-窗口評等類型 */
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum | null;
  /** 管理者-窗口-姓名 */
  managerContacterName: string | null;
  /** 管理者-窗口-Email */
  managerContacterEmail: string | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-取得多筆窗口-回應模型 */
export interface MbsSysCttHttpGetManyContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口列表 */
  managerContacterList: MbsSysCttHttpGetManyContacterRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-名單窗口-Http-取得多筆窗口-回應項目模型 */
export interface MbsSysCttHttpGetManyContacterRspItemMdl {
  /** 管理者-窗口-ID */
  managerContacterID: number;
  /** 管理者-公司-名稱 */
  managerCompanyName: string | null;
  /** 管理者-窗口-姓名 */
  managerContacterName: string | null;
  /** 管理者-窗口-Email */
  managerContacterEmail: string | null;
  /** 管理者-窗口-部門 */
  managerContacterDepartment: string | null;
  /** 管理者-窗口-職稱 */
  managerContacterJobTitle: string | null;
  /** 管理者-窗口-電話 */
  managerContacterPhone: string | null;
  /** 原子-窗口評等類型 */
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-取得單筆窗口-請求模型 */
export interface MbsSysCttHttpGetContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口-ID */
  managerContacterID: number;
}

/** 管理者後台-系統-名單窗口-Http-取得單筆窗口-回應模型 */
export interface MbsSysCttHttpGetContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口-ID */
  managerContacterID: number;
  /** 管理者-公司-ID */
  managerCompanyID: number;
  /** 管理者-窗口-姓名 */
  managerContacterName: string;
  /** 管理者-窗口-Email */
  managerContacterEmail: string;
  /** 管理者-窗口-電話 */
  managerContacterPhone: string;
  /** 管理者-窗口-部門 */
  managerContacterDepartment: string;
  /** 管理者-窗口-職稱 */
  managerContacterTitle: string;
  /** 管理者-窗口-電話(市話) */
  managerContacterTelephone: string;
  /** 原子-管理者窗口狀態 */
  atomManagerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 管理者-窗口-是否同意問卷 */
  managerContacterIsAgreeSurvey: boolean;
  /** 原子-窗口評等類型 */
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum;
  /** 管理者-窗口-備註  */
  managerContacterRemark: string | null;
  /** 管理者-公司-名稱 */
  managerCompanyName: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-新增窗口-請求模型 */
export interface MbsSysCttHttpAddContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-公司-ID */
  managerCompanyID: number;
  /** 管理者-窗口-姓名 */
  managerContacterName: string | null;
  /** 管理者-窗口-Email */
  managerContacterEmail: string | null;
  /** 管理者-窗口-手機 */
  managerContacterPhone: string | null;
  /** 管理者-窗口-部門 */
  managerContacterDepartment: string | null;
  /** 管理者-窗口-職稱 */
  managerContacterTitle: string | null;
  /** 管理者-窗口-電話(市話) */
  managerContacterTelephone: string | null;
  /** 原子-管理者窗口狀態 */
  atomManagerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 管理者-窗口-是否同意問卷 */
  managerContacterIsAgreeSurvey: boolean;
  /** 原子-窗口評等類型 */
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum;
  /** 管理者-窗口-評等原因列表 */
  managerContacterRatingList: MbsSysCttHttpAddContacterReqRatingItemMdl[];
}

/** 管理者後台-系統-名單窗口-Http-新增-窗口評等項目模型 */
export interface MbsSysCttHttpAddContacterReqRatingItemMdl {
  /** 管理者-窗口-評等原因-ID */
  managerContacterRatingReasonID: number;
  /** 管理者-窗口-評等原因-備註 */
  managerContacterRatingRemark: string | null;
}

/** 管理者後台-系統-名單窗口-Http-新增窗口-回應模型 */
export interface MbsSysCttHttpAddContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口-ID */
  managerContacterID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-更新窗口-請求模型 */
export interface MbsSysCttHttpUpdateContacterReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口-ID */
  managerContacterID: number;
  /** 管理者-窗口-姓名 */
  managerContacterName: string | null;
  /** 管理者-窗口-Email */
  managerContacterEmail: string | null;
  /** 管理者-窗口-手機 */
  managerContacterPhone: string | null;
  /** 管理者-窗口-部門 */
  managerContacterDepartment: string | null;
  /** 管理者-窗口-職稱 */
  managerContacterTitle: string | null;
  /** 管理者-窗口-電話(市話) */
  managerContacterTel: string | null;
  /** 原子-管理者窗口狀態 */
  atomManagerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 管理者-窗口-是否同意問卷 */
  managerContacterIsAgreeSurvey: boolean | null;
  /** 原子-窗口評等類型 */
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum;
  /** 管理者-窗口-備註  */
  managerContacterRemark: string | null;
  /** 管理者-公司-ID */
  managerCompanyID: number | null;
}

/** 管理者後台-系統-名單窗口-Http-更新窗口-回應模型 */
export interface MbsSysCttHttpUpdateContacterRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口-ID（如果修改了公司，回傳新的窗口 ID） */
  managerContacterID: number | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-取得多筆窗口開發評等原因-請求模型 */
export interface MbsSysCttHttpGetManyContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口開發評等原因-名稱 */
  managerContacterRatingReasonName: string | null;
  /** 管理者-窗口開發評等原因-狀態 */
  managerContacterRatingReasonStatus: boolean | null;
  /** 頁碼 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 管理者後台-系統-名單窗口-Http-取得多筆窗口開發評等原因-回應模型 */
export interface MbsSysCttHttpGetManyContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等原因-列表 */
  managerContacterRatingReasonList: MbsSysCttHttpGetManyContacterRatingReasonRspItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

/** 管理者後台-系統-名單窗口-Http-取得多筆窗口開發評等原因-回應項目模型 */
export interface MbsSysCttHttpGetManyContacterRatingReasonRspItemMdl {
  /** 管理者-窗口開發評等原因-ID */
  managerContacterRatingReasonID: number;
  /** 管理者-窗口開發評等原因-名稱 */
  managerContacterRatingReasonName: string | null;
  /** 管理者-窗口開發評等原因-狀態 */
  managerContacterRatingReasonStatus: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-取得單筆窗口開發評等原因-請求模型 */
export interface MbsSysCttHttpGetContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口開發評等原因-ID */
  managerContacterRatingReasonID: number;
}

/** 管理者後台-系統-名單窗口-Http-取得單筆窗口開發評等原因-回應模型 */
export interface MbsSysCttHttpGetContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等原因-名稱 */
  managerContacterRatingReasonName: string;
  /** 管理者-窗口開發評等原因-狀態 */
  managerContacterRatingReasonStatus: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-新增窗口開發評等原因-請求模型 */
export interface MbsSysCttHttpAddContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口開發評等原因-名稱 */
  managerContacterRatingReasonName: string;
  /** 管理者-窗口開發評等原因-狀態 */
  managerContacterRatingReasonStatus: boolean;
}

/** 管理者後台-系統-名單窗口-Http-新增窗口開發評等原因-回應模型 */
export interface MbsSysCttHttpAddContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等原因-ID */
  managerContacterRatingReasonID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-取得多筆窗口開發評等-請求模型 */
export interface MbsSysCttHttpGetManyContacterRatingReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口開發評等原因-ID */
  managerContacterRatingReasonID: number;
}

/** 管理者後台-系統-名單窗口-Http-取得多筆窗口開發評等-回應模型 */
export interface MbsSysCttHttpGetManyContacterRatingRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等列表 */
  managerContacterRatingList: MbsSysCttHttpGetManyContacterRatingRspItemMdl[];
}

/** 管理者後台-系統-名單窗口-Http-取得多筆窗口開發評等-回應項目模型 */
export interface MbsSysCttHttpGetManyContacterRatingRspItemMdl {
  /** 管理者-窗口開發評等-ID */
  managerContacterRatingID: number;
  /** 管理者-窗口開發評等原因名稱 */
  managerContacterRatingReasonName: string;
  /** 管理者-窗口開發評等-備註 */
  managerContacterRatingRemark: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-更新窗口開發評等原因-請求模型 */
export interface MbsSysCttHttpUpdateContacterRatingReasonReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口開發評等原因-ID */
  managerContacterRatingReasonID: number;
  /** 管理者-窗口開發評等原因-名稱 */
  managerContacterRatingReasonName: string | null;
  /** 管理者-窗口開發評等原因-狀態 */
  managerContacterRatingReasonStatus: boolean | null;
}

/** 管理者後台-系統-名單窗口-Http-更新窗口開發評等原因-回應模型 */
export interface MbsSysCttHttpUpdateContacterRatingReasonRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-取得單筆窗口開發評等-請求模型 */
export interface MbsSysCttHttpGetContacterRatingReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口開發評等-ID */
  managerContacterRatingID: number;
}

/** 管理者後台-系統-名單窗口-Http-取得單筆窗口開發評等-回應模型 */
export interface MbsSysCttHttpGetContacterRatingRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等原因-ID */
  managerContacterRatingReasonID: number;
  /** 管理者-窗口開發評等-備註 */
  managerContacterRatingRemark: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-新增窗口開發評等-請求模型 */
export interface MbsSysCttHttpAddContacterRatingReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口-ID */
  managerContacterRatingID: number;
  /** 管理者-窗口開發評等原因-ID */
  managerContacterRatingReasonID: number;
  /** 管理者-窗口開發評等-備註 */
  managerContacterRatingRemark: string | null;
}

/** 管理者後台-系統-名單窗口-Http-新增窗口開發評等-回應模型 */
export interface MbsSysCttHttpAddContacterRatingRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
  /** 管理者-窗口開發評等-ID */
  managerContacterRatingID: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-更新窗口開發評等-請求模型 */
export interface MbsSysCttHttpUpdateContacterRatingReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口開發評等-ID */
  managerContacterRatingID: number;
  /** 管理者-窗口開發評等原因-ID */
  managerContacterRatingReasonID: number | null;
  /** 管理者-窗口開發評等-備註 */
  managerContacterRatingRemark: string | null;
}

/** 管理者後台-系統-名單窗口-Http-更新窗口開發評等-回應模型 */
export interface MbsSysCttHttpUpdateContacterRatingRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-Http-移除窗口開發評等-請求模型 */
export interface MbsSysCttHttpRemoveContacterRatingReqMdl {
  /** 員工登入令牌 */
  employeeLoginToken: string;
  /** 管理者-窗口開發評等-ID */
  managerContacterRatingID: number;
}

/** 管理者後台-系統-名單窗口-Http-移除窗口開發評等-回應模型 */
export interface MbsSysCttHttpRemoveContacterRatingRspMdl {
  /** 錯誤代碼 */
  errorCode: MbsErrorCodeEnum;
}
