import { AxiosError } from "axios";
import { apiJsonClient, apiFormClient } from "@/services/httpClient";
import type {
  MbsCrmActCtlAddActivityReqMdl,
  MbsCrmActCtlAddActivityRspMdl,
  MbsCrmActCtlAddActivityReqProductItemMdl,
  MbsCrmActCtlAddActivityReqSponsorItemMdl,
  MbsCrmActCtlAddActivityReqExpenseItemMdl,
  MbsCrmActCtlGetActivityReqMdl,
  MbsCrmActCtlGetActivityRspMdl,
  MbsCrmActCtlGetManyActivityReqMdl,
  MbsCrmActCtlGetManyActivityRspMdl,
  MbsCrmActCtlGetManyActivityRspItemMdl,
  MbsCrmActCtlUpdateActivityReqMdl,
  MbsCrmActCtlUpdateActivityRspMdl,
  MbsCrmActCtlAddActivityProductReqMdl,
  MbsCrmActCtlAddActivityProductRspMdl,
  MbsCrmActCtlGetActivityProductReqMdl,
  MbsCrmActCtlGetActivityProductRspMdl,
  MbsCrmActCtlGetManyActivityProductReqMdl,
  MbsCrmActCtlGetManyActivityProductRspMdl,
  MbsCrmActCtlGetManyActivityProductRspItemMdl,
  MbsCrmActCtlRemoveActivityProductReqMdl,
  MbsCrmActCtlRemoveActivityProductRspMdl,
  MbsCrmActCtlUpdateActivityProductReqMdl,
  MbsCrmActCtlUpdateActivityProductRspMdl,
  MbsCrmActCtlAddActivitySponsorReqMdl,
  MbsCrmActCtlAddActivitySponsorRspMdl,
  MbsCrmActCtlGetActivitySponsorReqMdl,
  MbsCrmActCtlGetActivitySponsorRspMdl,
  MbsCrmActCtlGetManyActivitySponsorReqMdl,
  MbsCrmActCtlGetManyActivitySponsorRspMdl,
  MbsCrmActCtlGetManyActivitySponsorRspItemMdl,
  MbsCrmActCtlRemoveActivitySponsorReqMdl,
  MbsCrmActCtlRemoveActivitySponsorRspMdl,
  MbsCrmActCtlUpdateActivitySponsorReqMdl,
  MbsCrmActCtlUpdateActivitySponsorRspMdl,
  MbsCrmActCtlAddActivityExpenseReqMdl,
  MbsCrmActCtlAddActivityExpenseRspMdl,
  MbsCrmActCtlGetActivityExpenseReqMdl,
  MbsCrmActCtlGetActivityExpenseRspMdl,
  MbsCrmActCtlGetManyActivityExpenseReqMdl,
  MbsCrmActCtlGetManyActivityExpenseRspMdl,
  MbsCrmActCtlGetManyActivityExpenseRspItemMdl,
  MbsCrmActCtlRemoveActivityExpenseReqMdl,
  MbsCrmActCtlRemoveActivityExpenseRspMdl,
  MbsCrmActCtlUpdateActivityExpenseReqMdl,
  MbsCrmActCtlUpdateActivityExpenseRspMdl,
  MbsCrmActCtlAddActivityEmployeePipelineReqMdl,
  MbsCrmActCtlAddActivityEmployeePipelineRspMdl,
  MbsCrmActCtlGetActivityEmployeePipelineReqMdl,
  MbsCrmActCtlGetActivityEmployeePipelineRspMdl,
  MbsCrmActCtlGetManyActivityEmployeePipelineReqMdl,
  MbsCrmActCtlGetManyActivityEmployeePipelineRspMdl,
  MbsCrmActCtlGetManyActivityEmployeePipelineRspItemMdl,
  MbsCrmActCtlRemoveManyActivityEmployeePipelineReqMdl,
  MbsCrmActCtlRemoveManyActivityEmployeePipelineRspMdl,
  MbsCrmActCtlUpdateManyActivityEmployeePipelineReqMdl,
  MbsCrmActCtlUpdateManyActivityEmployeePipelineRspMdl,
  MbsCrmActCtlGetManyPastActivityReqMdl,
  MbsCrmActCtlGetManyPastActivityRspMdl,
  MbsCrmActCtlGetManyPastActivityRspItemMdl,
  MbsCrmActCtlDownloadEdmTemplateReqMdl,
  MbsCrmActCtlDownloadEdmTemplateRspMdl,
  // MbsCrmActCtlImportEdmReqMdl,
  MbsCrmActCtlImportEdmRspMdl,
  MbsCrmActCtlImportEdmRspItemMdl,
  MbsCrmActCtlDownloadTeamsTemplateReqMdl,
  MbsCrmActCtlDownloadTeamsTemplateRspMdl,
  // MbsCrmActCtlImportTeamsReqMdl,
  MbsCrmActCtlImportTeamsRspMdl,
  MbsCrmActCtlImportTeamsRspItemMdl,
  MbsCrmActCtlAddActivitySurveyReqMdl,
  MbsCrmActCtlAddActivitySurveyReqItemMdl,
  MbsCrmActCtlAddActivitySurveyRspMdl,
} from "./crmActivityControllerFormat";
import type {
  MbsCrmActHttpAddActivityReqMdl,
  MbsCrmActHttpAddActivityRspMdl,
  MbsCrmActHttpAddActivityReqProductItemMdl,
  MbsCrmActHttpAddActivityReqSponsorItemMdl,
  MbsCrmActHttpAddActivityReqExpenseItemMdl,
  MbsCrmActHttpGetActivityReqMdl,
  MbsCrmActHttpGetActivityRspMdl,
  MbsCrmActHttpGetManyActivityReqMdl,
  MbsCrmActHttpGetManyActivityRspMdl,
  MbsCrmActHttpGetManyActivityRspItemMdl,
  MbsCrmActHttpUpdateActivityReqMdl,
  MbsCrmActHttpUpdateActivityRspMdl,
  MbsCrmActHttpAddActivityProductReqMdl,
  MbsCrmActHttpAddActivityProductRspMdl,
  MbsCrmActHttpGetActivityProductReqMdl,
  MbsCrmActHttpGetActivityProductRspMdl,
  MbsCrmActHttpGetManyActivityProductReqMdl,
  MbsCrmActHttpGetManyActivityProductRspMdl,
  MbsCrmActHttpGetManyActivityProductRspItemMdl,
  MbsCrmActHttpRemoveActivityProductReqMdl,
  MbsCrmActHttpRemoveActivityProductRspMdl,
  MbsCrmActHttpUpdateActivityProductReqMdl,
  MbsCrmActHttpUpdateActivityProductRspMdl,
  MbsCrmActHttpAddActivitySponsorReqMdl,
  MbsCrmActHttpAddActivitySponsorRspMdl,
  MbsCrmActHttpGetActivitySponsorReqMdl,
  MbsCrmActHttpGetActivitySponsorRspMdl,
  MbsCrmActHttpGetManyActivitySponsorReqMdl,
  MbsCrmActHttpGetManyActivitySponsorRspMdl,
  MbsCrmActHttpGetManyActivitySponsorRspItemMdl,
  MbsCrmActHttpRemoveActivitySponsorReqMdl,
  MbsCrmActHttpRemoveActivitySponsorRspMdl,
  MbsCrmActHttpUpdateActivitySponsorReqMdl,
  MbsCrmActHttpUpdateActivitySponsorRspMdl,
  MbsCrmActHttpAddActivityExpenseReqMdl,
  MbsCrmActHttpAddActivityExpenseRspMdl,
  MbsCrmActHttpGetActivityExpenseReqMdl,
  MbsCrmActHttpGetActivityExpenseRspMdl,
  MbsCrmActHttpGetManyActivityExpenseReqMdl,
  MbsCrmActHttpGetManyActivityExpenseRspMdl,
  MbsCrmActHttpGetManyActivityExpenseRspItemMdl,
  MbsCrmActHttpRemoveActivityExpenseReqMdl,
  MbsCrmActHttpRemoveActivityExpenseRspMdl,
  MbsCrmActHttpUpdateActivityExpenseReqMdl,
  MbsCrmActHttpUpdateActivityExpenseRspMdl,
  MbsCrmActHttpAddActivityEmployeePipelineReqMdl,
  MbsCrmActHttpAddActivityEmployeePipelineRspMdl,
  MbsCrmActHttpGetActivityEmployeePipelineReqMdl,
  MbsCrmActHttpGetActivityEmployeePipelineRspMdl,
  MbsCrmActHttpGetManyActivityEmployeePipelineReqMdl,
  MbsCrmActHttpGetManyActivityEmployeePipelineRspMdl,
  MbsCrmActHttpGetManyActivityEmployeePipelineRspItemMdl,
  MbsCrmActHttpRemoveManyActivityEmployeePipelineReqMdl,
  MbsCrmActHttpRemoveManyActivityEmployeePipelineRspMdl,
  MbsCrmActHttpUpdateManyActivityEmployeePipelineReqMdl,
  MbsCrmActHttpUpdateManyActivityEmployeePipelineRspMdl,
  MbsCrmActHttpGetManyPastActivityReqMdl,
  MbsCrmActHttpGetManyPastActivityRspMdl,
  MbsCrmActHttpGetManyPastActivityRspItemMdl,
  MbsCrmActHttpDownloadEdmTemplateReqMdl,
  // MbsCrmActHttpDownloadEdmTemplateRspMdl,
  MbsCrmActHttpImportEdmReqMdl,
  MbsCrmActHttpImportEdmRspMdl,
  MbsCrmActHttpImportEdmRspItemMdl,
  MbsCrmActHttpDownloadTeamsTemplateReqMdl,
  // MbsCrmActHttpDownloadTeamsTemplateRspMdl,
  MbsCrmActHttpImportTeamsReqMdl,
  MbsCrmActHttpImportTeamsRspMdl,
  MbsCrmActHttpImportTeamsRspItemMdl,
  MbsCrmActHttpAddActivitySurveyReqMdl,
  MbsCrmActHttpAddActivitySurveyReqItemMdl,
  MbsCrmActHttpAddActivitySurveyRspMdl,
} from "./crmActivityHttpFormat";

