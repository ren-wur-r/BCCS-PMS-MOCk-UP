import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbManagerProductKindEnum } from "@/constants/DbManagerProductKindEnum";

//#region 管理者後台-系統-產品
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-取得多筆產品-請求模型 */
export interface MbsSysPrdCtlGetManyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品主分類-ID */
  a: number | null;
  /** 管理者-產品子分類-ID */
  b: number | null;
  /** 管理者-產品-是否為主力產品 */
  c: boolean | null;
  /** 管理者-產品-名稱 */
  d: string | null;
  /** 產品規格-名稱 */
  e: string | null;
  /** 產品規格-是否啟用 */
  f: boolean | null;
  /** 頁面索引 */
  g: number;
  /** 頁面筆數 */
  h: number;
}

/** 管理者後台-系統-產品-控制器-取得多筆產品-回應模型 */
export interface MbsSysPrdCtlGetManyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品-列表 */
  a: MbsSysPrdCtlGetManyRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-產品-控制器-取得多筆產品-回應項目模型 */
export interface MbsSysPrdCtlGetManyRspItemMdl {
  /** 管理者-產品-ID */
  a: number;
  /** 管理者-產品-名稱 */
  b: string;
  /** 管理者-產品主分類-名稱 */
  c: string;
  /** 管理者-產品子分類-名稱 */
  d: string;
  /** 管理者-產品-是否為主力產品 */
  e: boolean;
  /** 管理者-產品規格-名稱 */
  f: string;
  /** 管理者-產品規格-售價 */
  g: number;
  /** 管理者-產品規格-成本 */
  h: number;
  /** 管理者-產品規格-是否啟用 */
  i: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-取得單筆產品-請求模型 */
export interface MbsSysPrdCtlGetReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品-ID */
  a: number;
}

/** 管理者後台-系統-產品-控制器-取得單筆產品-回應模型 */
export interface MbsSysPrdCtlGetRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品-名稱 */
  a: string;
  /** 管理者-產品主分類-ID */
  b: number;
  /** 管理者-產品主分類-名稱 */
  c: string;
  /** 管理者-產品子分類-ID */
  d: number;
  /** 管理者-產品子分類-名稱 */
  e: string;
  /** 管理者-產品種類列舉 */
  f: DbManagerProductKindEnum;
  /** 管理者-產品-是否為主力產品 */
  g: boolean;
  /** 管理者-產品-備註 */
  h: string;
  /** 管理者-產品-是否啟用 */
  i: boolean;
  /** 管理者-產品規格-列表 */
  j: MbsSysPrdCtlGetSpecificationRspItemMdl[];
}

/** 管理者後台-系統-產品-控制器-取得單筆產品-回應模型-規格項目 */
export interface MbsSysPrdCtlGetSpecificationRspItemMdl {
  /** 管理者-產品規格-ID */
  a: number;
  /** 管理者-產品規格-名稱 */
  b: string;
  /** 管理者-產品規格-售價 */
  c: number;
  /** 管理者-產品規格-成本 */
  d: number;
  /** 管理者-產品規格-是否啟用 */
  e: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-新增產品-請求模型 */
export interface MbsSysPrdCtlAddReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品-名稱 */
  a: string;
  /** 管理者-產品主分類-ID */
  b: number;
  /** 管理者-產品子分類-ID */
  c: number;
  /** 管理者-產品種類列舉 */
  d: DbManagerProductKindEnum;
  /** 管理者-產品-是否為主力產品 */
  e: boolean;
  /** 管理者-產品-備註 */
  f: string | null;
  /** 管理者-產品-是否啟用 */
  g: boolean;
  /** 管理者-產品規格-列表 */
  h: MbsSysPrdCtlAddSpecificationReqItemMdl[];
}

