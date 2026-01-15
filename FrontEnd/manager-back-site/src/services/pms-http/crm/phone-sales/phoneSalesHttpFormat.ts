import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomPhoneSalesCustomerStatusEnum } from "@/constants/DbAtomPhoneSalesCustomerStatusEnum";
import { DbAtomPhoneSalesCustomerValueEnum } from "@/constants/DbAtomPhoneSalesCustomerValueEnum";
import { DbAtomPhoneSalesDataSourceEnum } from "@/constants/DbAtomPhoneSalesDataSourceEnum";

export interface MbsCrmPhsHttpGetManyCustomerReqMdl {
  employeeLoginToken: string;
  atomPhoneSalesCustomerStatus: DbAtomPhoneSalesCustomerStatusEnum | null;
  atomPhoneSalesCustomerValue: DbAtomPhoneSalesCustomerValueEnum | null;
  companyName: string | null;
  industry: string | null;
  contacterName: string | null;
  contacterEmail: string | null;
  isExistingCustomer: boolean | null;
  pageIndex: number;
  pageSize: number;
}

export interface MbsCrmPhsHttpGetManyCustomerRspMdl {
  errorCode: MbsErrorCodeEnum;
  customerList: MbsCrmPhsHttpGetManyCustomerRspItemMdl[];
  totalCount: number;
}

export interface MbsCrmPhsHttpGetManyCustomerRspItemMdl {
  id: number;
  companyName: string;
  industry: string;
  atomPhoneSalesCustomerStatus: DbAtomPhoneSalesCustomerStatusEnum;
  atomPhoneSalesCustomerValue: DbAtomPhoneSalesCustomerValueEnum;
  atomPhoneSalesDataSource: DbAtomPhoneSalesDataSourceEnum;
  isExistingCustomer: boolean;
  customOrder: number;
  contacterList: MbsCrmPhsHttpGetManyCustomerRspContacterItemMdl[];
}

export interface MbsCrmPhsHttpGetManyCustomerRspContacterItemMdl {
  contacterName: string;
  contacterJobTitle: string;
  contacterPhone: string;
  contacterEmail: string;
}

export interface MbsCrmPhsHttpGetCustomerReqMdl {
  employeeLoginToken: string;
  id: number;
}

export interface MbsCrmPhsHttpGetCustomerRspMdl {
  errorCode: MbsErrorCodeEnum;
  id: number;
  companyName: string;
  unifiedNumber: string;
  industry: string;
  companyAddress: string;
  companyPhone: string;
  companyWebsite: string;
  atomPhoneSalesCustomerStatus: DbAtomPhoneSalesCustomerStatusEnum;
  atomPhoneSalesCustomerValue: DbAtomPhoneSalesCustomerValueEnum;
  atomPhoneSalesDataSource: DbAtomPhoneSalesDataSourceEnum;
  isExistingCustomer: boolean;
  remark: string;
  contacterList: MbsCrmPhsHttpGetCustomerRspContacterItemMdl[];
}

export interface MbsCrmPhsHttpGetCustomerRspContacterItemMdl {
  id: number;
  contacterName: string;
  contacterJobTitle: string;
  contacterPhone: string;
  contacterEmail: string;
  contacterLineID: string;
  isPrimary: boolean;
}

export interface MbsCrmPhsHttpAddCustomerReqMdl {
  employeeLoginToken: string;
  companyName: string;
  unifiedNumber: string;
  industry: string;
  companyAddress: string;
  companyPhone: string;
  companyWebsite: string;
  atomPhoneSalesCustomerValue: DbAtomPhoneSalesCustomerValueEnum;
  atomPhoneSalesDataSource: DbAtomPhoneSalesDataSourceEnum;
  isExistingCustomer: boolean;
  remark: string;
  contacterList: MbsCrmPhsHttpAddCustomerReqContacterItemMdl[];
}

export interface MbsCrmPhsHttpAddCustomerReqContacterItemMdl {
  contacterName: string;
  contacterJobTitle: string;
  contacterPhone: string;
  contacterEmail: string;
  contacterLineID: string;
  isPrimary: boolean;
}

export interface MbsCrmPhsHttpAddCustomerRspMdl {
  errorCode: MbsErrorCodeEnum;
  customerID: number;
}

export interface MbsCrmPhsHttpUpdateCustomerReqMdl {
  employeeLoginToken: string;
  id: number;
  companyName: string;
  unifiedNumber: string;
  industry: string;
  companyAddress: string;
  companyPhone: string;
  companyWebsite: string;
  atomPhoneSalesCustomerStatus: DbAtomPhoneSalesCustomerStatusEnum;
  atomPhoneSalesCustomerValue: DbAtomPhoneSalesCustomerValueEnum;
  isExistingCustomer: boolean;
  remark: string;
  contacterList: MbsCrmPhsHttpUpdateCustomerReqContacterItemMdl[];
}

export interface MbsCrmPhsHttpUpdateCustomerReqContacterItemMdl {
  id: number | null;
  contacterName: string;
  contacterJobTitle: string;
  contacterPhone: string;
  contacterEmail: string;
  contacterLineID: string;
  isPrimary: boolean;
}

export interface MbsCrmPhsHttpUpdateCustomerRspMdl {
  errorCode: MbsErrorCodeEnum;
}

export interface MbsCrmPhsHttpDeleteCustomerReqMdl {
  employeeLoginToken: string;
  id: number;
}

export interface MbsCrmPhsHttpDeleteCustomerRspMdl {
  errorCode: MbsErrorCodeEnum;
}

export interface MbsCrmPhsHttpUpdateCustomerOrderReqMdl {
  employeeLoginToken: string;
  customerIDList: number[];
}

export interface MbsCrmPhsHttpUpdateCustomerOrderRspMdl {
  errorCode: MbsErrorCodeEnum;
}
