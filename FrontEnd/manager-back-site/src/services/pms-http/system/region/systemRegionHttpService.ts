import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import type {
  MbsSysRgnCtlGetManyReqMdl,
  MbsSysRgnCtlGetManyRspMdl,
  MbsSysRgnCtlGetManyRspItemMdl,
  MbsSysRgnCtlAddReqMdl,
  MbsSysRgnCtlAddRspMdl,
  MbsSysRgnCtlUpdateReqMdl,
  MbsSysRgnCtlUpdateRspMdl,
  MbsSysRgnCtlGetReqMdl,
  MbsSysRgnCtlGetRspMdl,
} from "./systemRegionControllerFormat";
import type {
  MbsSysRgnHttpGetManyReqMdl,
  MbsSysRgnHttpGetManyRspMdl,
  MbsSysRgnHttpGetManyRspItemMdl,
  MbsSysRgnHttpAddReqMdl,
  MbsSysRgnHttpAddRspMdl,
  MbsSysRgnHttpUpdateReqMdl,
  MbsSysRgnHttpUpdateRspMdl,
  MbsSysRgnHttpGetReqMdl,
  MbsSysRgnHttpGetRspMdl,
} from "./systemRegionHttpFormat";

/** 管理者後台-系統-地區-取得多筆 */
export const getManySystemRegion = async (
  requestData: MbsSysRgnHttpGetManyReqMdl
): Promise<MbsSysRgnHttpGetManyRspMdl | null> => {
  const httpRequestData: MbsSysRgnCtlGetManyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRegionName,
    b: requestData.managerRegionIsEnable,
    c: requestData.pageIndex,
    d: requestData.pageSize,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsSysRgnCtlGetManyRspMdl>(
        "/api/MbsSystemRegion/GetMany",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysRgnHttpGetManyRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerRegionList:
        httpResponseData.data.a?.map(
          (item: MbsSysRgnCtlGetManyRspItemMdl) =>
            ({
              managerRegionID: item.a,
              managerRegionName: item.b,
              managerRegionIsEnable: item.c,
            }) satisfies MbsSysRgnHttpGetManyRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-地區-取得單筆 */
export const getSystemRegion = async (
  requestData: MbsSysRgnHttpGetReqMdl
): Promise<MbsSysRgnHttpGetRspMdl | null> => {
  const httpRequestData: MbsSysRgnCtlGetReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRegionID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysRgnCtlGetRspMdl>(
      "/api/MbsSystemRegion/Get",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysRgnHttpGetRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerRegionName: httpResponseData.data.a,
      managerRegionIsEnable: httpResponseData.data.b,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-地區-新增 */
export const   addSystemRegion = async (
  requestData: MbsSysRgnHttpAddReqMdl
): Promise<MbsSysRgnHttpAddRspMdl | null> => {
  const httpRequestData: MbsSysRgnCtlAddReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRegionName,
    b: requestData.managerRegionIsEnable,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysRgnCtlAddRspMdl>(
      "/api/MbsSystemRegion/Add",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysRgnHttpAddRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-地區-更新 */
export const updateSystemRegion = async (
  requestData: MbsSysRgnHttpUpdateReqMdl
): Promise<MbsSysRgnHttpUpdateRspMdl | null> => {
  const httpRequestData: MbsSysRgnCtlUpdateReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRegionID,
    b: requestData.managerRegionName,
    c: requestData.managerRegionIsEnable,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysRgnCtlUpdateRspMdl>(
      "/api/MbsSystemRegion/Update",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysRgnHttpUpdateRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