//#region 活動
/** 管理者後台-CRM-活動管理-新增活動 */
export const addActivity = async (
  requestData: MbsCrmActHttpAddActivityReqMdl
): Promise<MbsCrmActHttpAddActivityRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlAddActivityReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityName,
    b: requestData.managerActivityKind,
    c: requestData.managerActivityStartTime,
    d: requestData.managerActivityEndTime,
    e: requestData.managerActivityLocation,
    f: requestData.managerActivityExpectedLeadCount,
    g: requestData.managerActivityContent,
    h:
      requestData.managerActivityProductList?.map(
        (item: MbsCrmActHttpAddActivityReqProductItemMdl) =>
          ({
            a: item.managerProductID,
          }) satisfies MbsCrmActCtlAddActivityReqProductItemMdl
      ) ?? [],
    i:
      requestData.managerActivitySponsorList?.map(
        (item: MbsCrmActHttpAddActivityReqSponsorItemMdl) =>
          ({
            a: item.managerActivitySponsorName,
            b: item.managerActivitySponsorAmount,
          }) satisfies MbsCrmActCtlAddActivityReqSponsorItemMdl
      ) ?? [],
    j:
      requestData.managerActivityExpenseList?.map(
        (item: MbsCrmActHttpAddActivityReqExpenseItemMdl) =>
          ({
            a: item.managerActivityExpenseItem,
            b: item.managerActivityExpenseQuantity,
            c: item.managerActivityExpenseTotalAmount,
          }) satisfies MbsCrmActCtlAddActivityReqExpenseItemMdl
      ) ?? [],
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlAddActivityRspMdl>(
      "/api/MbsCrmActivity/AddActivity",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpAddActivityRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivityID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得單筆活動 */
export const getActivity = async (
  requestData: MbsCrmActHttpGetActivityReqMdl
): Promise<MbsCrmActHttpGetActivityRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetActivityReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetActivityRspMdl>(
      "/api/MbsCrmActivity/GetActivity",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetActivityRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivityName: httpResponseData.data.a,
      managerActivityKind: httpResponseData.data.b,
      managerActivityStartTime: httpResponseData.data.c,
      managerActivityEndTime: httpResponseData.data.d,
      managerActivityLocation: httpResponseData.data.e,
      managerActivityExpectedLeadCount: httpResponseData.data.f,
      managerActivityContent: httpResponseData.data.g,
      managerActivityRegisteredCount: httpResponseData.data.h,
      managerActivityTransferredToSalesCount: httpResponseData.data.i,
      managerActivityOpportunityCount: httpResponseData.data.j,
      managerActivityProductList:
        httpResponseData.data.k?.map(
          (item: MbsCrmActCtlGetManyActivityProductRspItemMdl) =>
            ({
              managerActivityProductID: item.a,
              managerProductID: item.b,
              managerProductName: item.c,
              managerProductMainKindID: item.d,
              managerProductMainKindName: item.e,
              managerProductSubKindID: item.f,
              managerProductSubKindName: item.g,
            }) satisfies MbsCrmActHttpGetManyActivityProductRspItemMdl
        ) ?? [],
      managerActivitySponsorList:
        httpResponseData.data.l?.map(
          (item: MbsCrmActCtlGetManyActivitySponsorRspItemMdl) =>
            ({
              managerActivitySponsorID: item.a,
              managerActivitySponsorName: item.b,
              managerActivitySponsorAmount: item.c,
            }) satisfies MbsCrmActHttpGetManyActivitySponsorRspItemMdl
        ) ?? [],
      managerActivityExpenseList:
        httpResponseData.data.m?.map(
          (item: MbsCrmActCtlGetManyActivityExpenseRspItemMdl) =>
            ({
              managerActivityExpenseID: item.a,
              managerActivityExpenseItem: item.b,
              managerActivityExpenseQuantity: item.c,
              managerActivityExpenseTotalAmount: item.d,
            }) satisfies MbsCrmActHttpGetManyActivityExpenseRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得多筆活動 */
export const getManyActivity = async (
  requestData: MbsCrmActHttpGetManyActivityReqMdl
): Promise<MbsCrmActHttpGetManyActivityRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetManyActivityReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityStartTime,
    b: requestData.managerActivityEndTime,
    c: requestData.managerActivityKind,
    d: requestData.managerActivityName,
    e: requestData.pageIndex,
    f: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetManyActivityRspMdl>(
      "/api/MbsCrmActivity/GetManyActivity",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetManyActivityRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerActivityList:
        httpResponseData.data.a?.map(
          (item: MbsCrmActCtlGetManyActivityRspItemMdl) =>
            ({
              managerActivityID: item.a,
              managerActivityName: item.b,
              managerActivityKind: item.c,
              managerActivityStartTime: item.d,
              managerActivityEndTime: item.e,
              managerActivityLocation: item.f,
              managerActivityExpectedLeadCount: item.g,
              managerActivityRegisteredCount: item.h,
              managerActivityTransferredToSalesCount: item.i,
              managerActivitySponsorTotalSponsorAmount: item.j,
              managerActivityExpenseTotalExpenseAmount: item.k,
              managerActivityOpportunityCount: item.l,
            }) satisfies MbsCrmActHttpGetManyActivityRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-更新活動 */
export const updateActivity = async (
  requestData: MbsCrmActHttpUpdateActivityReqMdl
): Promise<MbsCrmActHttpUpdateActivityRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlUpdateActivityReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
    b: requestData.managerActivityName,
    c: requestData.managerActivityStartTime,
    d: requestData.managerActivityEndTime,
    e: requestData.managerActivityLocation,
    f: requestData.managerActivityExpectedLeadCount,
    g: requestData.managerActivityContent,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlUpdateActivityRspMdl>(
      "/api/MbsCrmActivity/UpdateActivity",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpUpdateActivityRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 活動產品
/** 管理者後台-CRM-活動管理-新增活動產品 */
export const addActivityProduct = async (
  requestData: MbsCrmActHttpAddActivityProductReqMdl
): Promise<MbsCrmActHttpAddActivityProductRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlAddActivityProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
    b: requestData.managerProductID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlAddActivityProductRspMdl>(
      "/api/MbsCrmActivity/AddActivityProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpAddActivityProductRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivityProductID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-更新活動產品 */
export const updateActivityProduct = async (
  requestData: MbsCrmActHttpUpdateActivityProductReqMdl
): Promise<MbsCrmActHttpUpdateActivityProductRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlUpdateActivityProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityProductID,
    b: requestData.managerProductID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlUpdateActivityProductRspMdl>(
      "/api/MbsCrmActivity/UpdateActivityProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpUpdateActivityProductRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-刪除活動產品 */
export const removeActivityProduct = async (
  requestData: MbsCrmActHttpRemoveActivityProductReqMdl
): Promise<MbsCrmActHttpRemoveActivityProductRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlRemoveActivityProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityProductID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlRemoveActivityProductRspMdl>(
      "/api/MbsCrmActivity/RemoveActivityProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpRemoveActivityProductRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得單筆活動產品 */
export const getActivityProduct = async (
  requestData: MbsCrmActHttpGetActivityProductReqMdl
): Promise<MbsCrmActHttpGetActivityProductRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetActivityProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityProductID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetActivityProductRspMdl>(
      "/api/MbsCrmActivity/GetActivityProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetActivityProductRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivityProductID: httpResponseData.data.a,
      managerProductID: httpResponseData.data.b,
      managerProductMainKindID: httpResponseData.data.c,
      managerProductSubKindID: httpResponseData.data.d,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得多筆活動產品 */
export const getManyActivityProduct = async (
  requestData: MbsCrmActHttpGetManyActivityProductReqMdl
): Promise<MbsCrmActHttpGetManyActivityProductRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetManyActivityProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetManyActivityProductRspMdl>(
      "/api/MbsCrmActivity/GetManyActivityProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetManyActivityProductRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivityProductList:
        httpResponseData.data.a?.map(
          (item: MbsCrmActCtlGetManyActivityProductRspItemMdl) =>
            ({
              managerActivityProductID: item.a,
              managerProductID: item.b,
              managerProductName: item.c,
              managerProductMainKindID: item.d,
              managerProductMainKindName: item.e,
              managerProductSubKindID: item.f,
              managerProductSubKindName: item.g,
            }) satisfies MbsCrmActHttpGetManyActivityProductRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 活動贊助
/** 管理者後台-CRM-活動管理-新增活動贊助 */
export const addActivitySponsor = async (
  requestData: MbsCrmActHttpAddActivitySponsorReqMdl
): Promise<MbsCrmActHttpAddActivitySponsorRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlAddActivitySponsorReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
    b: requestData.managerActivitySponsorName,
    c: requestData.managerActivitySponsorAmount,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlAddActivitySponsorRspMdl>(
      "/api/MbsCrmActivity/AddActivitySponsor",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpAddActivitySponsorRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivitySponsorID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-更新活動贊助 */
export const updateActivitySponsor = async (
  requestData: MbsCrmActHttpUpdateActivitySponsorReqMdl
): Promise<MbsCrmActHttpUpdateActivitySponsorRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlUpdateActivitySponsorReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivitySponsorID,
    b: requestData.managerActivitySponsorName,
    c: requestData.managerActivitySponsorAmount,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlUpdateActivitySponsorRspMdl>(
      "/api/MbsCrmActivity/UpdateActivitySponsor",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpUpdateActivitySponsorRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-刪除活動贊助 */
export const removeActivitySponsor = async (
  requestData: MbsCrmActHttpRemoveActivitySponsorReqMdl
): Promise<MbsCrmActHttpRemoveActivitySponsorRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlRemoveActivitySponsorReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivitySponsorID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlRemoveActivitySponsorRspMdl>(
      "/api/MbsCrmActivity/RemoveActivitySponsor",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpRemoveActivitySponsorRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得單筆活動贊助 */
export const getActivitySponsor = async (
  requestData: MbsCrmActHttpGetActivitySponsorReqMdl
): Promise<MbsCrmActHttpGetActivitySponsorRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetActivitySponsorReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivitySponsorID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetActivitySponsorRspMdl>(
      "/api/MbsCrmActivity/GetActivitySponsor",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetActivitySponsorRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivitySponsorName: httpResponseData.data.a,
      managerActivitySponsorAmount: httpResponseData.data.b,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得多筆活動贊助 */
export const getManyActivitySponsor = async (
  requestData: MbsCrmActHttpGetManyActivitySponsorReqMdl
): Promise<MbsCrmActHttpGetManyActivitySponsorRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetManyActivitySponsorReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetManyActivitySponsorRspMdl>(
      "/api/MbsCrmActivity/GetManyActivitySponsor",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetManyActivitySponsorRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivitySponsorList:
        httpResponseData.data.a?.map(
          (item: MbsCrmActCtlGetManyActivitySponsorRspItemMdl) =>
            ({
              managerActivitySponsorID: item.a,
              managerActivitySponsorName: item.b,
              managerActivitySponsorAmount: item.c,
            }) satisfies MbsCrmActHttpGetManyActivitySponsorRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 活動支出
/** 管理者後台-CRM-活動管理-新增活動支出 */
export const addActivityExpense = async (
  requestData: MbsCrmActHttpAddActivityExpenseReqMdl
): Promise<MbsCrmActHttpAddActivityExpenseRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlAddActivityExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
    b: requestData.managerActivityExpenseItem,
    c: requestData.managerActivityExpenseQuantity,
    d: requestData.managerActivityExpenseTotalAmount,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlAddActivityExpenseRspMdl>(
      "/api/MbsCrmActivity/AddActivityExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpAddActivityExpenseRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivityExpenseID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-更新活動支出 */
export const updateActivityExpense = async (
  requestData: MbsCrmActHttpUpdateActivityExpenseReqMdl
): Promise<MbsCrmActHttpUpdateActivityExpenseRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlUpdateActivityExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityExpenseID,
    b: requestData.managerActivityExpenseItem,
    c: requestData.managerActivityExpenseQuantity,
    d: requestData.managerActivityExpenseTotalAmount,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlUpdateActivityExpenseRspMdl>(
      "/api/MbsCrmActivity/UpdateActivityExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpUpdateActivityExpenseRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-刪除活動支出 */
export const removeActivityExpense = async (
  requestData: MbsCrmActHttpRemoveActivityExpenseReqMdl
): Promise<MbsCrmActHttpRemoveActivityExpenseRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlRemoveActivityExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityExpenseID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlRemoveActivityExpenseRspMdl>(
      "/api/MbsCrmActivity/RemoveActivityExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpRemoveActivityExpenseRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得單筆活動支出 */
export const getActivityExpense = async (
  requestData: MbsCrmActHttpGetActivityExpenseReqMdl
): Promise<MbsCrmActHttpGetActivityExpenseRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetActivityExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityExpenseID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetActivityExpenseRspMdl>(
      "/api/MbsCrmActivity/GetActivityExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetActivityExpenseRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivityExpenseID: httpResponseData.data.a,
      managerActivityExpenseItem: httpResponseData.data.b,
      managerActivityExpenseQuantity: httpResponseData.data.c,
      managerActivityExpenseTotalAmount: httpResponseData.data.d,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得多筆活動支出 */
export const getManyActivityExpense = async (
  requestData: MbsCrmActHttpGetManyActivityExpenseReqMdl
): Promise<MbsCrmActHttpGetManyActivityExpenseRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetManyActivityExpenseReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetManyActivityExpenseRspMdl>(
      "/api/MbsCrmActivity/GetManyActivityExpense",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetManyActivityExpenseRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivityExpenseList:
        httpResponseData.data.a?.map(
          (item: MbsCrmActCtlGetManyActivityExpenseRspItemMdl) =>
            ({
              managerActivityExpenseID: item.a,
              managerActivityExpenseItem: item.b,
              managerActivityExpenseQuantity: item.c,
              managerActivityExpenseTotalAmount: item.d,
            }) satisfies MbsCrmActHttpGetManyActivityExpenseRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 活動名單
/** 管理者後台-CRM-活動管理-取得多筆活動名單 */
export const getManyActivityEmployeePipeline = async (
  requestData: MbsCrmActHttpGetManyActivityEmployeePipelineReqMdl
): Promise<MbsCrmActHttpGetManyActivityEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetManyActivityEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
    b: requestData.atomPipelineStatus,
    c: requestData.managerCompanyName,
    d: requestData.managerContacterName,
    e: requestData.managerContacterEmail,
    f: requestData.pageIndex,
    g: requestData.pageSize,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmActCtlGetManyActivityEmployeePipelineRspMdl>(
        "/api/MbsCrmActivity/GetManyActivityEmployeePipeline",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetManyActivityEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      employeePipelineList:
        httpResponseData.data.a?.map(
          (item: MbsCrmActCtlGetManyActivityEmployeePipelineRspItemMdl) =>
            ({
              employeePipelineID: item.a,
              atomPipelineStatus: item.b,
              managerCompanyName: item.c,
              managerContacterDepartment: item.d,
              managerContacterJobTitle: item.e,
              managerContacterName: item.f,
              managerContacterEmail: item.g,
              managerContacterPhone: item.h,
              managerContacterTelephone: item.i,
            }) satisfies MbsCrmActHttpGetManyActivityEmployeePipelineRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得單筆活動名單 */
export const getActivityEmployeePipeline = async (
  requestData: MbsCrmActHttpGetActivityEmployeePipelineReqMdl
): Promise<MbsCrmActHttpGetActivityEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetActivityEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmActCtlGetActivityEmployeePipelineRspMdl>(
        "/api/MbsCrmActivity/GetActivityEmployeePipeline",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetActivityEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
      atomPipelineStatus: httpResponseData.data.a,
      managerCompanyUnifiedNumber: httpResponseData.data.b,
      managerCompanyName: httpResponseData.data.c,
      atomEmployeeRange: httpResponseData.data.d,
      atomCustomerGrade: httpResponseData.data.e,
      managerCompanyMainKindName: httpResponseData.data.f,
      managerCompanySubKindName: httpResponseData.data.g,
      atomCity: httpResponseData.data.h,
      managerCompanyLocationAddress: httpResponseData.data.i,
      managerCompanyLocationTelephone: httpResponseData.data.j,
      atomManagerCompanyStatus: httpResponseData.data.k,
      managerContacterName: httpResponseData.data.l,
      managerContacterEmail: httpResponseData.data.m,
      managerContacterPhone: httpResponseData.data.n,
      managerContacterDepartment: httpResponseData.data.o,
      managerContacterJobTitle: httpResponseData.data.p,
      managerContacterTelephone: httpResponseData.data.q,
      managerContacterIsConsent: httpResponseData.data.r,
      managerContacterStatus: httpResponseData.data.s,
      atomRatingKind: httpResponseData.data.t,
      teamsMeetingDuration: httpResponseData.data.u,
      teamsRole: httpResponseData.data.v,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-新增活動名單 */
export const addActivityEmployeePipeline = async (
  requestData: MbsCrmActHttpAddActivityEmployeePipelineReqMdl
): Promise<MbsCrmActHttpAddActivityEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlAddActivityEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
    b: requestData.managerCompanyID,
    c: requestData.managerCompanyLocationID,
    d: requestData.managerContacterID,
    e: requestData.atomPipelineStatus,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmActCtlAddActivityEmployeePipelineRspMdl>(
        "/api/MbsCrmActivity/AddActivityEmployeePipeline",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpAddActivityEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-更新多筆活動名單 */
export const updateManyActivityEmployeePipeline = async (
  requestData: MbsCrmActHttpUpdateManyActivityEmployeePipelineReqMdl
): Promise<MbsCrmActHttpUpdateManyActivityEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlUpdateManyActivityEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineIDList,
    b: requestData.atomPipelineStatus,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmActCtlUpdateManyActivityEmployeePipelineRspMdl>(
        "/api/MbsCrmActivity/UpdateManyActivityEmployeePipeline",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpUpdateManyActivityEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-刪除多筆活動名單 */
export const removeManyActivityEmployeePipeline = async (
  requestData: MbsCrmActHttpRemoveManyActivityEmployeePipelineReqMdl
): Promise<MbsCrmActHttpRemoveManyActivityEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlRemoveManyActivityEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineIDList,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmActCtlRemoveManyActivityEmployeePipelineRspMdl>(
        "/api/MbsCrmActivity/RemoveManyActivityEmployeePipeline",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpRemoveManyActivityEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-取得多筆客戶過往活動 */
export const getManyPastActivity = async (
  requestData: MbsCrmActHttpGetManyPastActivityReqMdl
): Promise<MbsCrmActHttpGetManyPastActivityRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlGetManyPastActivityReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.pageIndex,
    c: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlGetManyPastActivityRspMdl>(
      "/api/MbsCrmActivity/GetManyPastActivity",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpGetManyPastActivityRspMdl = {
      errorCode: httpResponseData.data.aa,
      latestPastActivity: {
        managerActivityName: httpResponseData.data.a.a,
        managerActivityStartTime: httpResponseData.data.a.b,
        managerActivityEndTime: httpResponseData.data.a.c,
      } satisfies MbsCrmActHttpGetManyPastActivityRspItemMdl,
      pastActivityList:
        httpResponseData.data.b?.map(
          (item: MbsCrmActCtlGetManyPastActivityRspItemMdl) =>
            ({
              managerActivityName: item.a,
              managerActivityStartTime: item.b,
              managerActivityEndTime: item.c,
            }) satisfies MbsCrmActHttpGetManyPastActivityRspItemMdl
        ) ?? [],
      totalCount: httpResponseData.data.c,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region eDM
/** 管理者後台-CRM-活動管理-下載eDM範本 */
export const downloadEdmTemplate = async (
  requestData: MbsCrmActHttpDownloadEdmTemplateReqMdl
): Promise<Blob | null> => {
  const httpRequestData: MbsCrmActCtlDownloadEdmTemplateReqMdl = {
    aa: requestData.employeeLoginToken,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlDownloadEdmTemplateRspMdl>(
      "/api/MbsCrmActivity/DownloadEdmTemplate",
      httpRequestData,
      {
        responseType: "blob",
      }
    );

    if (!httpResponseData?.data) {
      return null;
    }

    return httpResponseData.data;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-匯入eDM */
export const importEdm = async (
  requestData: MbsCrmActHttpImportEdmReqMdl
): Promise<MbsCrmActHttpImportEdmRspMdl | null> => {
  const buildFormData = (model: MbsCrmActHttpImportEdmReqMdl): FormData => {
    const formData = new FormData();

    formData.append("aa", model.employeeLoginToken); // 員工登入令牌
    formData.append("a", model.managerActivityID.toString()); // 管理者活動ID
    formData.append("b", model.edmFile); // 匯入的eDM檔案

    return formData;
  };

  const formData = buildFormData(requestData);

  try {
    const httpResponseData = await apiFormClient.post<MbsCrmActCtlImportEdmRspMdl>(
      "/api/MbsCrmActivity/ImportEdm",
      formData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpImportEdmRspMdl = {
      errorCode: httpResponseData.data.aa,
      errorDataList:
        httpResponseData.data.a?.map(
          (item: MbsCrmActCtlImportEdmRspItemMdl) =>
            ({
              edmContacterEmail: item.a,
              edmFirstName: item.b,
              edmLastName: item.c,
              edmContacterTelephone: item.d,
              eEdmContacterPhone: item.e,
              edmCompanyName: item.f,
              edmContacterJobTitle: item.g,
              edmCompanyPhone: item.h,
              edmCompanyFax: item.i,
              edmCompanyAddress: item.j,
              edmRemark: item.k,
              edmContacterDepartment: item.l,
              edmCompanyMainKind: item.m,
              edmCompanySubKind: item.n,
              edmAccountSales: item.o,
              edmRegion: item.p,
              edmCreatedDate: item.q,
              edmDevice: item.r,
            }) satisfies MbsCrmActHttpImportEdmRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region Teams
/** 管理者後台-CRM-活動管理-下載Teams範本 */
export const downloadTeamsTemplate = async (
  requestData: MbsCrmActHttpDownloadTeamsTemplateReqMdl
): Promise<Blob | null> => {
  const httpRequestData: MbsCrmActCtlDownloadTeamsTemplateReqMdl = {
    aa: requestData.employeeLoginToken,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlDownloadTeamsTemplateRspMdl>(
      "/api/MbsCrmActivity/DownloadTeamsTemplate",
      httpRequestData,
      {
        responseType: "blob",
      }
    );

    if (!httpResponseData?.data) {
      return null;
    }

    return httpResponseData.data as Blob;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-CRM-活動管理-匯入Teams */
export const importTeams = async (
  requestData: MbsCrmActHttpImportTeamsReqMdl
): Promise<MbsCrmActHttpImportTeamsRspMdl | null> => {
  const buildFormData = (model: MbsCrmActHttpImportTeamsReqMdl): FormData => {
    const formData = new FormData();

    formData.append("aa", model.employeeLoginToken);
    formData.append("a", model.managerActivityID.toString());
    formData.append("b", model.teamsFile);

    return formData;
  };

  const formData = buildFormData(requestData);
  try {
    const httpResponseData = await apiFormClient.post<MbsCrmActCtlImportTeamsRspMdl>(
      "/api/MbsCrmActivity/ImportTeams",
      formData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpImportTeamsRspMdl = {
      errorCode: httpResponseData.data.aa,
      errorDataList:
        httpResponseData.data.a?.map(
          (item: MbsCrmActCtlImportTeamsRspItemMdl) =>
            ({
              teamsName: item.a,
              teamsFirstJoinTime: item.b,
              teamsLastLeaveTime: item.c,
              teamsMeetingDuration: item.d,
              teamsEmail: item.e,
              teamsParticipantId: item.f,
              teamsRole: item.g,
              teamsContacterRegisterLastName: item.h,
              teamsContacterRegisterFirstName: item.i,
              teamsContacterRegisterEmail: item.j,
              teamsRegistrationTime: item.k,
              teamsRegistrationStatus: item.l,
              teamsCompanyName: item.m,
              teamsContacterDepartment: item.n,
              teamsContacterJobTitle: item.o,
              teamsCompanyTelephone: item.p,
              teamsContacterPhone: item.q,
              teamsActivityInfoSource: item.r,
              teamsContacterIsConsent: item.s,
            }) satisfies MbsCrmActHttpImportTeamsRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 活動問卷
/** 管理者後台-CRM-活動管理-新增活動問卷問題 */
export const addActivitySurvey = async (
  requestData: MbsCrmActHttpAddActivitySurveyReqMdl
): Promise<MbsCrmActHttpAddActivitySurveyRspMdl | null> => {
  const httpRequestData: MbsCrmActCtlAddActivitySurveyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
    b: requestData.managerActivitySurveyID,
    c: requestData.managerActivitySurveyQuestionKind,
    d: requestData.managerActivitySurveyQuestionTitle,
    e: requestData.managerActivitySurveyQuestionContent,
    f: requestData.isOther,
    g: requestData.managerActivitySurveyQuestionSort,
    h:
      requestData.managerActivitySurveyQuestionItemList?.map(
        (item: MbsCrmActHttpAddActivitySurveyReqItemMdl) =>
          ({
            a: item.managerActivitySurveyQuestionItemName,
            b: item.managerActivitySurveyQuestionItemSort,
          }) satisfies MbsCrmActCtlAddActivitySurveyReqItemMdl
      ) ?? [],
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmActCtlAddActivitySurveyRspMdl>(
      "/api/MbsCrmActivity/AddActivitySurvey",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmActHttpAddActivitySurveyRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerActivitySurveyID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion
