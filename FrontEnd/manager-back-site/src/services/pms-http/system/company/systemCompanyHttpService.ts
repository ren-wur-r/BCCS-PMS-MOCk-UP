import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import {
  MbsSysComHttpGetManyCompanyMainKindReqMdl,
  MbsSysComHttpGetManyCompanyMainKindRspMdl,
  MbsSysComHttpGetManyCompanyMainKindRspItemMdl,
  MbsSysComHttpGetCompanyMainKindReqMdl,
  MbsSysComHttpGetCompanyMainKindRspMdl,
  MbsSysComHttpAddCompanyMainKindReqMdl,
  MbsSysComHttpAddCompanyMainKindRspMdl,
  MbsSysComHttpUpdateCompanyMainKindReqMdl,
  MbsSysComHttpUpdateCompanyMainKindRspMdl,
  MbsSysComHttpGetManyCompanySubKindReqMdl,
  MbsSysComHttpGetManyCompanySubKindRspMdl,
  MbsSysComHttpGetManyCompanySubKindRspItemMdl,
  MbsSysComHttpGetCompanySubKindReqMdl,
  MbsSysComHttpGetCompanySubKindRspMdl,
  MbsSysComHttpAddCompanySubKindReqMdl,
  MbsSysComHttpAddCompanySubKindRspMdl,
  MbsSysComHttpUpdateCompanySubKindReqMdl,
  MbsSysComHttpUpdateCompanySubKindRspMdl,
  MbsSysComHttpGetManyCompanyReqMdl,
  MbsSysComHttpGetManyCompanyRspMdl,
  MbsSysComHttpGetManyCompanyRspItemMdl,
  MbsSysComHttpGetCompanyReqMdl,
  MbsSysComHttpGetCompanyRspMdl,
  MbsSysComHttpAddCompanyReqMdl,
  MbsSysComHttpAddCompanyRspMdl,
  MbsSysComHttpUpdateCompanyReqMdl,
  MbsSysComHttpUpdateCompanyRspMdl,
  MbsSysComHttpGetManyCompanyLocationReqMdl,
  MbsSysComHttpGetManyCompanyLocationRspMdl,
  MbsSysComHttpGetManyCompanyLocationRspItemMdl,
  MbsSysComHttpGetCompanyLocationReqMdl,
  MbsSysComHttpGetCompanyLocationRspMdl,
  MbsSysComHttpAddCompanyLocationReqMdl,
  MbsSysComHttpAddCompanyLocationRspMdl,
  MbsSysComHttpUpdateCompanyLocationReqMdl,
  MbsSysComHttpUpdateCompanyLocationRspMdl,
  MbsSysComHttpGetManyCompanyAffiliateReqMdl,
  MbsSysComHttpGetManyCompanyAffiliateRspMdl,
  MbsSysComHttpGetManyCompanyAffiliateRspItemMdl,
  MbsSysComHttpGetCompanyAffiliateReqMdl,
  MbsSysComHttpGetCompanyAffiliateRspMdl,
  MbsSysComHttpAddCompanyAffiliateReqMdl,
  MbsSysComHttpAddCompanyAffiliateRspMdl,
  MbsSysComHttpUpdateCompanyAffiliateReqMdl,
  MbsSysComHttpUpdateCompanyAffiliateRspMdl,
} from "./systemCompanyHttpFormat";