/** 管理者後台-系統-產品-控制器-新增產品-規格項目模型 */
export interface MbsSysPrdCtlAddSpecificationReqItemMdl {
  /** 管理者-產品規格-名稱 */
  a: string;
  /** 管理者-產品規格-售價 */
  b: number;
  /** 管理者-產品規格-成本 */
  c: number;
  /** 管理者-產品規格-是否啟用 */
  d: boolean;
}

/** 管理者後台-系統-產品-控制器-新增產品-回應模型 */
export interface MbsSysPrdCtlAddRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品-ID */
  a: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-更新產品-請求模型 */
export interface MbsSysPrdCtlUpdateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品-ID */
  a: number;
  /** 管理者-產品-名稱 */
  b: string | null;
  /** 管理者-產品主分類-ID */
  c: number | null;
  /** 管理者-產品子分類-ID */
  d: number | null;
  /** 管理者-產品種類列舉 */
  e: DbManagerProductKindEnum | null;
  /** 管理者-產品-是否為主力產品 */
  f: boolean | null;
  /** 管理者-產品-備註 */
  g: string | null;
  /** 管理者-產品-是否啟用 */
  h: boolean | null;
}

/** 管理者後台-系統-產品-控制器-更新產品-回應模型 */
export interface MbsSysPrdCtlUpdateRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
//#endregion

// #region 管理者後台-系統-產品-控制器-產品規格
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-新增產品規格-請求模型 */
export interface MbsSysPrdCtlAddSpecificationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品-ID */
  a: number;
  /** 管理者-產品規格-名稱 */
  b: string;
  /** 管理者-產品規格-售價 */
  c: number;
  /** 管理者-產品規格-成本 */
  d: number;
  /** 管理者-產品規格-是否啟用 */
  e: boolean;
}

/** 管理者後台-系統-產品-控制器-新增產品規格-回應模型 */
export interface MbsSysPrdCtlAddSpecificationRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品規格-ID */
  a: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-更新產品規格-請求模型 */
export interface MbsSysPrdCtlUpdateSpecificationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品-ID */
  a: number;
  /** 管理者-產品規格-ID */
  b: number;
  /** 管理者-產品規格-名稱 */
  c: string | null;
  /** 管理者-產品規格-售價 */
  d: number;
  /** 管理者-產品規格-成本 */
  e: number;
  /** 管理者-產品規格-是否啟用 */
  f: boolean;
}

/** 管理者後台-系統-產品-控制器-更新產品規格-回應模型 */
export interface MbsSysPrdCtlUpdateSpecificationRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
//#endregion

//#region 管理者後台-系統-產品-控制器-產品主分類
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-取得多筆產品主分類-請求模型 */
export interface MbsSysPrdCtlGetManyMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品主分類-名稱 */
  a: string | null;
  /** 管理者-產品主分類-是否啟用 */
  b: boolean | null;
  /** 頁數 */
  c: number;
  /** 每頁筆數 */
  d: number;
}

/** 管理者後台-系統-產品-控制器-取得多筆產品主分類-回應模型 */
export interface MbsSysPrdCtlGetManyMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品主分類-列表 */
  a: MbsSysPrdCtlGetManyMainKindRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-產品-控制器-取得多筆產品主分類-回應項目模型 */
export interface MbsSysPrdCtlGetManyMainKindRspItemMdl {
  /** 管理者-產品主分類-ID */
  a: number;
  /** 管理者-產品主分類-名稱 */
  b: string;
  /** 管理者-產品主分類-業務毛利率 */
  c: number;
  /** 管理者-產品主分類-是否啟用 */
  d: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-取得單筆產品主分類-請求模型 */
export interface MbsSysPrdCtlGetMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品主分類-ID */
  a: number;
}

/** 管理者後台-系統-產品-控制器-取得單筆產品主分類-回應模型 */
export interface MbsSysPrdCtlGetMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品主分類-ID */
  a: number;
  /** 管理者-產品主分類-名稱 */
  b: string;
  /** 管理者-產品主分類-業務毛利率 */
  c: number;
  /** 管理者-產品主分類-是否啟用 */
  d: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-新增產品主分類-請求模型 */
export interface MbsSysPrdCtlAddMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品主分類-名稱 */
  a: string;
  /** 管理者-產品主分類-業務毛利率 */
  b: number;
  /** 管理者-產品主分類-是否啟用 */
  c: boolean;
}

/** 管理者後台-系統-產品-控制器-新增產品主分類-回應模型 */
export interface MbsSysPrdCtlAddMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品主分類-ID */
  a: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-更新產品主分類-請求模型 */
export interface MbsSysPrdCtlUpdateMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品主分類-ID */
  a: number;
  /** 管理者-產品主分類-名稱 */
  b: string | null;
  /** 管理者-產品主分類-業務毛利率 */
  c: number | null;
  /** 管理者-產品主分類-是否啟用 */
  d: boolean | null;
}

/** 管理者後台-系統-產品-控制器-更新產品主分類-回應模型 */
export interface MbsSysPrdCtlUpdateMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
//#endregion

// #region 管理者後台-系統-產品-控制器-產品子分類
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-取得多筆產品子分類-請求模型 */
export interface MbsSysPrdCtlGetManySubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品主分類-ID */
  a: number | null;
  /** 管理者-產品子分類-名稱 */
  b: string | null;
  /** 管理者-產品子分類-是否啟用 */
  c: boolean | null;
  /** 頁數 */
  d: number;
  /** 每頁筆數 */
  e: number;
}

/** 管理者後台-系統-產品-控制器-取得多筆產品子分類-回應模型 */
export interface MbsSysPrdCtlGetManySubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品子分類-列表 */
  a: MbsSysPrdCtlGetManySubKindRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-產品-控制器-取得多筆產品子分類-回應項目模型 */
export interface MbsSysPrdCtlGetManySubKindRspItemMdl {
  /** 管理者-產品主分類-ID */
  a: number;
  /** 管理者-產品主分類-名稱 */
  b: string;
  /** 管理者-產品子分類-ID */
  c: number;
  /** 管理者-產品子分類-名稱 */
  d: string;
  /** 管理者-產品子分類-業務毛利率 */
  e: number;
  /** 管理者-產品子分類-是否啟用 */
  f: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-取得單筆產品子分類-請求模型 */
export interface MbsSysPrdCtlGetSubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品子分類-ID */
  a: number;
}

/** 管理者後台-系統-產品-控制器-取得單筆產品子分類-回應模型 */
export interface MbsSysPrdCtlGetSubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品主分類-名稱 */
  a: string;
  /** 管理者-產品子分類-名稱 */
  b: string;
  /** 管理者-產品子分類-業務毛利率 */
  c: number;
  /** 管理者-產品子分類-是否啟用 */
  d: boolean;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-新增產品子分類-請求模型 */
export interface MbsSysPrdCtlAddSubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品主分類-ID */
  a: number;
  /** 管理者-產品子分類-名稱 */
  b: string;
  /** 管理者-產品子分類-業務毛利率 */
  c: number;
  /** 管理者-產品子分類-是否啟用 */
  d: boolean;
}

/** 管理者後台-系統-產品-控制器-新增產品子分類-回應模型 */
export interface MbsSysPrdCtlAddSubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-產品子分類-ID */
  a: number;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-產品-控制器-更新產品子分類-請求模型 */
export interface MbsSysPrdCtlUpdateSubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-產品子分類-ID */
  a: number;
  /** 管理者-產品子分類-名稱 */
  b: string | null;
  /** 管理者-產品子分類-業務毛利率 */
  c: number | null;
  /** 管理者-產品子分類-是否啟用 */
  d: boolean | null;
}

/** 管理者後台-系統-產品-控制器-更新產品子分類-回應模型 */
export interface MbsSysPrdCtlUpdateSubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}
//----------------------------------------------------------------------------------
//#endregion
