import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomSecurityGradeEnum } from "@/constants/DbAtomSecurityGradeEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

//#region 管理者後台-系統-名單公司
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得多筆公司主分類-請求模型 */
export interface MbsSysComCtlGetManyCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司主分類-名稱 */
  a: string | null;
  /** 管理者-公司主分類-是否啟用 */
  b: boolean | null;
  /** 頁面索引 */
  c: number;
  /** 頁面筆數 */
  d: number;
}

/** 管理者後台-系統-名單公司-控制器-取得多筆公司主分類-回應模型 */
export interface MbsSysComCtlGetManyCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司主分類-列表 */
  a: MbsSysComCtlGetManyCompanyMainKindRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-名單公司-控制器-取得多筆公司主分類-回應項目模型 */
export interface MbsSysComCtlGetManyCompanyMainKindRspItemMdl {
  /** 管理者-公司主分類-ID */
  a: number;
  /** 管理者-公司主分類-名稱 */
  b: string;
  /** 管理者-公司主分類-是否啟用 */
  c: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得公司主分類-請求模型 */
export interface MbsSysComCtlGetCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司主分類-ID */
  a: number;
}

/** 管理者後台-系統-名單公司-控制器-取得公司主分類-回應模型 */
export interface MbsSysComCtlGetCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司主分類-名稱 */
  a: string | null;
  /** 管理者-公司主分類-是否啟用 */
  b: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-新增公司主分類-請求模型 */
export interface MbsSysComCtlAddCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司主分類-名稱 */
  a: string;
  /** 管理者-公司主分類-是否啟用 */
  b: boolean;
}

/** 管理者後台-系統-名單公司-控制器-新增公司主分類-回應模型 */
export interface MbsSysComCtlAddCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司主分類-ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-更新公司主分類-請求模型 */
export interface MbsSysComCtlUpdateCompanyMainKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司主分類-ID */
  a: number;
  /** 管理者-公司主分類-名稱 */
  b: string | null;
  /** 管理者-公司主分類-是否啟用 */
  c: boolean | null;
}

/** 管理者後台-系統-名單公司-控制器-更新公司主分類-回應模型 */
export interface MbsSysComCtlUpdateCompanyMainKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得多筆公司子分類-請求模型 */
export interface MbsSysComCtlGetManyCompanySubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司子分類-名稱 */
  a: string | null;
  /** 管理者-公司子分類-是否啟用 */
  b: boolean | null;
  /** 管理者-公司主分類-ID */
  c: number | null;
  /** 頁面索引 */
  d: number;
  /** 頁面筆數 */
  e: number;
}

/** 管理者後台-系統-名單公司-控制器-取得多筆公司子分類-回應模型 */
export interface MbsSysComCtlGetManyCompanySubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司子分類-列表 */
  a: MbsSysComCtlGetManyCompanySubKindRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-名單公司-控制器-取得多筆公司子分類-回應項目模型 */
export interface MbsSysComCtlGetManyCompanySubKindRspItemMdl {
  /** 管理者-公司子分類-ID */
  a: number;
  /** 管理者-公司子分類-名稱 */
  b: string | null;
  /** 管理者-公司主分類-ID */
  c: number;
  /** 管理者-公司主分類-名稱 */
  d: string | null;
  /** 管理者-公司子分類-是否啟用 */
  e: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得公司子分類-請求模型 */
export interface MbsSysComCtlGetCompanySubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司子分類-ID */
  a: number;
}

/** 管理者後台-系統-名單公司-控制器-取得公司子分類-回應模型 */
export interface MbsSysComCtlGetCompanySubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司子分類-名稱 */
  a: string ;
  /** 管理者-公司主分類-ID */
  b: number;
  /** 管理者-公司子分類-是否啟用 */
  c: boolean;
  /** 管理者-公司主分類-名稱 */
  d: string ;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-新增公司子分類-請求模型 */
export interface MbsSysComCtlAddCompanySubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司子分類-名稱 */
  a: string;
  /** 管理者-公司主分類-ID */
  b: number;
  /** 管理者-公司子分類-是否啟用 */
  c: boolean;
}

