import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import {
  MbsWrkPrjHttpGetDashboardReqMdl,
  MbsWrkPrjHttpGetDashboardRspMdl,
  MbsWrkPrjHttpGetManyProjectReqMdl,
  MbsWrkPrjHttpGetManyProjectRspMdl,
  MbsWrkPrjHttpGetProjectReqMdl,
  MbsWrkPrjHttpGetProjectRspMdl,
  MbsWrkPrjHttpAddContractReqMdl,
  MbsWrkPrjHttpAddContractRspMdl,
  MbsWrkPrjHttpAddWorkReqMdl,
  MbsWrkPrjHttpAddWorkRspMdl,
  MbsWrkPrjHttpAddMemberReqMdl,
  MbsWrkPrjHttpAddMemberRspMdl,
  MbsWrkPrjHttpRemoveMemberReqMdl,
  MbsWrkPrjHttpRemoveMemberRspMdl,
  MbsWrkPrjHttpGetManyMemberRoleReqMdl,
  MbsWrkPrjHttpGetManyMemberRoleRspMdl,
  MbsWrkPrjHttpAddProjectReqMdl,
  MbsWrkPrjHttpAddProjectRspMdl,
  MbsWrkPrjHttpGetDashboardRspItemMdl,
  MbsWrkPrjHttpAddProjectStoneReqMdl,
  MbsWrkPrjHttpAddProjectStoneRspMdl,
  MbsWrkPrjHttpGetManyProjectStoneReqMdl,
  MbsWrkPrjHttpGetManyProjectStoneRspMdl,
  MbsWrkPrjHttpGetManyProjectStoneTaskReqMdl,
  MbsWrkPrjHttpGetManyProjectStoneTaskRspMdl,
  MbsWrkPrjHttpGetProjectStoneTaskReqMdl,
  MbsWrkPrjHttpGetProjectStoneTaskRspMdl,
  MbsWrkPrjHttpUpdateProjectStoneTaskReqMdl,
  MbsWrkPrjHttpUpdateProjectStoneTaskRspMdl,
  MbsWrkPrjHttpUpdateProjectStoneTaskBucketReqMdl,
  MbsWrkPrjHttpUpdateProjectStoneTaskBucketRspMdl,
  MbsWrkPrjHttpUpdateProjectStoneReqMdl,
  MbsWrkPrjHttpUpdateProjectStoneRspMdl,
  MbsWrkPrjHttpRemoveProjectStoneReqMdl,
  MbsWrkPrjHttpRemoveProjectStoneRspMdl,
  MbsWrkPrjHttpGetProjectStoneRspMdl,
  MbsWrkPrjHttpGetProjectStoneReqMdl,
  MbsWrkPrjHttpGetManyMemberRoleRspItemMdl,
  MbsWrkPrjHttpGetManyExpenseReqMdl,
  MbsWrkPrjHttpGetManyExpenseRspMdl,
  MbsWrkPrjHttpGetExpenseReqMdl,
  MbsWrkPrjHttpGetExpenseRspMdl,
  MbsWrkPrjHttpAddExpenseReqMdl,
  MbsWrkPrjHttpAddExpenseRspMdl,
  MbsWrkPrjHttpUpdateExpenseReqMdl,
  MbsWrkPrjHttpUpdateExpenseRspMdl,
  MbsWrkPrjHttpUpdateProjectRspMdl,
  MbsWrkPrjHttpUpdateProjectReqMdl,
} from "./workProjectHttpFormat";

