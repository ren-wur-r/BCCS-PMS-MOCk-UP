import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";

import {
  MbsWrkJobHttpGetManyProjectStoneJobReqMdl,
  MbsWrkJobHttpGetManyProjectStoneJobRspMdl,
  MbsWrkJobHttpGetProjectStoneJobReqMdl,
  MbsWrkJobHttpGetProjectStoneJobRspMdl,
  MbsWrkJobHttpUpdateProjectStoneJobReqMdl,
  MbsWrkJobHttpUpdateProjectStoneJobRspMdl,
  MbsWrkJobHttpAddProjectStoneJobWorkReqMdl,
  MbsWrkJobHttpAddProjectStoneJobWorkRspMdl,
  MbsWrkJobHttpGetManyProjectStoneJobRspItemExecutorMdl,
  MbsWrkJobHttpUpdateProjectStoneJobWorkReqMdl,
  MbsWrkJobHttpUpdateProjectStoneJobWorkRspMdl,
  MbsWrkJobHttpGetProjectStoneJobWorkReqMdl,
  MbsWrkJobHttpGetProjectStoneJobWorkRspMdl,
  MbsWrkJobHttpGetManyProjectStoneJobWorkReqMdl,
  MbsWrkJobHttpGetManyProjectStoneJobWorkRspMdl,
  MbsWrkJobHttpGetManyProjectStoneJobWorkRspItemMdl,
  MbsWrkJobHttpRemoveProjectStoneJobWorkRspMdl,
  MbsWrkJobHttpRemoveProjectStoneJobWorkReqMdl,
} from "./workJobHttpFormat";

import {
  MbsWrkJobCtlGetManyProjectStoneJobReqMdl,
  MbsWrkJobCtlGetManyProjectStoneJobRspMdl,
  MbsWrkJobCtlGetProjectStoneJobReqMdl,
  MbsWrkJobCtlGetProjectStoneJobRspMdl,
  MbsWrkJobCtlUpdateProjectStoneJobReqMdl,
  MbsWrkJobCtlUpdateProjectStoneJobRspMdl,
  MbsWrkJobCtlAddProjectStoneJobWorkReqMdl,
  MbsWrkJobCtlAddProjectStoneJobWorkRspMdl,
  MbsWrkJobCtlGetManyProjectStoneJobRspItemExecutorMdl,
  MbsWrkJobCtlUpdateProjectStoneJobWorkReqMdl,
  MbsWrkJobCtlGetProjectStoneJobWorkReqMdl,
  MbsWrkJobCtlGetProjectStoneJobWorkRspMdl,
  MbsWrkJobCtlGetManyProjectStoneJobWorkRspMdl,
  MbsWrkJobCtlGetManyProjectStoneJobWorkReqMdl,
  MbsWrkJobCtlRemoveProjectStoneJobWorkRspMdl,
  MbsWrkJobCtlRemoveProjectStoneJobWorkReqMdl,
} from "./workJobControllerFormat";