import {
  MbsSysComCtlGetManyCompanyMainKindReqMdl,
  MbsSysComCtlGetManyCompanyMainKindRspMdl,
  MbsSysComCtlGetManyCompanyMainKindRspItemMdl,
  MbsSysComCtlGetCompanyMainKindReqMdl,
  MbsSysComCtlGetCompanyMainKindRspMdl,
  MbsSysComCtlAddCompanyMainKindReqMdl,
  MbsSysComCtlAddCompanyMainKindRspMdl,
  MbsSysComCtlUpdateCompanyMainKindReqMdl,
  MbsSysComCtlUpdateCompanyMainKindRspMdl,
  MbsSysComCtlGetManyCompanySubKindReqMdl,
  MbsSysComCtlGetManyCompanySubKindRspMdl,
  MbsSysComCtlGetManyCompanySubKindRspItemMdl,
  MbsSysComCtlGetCompanySubKindReqMdl,
  MbsSysComCtlGetCompanySubKindRspMdl,
  MbsSysComCtlAddCompanySubKindReqMdl,
  MbsSysComCtlAddCompanySubKindRspMdl,
  MbsSysComCtlUpdateCompanySubKindReqMdl,
  MbsSysComCtlUpdateCompanySubKindRspMdl,
  MbsSysComCtlGetManyCompanyReqMdl,
  MbsSysComCtlGetManyCompanyRspMdl,
  MbsSysComCtlGetManyCompanyRspItemMdl,
  MbsSysComCtlGetCompanyReqMdl,
  MbsSysComCtlGetCompanyRspMdl,
  MbsSysComCtlAddCompanyReqMdl,
  MbsSysComCtlAddCompanyRspMdl,
  MbsSysComCtlUpdateCompanyReqMdl,
  MbsSysComCtlUpdateCompanyRspMdl,
  MbsSysComCtlGetManyCompanyLocationReqMdl,
  MbsSysComCtlGetManyCompanyLocationRspMdl,
  MbsSysComCtlGetManyCompanyLocationRspItemMdl,
  MbsSysComCtlGetCompanyLocationReqMdl,
  MbsSysComCtlGetCompanyLocationRspMdl,
  MbsSysComCtlAddCompanyLocationReqMdl,
  MbsSysComCtlAddCompanyLocationRspMdl,
  MbsSysComCtlUpdateCompanyLocationReqMdl,
  MbsSysComCtlUpdateCompanyLocationRspMdl,
  MbsSysComCtlGetManyCompanyAffiliateReqMdl,
  MbsSysComCtlGetManyCompanyAffiliateRspMdl,
  MbsSysComCtlGetManyCompanyAffiliateRspItemMdl,
  MbsSysComCtlGetCompanyAffiliateReqMdl,
  MbsSysComCtlGetCompanyAffiliateRspMdl,
  MbsSysComCtlAddCompanyAffiliateReqMdl,
  MbsSysComCtlAddCompanyAffiliateRspMdl,
  MbsSysComCtlUpdateCompanyAffiliateReqMdl,
  MbsSysComCtlUpdateCompanyAffiliateRspMdl,
  MbsSysComCtlAddCompanyAffiliateReqItemMdl,
  MbsSysComCtlAddCompanyLocationReqItemMdl,
} from "./systemCompanyControllerFormat";