import {
  MbsWrkPrjCtlGetDashboardReqMdl,
  MbsWrkPrjCtlGetDashboardRspMdl,
  MbsWrkPrjCtlGetManyProjectReqMdl,
  MbsWrkPrjCtlGetManyProjectRspMdl,
  MbsWrkPrjCtlGetProjectReqMdl,
  MbsWrkPrjCtlGetProjectRspMdl,
  MbsWrkPrjCtlAddContractReqMdl,
  MbsWrkPrjCtlAddContractRspMdl,
  MbsWrkPrjCtlAddWorkReqMdl,
  MbsWrkPrjCtlAddWorkRspMdl,
  MbsWrkPrjCtlAddMemberReqMdl,
  MbsWrkPrjCtlAddMemberRspMdl,
  MbsWrkPrjCtlRemoveMemberReqMdl,
  MbsWrkPrjCtlRemoveMemberRspMdl,
  MbsWrkPrjCtlGetManyMemberRoleReqMdl,
  MbsWrkPrjCtlGetManyMemberRoleRspMdl,
  MbsWrkPrjCtlAddProjectReqMdl,
  MbsWrkPrjCtlAddProjectRspMdl,
  MbsWrkPrjCtlGetDashboardRspItemMdl,
  MbsWrkPrjCtlAddProjectStoneReqMdl,
  MbsWrkPrjCtlAddProjectStoneRspMdl,
  MbsWrkPrjCtlGetManyProjectStoneReqMdl,
  MbsWrkPrjCtlGetManyProjectStoneRspMdl,
  MbsWrkPrjCtlGetManyProjectStoneRspItemStoneMdl,
  MbsWrkPrjCtlGetManyProjectStoneRspItemTaskMdl,
  MbsWrkPrjCtlGetManyProjectStoneTaskReqMdl,
  MbsWrkPrjCtlGetManyProjectStoneTaskRspMdl,
  MbsWrkPrjCtlGetManyProjectStoneTaskRspStoneItemMdl,
  MbsWrkPrjCtlGetManyProjectStoneTaskRspTaskItemMdl,
  MbsWrkPrjCtlGetManyProjectStoneTaskRspBucketItemMdl,
  MbsWrkPrjCtlGetProjectStoneTaskReqMdl,
  MbsWrkPrjCtlGetProjectStoneTaskRspMdl,
  MbsWrkPrjCtlGetProjectStoneTaskRspExecutorItemMdl,
  MbsWrkPrjCtlGetProjectStoneTaskRspBucketItemMdl,
  MbsWrkPrjCtlUpdateProjectStoneTaskReqMdl,
  MbsWrkPrjCtlUpdateProjectStoneTaskRspMdl,
  MbsWrkPrjCtlUpdateProjectStoneTaskBucketReqMdl,
  MbsWrkPrjCtlUpdateProjectStoneTaskBucketRspMdl,
  MbsWrkPrjCtlUpdateProjectStoneReqMdl,
  MbsWrkPrjCtlUpdateProjectStoneRspMdl,
  MbsWrkPrjCtlRemoveProjectStoneReqMdl,
  MbsWrkPrjCtlRemoveProjectStoneRspMdl,
  MbsWrkPrjCtlGetProjectStoneRspItemTaskMdl,
  MbsWrkPrjCtlGetProjectStoneRspMdl,
  MbsWrkPrjCtlGetProjectStoneReqMdl,
  MbsWrkPrjCtlGetProjectStoneRspItemExecutorMdl,
  MbsWrkPrjCtlGetManyExpenseReqMdl,
  MbsWrkPrjCtlGetManyExpenseRspMdl,
  MbsWrkPrjCtlGetManyExpenseRspItemMdl,
  MbsWrkPrjCtlGetEipExpenseRspItemMdl,
  MbsWrkPrjCtlGetEipTravelExpenseRspItemMdl,
  MbsWrkPrjCtlGetExpenseReqMdl,
  MbsWrkPrjCtlGetExpenseRspMdl,
  MbsWrkPrjCtlAddExpenseReqMdl,
  MbsWrkPrjCtlAddExpenseRspMdl,
  MbsWrkPrjCtlUpdateExpenseReqMdl,
  MbsWrkPrjCtlUpdateExpenseRspMdl,
  MbsWrkPrjCtlUpdateProjectRspMdl,
  MbsWrkPrjCtlUpdateProjectReqMdl,
} from "./workProjectControllerFormat";

