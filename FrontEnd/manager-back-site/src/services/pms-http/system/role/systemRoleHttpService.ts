import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import type {
  MbsSysRolCtlGetManyReqMdl,
  MbsSysRolCtlGetManyRspMdl,
  MbsSysRolCtlGetManyRspItemMdl,
  MbsSysRolCtlAddReqMdl,
  MbsSysRolCtlAddRspMdl,
  MbsSysRolCtlAddReqItemMdl,
  MbsSysRolCtlUpdateReqMdl,
  MbsSysRolCtlUpdateRspMdl,
  MbsSysRolCtlUpdateReqItemMdl,
  MbsSysRolCtlGetReqMdl,
  MbsSysRolCtlGetRspMdl,
  MbsSysRolCtlGetRspItemMdl,
} from "./systemRoleControllerFormat";
import type {
  MbsSysRolHttpGetManyReqMdl,
  MbsSysRolHttpGetManyRspMdl,
  MbsSysRolHttpGetManyRspItemMdl,
  MbsSysRolHttpAddReqMdl,
  MbsSysRolHttpAddRspMdl,
  MbsSysRolHttpUpdateReqMdl,
  MbsSysRolHttpUpdateRspMdl,
  MbsSysRolHttpUpdateReqItemMdl,
  MbsSysRolHttpGetReqMdl,
  MbsSysRolHttpGetRspMdl,
  MbsSysRolHttpGetRspItemMdl,
  MbsSysRolHttpAddReqItemMdl,
} from "./systemRoleHttpFormat";

/** 管理者後台-系統-角色-取得多筆 */
export const getManySystemRole = async (
  requestData: MbsSysRolHttpGetManyReqMdl
): Promise<MbsSysRolHttpGetManyRspMdl | null> => {
  const httpRequestData: MbsSysRolCtlGetManyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRoleName,
    b: requestData.managerRoleIsEnable,
    c: requestData.pageIndex,
    d: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysRolCtlGetManyRspMdl>(
      "/api/MbsSystemRole/GetMany",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysRolHttpGetManyRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerRoleList:
        httpResponseData.data.a?.map(
          (item: MbsSysRolCtlGetManyRspItemMdl) =>
            ({
              managerRoleID: item.a,
              managerRoleName: item.b,
              employeeCount: item.c,
              managerRoleIsEnable: item.d,
            }) satisfies MbsSysRolHttpGetManyRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-角色-取得單筆 */
export const getSystemRole = async (
  requestData: MbsSysRolHttpGetReqMdl
): Promise<MbsSysRolHttpGetRspMdl | null> => {
  const httpRequestData: MbsSysRolCtlGetReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRoleID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysRolCtlGetRspMdl>(
      "/api/MbsSystemRole/Get",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysRolHttpGetRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerRoleName: httpResponseData.data.a,
      managerRegionID: httpResponseData.data.b,
      managerRegionName: httpResponseData.data.c,
      managerDepartmentID: httpResponseData.data.d,
      managerDepartmentName: httpResponseData.data.e,
      managerRoleRemark: httpResponseData.data.f,
      managerRoleIsEnable: httpResponseData.data.g,
      employeeCount: httpResponseData.data.h,
      managerRolePermissionList:
        httpResponseData.data.i?.map(
          (item: MbsSysRolCtlGetRspItemMdl) =>
            ({
              atomMenu: item.a,
              managerRegionID: item.b,
              atomPermissionKindIdView: item.c,
              atomPermissionKindIdCreate: item.d,
              atomPermissionKindIdEdit: item.e,
              atomPermissionKindIdDelete: item.f,
            }) satisfies MbsSysRolHttpGetRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-角色-新增 */
export const addSystemRole = async (
  requestData: MbsSysRolHttpAddReqMdl
): Promise<MbsSysRolHttpAddRspMdl | null> => {
  const httpRequestData: MbsSysRolCtlAddReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRoleName,
    b: requestData.managerRegionID,
    c: requestData.managerDepartmentID,
    d: requestData.managerRoleRemark,
    e: requestData.managerRoleIsEnable,
    f:
      requestData.managerRolePermissionList?.map(
        (item: MbsSysRolHttpAddReqItemMdl) =>
          ({
            a: item.atomMenu,
            b: item.managerRegionID,
            c: item.atomPermissionKindIdView,
            d: item.atomPermissionKindIdCreate,
            e: item.atomPermissionKindIdEdit,
            f: item.atomPermissionKindIdDelete,
          }) satisfies MbsSysRolCtlAddReqItemMdl
      ) ?? null,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysRolCtlAddRspMdl>(
      "/api/MbsSystemRole/Add",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysRolHttpAddRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerRoleID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-角色-更新 */
export const updateSystemRole = async (
  requestData: MbsSysRolHttpUpdateReqMdl
): Promise<MbsSysRolHttpUpdateRspMdl | null> => {
  const httpRequestData: MbsSysRolCtlUpdateReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRoleID,
    b: requestData.managerRoleName,
    c: requestData.managerRoleRegionID,
    d: requestData.managerDepartmentID,
    e: requestData.managerRoleRemark,
    f: requestData.managerRoleIsEnable,
    g:
      requestData.managerRolePermissionList?.map(
        (item: MbsSysRolHttpUpdateReqItemMdl) =>
          ({
            a: item.atomMenu,
            b: item.managerRegionID,
            c: item.atomPermissionKindIdView,
            d: item.atomPermissionKindIdCreate,
            e: item.atomPermissionKindIdEdit,
            f: item.atomPermissionKindIdDelete,
          }) satisfies MbsSysRolCtlUpdateReqItemMdl
      ) ?? null,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysRolCtlUpdateRspMdl>(
      "/api/MbsSystemRole/Update",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysRolHttpUpdateRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