/** 管理者後台-系統-名單公司-控制器-新增公司子分類-回應模型 */
export interface MbsSysComCtlAddCompanySubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司子分類-ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-更新公司子分類-請求模型 */
export interface MbsSysComCtlUpdateCompanySubKindReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司子分類-ID */
  a: number;
  /** 管理者-公司子分類-名稱 */
  b: string | null;
  /** 管理者-公司子分類-是否啟用 */
  c: boolean | null;
}

/** 管理者後台-系統-名單公司-控制器-更新公司子分類-回應模型 */
export interface MbsSysComCtlUpdateCompanySubKindRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得多筆公司-請求模型 */
export interface MbsSysComCtlGetManyCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 原子-客戶等級 */
  a: DbAtomCustomerGradeEnum;
  /** 管理者-公司主分類-ID */
  b: number | null;
  /** 管理者-公司子分類-ID */
  c: number | null;
  /** 管理者-公司-名稱 */
  d: string | null;
  /** 頁面索引 */
  e: number;
  /** 頁面筆數 */
  f: number;
}

/** 管理者後台-系統-名單公司-控制器-取得多筆公司-回應模型 */
export interface MbsSysComCtlGetManyCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司-列表 */
  a: MbsSysComCtlGetManyCompanyRspItemMdl[];
  /** 總筆數 */
  b: number;
}

/** 管理者後台-系統-名單公司-控制器-取得多筆公司-回應項目模型 */
export interface MbsSysComCtlGetManyCompanyRspItemMdl {
  /** 管理者-公司-ID */
  a: number;
  /** 管理者-公司-統一編號 */
  b: string;
  /** 管理者-公司-名稱 */
  c: string | null;
  /** 原子-客戶等級 */
  d: number;
  /** 管理者-公司主分類-名稱 */
  e: string | null;
  /** 管理者-公司子分類-名稱 */
  f: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得單筆公司-請求模型 */
export interface MbsSysComCtlGetCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-ID */
  a: number;
}

/** 管理者後台-系統-名單公司-控制器-取得單筆公司-回應模型 */
export interface MbsSysComCtlGetCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司-統一編號 */
  a: string;
  /** 管理者-公司-名稱 */
  b: string | null;
  /** 原子-管理者-公司-狀態 */
  c: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司主分類-ID */
  d: number;
  /** 管理者-公司主分類-名稱 */
  e: string | null;
  /** 管理者-公司子分類-ID */
  f: number;
  /** 管理者-公司子分類-名稱 */
  g: string | null;
  /** 原子-客戶等級 */
  h: DbAtomCustomerGradeEnum;
  /** 原子-安全等級 */
  i: DbAtomSecurityGradeEnum;
  /** 原子-員工規模 */
  j: DbAtomEmployeeRangeEnum;
}
//--------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-新增公司-請求模型 */
export interface MbsSysComCtlAddCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-統一編號 */
  a: string;
  /** 管理者-公司-名稱 */
  b: string;
  /** 原子-管理者公司狀態 */
  c: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司主分類-ID */
  d: number;
  /** 管理者-公司子分類-ID */
  e: number;
  /** 原子-客戶等級 */
  f: DbAtomCustomerGradeEnum;
  /** 原子-安全等級 */
  g: DbAtomSecurityGradeEnum;
  /** 原子-員工規模 */
  h: DbAtomEmployeeRangeEnum;
  /** 管理者-關係公司列表 */
  i: MbsSysComCtlAddCompanyAffiliateReqItemMdl[];
  /** 管理者-公司營業地點列表 */
  j: MbsSysComCtlAddCompanyLocationReqItemMdl[];
}

/** 管理者後台-系統-名單公司-控制器-新增公司-回應模型 */
export interface MbsSysComCtlAddCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
  /** 管理者-公司-ID */
  a: number;
}