//#region 管理者後台-工作-專案-儀表板
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-取得儀表板 */
export const getDashboard = async (
  requestData: MbsWrkPrjHttpGetDashboardReqMdl
): Promise<MbsWrkPrjHttpGetDashboardRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlGetDashboardReqMdl = {
    aa: requestData.employeeLoginToken,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetDashboardRspMdl>(
      "/api/MbsWorkProject/GetDashboard",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      delayedEmployeeProjectList:
        httpResponseData.data.a?.map(
          (item: MbsWrkPrjCtlGetDashboardRspItemMdl) =>
            ({
              employeeProjectID: item.a,
              managerCompanyName: item.b,
              employeeProjectName: item.c,
              employeeProjectStartTime: item.d,
              employeeProjectEndTime: item.e,
            }) satisfies MbsWrkPrjHttpGetDashboardRspItemMdl
        ) ?? [],
      atRiskEmployeeProjectList:
        httpResponseData.data.b?.map(
          (item: MbsWrkPrjCtlGetDashboardRspItemMdl) =>
            ({
              employeeProjectID: item.a,
              managerCompanyName: item.b,
              employeeProjectName: item.c,
              employeeProjectStartTime: item.d,
              employeeProjectEndTime: item.e,
            }) satisfies MbsWrkPrjHttpGetDashboardRspItemMdl
        ) ?? [],
      notAssignedEmployeeProjectList:
        httpResponseData.data.c?.map(
          (item: MbsWrkPrjCtlGetDashboardRspItemMdl) =>
            ({
              employeeProjectID: item.a,
              managerCompanyName: item.b,
              employeeProjectName: item.c,
              employeeProjectStartTime: item.d,
              employeeProjectEndTime: item.e,
            }) satisfies MbsWrkPrjHttpGetDashboardRspItemMdl
        ) ?? [],
    };
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-專案
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-取得多筆專案 */
export const getManyProject = async (
  requestData: MbsWrkPrjHttpGetManyProjectReqMdl
): Promise<MbsWrkPrjHttpGetManyProjectRspMdl | null> => {
  const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";
  const httpRequestData: MbsWrkPrjCtlGetManyProjectReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.atomEmployeeProjectStatus,
    b: requestData.employeeProjectName,
    c: requestData.pageIndex,
    d: requestData.pageSize,
  };
  const cacheKey = "cache.work.project.list";
  const snapshotKey = "cache.work.project.snapshot";
  const readCache = () => {
    const raw = localStorage.getItem(cacheKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as MbsWrkPrjHttpGetManyProjectRspMdl;
    } catch {
      return null;
    }
  };
  const writeCache = (payload: MbsWrkPrjHttpGetManyProjectRspMdl) => {
    localStorage.setItem(cacheKey, JSON.stringify(payload));
  };
  const writeSnapshot = (payload: MbsWrkPrjHttpGetManyProjectRspMdl) => {
    const list = payload.employeeProjectList ?? [];
    if (list.length === 0) return;
    const snapshot = list.map((item) => ({
      id: item.employeeProjectID,
      status: item.atomEmployeeProjectStatus,
      name: item.employeeProjectName,
      companyName: item.managerCompanyName,
      startTime: item.employeeProjectStartTime,
      endTime: item.employeeProjectEndTime,
    }));
    localStorage.setItem(snapshotKey, JSON.stringify(snapshot));
  };
  const buildFallback = (): MbsWrkPrjHttpGetManyProjectRspMdl => {
    const queryName = requestData.employeeProjectName?.trim().toLowerCase() ?? "";
    const filtered = mockDataSets.workProjects.filter((item) => {
      const matchesStatus =
        requestData.atomEmployeeProjectStatus == null ||
        item.status === requestData.atomEmployeeProjectStatus;
      const matchesName =
        !queryName || item.name.toLowerCase().includes(queryName);
      return matchesStatus && matchesName;
    });
    const startIndex = Math.max(0, (requestData.pageIndex - 1) * requestData.pageSize);
    const pageItems = filtered.slice(startIndex, startIndex + requestData.pageSize);
    return {
      errorCode: MbsErrorCodeEnum.Success,
      totalCount: filtered.length,
      employeeProjectList: pageItems.map((item) => ({
        employeeProjectID: item.id,
        atomEmployeeProjectStatus: item.status,
        employeeProjectName: item.name,
        managerCompanyName: item.companyName,
        employeeProjectStartTime: item.startTime,
        employeeProjectEndTime: item.endTime,
      })),
    };
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetManyProjectRspMdl>(
      "/api/MbsWorkProject/GetManyProject",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      const cached = readCache();
      if (cached) return cached;
      return buildFallback();
    }

    const normalized: MbsWrkPrjHttpGetManyProjectRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b ?? 0,
      employeeProjectList: Array.isArray(httpResponseData.data.a)
        ? httpResponseData.data.a
            .filter(Boolean)
            .map((item) => ({
              employeeProjectID: item.a,
              atomEmployeeProjectStatus: item.b,
              employeeProjectName: item.c,
              managerCompanyName: item.d,
              employeeProjectStartTime: item.e,
              employeeProjectEndTime: item.f,
            }))
        : [],
    };
    writeCache(normalized);
    writeSnapshot(normalized);
    const cached = readCache();
    if (useMockData && cached) {
      return cached;
    }
    return normalized;
  } catch (err) {
    console.warn("API fallback:", (err as AxiosError).response?.data ?? err);
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  }
};
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-取得單筆專案 */
export const getProject = async (
  requestData: MbsWrkPrjHttpGetProjectReqMdl
): Promise<MbsWrkPrjHttpGetProjectRspMdl | null> => {
  const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";
  const httpRequestData: MbsWrkPrjCtlGetProjectReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
  };
  const cacheKey = `cache.work.project.detail.${requestData.employeeProjectID}`;
  const readCache = () => {
    const raw = localStorage.getItem(cacheKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as MbsWrkPrjHttpGetProjectRspMdl;
    } catch {
      return null;
    }
  };
  const writeCache = (payload: MbsWrkPrjHttpGetProjectRspMdl) => {
    localStorage.setItem(cacheKey, JSON.stringify(payload));
  };
  const buildFallback = (): MbsWrkPrjHttpGetProjectRspMdl => {
    const fallback =
      mockDataSets.workProjects.find((item) => item.id === requestData.employeeProjectID) ??
      mockDataSets.workProjects[0];
    return {
      errorCode: MbsErrorCodeEnum.Success,
      atomEmployeeProjectStatus: fallback?.status ?? 0,
      employeeProjectCode: "",
      employeeProjectContractCreateTime: "",
      employeeProjectContractUrl: null,
      employeeProjectWorkCreateTime: "",
      employeeProjectWorkUrl: null,
      employeeProjectMemberList: [],
      employeeProjectName: fallback?.name ?? "",
      employeeProjectStartTime: fallback?.startTime ?? "",
      employeeProjectEndTime: fallback?.endTime ?? "",
      managerCompanyName: fallback?.companyName ?? "",
    };
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetProjectRspMdl>(
      "/api/MbsWorkProject/GetProject",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      const cached = readCache();
      if (cached) return cached;
      return buildFallback();
    }

    const normalized: MbsWrkPrjHttpGetProjectRspMdl = {
      errorCode: httpResponseData.data.aa,
      atomEmployeeProjectStatus: httpResponseData.data.a,
      employeeProjectCode: httpResponseData.data.b,
      employeeProjectContractCreateTime: httpResponseData.data.c,
      employeeProjectContractUrl: httpResponseData.data.d,
      employeeProjectWorkCreateTime: httpResponseData.data.e,
      employeeProjectWorkUrl: httpResponseData.data.f,
      employeeProjectMemberList:
        httpResponseData.data.g?.map((item) => ({
          employeeProjectMemberID: item.a,
          employeeProjectMemberRole: item.b,
          managerRegionName: item.c,
          managerDepartmentName: item.d,
          employeeName: item.e,
          employeeID: item.f,
        })) ?? [],
      employeeProjectName: httpResponseData.data.h,
      employeeProjectStartTime: httpResponseData.data.i,
      employeeProjectEndTime: httpResponseData.data.j,
      managerCompanyName: httpResponseData.data.k,
    } satisfies MbsWrkPrjHttpGetProjectRspMdl;
    writeCache(normalized);
    const cached = readCache();
    if (useMockData && cached) {
      return cached;
    }
    return normalized;
  } catch (err) {
    console.warn("API fallback:", (err as AxiosError).response?.data ?? err);
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  }
};
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-新增專案 */
export const addProject = async (
  requestData: MbsWrkPrjHttpAddProjectReqMdl
): Promise<MbsWrkPrjHttpAddProjectRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlAddProjectReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
    b: requestData.employeeProjectName,
    c: requestData.employeeProjectCode,
    d: requestData.employeeProjectStartTime,
    e: requestData.employeeProjectEndTime,
    f: requestData.employeeProjectContractUrl,
    g: requestData.employeeProjectWorkUrl,
    h:
      requestData.employeeProjectMemberList?.map((item) => ({
        a: item.employeeProjectMemberRole,
        b: item.employeeID,
      })) ?? [],
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlAddProjectRspMdl>(
      "/api/MbsWorkProject/AddProject",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpAddProjectRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-更新專案 */
export const updateProject = async (
  requestData: MbsWrkPrjHttpUpdateProjectReqMdl
): Promise<MbsWrkPrjHttpUpdateProjectRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlUpdateProjectReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectCode,
    c: requestData.employeeProjectName,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlUpdateProjectRspMdl>(
      "/api/MbsWorkProject/UpdateProject",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;
    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpUpdateProjectRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//#endregion

//#region 管理者後台-工作-專案-合約
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-新增專案合約 */
export const addProjectContract = async (
  requestData: MbsWrkPrjHttpAddContractReqMdl
): Promise<MbsWrkPrjHttpAddContractRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlAddContractReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectContractUrl,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlAddContractRspMdl>(
      "/api/MbsWorkProject/AddContract",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    };
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-工作計劃書
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-新增工作計劃書*/
export const addProjectWork = async (
  requestData: MbsWrkPrjHttpAddWorkReqMdl
): Promise<MbsWrkPrjHttpAddWorkRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlAddWorkReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectWorkUrl,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlAddWorkRspMdl>(
      "/api/MbsWorkProject/AddWork",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    };
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-成員
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-新增成員 */
export const addProjectMember = async (
  requestData: MbsWrkPrjHttpAddMemberReqMdl
): Promise<MbsWrkPrjHttpAddMemberRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlAddMemberReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeID,
    c: requestData.employeeProjectMemberRole,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlAddMemberRspMdl>(
      "/api/MbsWorkProject/AddMember",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    };
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-移除成員 */
export const removeProjectMember = async (
  requestData: MbsWrkPrjHttpRemoveMemberReqMdl
): Promise<MbsWrkPrjHttpRemoveMemberRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlRemoveMemberReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectMemberID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlRemoveMemberRspMdl>(
      "/api/MbsWorkProject/RemoveMember",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    };
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------------------------------
/** 管理者後台-工作-專案-取得多筆專案成員角色 */
export const getManyProjectMemberRole = async (
  requestData: MbsWrkPrjHttpGetManyMemberRoleReqMdl
): Promise<MbsWrkPrjHttpGetManyMemberRoleRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlGetManyMemberRoleReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetManyMemberRoleRspMdl>(
      "/api/MbsWorkProject/GetManyMemberRole",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectMemberList:
        httpResponseData.data.a?.map(
          (item) =>
            ({
              employeeProjectMemberRole: item.a,
            }) satisfies MbsWrkPrjHttpGetManyMemberRoleRspItemMdl
        ) ?? [],
    };
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-里程碑
//-------------------------------------------
/** 管理者後台-工作-專案-取得多筆里程碑 */
export const getManyProjectStone = async (
  requestData: MbsWrkPrjHttpGetManyProjectStoneReqMdl
): Promise<MbsWrkPrjHttpGetManyProjectStoneRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlGetManyProjectStoneReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetManyProjectStoneRspMdl>(
      "/api/MbsWorkProject/GetManyProjectStone",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectStoneList:
        httpResponseData.data.a?.map((stone: MbsWrkPrjCtlGetManyProjectStoneRspItemStoneMdl) => ({
          employeeProjectStoneID: stone.a,
          employeeProjectStoneName: stone.b,
          employeeProjectStoneStartTime: stone.c,
          employeeProjectStoneEndTime: stone.d,
          employeeProjectStonePreNotifyDay: stone.e,
          atomEmployeeProjectStatus: stone.f,
          employeeProjectStoneTaskList:
            stone.g?.map((task: MbsWrkPrjCtlGetManyProjectStoneRspItemTaskMdl) => ({
              employeeProjectStoneTaskID: task.a,
              employeeProjectStoneTaskName: task.b,
              employeeProjectStoneTaskStartTime: task.c,
              employeeProjectStoneTaskEndTime: task.d,
              atomEmployeeProjectStatus: task.e,
              employeeProjectStoneTaskExecutorCount: task.f,
            })) ?? [],
        })) ?? [],
    } satisfies MbsWrkPrjHttpGetManyProjectStoneRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
/** 管理者後台-工作-專案-取得單筆里程碑 */
export const getProjectStone = async (
  requestData: MbsWrkPrjHttpGetProjectStoneReqMdl
): Promise<MbsWrkPrjHttpGetProjectStoneRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlGetProjectStoneReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetProjectStoneRspMdl>(
      "/api/MbsWorkProject/GetProjectStone",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectStoneName: httpResponseData.data.a,
      employeeProjectStoneStartTime: httpResponseData.data.b,
      employeeProjectStoneEndTime: httpResponseData.data.c,
      employeeProjectStonePreNotifyDay: httpResponseData.data.d,

      employeeProjectStoneTaskList:
        httpResponseData.data.e?.map((task: MbsWrkPrjCtlGetProjectStoneRspItemTaskMdl) => ({
          employeeProjectStoneTaskID: task.a,
          employeeProjectStoneTaskName: task.b,
          employeeProjectStoneTaskStartTime: task.c,
          employeeProjectStoneTaskEndTime: task.d,
          employeeProjectStoneTaskWorkHour: task.e,
          employeeProjectStoneTaskExecutorList:
            task.f?.map((executor: MbsWrkPrjCtlGetProjectStoneRspItemExecutorMdl) => ({
              employeeProjectStoneTaskExecutorEmployeeID: executor.a,
              employeeProjectStoneTaskExecutorEmployeeName: executor.b,
            })) ?? [],
        })) ?? [],
    } satisfies MbsWrkPrjHttpGetProjectStoneRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};

//-------------------------------------------
/** 管理者後台-工作-專案-新增里程碑 */
export const addProjectStone = async (
  requestData: MbsWrkPrjHttpAddProjectStoneReqMdl
): Promise<MbsWrkPrjHttpAddProjectStoneRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlAddProjectStoneReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectStoneName,
    c: requestData.employeeProjectStonePreNotifyDay,
    d: requestData.employeeProjectStoneStartTime,
    e: requestData.employeeProjectStoneEndTime,
    f:
      requestData.employeeProjectStoneTaskList?.map((item) => ({
        a: item.employeeProjectStoneTaskName,
        b: item.employeeProjectStoneTaskStartTime,
        c: item.employeeProjectStoneTaskEndTime,
        d: item.employeeProjectStoneTaskWorkHour,
        e: item.employeeProjectStoneTaskRemark,
        f:
          item.employeeProjectStoneTaskExecutorList?.map((exeItem) => ({
            a: exeItem.employeeID,
          })) ?? [],
      })) ?? null,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlAddProjectStoneRspMdl>(
      "/api/MbsWorkProject/AddProjectStone",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpAddProjectStoneRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//--------------------------------------------
/** 管理者後台-工作-專案-更新里程碑 */
export const updateProjectStone = async (
  requestData: MbsWrkPrjHttpUpdateProjectStoneReqMdl
): Promise<MbsWrkPrjHttpUpdateProjectStoneRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlUpdateProjectStoneReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneID,
    b: requestData.employeeProjectStoneName,
    c: requestData.employeeProjectStoneStartTime,
    d: requestData.employeeProjectStoneEndTime,
    e: requestData.employeeProjectStonePreNotifyDay,
    f:
      requestData.employeeProjectStoneTaskList?.map((item) => ({
        a: item.employeeProjectStoneTaskID,
        b: item.employeeProjectStoneTaskName,
        c: item.employeeProjectStoneTaskStartTime,
        d: item.employeeProjectStoneTaskEndTime,
        e: item.employeeProjectStoneTaskWorkHour,
        f:
          item.employeeProjectStoneTaskExecutorList?.map((exeItem) => ({
            a: exeItem.employeeID,
          })) ?? [],
      })) ?? null,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlUpdateProjectStoneRspMdl>(
      "/api/MbsWorkProject/UpdateProjectStone",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpUpdateProjectStoneRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//--------------------------------------------
/** 管理者後台-工作-專案-移除里程碑 */
export const removeProjectStone = async (
  requestData: MbsWrkPrjHttpRemoveProjectStoneReqMdl
): Promise<MbsWrkPrjHttpRemoveProjectStoneRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlRemoveProjectStoneReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlRemoveProjectStoneRspMdl>(
      "/api/MbsWorkProject/RemoveProjectStone",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpRemoveProjectStoneRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//--------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-工項
//-------------------------------------------
/** 管理者後台-工作-專案-取得多筆里程碑工項 */
export const getManyProjectStoneTask = async (
  requestData: MbsWrkPrjHttpGetManyProjectStoneTaskReqMdl
): Promise<MbsWrkPrjHttpGetManyProjectStoneTaskRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlGetManyProjectStoneTaskReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetManyProjectStoneTaskRspMdl>(
      "/api/MbsWorkProject/GetManyProjectStoneTask",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectStoneList:
        httpResponseData.data.a?.map(
          (stone: MbsWrkPrjCtlGetManyProjectStoneTaskRspStoneItemMdl) => ({
            employeeProjectStoneID: stone.a,
            employeeProjectStoneName: stone.b,
            employeeProjectStonePreNotifyDay: stone.c,
            employeeProjectStoneStartTime: stone.d,
            employeeProjectStoneEndTime: stone.e,
            atomEmployeeProjectStatus: stone.f,
            employeeProjectStoneTaskList:
              stone.g?.map((task: MbsWrkPrjCtlGetManyProjectStoneTaskRspTaskItemMdl) => ({
                employeeProjectStoneTaskID: task.a,
                employeeProjectStoneTaskName: task.b,
                employeeProjectStoneTaskStartTime: task.c,
                employeeProjectStoneTaskEndTime: task.d,
                employeeProjectStoneTaskWorkHour: task.e,
                atomEmployeeProjectStatus: task.f,
                employeeProjectStoneTaskExecutorCount: task.g,
                employeeProjectStoneTaskBucketList:
                  task.h?.map((bucket: MbsWrkPrjCtlGetManyProjectStoneTaskRspBucketItemMdl) => ({
                    employeeProjectStoneTaskBucketID: bucket.a,
                    employeeProjectStoneTaskBucketName: bucket.b,
                    employeeProjectStoneTaskBucketIsFinish: bucket.c,
                  })) ?? [],
              })) ?? [],
          })
        ) ?? [],
    } satisfies MbsWrkPrjHttpGetManyProjectStoneTaskRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
/** 管理者後台-工作-專案-取得單筆里程碑工項 */
export const getProjectStoneTask = async (
  requestData: MbsWrkPrjHttpGetProjectStoneTaskReqMdl
): Promise<MbsWrkPrjHttpGetProjectStoneTaskRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlGetProjectStoneTaskReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneTaskID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetProjectStoneTaskRspMdl>(
      "/api/MbsWorkProject/GetProjectStoneTask",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectStoneID: httpResponseData.data.a,
      employeeProjectStoneName: httpResponseData.data.b,
      employeeProjectStoneStartTime: httpResponseData.data.c,
      employeeProjectStoneEndTime: httpResponseData.data.d,
      employeeProjectStoneTaskName: httpResponseData.data.e,
      employeeProjectStoneTaskStartTime: httpResponseData.data.f,
      employeeProjectStoneTaskEndTime: httpResponseData.data.g,
      employeeProjectStoneTaskWorkHour: httpResponseData.data.h,
      atomEmployeeProjectStatus: httpResponseData.data.i,
      employeeProjectStoneTaskRemark: httpResponseData.data.j ?? null,
      employeeProjectStoneTaskExecutorList:
        httpResponseData.data.k?.map(
          (executor: MbsWrkPrjCtlGetProjectStoneTaskRspExecutorItemMdl) => ({
            employeeProjectStoneTaskExecutorEmployeeID: executor.a,
            employeeProjectStoneTaskExecutorEmployeeName: executor.b,
          })
        ) ?? [],
      employeeProjectStoneTaskBucketList:
        httpResponseData.data.l?.map((bucket: MbsWrkPrjCtlGetProjectStoneTaskRspBucketItemMdl) => ({
          employeeProjectStoneTaskBucketID: bucket.a,
          employeeProjectStoneTaskBucketName: bucket.b,
          employeeProjectStoneTaskBucketIsFinish: bucket.c,
        })) ?? [],
    } satisfies MbsWrkPrjHttpGetProjectStoneTaskRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
/** 管理者後台-工作-專案-更新里程碑工項 */
export const updateProjectStoneTask = async (
  requestData: MbsWrkPrjHttpUpdateProjectStoneTaskReqMdl
): Promise<MbsWrkPrjHttpUpdateProjectStoneTaskRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlUpdateProjectStoneTaskReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneTaskID,
    b: requestData.employeeProjectStoneTaskRemark,
    c: requestData.employeeProjectStoneTaskExecutorIdList,
    d:
      requestData.employeeProjectStoneTaskBucketList?.map((item) => ({
        a: item.employeeProjectStoneTaskBucketID,
        b: item.employeeProjectStoneTaskBucketName,
        c: item.employeeProjectStoneTaskBucketIsFinish,
      })) ?? [],
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlUpdateProjectStoneTaskRspMdl>(
      "/api/MbsWorkProject/UpdateProjectStoneTask",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpUpdateProjectStoneTaskRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
/** 管理者後台-工作-專案-更新里程碑工項清單 */
export const updateProjectStoneTaskBucket = async (
  requestData: MbsWrkPrjHttpUpdateProjectStoneTaskBucketReqMdl
): Promise<MbsWrkPrjHttpUpdateProjectStoneTaskBucketRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlUpdateProjectStoneTaskBucketReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneTaskBucketID,
    b: requestData.employeeProjectStoneTaskBucketIsFinish,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsWrkPrjCtlUpdateProjectStoneTaskBucketRspMdl>(
        "/api/MbsWorkProject/UpdateProjectStoneTaskBucket",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpUpdateProjectStoneTaskBucketRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//--------------------------------------------
//#endregion

//#region 管理者後台-工作-專案-收支
//-------------------------------------------

/** 管理者後台-工作-專案-取得專案支出 */
export const getExpense = async (
  requestData: MbsWrkPrjHttpGetExpenseReqMdl
): Promise<MbsWrkPrjHttpGetExpenseRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlGetExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectExpenseID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetExpenseRspMdl>(
      "/api/MbsWorkProject/GetExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectID: httpResponseData.data.a,
      employeeProjectExpenseName: httpResponseData.data.b,
      employeeProjectExpensePredictAmount: httpResponseData.data.c,
      employeeProjectExpenseActualAmount: httpResponseData.data.d,
      employeeProjectExpenseBillNumber: httpResponseData.data.e,
      employeeProjectExpenseBillTime: httpResponseData.data.f,
      employeeProjectExpenseRemark: httpResponseData.data.g,
    } satisfies MbsWrkPrjHttpGetExpenseRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};

/** 管理者後台-工作-專案-取得多筆專案支出 */
export const getManyExpense = async (
  requestData: MbsWrkPrjHttpGetManyExpenseReqMdl
): Promise<MbsWrkPrjHttpGetManyExpenseRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlGetManyExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlGetManyExpenseRspMdl>(
      "/api/MbsWorkProject/GetManyExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectExpenseList:
        httpResponseData.data.a?.map((item: MbsWrkPrjCtlGetManyExpenseRspItemMdl) => ({
          employeeProjectExpenseID: item.a,
          employeeProjectExpenseName: item.b,
          employeeProjectExpensePredictAmount: item.c,
          employeeProjectExpenseActualAmount: item.d,
          employeeProjectExpenseBillNumber: item.e,
          employeeProjectExpenseBillTime: item.f,
          employeeProjectExpenseRemark: item.g,
        })) ?? [],
      eipProjectExpenseList:
        httpResponseData.data.b?.map((item: MbsWrkPrjCtlGetEipExpenseRspItemMdl) => ({
          projectExpenseApprovalStatus: item.a,
          projectExpenseApplicant: item.b,
          projectExpenseDate: item.c,
          projectExpenseReason: item.d,
          projectExpenseParticipants: item.e,
          projectExpenseAmount: item.f,
        })) ?? [],
      eipProjectTravelExpenseList:
        httpResponseData.data.c?.map((item: MbsWrkPrjCtlGetEipTravelExpenseRspItemMdl) => ({
          projectTravelExpenseApprovalStatus: item.a,
          projectTravelExpenseApplicant: item.b,
          projectTravelExpenseTravelDate: item.c,
          projectTravelExpenseTravelRoute: item.d,
          projectTravelExpenseWorkDescription: item.e,
          projectTravelExpenseTransportationTool: item.f,
          projectTravelExpenseTransportationAmount: item.g,
          projectTravelExpenseMileage: item.h,
          projectTravelExpenseAccommodationAmount: item.i,
          projectTravelExpenseSpecialExpenseReason: item.j,
          projectTravelExpenseSpecialExpenseAmount: item.k,
          projectTravelExpenseTotalAmount: item.l,
        })) ?? [],
    } satisfies MbsWrkPrjHttpGetManyExpenseRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};

/** 管理者後台-工作-專案-新增專案支出 */
export const addExpense = async (
  requestData: MbsWrkPrjHttpAddExpenseReqMdl
): Promise<MbsWrkPrjHttpAddExpenseRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlAddExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectExpenseName,
    c: requestData.employeeProjectExpensePredictAmount,
    d: requestData.employeeProjectExpenseActualAmount,
    e: requestData.employeeProjectExpenseBillNumber,
    f: requestData.employeeProjectExpenseBillTime,
    g: requestData.employeeProjectExpenseRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlAddExpenseRspMdl>(
      "/api/MbsWorkProject/AddExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpAddExpenseRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};

/** 管理者後台-工作-專案-更新專案支出 */
export const updateExpense = async (
  requestData: MbsWrkPrjHttpUpdateExpenseReqMdl
): Promise<MbsWrkPrjHttpUpdateExpenseRspMdl | null> => {
  const httpRequestData: MbsWrkPrjCtlUpdateExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectExpenseID,
    b: requestData.employeeProjectExpenseName,
    c: requestData.employeeProjectExpensePredictAmount,
    d: requestData.employeeProjectExpenseActualAmount,
    e: requestData.employeeProjectExpenseBillNumber,
    f: requestData.employeeProjectExpenseBillTime,
    g: requestData.employeeProjectExpenseRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkPrjCtlUpdateExpenseRspMdl>(
      "/api/MbsWorkProject/UpdateExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkPrjHttpUpdateExpenseRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//#endregion
