import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import type {
  MbsSysPrdHttpGetManyMainKindReqMdl,
  MbsSysPrdHttpGetManyMainKindRspMdl,
  MbsSysPrdHttpGetMainKindReqMdl,
  MbsSysPrdHttpGetMainKindRspMdl,
  MbsSysPrdHttpAddMainKindReqMdl,
  MbsSysPrdHttpAddMainKindRspMdl,
  MbsSysPrdHttpUpdateMainKindReqMdl,
  MbsSysPrdHttpUpdateMainKindRspMdl,
  MbsSysPrdHttpGetManySubKindReqMdl,
  MbsSysPrdHttpGetManySubKindRspMdl,
  MbsSysPrdHttpGetSubKindReqMdl,
  MbsSysPrdHttpGetSubKindRspMdl,
  MbsSysPrdHttpAddSubKindReqMdl,
  MbsSysPrdHttpAddSubKindRspMdl,
  MbsSysPrdHttpUpdateSubKindReqMdl,
  MbsSysPrdHttpUpdateSubKindRspMdl,
  MbsSysPrdHttpGetReqMdl,
  MbsSysPrdHttpGetRspMdl,
  MbsSysPrdHttpAddReqMdl,
  MbsSysPrdHttpAddRspMdl,
  MbsSysPrdHttpUpdateReqMdl,
  MbsSysPrdHttpUpdateRspMdl,
  MbsSysPrdHttpAddSpecificationReqMdl,
  MbsSysPrdHttpAddSpecificationRspMdl,
  MbsSysPrdHttpUpdateSpecificationReqMdl,
  MbsSysPrdHttpUpdateSpecificationRspMdl,
  MbsSysPrdHttpGetManyMainKindRspItemMdl,
  MbsSysPrdHttpGetManyRspMdl,
  MbsSysPrdHttpGetManyReqMdl,
  MbsSysPrdHttpGetManyRspItemMdl,
  MbsSysPrdHttpAddSpecificationReqItemMdl,
  MbsSysPrdHttpGetSpecificationRspItemMdl,
} from "@/services/pms-http/system/product/systemProductHttpFormat";
import {
  MbsSysPrdCtlAddSpecificationReqItemMdl,
  MbsSysPrdCtlGetManyMainKindRspItemMdl,
  MbsSysPrdCtlGetManyRspItemMdl,
  MbsSysPrdCtlGetManySubKindRspItemMdl,
  MbsSysPrdCtlGetSpecificationRspItemMdl,
} from "./systemProductControllerFormat";

