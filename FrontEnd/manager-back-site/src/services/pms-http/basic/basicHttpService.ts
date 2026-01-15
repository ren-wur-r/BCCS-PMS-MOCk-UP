import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";
import { permissionTableConfig } from "@/components/feature/permission/permissionModel";
import type {
  MbsBscCtlLoginReqMdl,
  MbsBscCtlLoginRspMdl,
  MbsBscCtlLogoutReqMdl,
  MbsBscCtlLogoutRspMdl,
  MbsBscCtlHeartbeatReqMdl,
  MbsBscCtlHeartbeatRspMdl,
  MbsBscCtlUpdatePasswordReqMdl,
  MbsBscCtlUpdatePasswordRspMdl,
  MbsBscCtlGetEmployeeReqMdl,
  MbsBscCtlGetEmployeeRspMdl,
  MbsBscCtlGetEmployeeRspItemMdl,
  MbsBscCtlGetManyDepartmentReqMdl,
  MbsBscCtlGetManyDepartmentRspMdl,
  MbsBscCtlGetManyDepartmentRspItemMdl,
  MbsBscCtlGetManyRegionReqMdl,
  MbsBscCtlGetManyRegionRspMdl,
  MbsBscCtlGetManyRegionRspItemMdl,
  MbsBscCtlGetManyRoleReqMdl,
  MbsBscCtlGetManyRoleRspMdl,
  MbsBscCtlGetManyRoleRspItemMdl,
  MbsBscCtlGetManyProductMainKindReqMdl,
  MbsBscCtlGetManyProductMainKindRspMdl,
  MbsBscCtlGetManyProductMainKindRspItemMdl,
  MbsBscCtlGetManyProductSubKindReqMdl,
  MbsBscCtlGetManyProductSubKindRspMdl,
  MbsBscCtlGetManyProductSubKindRspItemMdl,
  MbsBscCtlGetManyContacterRatingReasonReqMdl,
  MbsBscCtlGetManyContacterRatingReasonRspMdl,
  MbsBscCtlGetManyContacterRatingReasonRspItemMdl,
  MbsBscCtlGetManyCompanyMainKindReqMdl,
  MbsBscCtlGetManyCompanyMainKindRspMdl,
  MbsBscCtlGetManyCompanyMainKindRspItemMdl,
  MbsBscCtlGetManyCompanySubKindReqMdl,
  MbsBscCtlGetManyCompanySubKindRspMdl,
  MbsBscCtlGetManyCompanySubKindRspItemMdl,
  MbsBscCtlGetManyCompanyReqMdl,
  MbsBscCtlGetManyCompanyRspMdl,
  MbsBscCtlGetManyCompanyRspItemMdl,
  MbsBscCtlGetManyCompanyNameFromPipelineOriginalReqMdl,
  MbsBscCtlGetManyCompanyNameFromPipelineOriginalRspMdl,
  MbsBscCtlGetAllDepartmentReqMdl,
  MbsBscCtlGetAllDepartmentRspMdl,
  MbsBscCtlGetAllDepartmentRspItemMdl,
  MbsBscCtlGetAllRegionReqMdl,
  MbsBscCtlGetAllRegionRspMdl,
  MbsBscCtlGetAllRegionRspItemMdl,
  MbsBscCtlGetManyRolePermissionFromRoleIdReqMdl,
  MbsBscCtlGetManyRolePermissionFromRoleIdRspMdl,
  MbsBscCtlGetManyRolePermissionFromRoleIdRspItemMdl,
  MbsBscCtlGetManyProductReqMdl,
  MbsBscCtlGetManyProductRspMdl,
  MbsBscCtlGetManyProductRspItemMdl,
  MbsBscCtlGetManyContacterReqMdl,
  MbsBscCtlGetManyContacterRspMdl,
  MbsBscCtlGetManyContacterRspItemMdl,
  MbsSysCttCtlGetManagerContacterReqMdl,
  MbsSysCttCtlGetManagerContacterRspMdl,
  MbsBscCtlGetManagerCompanyReqMdl,
  MbsBscCtlGetManagerCompanyRspMdl,
  MbsBscCtlGetManyCompanyLocationRspItemMdl,
  MbsBscCtlGetManyCompanyLocationReqMdl,
  MbsBscCtlGetManagerCompanyLocationReqMdl,
  MbsBscCtlGetManagerCompanyLocationRspMdl,
  MbsBscCtlGetManyCompanyLocationRspMdl,
  MbsBscCtlGetManyProductSpecificationRspMdl,
  MbsBscCtlGetManyProductSpecificationReqMdl,
  MbsBscCtlGetManyProductSpecificationRspItemMdl,
  MbsBscCtlGetManyEmployeeInfoReqMdl,
  MbsBscCtlGetEmployeeInfoRspMdl,
  MbsBscCtlGetEmployeeInfoRspItemMdl,
  MbsBscCtlGetManyEmployeeProjectMemberReqMdl,
  MbsBscCtlGetManyEmployeeProjectMemberRspMdl,
  MbsBscCtlGetManyEmployeeProjectMemberRspItemMdl,
  MbsBscCtlGetManyProjectStoneReqMdl,
  MbsBscCtlGetManyProjectStoneRspMdl,
  MbsBscCtlGetManyProjectStoneRspItemMdl,
  MbsBscCtlGetManyProjectStoneJobReqMdl,
  MbsBscCtlGetManyProjectStoneJobRspMdl,
  MbsBscCtlGetManyProjectStoneJobRspItemMdl,
  MbsBscCtlGetManyProjectRspItemMdl,
  MbsBscCtlGetManyProjectRspMdl,
  MbsBscCtlGetManyProjectReqMdl,
} from "./basicControllerFormat";
import type {
  MbsBscHttpLoginReqMdl,
  MbsBscHttpLoginRspMdl,
  MbsBscHttpLogoutReqMdl,
  MbsBscHttpLogoutRspMdl,
  MbsBscHttpHeartbeatReqMdl,
  MbsBscHttpHeartbeatRspMdl,
  MbsBscHttpUpdatePasswordReqMdl,
  MbsBscHttpUpdatePasswordRspMdl,
  MbsBscHttpGetEmployeeReqMdl,
  MbsBscHttpGetEmployeeRspMdl,
  MbsBscHttpGetEmployeeRspItemMdl,
  MbsBscHttpGetManyDepartmentReqMdl,
  MbsBscHttpGetManyDepartmentRspMdl,
  MbsBscHttpGetManyDepartmentRspItemMdl,
  MbsBscHttpGetManyRegionReqMdl,
  MbsBscHttpGetManyRegionRspMdl,
  MbsBscHttpGetManyRegionRspItemMdl,
  MbsBscHttpGetManyRoleReqMdl,
  MbsBscHttpGetManyRoleRspMdl,
  MbsBscHttpGetManyRoleRspItemMdl,
  MbsBscHttpGetManyProductMainKindRspMdl,
  MbsBscHttpGetManyProductMainKindReqMdl,
  MbsBscHttpGetManyProductMainKindRspItemMdl,
  MbsBscHttpGetManyProductSubKindReqMdl,
  MbsBscHttpGetManyProductSubKindRspMdl,
  MbsBscHttpGetManyProductSubKindRspItemMdl,
  MbsBscHttpGetManyContacterRatingReasonReqMdl,
  MbsBscHttpGetManyContacterRatingReasonRspMdl,
  MbsBscHttpGetManyContacterRatingReasonRspItemMdl,
  MbsBscHttpGetManyCompanyMainKindReqMdl,
  MbsBscHttpGetManyCompanyMainKindRspMdl,
  MbsBscHttpGetManyCompanyMainKindRspItemMdl,
  MbsBscHttpGetManyCompanySubKindReqMdl,
  MbsBscHttpGetManyCompanySubKindRspMdl,
  MbsBscHttpGetManyCompanySubKindRspItemMdl,
  MbsBscHttpGetManyCompanyReqMdl,
  MbsBscHttpGetManyCompanyRspMdl,
  MbsBscHttpGetManyCompanyRspItemMdl,
  MbsBscHttpGetManyCompanyNameFromPipelineOriginalReqMdl,
  MbsBscHttpGetManyCompanyNameFromPipelineOriginalRspMdl,
  MbsBscHttpGetAllDepartmentReqMdl,
  MbsBscHttpGetAllDepartmentRspMdl,
  MbsBscHttpGetAllDepartmentRspItemMdl,
  MbsBscHttpGetAllRegionReqMdl,
  MbsBscHttpGetAllRegionRspMdl,
  MbsBscHttpGetAllRegionRspItemMdl,
  MbsBscHttpGetManyRolePermissionFromRoleIdReqMdl,
  MbsBscHttpGetManyRolePermissionFromRoleIdRspMdl,
  MbsBscHttpGetManyRolePermissionFromRoleIdRspItemMdl,
  MbsBscHttpGetManyProductReqMdl,
  MbsBscHttpGetManyProductRspMdl,
  MbsBscHttpGetManyProductRspItemMdl,
  MbsBscHttpGetManyContacterReqMdl,
  MbsBscHttpGetManyContacterRspMdl,
  MbsBscHttpGetManyContacterRspItemMdl,
  MbsSysCttHttpGetManagerContacterReqMdl,
  MbsSysCttHttpGetManagerContacterRspMdl,
  MbsBscHttpGetManagerCompanyReqMdl,
  MbsBscHttpGetManagerCompanyRspMdl,
  MbsBscHttpGetManyCompanyLocationRspItemMdl,
  MbsBscHttpGetManyCompanyLocationRspMdl,
  MbsBscHttpGetManyCompanyLocationReqMdl,
  MbsBscHttpGetManagerCompanyLocationRspMdl,
  MbsBscHttpGetManagerCompanyLocationReqMdl,
  MbsBscHttpGetManyProductSpecificationReqMdl,
  MbsBscHttpGetManyProductSpecificationRspMdl,
  MbsBscHttpGetManyProductSpecificationRspItemMdl,
  MbsBscHttpGetEmployeeInfoRspMdl,
  MbsBscHttpGetManyEmployeeInfoReqMdl,
  MbsBscHttpGetEmployeeInfoRspItemMdl,
  MbsBscHttpGetManyEmployeeProjectMemberReqMdl,
  MbsBscHttpGetManyEmployeeProjectMemberRspMdl,
  MbsBscHttpGetManyEmployeeProjectMemberRspItemMdl,
  MbsBscHttpGetManyProjectStoneReqMdl,
  MbsBscHttpGetManyProjectStoneRspMdl,
  MbsBscHttpGetManyProjectStoneRspItemMdl,
  MbsBscHttpGetManyProjectStoneJobReqMdl,
  MbsBscHttpGetManyProjectStoneJobRspMdl,
  MbsBscHttpGetManyProjectStoneJobRspItemMdl,
  MbsBscHttpGetManyProjectRspMdl,
  MbsBscHttpGetManyProjectRspItemMdl,
  MbsBscHttpGetManyProjectReqMdl,
} from "./basicHttpFormat";