/** 管理者後台-系統-名單公司-控制器-新增公司-營業地點-請求項目模型 */
export interface MbsSysComCtlAddCompanyLocationReqItemMdl {
  /** 管理者-公司營業地點-名稱 */
  a: string;
  /** 原子-縣市 */
  b: DbAtomCityEnum | null;
  /** 管理者-公司營業地點-地址 */
  c: string;
  /** 管理者-公司營業地點-電話 */
  d: string | null;
  /** 原子-管理者-公司-狀態 */
  e: DbAtomManagerCompanyStatusEnum | null;
}

/** 管理者後台-系統-名單公司-控制器-新增關係公司-請求項目模型 */
export interface MbsSysComCtlAddCompanyAffiliateReqItemMdl {
  /** 管理者-公司關係-統一編號 */
  a: string;
  /** 管理者-公司關係-名稱 */
  b: string;
}
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-更新公司-請求模型 */
export interface MbsSysComCtlUpdateCompanyReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-ID */
  a: number;
  /** 管理者-公司-統一編號 */
  b: string | null;
  /** 管理者-公司-名稱 */
  c: string | null;
  /** 原子-管理者-公司狀態 */
  d: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司主分類-ID */
  e: number | null;
  /** 管理者-公司子分類-ID */
  f: number | null;
  /** 原子-客戶等級 */
  g: DbAtomCustomerGradeEnum;
  /** 原子-安全等級 */
  h: DbAtomSecurityGradeEnum;
  /** 原子-員工規模 */
  i: DbAtomEmployeeRangeEnum;
}

/** 管理者後台-系統-名單公司-控制器-更新公司-回應模型 */
export interface MbsSysComCtlUpdateCompanyRspMdl {
  /** 錯誤代碼 */
  aa: MbsErrorCodeEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得多筆公司營業地點-請求模型 */
export interface MbsSysComCtlGetManyCompanyLocationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-ID */
  a: number;
}

/** 管理者後台-系統-名單公司-控制器-取得多筆公司營業地點-回應模型 */
export interface MbsSysComCtlGetManyCompanyLocationRspMdl {
  /** 錯誤代碼 */
  aa: number;
  /** 管理者-公司營業地點-列表 */
  a: MbsSysComCtlGetManyCompanyLocationRspItemMdl[];
}

/** 管理者後台-系統-名單公司-控制器-取得多筆公司營業地點-回應項目模型 */
export interface MbsSysComCtlGetManyCompanyLocationRspItemMdl {
  /** 管理者-公司營業地點-ID */
  a: number;
  /** 管理者-公司營業地點-名稱 */
  b: string;
  /** 原子-縣市 */
  c: DbAtomCityEnum;
  /** 管理者-公司營業地點-地址 */
  d: string;
  /** 管理者-公司營業地點-電話 */
  e: string;
  /** 原子-管理者-公司-狀態 */
  f: DbAtomManagerCompanyStatusEnum;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得單筆公司營業地點-請求模型 */
export interface MbsSysComCtlGetCompanyLocationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司營業地點-ID */
  a: number;
}

/** 管理者後台-系統-名單公司-控制器-取得單筆公司營業地點-回應模型 */
export interface MbsSysComCtlGetCompanyLocationRspMdl {
  /** 錯誤代碼 */
  aa: number;
  /** 管理者-公司營業地點-ID */
  a: number;
  /** 管理者-公司營業地點-名稱 */
  b: string;
  /** 原子-縣市 */
  c: DbAtomCityEnum;
  /** 管理者-公司營業地點-地址 */
  d: string;
  /** 管理者-公司營業地點-電話 */
  e: string;
  /** 原子-管理者-公司-狀態 */
  f: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司營業地點-是否已刪除 */
  g: boolean;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-新增公司營業地點-請求模型 */
export interface MbsSysComCtlAddCompanyLocationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-ID */
  a: number;
  /** 管理者-公司營業地點-名稱 */
  b: string;
  /** 原子-縣市 */
  c: DbAtomCityEnum | null;
  /** 管理者-公司營業地點-地址 */
  d: string;
  /** 管理者-公司營業地點-電話 */
  e: string | null;
  /** 原子-管理者-公司-狀態 */
  f: DbAtomManagerCompanyStatusEnum | null;
}