//#region 管理者後台-系統-產品
//-----------------------------------------------------------
/** 管理者後台-系統-產品-取得多筆 */
export const getManyProduct = async (
  requestData: MbsSysPrdHttpGetManyReqMdl
): Promise<MbsSysPrdHttpGetManyRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindID,
    b: requestData.managerProductSubKindID,
    c: requestData.managerProductIsKey,
    d: requestData.managerProductName,
    e: requestData.managerProductSpecificationName,
    f: requestData.managerProductSpecificationIsEnable,
    g: requestData.pageIndex,
    h: requestData.pageSize,
  };

  try {
    const httpResponse = await apiJsonClient.post("/api/MbsSystemProduct/GetMany", httpRequestData);

    if (!httpResponse?.data) return null;
    const responseData: MbsSysPrdHttpGetManyRspMdl = {
      errorCode: httpResponse.data.aa,
      totalCount: httpResponse.data.b,
      managerProductList:
        httpResponse.data.a?.map(
          (item: MbsSysPrdCtlGetManyRspItemMdl) =>
            ({
              managerProductID: item.a,
              managerProductName: item.b,
              managerProductMainKindName: item.c,
              managerProductSubKindName: item.d,
              managerProductIsKey: item.e,
              managerProductSpecificationName: item.f,
              managerProductSpecificationSellPrice: item.g,
              managerProductSpecificationCostPrice: item.h,
              managerProductSpecificationIsEnable: item.i,
            }) satisfies MbsSysPrdHttpGetManyRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品-取得單筆 */
export const getProduct = async (
  requestData: MbsSysPrdHttpGetReqMdl
): Promise<MbsSysPrdHttpGetRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductID,
  };

  try {
    const httpResponse = await apiJsonClient.post("/api/MbsSystemProduct/Get", httpRequestData);

    if (!httpResponse?.data) return null;
    const responseData: MbsSysPrdHttpGetRspMdl = {
      errorCode: httpResponse.data.aa,
      managerProductName: httpResponse.data.a,
      managerProductMainKindID: httpResponse.data.b,
      managerProductMainKindName: httpResponse.data.c,
      managerProductSubKindID: httpResponse.data.d,
      managerProductSubKindName: httpResponse.data.e,
      managerProductKind: httpResponse.data.f,
      managerProductIsKey: httpResponse.data.g,
      managerProductRemark: httpResponse.data.h,
      managerProductIsEnable: httpResponse.data.i,
      managerProductSpecificationList:
        httpResponse.data.j?.map(
          (item: MbsSysPrdCtlGetSpecificationRspItemMdl): MbsSysPrdHttpGetSpecificationRspItemMdl =>
            ({
              managerProductSpecificationID: item.a,
              managerProductSpecificationName: item.b,
              managerProductSpecificationSellPrice: item.c,
              managerProductSpecificationCostPrice: item.d,
              managerProductSpecificationIsEnable: item.e,
            }) satisfies MbsSysPrdHttpGetSpecificationRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品-新增 */
export const addProduct = async (
  requestData: MbsSysPrdHttpAddReqMdl
): Promise<MbsSysPrdHttpAddRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductName,
    b: requestData.managerProductMainKindID,
    c: requestData.managerProductSubKindID,
    d: requestData.managerProductKind,
    e: requestData.managerProductIsKey,
    f: requestData.managerProductRemark,
    g: requestData.managerProductIsEnable,
    h: requestData.managerProductSpecificationList.map(
      (item: MbsSysPrdHttpAddSpecificationReqItemMdl) => {
        return {
          a: item.managerProductSpecificationName,
          b: item.managerProductSpecificationSellPrice,
          c: item.managerProductSpecificationCostPrice,
          d: item.managerProductSpecificationIsEnable,
        } satisfies MbsSysPrdCtlAddSpecificationReqItemMdl;
      }
    ),
  };

  try {
    const httpResponse = await apiJsonClient.post("/api/MbsSystemProduct/Add", httpRequestData);
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpAddRspMdl = {
      errorCode: httpResponse.data.aa,
      managerProductID: httpResponse.data.a,
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品-更新 */
export const updateProduct = async (
  requestData: MbsSysPrdHttpUpdateReqMdl
): Promise<MbsSysPrdHttpUpdateRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductID,
    b: requestData.managerProductName,
    c: requestData.managerProductMainKindID,
    d: requestData.managerProductSubKindID,
    e: requestData.managerProductKind,
    f: requestData.managerProductIsKey,
    g: requestData.managerProductRemark,
    h: requestData.managerProductIsEnable,
  };

  try {
    const httpResponse = await apiJsonClient.post("/api/MbsSystemProduct/Update", httpRequestData);

    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpUpdateRspMdl = {
      errorCode: httpResponse.data.aa,
    };
    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

//-----------------------------------------------------------
//#endregion

//#region 管理者後台-系統-產品-產品規格
//-----------------------------------------------------------
/** 管理者後台-系統-產品-新增產品規格 */
export const addProductSpecification = async (
  requestData: MbsSysPrdHttpAddSpecificationReqMdl
): Promise<MbsSysPrdHttpAddSpecificationRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductID,
    b: requestData.managerProductSpecificationName,
    c: requestData.managerProductSpecificationSellPrice,
    d: requestData.managerProductSpecificationCostPrice,
    e: requestData.managerProductSpecificationIsEnable,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/AddSpecification",
      httpRequestData
    );
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpAddSpecificationRspMdl = {
      errorCode: httpResponse.data.aa,
      managerProductSpecificationID: httpResponse.data.a,
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品-更新產品規格 */
export const updateProductSpecification = async (
  requestData: MbsSysPrdHttpUpdateSpecificationReqMdl
): Promise<MbsSysPrdHttpUpdateSpecificationRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductID,
    b: requestData.managerProductSpecificationID,
    c: requestData.managerProductSpecificationName,
    d: requestData.managerProductSpecificationSellPrice,
    e: requestData.managerProductSpecificationCostPrice,
    f: requestData.managerProductSpecificationIsEnable,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/UpdateSpecification",
      httpRequestData
    );

    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpUpdateSpecificationRspMdl = {
      errorCode: httpResponse.data.aa,
    };
    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//-----------------------------------------------------------
//#endregion

//#region 管理者後台-系統-產品主分類
//-----------------------------------------------------------
/** 管理者後台-系統-產品主分類-取得多筆 */
export const getManyProductMainKind = async (
  requestData: MbsSysPrdHttpGetManyMainKindReqMdl
): Promise<MbsSysPrdHttpGetManyMainKindRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindName,
    b: requestData.managerProductMainKindIsEnable,
    c: requestData.pageIndex,
    d: requestData.pageSize,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/GetManyMainKind",
      httpRequestData
    );

    if (!httpResponse?.data) return null;
    const responseData: MbsSysPrdHttpGetManyMainKindRspMdl = {
      errorCode: httpResponse.data.aa,
      totalCount: httpResponse.data.b,
      managerProductMainKindList:
        httpResponse.data.a?.map(
          (item: MbsSysPrdCtlGetManyMainKindRspItemMdl) =>
            ({
              managerProductMainKindID: item.a,
              managerProductMainKindName: item.b,
              managerProductMainKindCommissionRate: item.c,
              managerProductMainKindIsEnable: item.d,
            }) satisfies MbsSysPrdHttpGetManyMainKindRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品主分類-取得單筆 */
export const getProductMainKind = async (
  requestData: MbsSysPrdHttpGetMainKindReqMdl
): Promise<MbsSysPrdHttpGetMainKindRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindID,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/GetMainKind",
      httpRequestData
    );
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpGetMainKindRspMdl = {
      errorCode: httpResponse.data.aa,
      managerProductMainKindID: httpResponse.data.a,
      managerProductMainKindName: httpResponse.data.b,
      managerProductMainKindCommissionRate: httpResponse.data.c,
      managerProductMainKindIsEnable: httpResponse.data.d,
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品主分類-新增 */
export const addProductMainKind = async (
  requestData: MbsSysPrdHttpAddMainKindReqMdl
): Promise<MbsSysPrdHttpAddMainKindRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindName,
    b: requestData.managerProductMainKindCommissionRate,
    c: requestData.managerProductMainKindIsEnable,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/AddMainKind",
      httpRequestData
    );
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpAddMainKindRspMdl = {
      errorCode: httpResponse.data.aa,
      managerProductMainKindID: httpResponse.data.a,
    };
    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品主分類-更新 */
export const updateProductMainKind = async (
  requestData: MbsSysPrdHttpUpdateMainKindReqMdl
): Promise<MbsSysPrdHttpUpdateMainKindRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindID,
    b: requestData.managerProductMainKindName,
    c: requestData.managerProductMainKindCommissionRate,
    d: requestData.managerProductMainKindIsEnable,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/UpdateMainKind",
      httpRequestData
    );
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpUpdateMainKindRspMdl = {
      errorCode: httpResponse.data.aa,
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//-----------------------------------------------------------
//#endregion

//#region 管理者後台-系統-產品-產品子分類
//-----------------------------------------------------------
/** 管理者後台-系統-產品子分類-取得多筆 */
export const getManyProductSubKind = async (
  requestData: MbsSysPrdHttpGetManySubKindReqMdl
): Promise<MbsSysPrdHttpGetManySubKindRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindID,
    b: requestData.managerProductSubKindName,
    c: requestData.managerProductSubKindIsEnable,
    d: requestData.pageIndex,
    e: requestData.pageSize,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/GetManySubKind",
      httpRequestData
    );
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpGetManySubKindRspMdl = {
      errorCode: httpResponse.data.aa,
      totalCount: httpResponse.data.b,
      managerProductSubKindList:
        httpResponse.data.a?.map((item: MbsSysPrdCtlGetManySubKindRspItemMdl) => ({
          managerProductMainKindID: item.a,
          managerProductMainKindName: item.b,
          managerProductSubKindID: item.c,
          managerProductSubKindName: item.d,
          managerProductSubKindCommissionRate: item.e,
          managerProductSubKindIsEnable: item.f,
        })) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品-產品子分類-取得單筆 */
export const getProductSubKind = async (
  requestData: MbsSysPrdHttpGetSubKindReqMdl
): Promise<MbsSysPrdHttpGetSubKindRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductSubKindID,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/GetSubKind",
      httpRequestData
    );
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpGetSubKindRspMdl = {
      errorCode: httpResponse.data.aa,
      managerProductMainKindName: httpResponse.data.a,
      managerProductSubKindName: httpResponse.data.b,
      managerProductSubKindCommissionRate: httpResponse.data.c,
      managerProductSubKindIsEnable: httpResponse.data.d,
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品-產品子分類-新增 */
export const addProductSubKind = async (
  requestData: MbsSysPrdHttpAddSubKindReqMdl
): Promise<MbsSysPrdHttpAddSubKindRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindID,
    b: requestData.managerProductSubKindName,
    c: requestData.managerProductSubKindCommissionRate,
    d: requestData.managerProductSubKindIsEnable,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/AddSubKind",
      httpRequestData
    );
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpAddSubKindRspMdl = {
      errorCode: httpResponse.data.aa,
      managerProductSubKindID: httpResponse.data.a,
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-產品-產品子分類-更新 */
export const updateProductSubKind = async (
  requestData: MbsSysPrdHttpUpdateSubKindReqMdl
): Promise<MbsSysPrdHttpUpdateSubKindRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductSubKindID,
    b: requestData.managerProductSubKindName,
    c: requestData.managerProductSubKindCommissionRate,
    d: requestData.managerProductSubKindIsEnable,
  };

  try {
    const httpResponse = await apiJsonClient.post(
      "/api/MbsSystemProduct/UpdateSubKind",
      httpRequestData
    );
    if (!httpResponse?.data) return null;

    const responseData: MbsSysPrdHttpUpdateSubKindRspMdl = {
      errorCode: httpResponse.data.aa,
    };

    return responseData;
  } catch (error) {
    console.error("API 錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//-----------------------------------------------------------
//#endregion