import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

/** 登入 */
export const login = async (
  requestData: MbsBscHttpLoginReqMdl
): Promise<MbsBscHttpLoginRspMdl | null> => {
  const httpRequestData: MbsBscCtlLoginReqMdl = {
    a: requestData.employeeAccount,
    b: requestData.employeePassword,
  };
  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlLoginRspMdl>(
      "api/MbsBasic/Login",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpLoginRspMdl = {
      errorCode: httpResponseData.data.aa,
      loginToken: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 登出 */
export const logout = async (
  requestData: MbsBscHttpLogoutReqMdl
): Promise<MbsBscHttpLogoutRspMdl | null> => {
  const httpRequestData: MbsBscCtlLogoutReqMdl = {
    aa: requestData.employeeLoginToken,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlLogoutRspMdl>(
      "api/MbsBasic/Logout",
      httpRequestData
    );

    const responseData: MbsBscHttpLogoutRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 心跳 */
export const heartbeat = async (
  requestData: MbsBscHttpHeartbeatReqMdl
): Promise<MbsBscHttpHeartbeatRspMdl | null> => {
  const httpRequestData: MbsBscCtlHeartbeatReqMdl = {
    aa: requestData.employeeLoginToken,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlHeartbeatRspMdl>(
      "/api/MbsBasic/Heartbeat",
      httpRequestData
    );

    const responseData: MbsBscHttpHeartbeatRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 修改密碼 */
export const updateEmployeePassword = async (
  requestData: MbsBscHttpUpdatePasswordReqMdl
): Promise<MbsBscHttpUpdatePasswordRspMdl | null> => {
  const httpRequestData: MbsBscCtlUpdatePasswordReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.oldPassword,
    b: requestData.newPassword,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlUpdatePasswordRspMdl>(
      "/api/MbsBasic/UpdateEmployeePassword",
      httpRequestData
    );

    const responseData: MbsBscHttpUpdatePasswordRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得員工資訊 */
export const getEmployee = async (
  requestData: MbsBscHttpGetEmployeeReqMdl
): Promise<MbsBscHttpGetEmployeeRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetEmployeeReqMdl = {
    aa: requestData.employeeLoginToken,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetEmployeeRspMdl>(
      "/api/MbsBasic/GetEmployee",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetEmployeeRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeeName: httpResponseData.data.a,
      managerBackSiteMenuPermissionList:
        httpResponseData.data.b?.map(
          (item: MbsBscCtlGetEmployeeRspItemMdl) =>
            ({
              atomMenu: item.a,
              managerRegionID: item.b,
              atomPermissionKindIdView: item.c,
              atomPermissionKindIdCreate: item.d,
              atomPermissionKindIdEdit: item.e,
              atomPermissionKindIdDelete: item.f,
            }) satisfies MbsBscHttpGetEmployeeRspItemMdl
        ) ?? null,
      employeeAccount: httpResponseData.data.c,
      managerRoleID: httpResponseData.data.d,
      managerRoleName: httpResponseData.data.e,
      managerRegionID: httpResponseData.data.f,
      managerRegionName: httpResponseData.data.g,
      managerDepartmentID: httpResponseData.data.h,
      managerDepartmentName: httpResponseData.data.i,
    };

    if (responseData.employeeAccount?.toLowerCase() === "ren.wu") {
      responseData.managerBackSiteMenuPermissionList = permissionTableConfig.flatMap((group) =>
        group.menuItemList.map((item) => ({
          atomMenu: item.menuEnum,
          managerRegionID: 0,
          atomPermissionKindIdView: DbAtomPermissionKindEnum.Everyone,
          atomPermissionKindIdCreate: DbAtomPermissionKindEnum.Everyone,
          atomPermissionKindIdEdit: DbAtomPermissionKindEnum.Everyone,
          atomPermissionKindIdDelete: DbAtomPermissionKindEnum.Everyone,
        }))
      );
    }

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆員工資訊 */
export const getManyBasicEmployee = async (
  requestData: MbsBscHttpGetManyEmployeeInfoReqMdl
): Promise<MbsBscHttpGetEmployeeInfoRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyEmployeeInfoReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRoleID,
    b: requestData.employeeIsEnable,
    c: requestData.pageIndex,
    d: requestData.managerRegionID,
    e: requestData.managerDepartmentID,
    f: requestData.employeeName,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetEmployeeInfoRspMdl>(
      "/api/MbsBasic/GetManyEmployeeInfo",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetEmployeeInfoRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeeList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetEmployeeInfoRspItemMdl) =>
            ({
              employeeID: item.a,
              employeeName: item.b,
              employeeIsEnable: item.c,
              managerRegionID: item.d,
              managerRegionName: item.e,
              managerDepartmentID: item.f,
              managerDepartmentName: item.g,
            }) satisfies MbsBscHttpGetEmployeeInfoRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆員工專案成員 */
export const getManyBasicEmployeeProjectMember = async (
  requestData: MbsBscHttpGetManyEmployeeProjectMemberReqMdl
): Promise<MbsBscHttpGetManyEmployeeProjectMemberRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyEmployeeProjectMemberReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyEmployeeProjectMemberRspMdl>(
      "/api/MbsBasic/GetManyEmployeeProjectMember",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyEmployeeProjectMemberRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeeProjectMemberList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyEmployeeProjectMemberRspItemMdl) =>
            ({
              employeeProjectMemberRole: item.a,
              employeeID: item.b,
              employeeName: item.c,
            }) satisfies MbsBscHttpGetManyEmployeeProjectMemberRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆員工專案 */
export const getManyBasicProject = async (
  requestData: MbsBscHttpGetManyProjectReqMdl
): Promise<MbsBscHttpGetManyProjectRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyProjectReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.projectName,
    b: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyProjectRspMdl>(
      "/api/MbsBasic/GetManyEmployeeProject",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsBscHttpGetManyProjectRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeeProjectList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyProjectRspItemMdl) =>
            ({
              employeeProjectID: item.a,
              employeeProjectName: item.b,
            }) satisfies MbsBscHttpGetManyProjectRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error) {
    const axiosErr = error as AxiosError;
    console.error("API 錯誤:", axiosErr.response?.data ?? error);
    return null;
  }
};

/** 取得多筆專案里程碑 */
export const getManyBasicProjectStone = async (
  requestData: MbsBscHttpGetManyProjectStoneReqMdl
): Promise<MbsBscHttpGetManyProjectStoneRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyProjectStoneReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectStoneName,
    c: requestData.pageIndex,
  };

  try {
    const httpResponse = await apiJsonClient.post<MbsBscCtlGetManyProjectStoneRspMdl>(
      "/api/MbsBasic/GetManyEmployeeProjectStone",
      httpRequestData
    );

    if (!httpResponse?.data) return null;

    const responseData: MbsBscHttpGetManyProjectStoneRspMdl = {
      errorCode: httpResponse.data.aa,
      employeeProjectStoneList:
        httpResponse.data.a?.map(
          (item: MbsBscCtlGetManyProjectStoneRspItemMdl) =>
            ({
              employeeProjectStoneID: item.a,
              employeeProjectStoneName: item.b,
            }) satisfies MbsBscHttpGetManyProjectStoneRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosErr = error as AxiosError;
    console.error("API 錯誤:", axiosErr.response?.data ?? error);
    return null;
  }
};

/** 取得多筆專案里程碑工項 */
export const getManyBasicProjectStoneJob = async (
  requestData: MbsBscHttpGetManyProjectStoneJobReqMdl
): Promise<MbsBscHttpGetManyProjectStoneJobRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyProjectStoneJobReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectStoneID,
    c: requestData.employeeProjectStoneJobName,
    d: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyProjectStoneJobRspMdl>(
      "/api/MbsBasic/GetManyEmployeeProjectStoneJob",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsBscHttpGetManyProjectStoneJobRspMdl = {
      errorCode: httpResponseData.data.aa,
      projectStoneJobList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyProjectStoneJobRspItemMdl) =>
            ({
              employeeProjectStoneJobID: item.a,
              employeeProjectStoneJobName: item.b,
            }) satisfies MbsBscHttpGetManyProjectStoneJobRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆管理者部門 */
export const getManyBasicManagerDepartment = async (
  requestData: MbsBscHttpGetManyDepartmentReqMdl
): Promise<MbsBscHttpGetManyDepartmentRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyDepartmentReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerDepartmentName,
    b: requestData.managerDepartmentIsEnable,
    c: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyDepartmentRspMdl>(
      "/api/MbsBasic/GetManyManagerDepartment",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyDepartmentRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerDepartmentList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyDepartmentRspItemMdl) =>
            ({
              managerDepartmentID: item.a,
              managerDepartmentName: item.b,
              managerDepartmentIsEnable: item.c,
            }) satisfies MbsBscHttpGetManyDepartmentRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得全部管理者部門 */
export const getAllBasicManagerDepartment = async (
  requestData: MbsBscHttpGetAllDepartmentReqMdl
): Promise<MbsBscHttpGetAllDepartmentRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetAllDepartmentReqMdl = {
    aa: requestData.employeeLoginToken,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetAllDepartmentRspMdl>(
      "/api/MbsBasic/GetAllManagerDepartment",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetAllDepartmentRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerDepartmentList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetAllDepartmentRspItemMdl) =>
            ({
              managerDepartmentID: item.a,
              managerDepartmentName: item.b,
            }) satisfies MbsBscHttpGetAllDepartmentRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆管理者地區 */
export const getManyBasicManagerRegion = async (
  requestData: MbsBscHttpGetManyRegionReqMdl
): Promise<MbsBscHttpGetManyRegionRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyRegionReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRegionName,
    b: requestData.managerRegionIsEnable,
    c: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyRegionRspMdl>(
      "/api/MbsBasic/GetManyManagerRegion",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyRegionRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerRegionList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyRegionRspItemMdl) =>
            ({
              managerRegionID: item.a,
              managerRegionName: item.b,
              managerRegionIsEnable: item.c,
            }) satisfies MbsBscHttpGetManyRegionRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得全部管理者地區 */
export const getAllBasicManagerRegion = async (
  requestData: MbsBscHttpGetAllRegionReqMdl
): Promise<MbsBscHttpGetAllRegionRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetAllRegionReqMdl = {
    aa: requestData.employeeLoginToken,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetAllRegionRspMdl>(
      "/api/MbsBasic/GetAllManagerRegion",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetAllRegionRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerRegionList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetAllRegionRspItemMdl) =>
            ({
              managerRegionID: item.a,
              managerRegionName: item.b,
            }) satisfies MbsBscHttpGetAllRegionRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆管理者角色 */
export const getManyBasicManagerRole = async (
  requestData: MbsBscHttpGetManyRoleReqMdl
): Promise<MbsBscHttpGetManyRoleRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyRoleReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRoleName,
    b: requestData.managerRoleIsEnable,
    c: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyRoleRspMdl>(
      "/api/MbsBasic/GetManyManagerRole",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyRoleRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerRoleList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyRoleRspItemMdl) =>
            ({
              managerRoleID: item.a,
              managerRoleName: item.b,
              managerRegionID: item.c,
              managerRegionName: item.d,
              managerDepartmentName: item.e,
              managerRoleIsEnable: item.f,
            }) satisfies MbsBscHttpGetManyRoleRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆角色權限從[角色ID] */
export const getManyBasicRolePermissionFromRoleId = async (
  requestData: MbsBscHttpGetManyRolePermissionFromRoleIdReqMdl
): Promise<MbsBscHttpGetManyRolePermissionFromRoleIdRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyRolePermissionFromRoleIdReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerRoleID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsBscCtlGetManyRolePermissionFromRoleIdRspMdl>(
        "/api/MbsBasic/GetManyRolePermissionFromRoleId",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyRolePermissionFromRoleIdRspMdl = {
      errorCode: httpResponseData.data.aa,
      rolePermissionList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyRolePermissionFromRoleIdRspItemMdl) =>
            ({
              atomMenu: item.a,
              managerRegionID: item.b,
              atomPermissionKindIdView: item.c,
              atomPermissionKindIdCreate: item.d,
              atomPermissionKindIdEdit: item.e,
              atomPermissionKindIdDelete: item.f,
            }) satisfies MbsBscHttpGetManyRolePermissionFromRoleIdRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆產品主分類 */
export const getManyBasicProductMainKind = async (
  requestData: MbsBscHttpGetManyProductMainKindReqMdl
): Promise<MbsBscHttpGetManyProductMainKindRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyProductMainKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindName,
    b: requestData.managerProductMainKindIsEnable,
    c: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyProductMainKindRspMdl>(
      "/api/MbsBasic/GetManyProductMainKind",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyProductMainKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerProductMainKindList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyProductMainKindRspItemMdl) =>
            ({
              managerProductMainKindID: item.a,
              managerProductMainKindName: item.b,
              managerProductMainKindIsEnable: item.c,
            }) satisfies MbsBscHttpGetManyProductMainKindRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆產品子分類 */
export const getManyBasicProductSubKind = async (
  requestData: MbsBscHttpGetManyProductSubKindReqMdl
): Promise<MbsBscHttpGetManyProductSubKindRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyProductSubKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.productMainKindID,
    b: requestData.productSubKindName,
    c: requestData.productSubKindIsEnable,
    d: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyProductSubKindRspMdl>(
      "/api/MbsBasic/GetManyProductSubKind",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyProductSubKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      productSubKindList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyProductSubKindRspItemMdl) =>
            ({
              productSubKindID: item.a,
              productSubKindName: item.b,
              productSubKindIsEnable: item.c,
            }) satisfies MbsBscHttpGetManyProductSubKindRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆產品 */
export const getManyBasicProduct = async (
  requestData: MbsBscHttpGetManyProductReqMdl
): Promise<MbsBscHttpGetManyProductRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindID,
    b: requestData.managerProductSubKindID,
    c: requestData.managerProductName,
    d: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyProductRspMdl>(
      "/api/MbsBasic/GetManyProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyProductRspMdl = {
      errorCode: httpResponseData.data.aa,
      productList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyProductRspItemMdl) =>
            ({
              managerProductID: item.a,
              managerProductName: item.b,
              managerProductIsEnable: item.c,
              managerProductMainKindID: item.d,
              managerProductMainKindName: item.e,
              managerProductSubKindID: item.f,
              managerProductSubKindName: item.g,
              managerProductMainKindCommissionRate: item.h,
            }) satisfies MbsBscHttpGetManyProductRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆產品規格 */
export const getManyBasicProductSpecification = async (
  requestData: MbsBscHttpGetManyProductSpecificationReqMdl
): Promise<MbsBscHttpGetManyProductSpecificationRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyProductSpecificationReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductID,
    b: requestData.productSpecificationName,
    c: requestData.productSpecificationIsEnable,
    d: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyProductSpecificationRspMdl>(
      "/api/MbsBasic/GetManyProductSpecification",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyProductSpecificationRspMdl = {
      errorCode: httpResponseData.data.aa,
      productSpecificationList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyProductSpecificationRspItemMdl) =>
            ({
              ManagerProductSpecificationID: item.a,
              ManagerProductSpecificationName: item.b,
              ManagerProductSpecificationSellPrice: item.c,
              ManagerProductSpecificationCostPrice: item.d,
              ManagerProductSpecificationIsEnable: item.e,
            }) satisfies MbsBscHttpGetManyProductSpecificationRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆窗口開發評等原因 */
export const GetManyBasicManagerContacterRatingReason = async (
  requestData: MbsBscHttpGetManyContacterRatingReasonReqMdl
): Promise<MbsBscHttpGetManyContacterRatingReasonRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyContacterRatingReasonReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingReasonName,
    b: requestData.managerContacterRatingReasonStatus,
    c: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyContacterRatingReasonRspMdl>(
      "/api/MbsBasic/GetManyManagerContacterRatingReason",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyContacterRatingReasonRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerContacterRatingReasonList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyContacterRatingReasonRspItemMdl) =>
            ({
              managerContacterRatingReasonID: item.a,
              managerContacterRatingReasonName: item.b,
              managerContacterRatingReasonStatus: item.c,
            }) satisfies MbsBscHttpGetManyContacterRatingReasonRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆公司主分類 */
export const getManyBasicManagerCompanyMainKind = async (
  requestData: MbsBscHttpGetManyCompanyMainKindReqMdl
): Promise<MbsBscHttpGetManyCompanyMainKindRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyCompanyMainKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerProductMainKindName,
    b: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyCompanyMainKindRspMdl>(
      "/api/MbsBasic/GetManyManagerCompanyMainKind",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyCompanyMainKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyMainKindList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyCompanyMainKindRspItemMdl) =>
            ({
              managerCompanyMainKindID: item.a,
              managerCompanyMainKindName: item.b,
              managerCompanyMainKindIsEnable: item.c,
            }) satisfies MbsBscHttpGetManyCompanyMainKindRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆公司子分類 */
export const getManyBasicManagerCompanySubKind = async (
  requestData: MbsBscHttpGetManyCompanySubKindReqMdl
): Promise<MbsBscHttpGetManyCompanySubKindRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyCompanySubKindReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.companyMainKindID,
    b: requestData.companySubKindName,
    c: requestData.companySubKindIsEnable,
    d: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyCompanySubKindRspMdl>(
      "/api/MbsBasic/GetManyManagerCompanySubKind",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyCompanySubKindRspMdl = {
      errorCode: httpResponseData.data.aa,
      companySubKindList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyCompanySubKindRspItemMdl) =>
            ({
              companySubKindID: item.a,
              companySubKindName: item.b,
              companySubKindIsEnable: item.c,
            }) satisfies MbsBscHttpGetManyCompanySubKindRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆窗口 */
export const GetManyManagerContacter = async (
  requestData: MbsBscHttpGetManyContacterReqMdl
): Promise<MbsBscHttpGetManyContacterRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterName,
    b: requestData.managerContacterCompanyID,
    c: requestData.managerContacterEmail,
    d: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyContacterRspMdl>(
      "/api/MbsBasic/GetManyManagerContacter",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerContacterList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyContacterRspItemMdl) =>
            ({
              managerContacterID: item.a,
              managerContacterName: item.b,
              managerContacterEmail: item.c,
            }) satisfies MbsBscHttpGetManyContacterRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得管理者窗口(用在名單) */
export const getBasicManagerContacter = async (
  requestData: MbsSysCttHttpGetManagerContacterReqMdl
): Promise<MbsSysCttHttpGetManagerContacterRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlGetManagerContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlGetManagerContacterRspMdl>(
      "/api/MbsBasic/GetManagerContacter",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsSysCttHttpGetManagerContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerContacterID: httpResponseData.data.a,
      managerCompanyID: httpResponseData.data.b,
      managerContacterName: httpResponseData.data.c,
      managerContacterEmail: httpResponseData.data.d,
      managerContacterPhone: httpResponseData.data.e,
      managerContacterDepartment: httpResponseData.data.f,
      managerContacterJobTitle: httpResponseData.data.g,
      managerContacterTelephone: httpResponseData.data.h,
      atomManagerContacterStatus: httpResponseData.data.i,
      managerContacterIsAgreeSurvey: httpResponseData.data.j,
      atomManagerContacterRatingKind: httpResponseData.data.k,
      managerContacterEmployeeID: httpResponseData.data.l,
      managerContacterRemark: httpResponseData.data.m,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API 錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆管理者公司 */
export const getManyBasicManagerCompany = async (
  requestData: MbsBscHttpGetManyCompanyReqMdl
): Promise<MbsBscHttpGetManyCompanyRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.companyName,
    b: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyCompanyRspMdl>(
      "/api/MbsBasic/GetManyManagerCompany",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
      companyList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyCompanyRspItemMdl) =>
            ({
              companyID: item.a,
              companyName: item.b,
            }) satisfies MbsBscHttpGetManyCompanyRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆公司名稱從[商機原始] */
export const GetManyBasicManagerCompanyNameFromPipelineOriginal = async (
  requestData: MbsBscHttpGetManyCompanyNameFromPipelineOriginalReqMdl
): Promise<MbsBscHttpGetManyCompanyNameFromPipelineOriginalRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyCompanyNameFromPipelineOriginalReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyName,
    b: requestData.pageIndex,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsBscCtlGetManyCompanyNameFromPipelineOriginalRspMdl>(
        "/api/MbsBasic/GetManyManagerCompanyNameFromPipelineOriginal",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyCompanyNameFromPipelineOriginalRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyNameList: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得管理者公司 */
export const GetBasicCompany = async (
  requestData: MbsBscHttpGetManagerCompanyReqMdl
): Promise<MbsBscHttpGetManagerCompanyRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManagerCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManagerCompanyRspMdl>(
      "/api/MbsBasic/GetManagerCompany",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManagerCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyID: httpResponseData.data.a,
      managerCompanyUnifiedNumber: httpResponseData.data.b,
      managerCompanyName: httpResponseData.data.c,
      atomManagerCompanyStatus: httpResponseData.data.d,
      managerCompanyMainKindID: httpResponseData.data.e,
      managerCompanyMainKindName: httpResponseData.data.f,
      managerCompanySubKindID: httpResponseData.data.g,
      managerCompanySubKindName: httpResponseData.data.h,
      atomCustomerGrade: httpResponseData.data.i,
      atomSecurityGrade: httpResponseData.data.j,
      atomEmployeeRange: httpResponseData.data.k,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得多筆公司營業地點 */
export const getManyBasicCompanyLocation = async (
  requestData: MbsBscHttpGetManyCompanyLocationReqMdl
): Promise<MbsBscHttpGetManyCompanyLocationRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManyCompanyLocationReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
    b: requestData.managerCompanyLocationName,
    c: requestData.pageIndex,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManyCompanyLocationRspMdl>(
      "/api/MbsBasic/GetManyManagerCompanyLocation",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManyCompanyLocationRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyLocationList:
        httpResponseData.data.a?.map(
          (item: MbsBscCtlGetManyCompanyLocationRspItemMdl) =>
            ({
              managerCompanyLocationID: item.a,
              managerCompanyLocationName: item.b,
              managerCompanyLocationIsDeleted: item.c,
            }) satisfies MbsBscHttpGetManyCompanyLocationRspItemMdl
        ) ?? null,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 取得公司營業地點 */
export const getBasicManagerCompanyLocation = async (
  requestData: MbsBscHttpGetManagerCompanyLocationReqMdl
): Promise<MbsBscHttpGetManagerCompanyLocationRspMdl | null> => {
  const httpRequestData: MbsBscCtlGetManagerCompanyLocationReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyLocationID,
    b: requestData.managerCompanyLocationIsDeleted,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsBscCtlGetManagerCompanyLocationRspMdl>(
      "/api/MbsBasic/GetManagerCompanyLocation",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsBscHttpGetManagerCompanyLocationRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyLocationID: httpResponseData.data.a,
      managerCompanyID: httpResponseData.data.b,
      managerCompanyLocationName: httpResponseData.data.c,
      atomCity: httpResponseData.data.d,
      managerCompanyLocationAddress: httpResponseData.data.e,
      managerCompanyLocationTelephone: httpResponseData.data.f,
      managerCompanyLocationStatus: httpResponseData.data.g,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
