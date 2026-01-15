import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import type {
  MbsSysDptCtlGetManyReqMdl,
  MbsSysDptCtlGetManyRspMdl,
  MbsSysDptCtlGetManyRspItemMdl,
  MbsSysDptCtlAddReqMdl,
  MbsSysDptCtlAddRspMdl,
  MbsSysDptCtlUpdateReqMdl,
  MbsSysDptCtlUpdateRspMdl,
  MbsSysDptCtlGetReqMdl,
  MbsSysDptCtlGetRspMdl,
} from "./systemDepartmentControllerFormat";
import type {
  MbsSysDptHttpGetManyReqMdl,
  MbsSysDptHttpGetManyRspMdl,
  MbsSysDptHttpGetManyRspItemMdl,
  MbsSysDptHttpAddReqMdl,
  MbsSysDptHttpAddRspMdl,
  MbsSysDptHttpUpdateReqMdl,
  MbsSysDptHttpUpdateRspMdl,
  MbsSysDptHttpGetReqMdl,
  MbsSysDptHttpGetRspMdl,
} from "./systemDepartmentHttpFormat";

/** 管理者後台-系統-部門-取得多筆 */
export const getManySystemDepartment = async (
  requestData: MbsSysDptHttpGetManyReqMdl
): Promise<MbsSysDptHttpGetManyRspMdl | null> => {
  const httpRequestData: MbsSysDptCtlGetManyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerDepartmentName,
    b: requestData.managerDepartmentIsEnable,
    c: requestData.pageIndex,
    d: requestData.pageSize,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsSysDptCtlGetManyRspMdl>(
        "/api/MbsSystemDepartment/GetMany",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysDptHttpGetManyRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerDepartmentList:
        httpResponseData.data.a?.map(
          (item: MbsSysDptCtlGetManyRspItemMdl) =>
            ({
              managerDepartmentID: item.a,
              managerDepartmentName: item.b,
              managerDepartmentIsEnable: item.c,
            }) satisfies MbsSysDptHttpGetManyRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-部門-取得單筆 */
export const getSystemDepartment = async (
  requestData: MbsSysDptHttpGetReqMdl
): Promise<MbsSysDptHttpGetRspMdl | null> => {
  const httpRequestData: MbsSysDptCtlGetReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerDepartmentID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysDptCtlGetRspMdl>(
      "/api/MbsSystemDepartment/Get",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysDptHttpGetRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerDepartmentName: httpResponseData.data.a,
      managerDepartmentIsEnable: httpResponseData.data.b,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-部門-新增 */
export const addSystemDepartment = async (
  requestData: MbsSysDptHttpAddReqMdl
): Promise<MbsSysDptHttpAddRspMdl | null> => {
  const httpRequestData: MbsSysDptCtlAddReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerDepartmentName,
    b: requestData.managerDepartmentIsEnable,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysDptCtlAddRspMdl>(
      "/api/MbsSystemDepartment/Add",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysDptHttpAddRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-部門-更新 */
export const updateSystemDepartment = async (
  requestData: MbsSysDptHttpUpdateReqMdl
): Promise<MbsSysDptHttpUpdateRspMdl | null> => {
  const httpRequestData: MbsSysDptCtlUpdateReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerDepartmentID,
    b: requestData.managerDepartmentName,
    c: requestData.managerDepartmentIsEnable,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysDptCtlUpdateRspMdl>(
      "/api/MbsSystemDepartment/Update",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysDptHttpUpdateRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
