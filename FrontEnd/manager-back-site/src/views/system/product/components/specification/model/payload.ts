/** 系統-產品-主分類-新增-Payload */
export interface SystemProductMainKindAddPayload {
  managerProductMainKindName: string;
  managerProductMainKindCommissionRate: number;
  managerProductMainKindIsEnable: boolean;
}

/** 系統-產品-主分類-更新-Payload */
export interface SystemProductMainKindUpdatePayload {
  managerProductMainKindID: number;
  managerProductMainKindName: string | null;
  managerProductMainKindCommissionRate: number;
  managerProductMainKindIsEnable: boolean;
}

/** 系統-產品-子分類-新增-Payload */
export interface SystemProductSubKindAddPayloadMdl {
  managerProductMainKindID: number;
  managerProductSubKindName: string;
  managerProductSubKindCommissionRate: number;
  managerProductSubKindIsEnable: boolean;
}

/** 系統-產品-子分類-Payload */
export interface SystemProductSubKindUpdatePayload {
  managerProductSubKindID: number;
  managerProductSubKindName: string;
  managerProductSubKindCommissionRate: number;
  managerProductSubKindIsEnable: boolean;
  managerProductMainKindName: string;
}

/** 系統設定-產品-規格-新增-Payload */
export interface SysPrdSpecificationAddPayload {
  managerProductSpecificationName: string;
  managerProductSpecificationSellPrice: number;
  managerProductSpecificationCostPrice: number;
  managerProductSpecificationIsEnable: boolean;
}

/** 系統設定-產品-規格-更新-Payload */
export interface SysPrdSpecificationUpdatePayload {
  managerProductSpecificationID: number;
  managerProductSpecificationName: string;
  managerProductSpecificationSellPrice: number;
  managerProductSpecificationCostPrice: number;
  managerProductSpecificationIsEnable: boolean;
}
