import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import type {
  MbsSysEmpCtlGetManyReqMdl,
  MbsSysEmpCtlGetManyRspMdl,
  MbsSysEmpCtlAddReqMdl,
  MbsSysEmpCtlAddRspMdl,
  MbsSysEmpCtlUpdateReqMdl,
  MbsSysEmpCtlUpdateRspMdl,
  MbsSysEmpCtlGetReqMdl,
  MbsSysEmpCtlGetRspMdl,
  MbsSysEmpCtlGetManyRspItemMdl,
  MbsSysEmpCtlUpdateReqItemMdl,
  MbsSysEmpCtlAddReqItemMdl,
  MbsSysEmpCtlGetRspItemMdl,
} from "./systemEmployeeControllerFormat";
import type {
  MbsSysEmpHttpGetManyReqMdl,
  MbsSysEmpHttpGetManyRspMdl,
  MbsSysEmpHttpGetManyRspItemMdl,
  MbsSysEmpHttpAddReqMdl,
  MbsSysEmpHttpAddRspMdl,
  MbsSysEmpHttpUpdateReqMdl,
  MbsSysEmpHttpUpdateRspMdl,
  MbsSysEmpHttpUpdateReqItemMdl,
  MbsSysEmpHttpGetReqMdl,
  MbsSysEmpHttpGetRspMdl,
  MbsSysEmpHttpGetRspItemMdl,
  MbsSysEmpHttpAddReqItemMdl,
} from "./systemEmployeeHttpFormat";

/** 管理者後台-系統-員工-取得多筆 */
export const getManySystemEmployee = async (
  requestData: MbsSysEmpHttpGetManyReqMdl
): Promise<MbsSysEmpHttpGetManyRspMdl | null> => {
  const httpRequestData: MbsSysEmpCtlGetManyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerDepartmentID,
    b: requestData.managerRoleID,
    c: requestData.employeeIsEnable,
    d: requestData.employeeAccount,
    e: requestData.pageIndex,
    f: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysEmpCtlGetManyRspMdl>(
      "/api/MbsSystemEmployee/GetMany",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysEmpHttpGetManyRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      employeeList:
        httpResponseData.data.a?.map(
          (item: MbsSysEmpCtlGetManyRspItemMdl) =>
            ({
              managerDepartmentName: item.a,
              managerRoleName: item.b,
              employeeID: item.c,
              employeeName: item.d,
              employeeAccount: item.e,
              employeeIsEnable: item.f,
            }) satisfies MbsSysEmpHttpGetManyRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-員工-取得單筆 */
export const getSystemEmployee = async (
  requestData: MbsSysEmpHttpGetReqMdl
): Promise<MbsSysEmpHttpGetRspMdl | null> => {
  const httpRequestData: MbsSysEmpCtlGetReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysEmpCtlGetRspMdl>(
      "/api/MbsSystemEmployee/Get",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysEmpHttpGetRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeeAccount: httpResponseData.data.a,
      employeeEmail: httpResponseData.data.b,
      employeeName: httpResponseData.data.c,
      managerRoleID: httpResponseData.data.d,
      managerRoleName: httpResponseData.data.e,
      managerRegionID: httpResponseData.data.f,
      managerRegionName: httpResponseData.data.g,
      managerDepartmentName: httpResponseData.data.h,
      employeeIsEnable: httpResponseData.data.i,
      employeePermissionList:
        httpResponseData.data.j?.map(
          (item: MbsSysEmpCtlGetRspItemMdl) =>
            ({
              atomMenu: item.a,
              managerRegionID: item.b,
              atomPermissionKindIdView: item.c,
              atomPermissionKindIdCreate: item.d,
              atomPermissionKindIdEdit: item.e,
              atomPermissionKindIdDelete: item.f,
            }) satisfies MbsSysEmpHttpGetRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-員工-新增 */
export const addSystemEmployee = async (
  requestData: MbsSysEmpHttpAddReqMdl
): Promise<MbsSysEmpHttpAddRspMdl | null> => {
  const httpRequestData: MbsSysEmpCtlAddReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeAccount,
    b: requestData.employeePassword,
    c: requestData.employeeName,
    d: requestData.managerRoleID,
    e: requestData.employeeIsEnable,
    f:
      requestData.employeePermissionList?.map(
        (item: MbsSysEmpHttpAddReqItemMdl) =>
          ({
            a: item.atomMenu,
            b: item.managerRegionID,
            c: item.atomPermissionKindIdView,
            d: item.atomPermissionKindIdCreate,
            e: item.atomPermissionKindIdEdit,
            f: item.atomPermissionKindIdDelete,
          }) satisfies MbsSysEmpCtlAddReqItemMdl
      ) ?? null,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysEmpCtlAddRspMdl>(
      "/api/MbsSystemEmployee/Add",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysEmpHttpAddRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeeID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-員工-更新 */
export const updateSystemEmployee = async (
  requestData: MbsSysEmpHttpUpdateReqMdl
): Promise<MbsSysEmpHttpUpdateRspMdl | null> => {
  const httpRequestData: MbsSysEmpCtlUpdateReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeID,
    b: requestData.employeePassword,
    c: requestData.employeeName,
    d: requestData.managerRoleID,
    e: requestData.employeeIsEnable,
    f:
      requestData.employeePermissionList?.map(
        (item: MbsSysEmpHttpUpdateReqItemMdl) =>
          ({
            a: item.atomMenu,
            b: item.managerRegionID,
            c: item.atomPermissionKindIdView,
            d: item.atomPermissionKindIdCreate,
            e: item.atomPermissionKindIdEdit,
            f: item.atomPermissionKindIdDelete,
          }) satisfies MbsSysEmpCtlUpdateReqItemMdl
      ) ?? null,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysEmpCtlUpdateRspMdl>(
      "/api/MbsSystemEmployee/Update",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysEmpHttpUpdateRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