//#region 管理者後台-工作-工項-取得多筆專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-取得多筆專案里程碑工項 */
export const getManyProjectStoneJob = async (
  requestData: MbsWrkJobHttpGetManyProjectStoneJobReqMdl
): Promise<MbsWrkJobHttpGetManyProjectStoneJobRspMdl | null> => {
  const httpRequestData: MbsWrkJobCtlGetManyProjectStoneJobReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectStoneID,
    c: requestData.employeeProjectStoneJobID,
    d: requestData.employeeProjectStoneJobStatus,
    e: requestData.startEmployeeProjectStoneJobEndTime,
    f: requestData.endEmployeeProjectStoneJobEndTime,
    g: requestData.pageIndex,
    h: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkJobCtlGetManyProjectStoneJobRspMdl>(
      "/api/MbsWorkJob/GetManyProjectStoneJob",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectStoneJobList:
        httpResponseData.data.a?.map((item) => ({
          employeeProjectStoneJobID: item.a,
          employeeProjectName: item.b,
          employeeProjectStoneName: item.c,
          employeeProjectStoneJobName: item.d,
          employeeProjectStoneJobStatus: item.e,
          employeeProjectStoneJobStartTime: item.f,
          employeeProjectStoneJobEndTime: item.g,
          employeeProjectStoneJobExecutorList:
            item.h?.map(
              (executorItem: MbsWrkJobCtlGetManyProjectStoneJobRspItemExecutorMdl) =>
                ({
                  employeeProjectStoneJobExecutorName: executorItem.a,
                }) satisfies MbsWrkJobHttpGetManyProjectStoneJobRspItemExecutorMdl
            ) ?? [],
        })) ?? [],

      totalCount: httpResponseData.data.b,
    } satisfies MbsWrkJobHttpGetManyProjectStoneJobRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-取得專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-取得專案里程碑工項 */
export const getProjectStoneJob = async (
  requestData: MbsWrkJobHttpGetProjectStoneJobReqMdl
): Promise<MbsWrkJobHttpGetProjectStoneJobRspMdl | null> => {
  const httpRequestData: MbsWrkJobCtlGetProjectStoneJobReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneJobID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkJobCtlGetProjectStoneJobRspMdl>(
      "/api/MbsWorkJob/GetProjectStoneJob",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectName: httpResponseData.data.a,
      employeeProjectStartTime: httpResponseData.data.b,
      employeeProjectEndTime: httpResponseData.data.c,
      employeeProjectStoneName: httpResponseData.data.d,
      employeeProjectStoneStartTime: httpResponseData.data.e,
      employeeProjectStoneEndTime: httpResponseData.data.f,
      employeeProjectStoneJobName: httpResponseData.data.g,
      employeeProjectStoneJobStartTime: httpResponseData.data.h,
      employeeProjectStoneJobEndTime: httpResponseData.data.i,
      employeeProjectStoneJobStatus: httpResponseData.data.j,
      employeeProjectStoneJobRemark: httpResponseData.data.k,
      employeeProjectStoneJobExecutorList:
        httpResponseData.data.l?.map((item) => ({
          employeeProjectStoneJobExecutorName: item.a,
        })) ?? [],
      employeeProjectStoneJobBucketList:
        httpResponseData.data.m?.map((item) => ({
          employeeProjectStoneJobBucketID: item.a,
          employeeProjectStoneJobBucketName: item.b,
          employeeProjectStoneJobBucketIsFinished: item.c,
        })) ?? [],
      employeeProjectStoneJobWorkList:
        httpResponseData.data.n?.map((item) => ({
          employeeProjectStoneJobWorkID: item.a,
          employeeProjectStoneJobWorkStartTime: item.b,
          employeeProjectStoneJobWorkEndTime: item.c,
          employeeProjectStoneJobWorkRemark: item.d,
          employeeName: item.e,
        })) ?? [],
      employeeProjectStoneJobWorkFileList:
        httpResponseData.data.o?.map((item) => ({
          employeeProjectStoneJobWorkFileUrl: item.a,
        })) ?? [],
    } satisfies MbsWrkJobHttpGetProjectStoneJobRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-更新專案里程碑工項
//-------------------------------------------
/** 管理者後台-工作-工項-更新專案里程碑工項 */
export const updateProjectStoneJob = async (
  requestData: MbsWrkJobHttpUpdateProjectStoneJobReqMdl
): Promise<MbsWrkJobHttpUpdateProjectStoneJobRspMdl | null> => {
  const httpRequestData: MbsWrkJobCtlUpdateProjectStoneJobReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneJobID,
    b: requestData.employeeProjectStoneJobRemark,
    c:
      requestData.employeeProjectStoneJobBucketList?.map((item) => ({
        a: item.employeeProjectStoneJobBucketID,
        b: item.employeeProjectStoneJobBucketName,
        c: item.employeeProjectStoneJobBucketIsFinished,
      })) ?? [],
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkJobCtlUpdateProjectStoneJobRspMdl>(
      "/api/MbsWorkJob/UpdateProjectStoneJob",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkJobHttpUpdateProjectStoneJobRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-取得專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-取得專案里程碑工項工作 */
export const getProjectStoneJobWork = async (
  requestData: MbsWrkJobHttpGetProjectStoneJobWorkReqMdl
): Promise<MbsWrkJobHttpGetProjectStoneJobWorkRspMdl | null> => {
  const httpRequestData: MbsWrkJobCtlGetProjectStoneJobWorkReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneJobWorkID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkJobCtlGetProjectStoneJobWorkRspMdl>(
      "/api/MbsWorkJob/GetProjectStoneJobWork",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectStoneJobID: httpResponseData.data.a,
      employeeProjectStoneJobRemark: httpResponseData.data.b,
      employeeProjectStoneJobBucketList:
        httpResponseData.data.c?.map((item) => ({
          employeeProjectStoneJobBucketID: item.a,
          employeeProjectStoneJobBucketName: item.b,
          employeeProjectStoneJobBucketIsFinished: item.c,
        })) ?? [],
      employeeProjectStoneJobWorkStartTime: httpResponseData.data.d,
      employeeProjectStoneJobWorkEndTime: httpResponseData.data.e,
      employeeProjectStoneJobWorkRemark: httpResponseData.data.f,
      employeeProjectStoneJobWorkFileList:
        httpResponseData.data.g?.map((item) => ({
          employeeProjectStoneJobWorkFileID: item.a,
          employeeProjectStoneJobWorkFileUrl: item.b,
        })) ?? [],
      // 專案資訊
      employeeProjectName: httpResponseData.data.h,
      employeeProjectStartTime: httpResponseData.data.i,
      employeeProjectEndTime: httpResponseData.data.j,
      // 里程碑資訊
      employeeProjectStoneName: httpResponseData.data.k,
      employeeProjectStoneStartTime: httpResponseData.data.l,
      employeeProjectStoneEndTime: httpResponseData.data.m,
      // 工項資訊
      employeeProjectStoneJobName: httpResponseData.data.n,
      employeeProjectStoneJobStartTime: httpResponseData.data.o,
      employeeProjectStoneJobEndTime: httpResponseData.data.p,
    } satisfies MbsWrkJobHttpGetProjectStoneJobWorkRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-新增專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-新增專案里程碑工項工作 */
export const addProjectStoneJobWork = async (
  requestData: MbsWrkJobHttpAddProjectStoneJobWorkReqMdl
): Promise<MbsWrkJobHttpAddProjectStoneJobWorkRspMdl | null> => {
  const httpRequestData: MbsWrkJobCtlAddProjectStoneJobWorkReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneJobID,
    b: requestData.employeeProjectStoneJobRemark,
    c:
      requestData.employeeProjectStoneJobBucketList?.map((item) => ({
        a: item.employeeProjectStoneJobBucketID,
        b: item.employeeProjectStoneJobBucketIsFinished,
      })) ?? [],
    d: requestData.employeeProjectStoneJobWorkStartTime,
    e: requestData.employeeProjectStoneJobWorkEndTime,
    f: requestData.employeeProjectStoneJobWorkRemark,
    g:
      requestData.employeeProjectStoneJobWorkFileList?.map((item) => ({
        a: item.employeeProjectStoneJobWorkFileUrl,
      })) ?? [],
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkJobCtlAddProjectStoneJobWorkRspMdl>(
      "/api/MbsWorkJob/AddProjectStoneJobWork",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkJobHttpAddProjectStoneJobWorkRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-更新專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-更新專案里程碑工項工作 */
export const updateProjectStoneJobWork = async (
  requestData: MbsWrkJobHttpUpdateProjectStoneJobWorkReqMdl
): Promise<MbsWrkJobHttpUpdateProjectStoneJobWorkRspMdl | null> => {
  const httpRequestData: MbsWrkJobCtlUpdateProjectStoneJobWorkReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneJobID,
    b: requestData.employeeProjectStoneJobRemark,
    c:
      requestData.employeeProjectStoneJobBucketList?.map((item) => ({
        a: item.employeeProjectStoneJobBucketID,
        b: item.employeeProjectStoneJobBucketName,
        c: item.employeeProjectStoneJobBucketIsFinished,
      })) ?? [],
    d: requestData.employeeProjectStoneJobWorkID,
    e: requestData.employeeProjectStoneJobWorkStartTime,
    f: requestData.employeeProjectStoneJobWorkEndTime,
    g: requestData.employeeProjectStoneJobWorkRemark,
    h:
      requestData.employeeProjectStoneJobWorkFileList?.map((item) => ({
        a: item.employeeProjectStoneJobWorkFileID,
        b: item.employeeProjectStoneJobWorkFileUrl,
      })) ?? [],
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkJobCtlUpdateProjectStoneJobRspMdl>(
      "/api/MbsWorkJob/UpdateProjectStoneJobWork",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkJobHttpUpdateProjectStoneJobWorkRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//-------------------------------------------
//#endregion

//#region 管理者後台-工作-工項-取得多筆專案里程碑工項工作
//-------------------------------------------
/** 管理者後台-工作-工項-取得多筆專案里程碑工項工作 */
export const getManyProjectStoneJobWork = async (
  requestData: MbsWrkJobHttpGetManyProjectStoneJobWorkReqMdl
): Promise<MbsWrkJobHttpGetManyProjectStoneJobWorkRspMdl | null> => {
  const httpRequestData: MbsWrkJobCtlGetManyProjectStoneJobWorkReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectID,
    b: requestData.employeeProjectStoneID,
    c: requestData.employeeProjectStoneJobID,
    d: requestData.employeeID,
    e: requestData.employeeProjectStoneJobWorkStartTime,
    f: requestData.employeeProjectStoneJobWorkEndTime,
    g: requestData.pageIndex,
    h: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkJobCtlGetManyProjectStoneJobWorkRspMdl>(
      "/api/MbsWorkJob/GetManyProjectStoneJobWork",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
      employeeProjectStoneJobWorkList:
        httpResponseData.data.a?.map(
          (item) =>
            ({
              employeeProjectStoneJobWorkID: item.a,
              employeeProjectStoneJobWorkStartTime: item.b,
              employeeProjectStoneJobWorkEndTime: item.c,
              employeeProjectName: item.d,
              employeeProjectStoneName: item.e,
              employeeProjectStoneJobName: item.f,
              employeeName: item.g,
              employeeProjectStoneJobWorkRemark: item.h,
              employeeProjectStoneJobID: item.i,
            }) satisfies MbsWrkJobHttpGetManyProjectStoneJobWorkRspItemMdl
        ) ?? [],

      totalCount: httpResponseData.data.b,
    } satisfies MbsWrkJobHttpGetManyProjectStoneJobWorkRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};

/** 管理者後台-工作-工項-移除專案里程碑工項工作 */
export const removeProjectStoneJobWork = async (
  requestData: MbsWrkJobHttpRemoveProjectStoneJobWorkReqMdl
): Promise<MbsWrkJobHttpRemoveProjectStoneJobWorkRspMdl | null> => {
  const httpRequestData: MbsWrkJobCtlRemoveProjectStoneJobWorkReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeeProjectStoneJobWorkID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsWrkJobCtlRemoveProjectStoneJobWorkRspMdl>(
      "/api/MbsWorkJob/RemoveProjectStoneJobWork",
      httpRequestData
    );
    if (!httpResponseData?.data) return null;
    return {
      errorCode: httpResponseData.data.aa,
    } satisfies MbsWrkJobHttpRemoveProjectStoneJobWorkRspMdl;
  } catch (err) {
    console.error("API 錯誤:", (err as AxiosError).response?.data ?? err);
    return null;
  }
};
//------------------------------------------
//#endregion