//#region 管理者後台-系統-名單公司主分類
//-------------------------------------------------------------------
/** 管理者後台-系統-名單公司主分類-取得多筆 */
export const getManyCompanyMainKind = async (
  requestData: MbsSysComHttpGetManyCompanyMainKindReqMdl
): Promise<MbsSysComHttpGetManyCompanyMainKindRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetManyCompanyMainKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyMainKindName,
    b: requestData.managerCompanyMainKindIsEnable,
    c: requestData.pageIndex,
    d: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetManyCompanyMainKindRspMdl>(
      "/api/MbsSystemCompany/GetManyCompanyMainKind",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpGetManyCompanyMainKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerCompanyMainKindList:
        httpResponseData.data.a?.map(
          (item: MbsSysComCtlGetManyCompanyMainKindRspItemMdl) =>
            ({
              managerCompanyMainKindID: item.a,
              managerCompanyMainKindName: item.b,
              managerCompanyMainKindIsEnable: item.c,
            }) satisfies MbsSysComHttpGetManyCompanyMainKindRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司主分類-取得單筆 */
export const getCompanyMainKind = async (
  requestData: MbsSysComHttpGetCompanyMainKindReqMdl
): Promise<MbsSysComHttpGetCompanyMainKindRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetCompanyMainKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyMainKindID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetCompanyMainKindRspMdl>(
      "/api/MbsSystemCompany/GetCompanyMainKind",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpGetCompanyMainKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyMainKindName: httpResponseData.data.a,
      managerCompanyMainKindIsEnable: httpResponseData.data.b,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司主分類-新增 */
export const addCompanyMainKind = async (
  requestData: MbsSysComHttpAddCompanyMainKindReqMdl
): Promise<MbsSysComHttpAddCompanyMainKindRspMdl | null> => {
  const httpRequestData: MbsSysComCtlAddCompanyMainKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyMainKindName,
    b: requestData.managerCompanyMainKindIsEnable,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlAddCompanyMainKindRspMdl>(
      "/api/MbsSystemCompany/AddCompanyMainKind",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpAddCompanyMainKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyMainKindID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司主分類-更新 */
export const updateCompanyMainKind = async (
  requestData: MbsSysComHttpUpdateCompanyMainKindReqMdl
): Promise<MbsSysComHttpUpdateCompanyMainKindRspMdl | null> => {
  const httpRequestData: MbsSysComCtlUpdateCompanyMainKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyMainKindID,
    b: requestData.managerCompanyMainKindName,
    c: requestData.managerCompanyMainKindIsEnable,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlUpdateCompanyMainKindRspMdl>(
      "/api/MbsSystemCompany/UpdateCompanyMainKind",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpUpdateCompanyMainKindRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//-------------------------------------------------------------------
//#endregion

//#region 管理者後台-系統-名單公司子分類
//-------------------------------------------------------------------
/** 管理者後台-系統-名單公司子分類-取得多筆 */
export const getManyCompanySubKind = async (
  requestData: MbsSysComHttpGetManyCompanySubKindReqMdl
): Promise<MbsSysComHttpGetManyCompanySubKindRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetManyCompanySubKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanySubKindName,
    b: requestData.managerCompanySubKindIsEnable,
    c: requestData.managerCompanyMainKindID,
    d: requestData.pageIndex,
    e: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetManyCompanySubKindRspMdl>(
      "/api/MbsSystemCompany/GetManyCompanySubKind",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpGetManyCompanySubKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerCompanySubKindList:
        httpResponseData.data.a?.map(
          (item: MbsSysComCtlGetManyCompanySubKindRspItemMdl) =>
            ({
              managerCompanySubKindID: item.a,
              managerCompanySubKindName: item.b,
              managerCompanyMainKindID: item.c,
              managerCompanyMainKindName: item.d,
              managerCompanySubKindIsEnable: item.e,
            }) satisfies MbsSysComHttpGetManyCompanySubKindRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司子分類-取得單筆 */
export const getCompanySubKind = async (
  requestData: MbsSysComHttpGetCompanySubKindReqMdl
): Promise<MbsSysComHttpGetCompanySubKindRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetCompanySubKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanySubKindID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetCompanySubKindRspMdl>(
      "/api/MbsSystemCompany/GetCompanySubKind",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpGetCompanySubKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanySubKindName: httpResponseData.data.a,
      managerCompanyMainKindID: httpResponseData.data.b,
      managerCompanySubKindIsEnable: httpResponseData.data.c,
      managerCompanyMainKindName: httpResponseData.data.d,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司子分類-新增 */
export const addCompanySubKind = async (
  requestData: MbsSysComHttpAddCompanySubKindReqMdl
): Promise<MbsSysComHttpAddCompanySubKindRspMdl | null> => {
  const httpRequestData: MbsSysComCtlAddCompanySubKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanySubKindName,
    b: requestData.managerCompanyMainKindID,
    c: requestData.managerCompanySubKindIsEnable,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlAddCompanySubKindRspMdl>(
      "/api/MbsSystemCompany/AddCompanySubKind",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpAddCompanySubKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanySubKindID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司子分類-更新 */
export const updateCompanySubKind = async (
  requestData: MbsSysComHttpUpdateCompanySubKindReqMdl
): Promise<MbsSysComHttpUpdateCompanySubKindRspMdl | null> => {
  const httpRequestData: MbsSysComCtlUpdateCompanySubKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanySubKindID,
    b: requestData.managerCompanySubKindName,
    c: requestData.managerCompanySubKindIsEnable,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlUpdateCompanySubKindRspMdl>(
      "/api/MbsSystemCompany/UpdateCompanySubKind",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpUpdateCompanySubKindRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//-------------------------------------------------------------------
//#endregion

//#region 管理者後台-系統-名單公司
/** 管理者後台-系統-名單公司-取得多筆 */
export const getManyCompany = async (
  requestData: MbsSysComHttpGetManyCompanyReqMdl
): Promise<MbsSysComHttpGetManyCompanyRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetManyCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.atomCustomerGrade,
    b: requestData.managerCompanyMainKindID,
    c: requestData.managerCompanySubKindID,
    d: requestData.managerCompanyName,
    e: requestData.pageIndex,
    f: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetManyCompanyRspMdl>(
      "/api/MbsSystemCompany/GetManyCompany",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpGetManyCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerCompanyList:
        httpResponseData.data.a?.map(
          (item: MbsSysComCtlGetManyCompanyRspItemMdl) =>
            ({
              managerCompanyID: item.a,
              managerCompanyUnifiedNumber: item.b,
              managerCompanyName: item.c,
              atomCustomerGrade: item.d,
              managerCompanyMainKindName: item.e,
              managerCompanySubKindName: item.f,
            }) satisfies MbsSysComHttpGetManyCompanyRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司-取得單筆 */
export const getCompany = async (
  requestData: MbsSysComHttpGetCompanyReqMdl
): Promise<MbsSysComHttpGetCompanyRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetCompanyRspMdl>(
      "/api/MbsSystemCompany/GetCompany",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpGetCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyUnifiedNumber: httpResponseData.data.a,
      managerCompanyName: httpResponseData.data.b,
      atomManagerCompanyStatus: httpResponseData.data.c,
      managerCompanyMainKindID: httpResponseData.data.d,
      managerCompanyMainKindName: httpResponseData.data.e,
      managerCompanySubKindID: httpResponseData.data.f,
      managerCompanySubKindName: httpResponseData.data.g,
      atomCustomerGrade: httpResponseData.data.h,
      atomSecurityGrade: httpResponseData.data.i,
      atomEmployeeRange: httpResponseData.data.j,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司-新增 */
export const addCompany = async (
  requestData: MbsSysComHttpAddCompanyReqMdl
): Promise<MbsSysComHttpAddCompanyRspMdl | null> => {
  const httpRequestData: MbsSysComCtlAddCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyUnifiedNumber,
    b: requestData.managerCompanyName,
    c: requestData.atomManagerCompanyStatus,
    d: requestData.managerCompanyMainKindID,
    e: requestData.managerCompanySubKindID,
    f: requestData.atomCustomerGrade,
    g: requestData.atomSecurityGrade,
    h: requestData.atomEmployeeRange,
    i: requestData.managerCompanyAffiliateList.map(
      (item: MbsSysComHttpAddCompanyAffiliateReqMdl) => {
        return {
          a: item.managerCompanyAffiliateUnifiedNumber,
          b: item.managerCompanyAffiliateName,
        } satisfies MbsSysComCtlAddCompanyAffiliateReqItemMdl;
      }
    ),
    j: requestData.managerCompanyLocationList.map((item: MbsSysComHttpAddCompanyLocationReqMdl) => {
      return {
        a: item.managerCompanyLocationName,
        b: item.atomCity,
        c: item.managerCompanyLocationAddress,
        d: item.managerCompanyLocationTelephone,
        e: item.atomManagerCompanyStatus,
      } satisfies MbsSysComCtlAddCompanyLocationReqItemMdl;
    }),
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlAddCompanyRspMdl>(
      "/api/MbsSystemCompany/AddCompany",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpAddCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單公司-更新 */
export const updateCompany = async (
  requestData: MbsSysComHttpUpdateCompanyReqMdl
): Promise<MbsSysComHttpUpdateCompanyRspMdl | null> => {
  const httpRequestData: MbsSysComCtlUpdateCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
    b: requestData.managerCompanyUnifiedNumber,
    c: requestData.managerCompanyName,
    d: requestData.atomManagerCompanyStatus,
    e: requestData.managerCompanyMainKindID,
    f: requestData.managerCompanySubKindID,
    g: requestData.atomCustomerGrade,
    h: requestData.atomSecurityGrade,
    i: requestData.atomEmployeeRange,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlUpdateCompanyRspMdl>(
      "/api/MbsSystemCompany/UpdateCompany",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpUpdateCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 管理者後台-系統-名單公司營業地點
/** 管理者後台-客戶公司營業地點-取得多筆 */
export const getManyCompanyLocation = async (
  requestData: MbsSysComHttpGetManyCompanyLocationReqMdl
): Promise<MbsSysComHttpGetManyCompanyLocationRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetManyCompanyLocationReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetManyCompanyLocationRspMdl>(
      "/api/MbsSystemCompany/GetManyCompanyLocation",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpGetManyCompanyLocationRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyLocationList:
        httpResponseData.data.a?.map(
          (item: MbsSysComCtlGetManyCompanyLocationRspItemMdl) =>
            ({
              managerCompanyLocationID: item.a,
              managerCompanyLocationName: item.b,
              atomCity: item.c,
              managerCompanyLocationAddress: item.d,
              managerCompanyLocationTelephone: item.e,
              atomManagerCompanyStatus: item.f,
            }) satisfies MbsSysComHttpGetManyCompanyLocationRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-客戶公司營業地點-取得單筆 */
export const getCompanyLocation = async (
  requestData: MbsSysComHttpGetCompanyLocationReqMdl
): Promise<MbsSysComHttpGetCompanyLocationRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetCompanyLocationReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyLocationID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetCompanyLocationRspMdl>(
      "/api/MbsSystemCompany/GetCompanyLocation",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const data = httpResponseData.data;

    const responseData: MbsSysComHttpGetCompanyLocationRspMdl = {
      errorCode: data.aa,
      managerCompanyLocationID: data.a,
      managerCompanyLocationName: data.b,
      atomCity: data.c,
      managerCompanyLocationAddress: data.d,
      managerCompanyLocationTelephone: data.e,
      atomManagerCompanyStatus: data.f,
      managerCompanyLocationIsDeleted: data.g,
    };

    return responseData;
  } catch (error: unknown) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-客戶公司營業地點-新增 */
export const addCompanyLocation = async (
  requestData: MbsSysComHttpAddCompanyLocationReqMdl
): Promise<MbsSysComHttpAddCompanyLocationRspMdl | null> => {
  const httpRequestData: MbsSysComCtlAddCompanyLocationReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
    b: requestData.managerCompanyLocationName,
    c: requestData.atomCity,
    d: requestData.managerCompanyLocationAddress,
    e: requestData.managerCompanyLocationTelephone,
    f: requestData.atomManagerCompanyStatus,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlAddCompanyLocationRspMdl>(
      "/api/MbsSystemCompany/AddCompanyLocation",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      managerCompanyLocationID: httpResponseData.data.a,
    };
  } catch (error: unknown) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-客戶公司營業地點-更新 */
export const updateCompanyLocation = async (
  requestData: MbsSysComHttpUpdateCompanyLocationReqMdl
): Promise<MbsSysComHttpUpdateCompanyLocationRspMdl | null> => {
  const httpRequestData: MbsSysComCtlUpdateCompanyLocationReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyLocationID,
    b: requestData.managerCompanyLocationName,
    c: requestData.atomCity,
    d: requestData.managerCompanyLocationAddress,
    e: requestData.managerCompanyLocationTelephone,
    f: requestData.atomManagerCompanyStatus,
    g: requestData.managerCompanyLocationIsDeleted,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlUpdateCompanyLocationRspMdl>(
      "/api/MbsSystemCompany/UpdateCompanyLocation",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    };
  } catch (error: unknown) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//#endregion

//#region 管理者後台-系統-名單公司關係
/** 管理者後台-客戶公司關係-取得多筆 */
export const getManyCompanyAffiliate = async (
  requestData: MbsSysComHttpGetManyCompanyAffiliateReqMdl
): Promise<MbsSysComHttpGetManyCompanyAffiliateRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetManyCompanyAffiliateReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetManyCompanyAffiliateRspMdl>(
      "/api/MbsSystemCompany/GetManyCompanyAffiliate",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysComHttpGetManyCompanyAffiliateRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyAffiliateList:
        httpResponseData.data.a?.map(
          (item: MbsSysComCtlGetManyCompanyAffiliateRspItemMdl) =>
            ({
              managerCompanyAffiliateID: item.a,
              managerCompanyAffiliateUnifiedNumber: item.b,
              managerCompanyAffiliateName: item.c,
            }) satisfies MbsSysComHttpGetManyCompanyAffiliateRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-客戶公司關係-取得單筆 */
export const getCompanyAffiliate = async (
  requestData: MbsSysComHttpGetCompanyAffiliateReqMdl
): Promise<MbsSysComHttpGetCompanyAffiliateRspMdl | null> => {
  const httpRequestData: MbsSysComCtlGetCompanyAffiliateReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyAffiliateID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlGetCompanyAffiliateRspMdl>(
      "/api/MbsSystemCompany/GetCompanyAffiliate",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const data = httpResponseData.data;

    return {
      errorCode: data.aa,
      managerCompanyAffiliateID: data.a,
      managerCompanyAffiliateUnifiedNumber: data.b,
      managerCompanyAffiliateName: data.c,
    };
  } catch (error: unknown) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-客戶公司關係-新增 */
export const addCompanyAffiliate = async (
  requestData: MbsSysComHttpAddCompanyAffiliateReqMdl
): Promise<MbsSysComHttpAddCompanyAffiliateRspMdl | null> => {
  const httpRequestData: MbsSysComCtlAddCompanyAffiliateReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
    b: requestData.managerCompanyAffiliateUnifiedNumber,
    c: requestData.managerCompanyAffiliateName,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlAddCompanyAffiliateRspMdl>(
      "/api/MbsSystemCompany/AddCompanyAffiliate",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      managerCompanyAffiliateID: httpResponseData.data.a,
    };
  } catch (error: unknown) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-客戶公司關係-更新 */
export const updateCompanyAffiliate = async (
  requestData: MbsSysComHttpUpdateCompanyAffiliateReqMdl
): Promise<MbsSysComHttpUpdateCompanyAffiliateRspMdl | null> => {
  const httpRequestData: MbsSysComCtlUpdateCompanyAffiliateReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyAffiliateID,
    b: requestData.managerCompanyAffiliateUnifiedNumber,
    c: requestData.managerCompanyAffiliateName,
    d: requestData.managerCompanyAffiliateIsDeleted,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysComCtlUpdateCompanyAffiliateRspMdl>(
      "/api/MbsSystemCompany/UpdateCompanyAffiliate",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    };
  } catch (error: unknown) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//#endregion