/** 管理者後台-系統-名單公司-控制器-新增公司營業地點-回應模型 */
export interface MbsSysComCtlAddCompanyLocationRspMdl {
  /** 錯誤代碼 */
  aa: number;
  /** 管理者-公司營業地點-ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-更新公司營業地點-請求模型 */
export interface MbsSysComCtlUpdateCompanyLocationReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司營業地點-ID */
  a: number;
  /** 管理者-公司營業地點-名稱 */
  b: string | null;
  /** 原子-縣市 */
  c: DbAtomCityEnum;
  /** 管理者-公司營業地點-地址 */
  d: string | null;
  /** 管理者-公司營業地點-電話 */
  e: string | null;
  /** 原子-管理者-公司-狀態 */
  f: DbAtomManagerCompanyStatusEnum;
  /** 管理者-公司營業地點-是否已刪除 */
  g: boolean | null;
}

/** 管理者後台-系統-名單公司-控制器-更新公司營業地點-回應模型 */
export interface MbsSysComCtlUpdateCompanyLocationRspMdl {
  /** 錯誤代碼 */
  aa: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得多筆關係公司-請求模型 */
export interface MbsSysComCtlGetManyCompanyAffiliateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-ID */
  a: number;
}

/** 管理者後台-系統-名單公司-控制器-取得多筆關係公司-回應模型 */
export interface MbsSysComCtlGetManyCompanyAffiliateRspMdl {
  /** 錯誤代碼 */
  aa: number;
  /** 管理者-公司關係-列表 */
  a: MbsSysComCtlGetManyCompanyAffiliateRspItemMdl[];
}

/** 管理者後台-系統-名單公司-控制器-取得多筆關係公司-回應項目模型 */
export interface MbsSysComCtlGetManyCompanyAffiliateRspItemMdl {
  /** 管理者-公司關係-ID */
  a: number;
  /** 管理者-公司關係-統一編號 */
  b: string;
  /** 管理者-公司關係-名稱 */
  c: string;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-取得單筆關係公司-請求模型 */
export interface MbsSysComCtlGetCompanyAffiliateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司關係-ID */
  a: number;
}

/** 管理者後台-系統-名單公司-控制器-取得單筆關係公司-回應模型 */
export interface MbsSysComCtlGetCompanyAffiliateRspMdl {
  /** 錯誤代碼 */
  aa: number;
  /** 管理者-公司關係-ID */
  a: number;
  /** 管理者-公司關係-統一編號 */
  b: string;
  /** 管理者-公司關係-名稱 */
  c: string | null;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-新增關係公司-請求模型 */
export interface MbsSysComCtlAddCompanyAffiliateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司-ID */
  a: number;
  /** 管理者-公司關係-統一編號 */
  b: string;
  /** 管理者-公司關係-名稱 */
  c: string;
}

/** 管理者後台-系統-名單公司-控制器-新增關係公司-回應模型 */
export interface MbsSysComCtlAddCompanyAffiliateRspMdl {
  /** 錯誤代碼 */
  aa: number;
  /** 管理者-公司關係-ID */
  a: number;
}

//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單公司-控制器-更新關係公司-請求模型 */
export interface MbsSysComCtlUpdateCompanyAffiliateReqMdl {
  /** 員工登入令牌 */
  aa: string;
  /** 管理者-公司關係-ID */
  a: number;
  /** 管理者-公司關係-統一編號 */
  b: string | null;
  /** 管理者-公司關係-名稱 */
  c: string | null;
  /** 管理者-公司關係-是否已刪除 */
  d: boolean | null;
}

/** 管理者後台-系統-名單公司-控制器-更新關係公司-回應模型 */
export interface MbsSysComCtlUpdateCompanyAffiliateRspMdl {
  /** 錯誤代碼 */
  aa: number;
}
//--------------------------------------------------------------------------------
//#endregion
