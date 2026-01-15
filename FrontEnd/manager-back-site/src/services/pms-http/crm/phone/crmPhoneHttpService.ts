import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import type {
  MbsCrmPhnHttpGetActivityReqMdl,
  MbsCrmPhnHttpGetActivityRspMdl,
  MbsCrmPhnHttpGetActivityProductRspItemMdl,
  MbsCrmPhnHttpGetActivitySponsorRspItemMdl,
  MbsCrmPhnHttpGetActivityExpenseRspItemMdl,
  MbsCrmPhnHttpGetManyActivityReqMdl,
  MbsCrmPhnHttpGetManyActivityRspMdl,
  MbsCrmPhnHttpGetManyActivityRspItemMdl,
  MbsCrmPhnHttpGetManyActivityEmployeePipelineReqMdl,
  MbsCrmPhnHttpGetManyActivityEmployeePipelineRspMdl,
  MbsCrmPhnHttpGetManyActivityEmployeePipelineRspItemMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineReqMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineRspMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineRspProductItemMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineRspSalerItemMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineRspPhoneItemMdl,
  MbsCrmPhnHttpGetManyPastActivityReqMdl,
  MbsCrmPhnHttpGetManyPastActivityRspMdl,
  MbsCrmPhnHttpGetManyPastActivityRspItemMdl,
  MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusReqMdl,
  MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusRspMdl,
  MbsCrmPhnHttpGetEmployeePipelineCompanyReqMdl,
  MbsCrmPhnHttpGetEmployeePipelineCompanyRspMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineCompanyReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineCompanyRspMdl,
  MbsCrmPhnHttpGetEmployeePipelineCompanyRspItemMdl,
  MbsCrmPhnHttpGetOriginalEmployeePipelineContacterRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineContacterRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineContacterRspItemMdl,
  MbsCrmPhnHttpAddEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpAddEmployeePipelineContacterRspMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineContacterRspMdl,
  MbsCrmPhnHttpRemoveEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpRemoveEmployeePipelineContacterRspMdl,
  MbsCrmPhnHttpAddEmployeePipelineSalerReqMdl,
  MbsCrmPhnHttpAddEmployeePipelineSalerRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineSalerReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineSalerRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineSalerRspItemMdl,
  MbsCrmPhnHttpAddEmployeePipelinePhoneReqMdl,
  MbsCrmPhnHttpAddEmployeePipelinePhoneRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelinePhoneReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspItemMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineProductRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineProductRspItemMdl,
  MbsCrmPhnHttpGetEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpGetEmployeePipelineProductRspMdl,
  MbsCrmPhnHttpAddEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpAddEmployeePipelineProductRspMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineProductRspMdl,
  MbsCrmPhnHttpRemoveEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpRemoveEmployeePipelineProductRspMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineRspOriginalCompanyItemMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineRspCompanyItemMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineRspOriginalContacterItemMdl,
  MbsCrmPhnHttpGetOriginalEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpGetActivityEmployeePipelineRspContacterItemMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import type {
  MbsCrmPhnCtlGetActivityReqMdl,
  MbsCrmPhnCtlGetActivityRspMdl,
  MbsCrmPhnCtlGetActivityProductRspItemMdl,
  MbsCrmPhnCtlGetActivitySponsorRspItemMdl,
  MbsCrmPhnCtlGetActivityExpenseRspItemMdl,
  MbsCrmPhnCtlGetManyActivityReqMdl,
  MbsCrmPhnCtlGetManyActivityRspMdl,
  MbsCrmPhnCtlGetManyActivityRspItemMdl,
  MbsCrmPhnCtlGetManyActivityEmployeePipelineReqMdl,
  MbsCrmPhnCtlGetManyActivityEmployeePipelineRspMdl,
  MbsCrmPhnCtlGetManyActivityEmployeePipelineRspItemMdl,
  MbsCrmPhnCtlGetActivityEmployeePipelineReqMdl,
  MbsCrmPhnCtlGetActivityEmployeePipelineRspMdl,
  MbsCrmPhnCtlGetActivityEmployeePipelineRspProductItemMdl,
  MbsCrmPhnCtlGetActivityEmployeePipelineRspSalerItemMdl,
  MbsCrmPhnCtlGetActivityEmployeePipelineRspPhoneItemMdl,
  MbsCrmPhnCtlGetManyPastActivityReqMdl,
  MbsCrmPhnCtlGetManyPastActivityRspMdl,
  MbsCrmPhnCtlGetManyPastActivityRspItemMdl,
  MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusReqMdl,
  MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusRspMdl,
  MbsCrmPhnCtlGetEmployeePipelineCompanyReqMdl,
  MbsCrmPhnCtlGetEmployeePipelineCompanyRspMdl,
  MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl,
  MbsCrmPhnCtlUpdateEmployeePipelineCompanyReqMdl,
  MbsCrmPhnCtlUpdateEmployeePipelineCompanyRspMdl,
  MbsCrmPhnCtlGetOriginalEmployeePipelineContacterReqMdl,
  MbsCrmPhnCtlGetOriginalEmployeePipelineContacterRspMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineContacterReqMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineContacterRspMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineContacterRspItemMdl,
  MbsCrmPhnCtlUpdateEmployeePipelineContacterReqMdl,
  MbsCrmPhnCtlUpdateEmployeePipelineContacterRspMdl,
  MbsCrmPhnCtlRemoveEmployeePipelineContacterReqMdl,
  MbsCrmPhnCtlRemoveEmployeePipelineContacterRspMdl,
  MbsCrmPhnCtlAddEmployeePipelineSalerReqMdl,
  MbsCrmPhnCtlAddEmployeePipelineSalerRspMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineSalerReqMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineSalerRspMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineSalerRspItemMdl,
  MbsCrmPhnCtlAddEmployeePipelinePhoneReqMdl,
  MbsCrmPhnCtlAddEmployeePipelinePhoneRspMdl,
  MbsCrmPhnCtlGetManyEmployeePipelinePhoneReqMdl,
  MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspMdl,
  MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspItemMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineProductReqMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineProductRspMdl,
  MbsCrmPhnCtlGetManyEmployeePipelineProductRspItemMdl,
  MbsCrmPhnCtlGetEmployeePipelineProductReqMdl,
  MbsCrmPhnCtlGetEmployeePipelineProductRspMdl,
  MbsCrmPhnCtlAddEmployeePipelineProductReqMdl,
  MbsCrmPhnCtlAddEmployeePipelineProductRspMdl,
  MbsCrmPhnCtlUpdateEmployeePipelineProductReqMdl,
  MbsCrmPhnCtlUpdateEmployeePipelineProductRspMdl,
  MbsCrmPhnCtlRemoveEmployeePipelineProductReqMdl,
  MbsCrmPhnCtlRemoveEmployeePipelineProductRspMdl,
  MbsCrmPhnCtlAddEmployeePipelineContacterReqMdl,
  MbsCrmPhnCtlAddEmployeePipelineContacterRspMdl,
  MbsCrmPhnCtlGetActivityEmployeePipelineRspContacterItemMdl,
} from "@/services/pms-http/crm/phone/crmPhoneControllerFormat";

//#region 活動
/** 管理者後台-CRM-電銷管理-取得多筆活動 */
export const getManyPhoneActivity = async (
  requestData: MbsCrmPhnHttpGetManyActivityReqMdl
): Promise<MbsCrmPhnHttpGetManyActivityRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetManyActivityReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityStartTime,
    b: requestData.managerActivityEndTime,
    c: requestData.managerActivityKind,
    d: requestData.managerActivityName,
    e: requestData.pageIndex,
    f: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPhnCtlGetManyActivityRspMdl>(
      "/api/MbsCrmPhone/GetManyActivity",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpGetManyActivityRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerActivityList:
        httpResponseData.data.a?.map(
          (item: MbsCrmPhnCtlGetManyActivityRspItemMdl) =>
            ({
              managerActivityID: item.a,
              managerActivityName: item.b,
              managerActivityKind: item.c,
              managerActivityStartTime: item.d,
              managerActivityEndTime: item.e,
              managerActivityRegisteredCount: item.f,
              managerActivityTransferredToSalesCount: item.g,
              managerActivityPhoneCount: item.h,
              managerActivityEmployeePipelineCount: item.i,
            }) satisfies MbsCrmPhnHttpGetManyActivityRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-取得單筆活動 */
export const getPhoneActivity = async (
  requestData: MbsCrmPhnHttpGetActivityReqMdl
): Promise<MbsCrmPhnHttpGetActivityRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetActivityReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerActivityID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPhnCtlGetActivityRspMdl>(
      "/api/MbsCrmPhone/GetActivity",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpGetActivityRspMdl = {
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
      managerActivityEmployeePipelineCount: httpResponseData.data.j,
      managerActivityPhoneCount: httpResponseData.data.k,
      managerActivityProductList:
        httpResponseData.data.l?.map(
          (item: MbsCrmPhnCtlGetActivityProductRspItemMdl) =>
            ({
              managerActivityProductID: item.a,
              managerProductName: item.b,
              managerProductMainKindName: item.c,
              managerProductSubKindName: item.d,
            }) satisfies MbsCrmPhnHttpGetActivityProductRspItemMdl
        ) ?? [],
      managerActivitySponsorList:
        httpResponseData.data.m?.map(
          (item: MbsCrmPhnCtlGetActivitySponsorRspItemMdl) =>
            ({
              managerActivitySponsorID: item.a,
              managerActivitySponsorName: item.b,
              managerActivitySponsorAmount: item.c,
            }) satisfies MbsCrmPhnHttpGetActivitySponsorRspItemMdl
        ) ?? [],
      managerActivityExpenseList:
        httpResponseData.data.n?.map(
          (item: MbsCrmPhnCtlGetActivityExpenseRspItemMdl) =>
            ({
              managerActivityExpenseID: item.a,
              managerActivityExpenseItem: item.b,
              managerActivityExpenseQuantity: item.c,
              managerActivityExpenseTotalAmount: item.d,
            }) satisfies MbsCrmPhnHttpGetActivityExpenseRspItemMdl
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

//#region 名單
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-取得多筆名單 */
export const getManyPhoneActivityEmployeePipeline = async (
  requestData: MbsCrmPhnHttpGetManyActivityEmployeePipelineReqMdl
): Promise<MbsCrmPhnHttpGetManyActivityEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetManyActivityEmployeePipelineReqMdl = {
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
      await apiJsonClient.post<MbsCrmPhnCtlGetManyActivityEmployeePipelineRspMdl>(
        "/api/MbsCrmPhone/GetManyActivityEmployeePipeline",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpGetManyActivityEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      activityEmployeePipelineList:
        httpResponseData.data.a?.map(
          (item: MbsCrmPhnCtlGetManyActivityEmployeePipelineRspItemMdl) =>
            ({
              managerPipelineID: item.a,
              atomPipelineStatus: item.b,
              hasCompany: item.c,
              managerCompanyID: item.d,
              managerCompanyName: item.e,
              hasContacter: item.f,
              employeePipelineContacterID: item.g,
              managerContacterDepartment: item.h,
              managerContacterJobTitle: item.i,
              managerContacterName: item.j,
              managerContacterEmail: item.k,
              managerContacterPhone: item.l,
              managerContacterTelephone: item.m,
              employeePipelineSalerTrackingTime: item.n,
            }) satisfies MbsCrmPhnHttpGetManyActivityEmployeePipelineRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-取得單筆名單 */
export const getPhoneActivityEmployeePipeline = async (
  requestData: MbsCrmPhnHttpGetActivityEmployeePipelineReqMdl
): Promise<MbsCrmPhnHttpGetActivityEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetActivityEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlGetActivityEmployeePipelineRspMdl>(
        "/api/MbsCrmPhone/GetActivityEmployeePipeline",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpGetActivityEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
      atomPipelineStatus: httpResponseData.data.a,
      originalCompany: httpResponseData.data.b
        ? ({
          managerCompanyUnifiedNumber: httpResponseData.data.b.a,
          managerCompanyName: httpResponseData.data.b.b,
          atomEmployeeRange: httpResponseData.data.b.c,
          atomCustomerGrade: httpResponseData.data.b.d,
          managerCompanyMainKindName: httpResponseData.data.b.e,
          managerCompanySubKindName: httpResponseData.data.b.f,
          atomCity: httpResponseData.data.b.g,
          managerCompanyLocationAddress: httpResponseData.data.b.h,
          managerCompanyLocationTelephone: httpResponseData.data.b.i,
          managerCompanyLocationStatus: httpResponseData.data.b.j,
        } satisfies MbsCrmPhnHttpGetActivityEmployeePipelineRspOriginalCompanyItemMdl)
        : null,
      hasCompany: httpResponseData.data.c,
      company: httpResponseData.data.d
        ? ({
          managerCompanyUnifiedNumber: httpResponseData.data.d.a,
          managerCompanyID: httpResponseData.data.d.b,
          managerCompanyName: httpResponseData.data.d.c,
          atomEmployeeRange: httpResponseData.data.d.d,
          atomCustomerGrade: httpResponseData.data.d.e,
          managerCompanyMainKindName: httpResponseData.data.d.f,
          managerCompanySubKindName: httpResponseData.data.d.g,
          atomCity: httpResponseData.data.d.h,
          managerCompanyLocationID: httpResponseData.data.d.i,
          managerCompanyLocationAddress: httpResponseData.data.d.j,
          managerCompanyLocationTelephone: httpResponseData.data.d.k,
          managerCompanyLocationStatus: httpResponseData.data.d.l,
        } satisfies MbsCrmPhnHttpGetActivityEmployeePipelineRspCompanyItemMdl)
        : null,
      originalContacter: httpResponseData.data.e
        ? ({
          managerContacterName: httpResponseData.data.e.a,
          managerContacterEmail: httpResponseData.data.e.b,
          managerContacterPhone: httpResponseData.data.e.c,
          managerContacterDepartment: httpResponseData.data.e.d,
          managerContacterJobTitle: httpResponseData.data.e.e,
          managerContacterTelephone: httpResponseData.data.e.f,
          managerContacterIsConsent: httpResponseData.data.e.g,
          managerContacterStatus: httpResponseData.data.e.h,
          atomRatingKind: httpResponseData.data.e.i,
        } satisfies MbsCrmPhnHttpGetActivityEmployeePipelineRspOriginalContacterItemMdl)
        : null,
      contacterList: Array.isArray(httpResponseData.data.f)
        ? httpResponseData.data.f.map(
          (item: MbsCrmPhnCtlGetActivityEmployeePipelineRspContacterItemMdl) =>
            ({
              managerContacterID: item.a,
              employeePipelineContacterIsPrimary: item.b,
              managerContacterName: item.c,
              managerContacterEmail: item.d,
              managerContacterPhone: item.e,
              managerContacterDepartment: item.f,
              managerContacterJobTitle: item.g,
              managerContacterTelephone: item.h,
              managerContacterIsConsent: item.i,
              managerContacterStatus: item.j,
              atomRatingKind: item.k,
              managerContacterRemark: item.l,
            }) satisfies MbsCrmPhnHttpGetActivityEmployeePipelineRspContacterItemMdl
        )
        : [],
      teamsMeetingDuration: httpResponseData.data.g,
      teamsRole: httpResponseData.data.h,
      productList:
        httpResponseData.data.i?.map(
          (item: MbsCrmPhnCtlGetActivityEmployeePipelineRspProductItemMdl) =>
            ({
              employeePipelineProductID: item.a,
              managerProductID: item.b,
              managerProductName: item.c,
              managerProductMainKindID: item.d,
              managerProductMainKindName: item.e,
              managerProductSubKindID: item.f,
              managerProductSubKindName: item.g,
              managerProductSpecificationID: item.h,
              managerProductSpecificationName: item.i,
              managerProductSpecificationSellPrice: item.j,
            }) satisfies MbsCrmPhnHttpGetActivityEmployeePipelineRspProductItemMdl
        ) ?? [],
      phoneList:
        httpResponseData.data.j?.map(
          (item: MbsCrmPhnCtlGetActivityEmployeePipelineRspPhoneItemMdl) =>
            ({
              employeePipelinePhoneID: item.a,
              employeePipelinePhoneRecordTime: item.b,
              managerContacterName: item.c,
              employeePipelinePhoneTitle: item.d,
              employeePipelinePhoneRemark: item.e,
              employeePipelinePhoneCreateEmployeeName: item.f,
            }) satisfies MbsCrmPhnHttpGetActivityEmployeePipelineRspPhoneItemMdl
        ) ?? [],
      salerList:
        httpResponseData.data.k?.map(
          (item: MbsCrmPhnCtlGetActivityEmployeePipelineRspSalerItemMdl) =>
            ({
              employeePipelineSalerID: item.a,
              employeePipelineSalerStatus: item.b,
              employeePipelineSalerCreateTime: item.c,
              employeePipelineSalerCreateEmployeeName: item.d,
              employeePipelineSalerReplyTime: item.e,
              employeePipelineSalerEmployeeName: item.f,
              employeePipelineSalerRemark: item.g,
            }) satisfies MbsCrmPhnHttpGetActivityEmployeePipelineRspSalerItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-更新名單狀態 */
export const updatePhoneActivityEmployeePipelineStatus = async (
  requestData: MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusReqMdl
): Promise<MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusRspMdl>(
        "/api/MbsCrmPhone/UpdateActivityEmployeePipelineStatus",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    return { errorCode: httpResponseData.data.aa };
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-取得多筆客戶過往活動 */
export const getManyPhonePastActivity = async (
  requestData: MbsCrmPhnHttpGetManyPastActivityReqMdl
): Promise<MbsCrmPhnHttpGetManyPastActivityRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetManyPastActivityReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.pageIndex,
    c: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPhnCtlGetManyPastActivityRspMdl>(
      "/api/MbsCrmPhone/GetManyPastActivity",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpGetManyPastActivityRspMdl = {
      errorCode: httpResponseData.data.aa,
      latestPastActivity:
        httpResponseData.data.a?.map(
          (item: MbsCrmPhnCtlGetManyPastActivityRspItemMdl) =>
            ({
              managerActivityName: item.a,
              managerActivityStartTime: item.b,
              managerActivityEndTime: item.c,
            }) satisfies MbsCrmPhnHttpGetManyPastActivityRspItemMdl
        ) ?? [],
      pastActivityList:
        httpResponseData.data.b?.map(
          (item: MbsCrmPhnCtlGetManyPastActivityRspItemMdl) =>
            ({
              managerActivityName: item.a,
              managerActivityStartTime: item.b,
              managerActivityEndTime: item.c,
            }) satisfies MbsCrmPhnHttpGetManyPastActivityRspItemMdl
        ) ?? [],
      totalCount: httpResponseData.data.c,
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 客戶
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-取得名單客戶 */
export const getPhoneEmployeePipelineCompany = async (
  requestData: MbsCrmPhnHttpGetEmployeePipelineCompanyReqMdl
): Promise<MbsCrmPhnHttpGetEmployeePipelineCompanyRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetEmployeePipelineCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPhnCtlGetEmployeePipelineCompanyRspMdl>(
      "/api/MbsCrmPhone/GetEmployeePipelineCompany",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const rsp = httpResponseData.data;

    const mapCompanyItem = (
      item: MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl
    ): MbsCrmPhnHttpGetEmployeePipelineCompanyRspItemMdl => ({
      managerCompanyUnifiedNumber: item.a,
      managerCompanyName: item.b,
      atomEmployeeRange: item.c,
      atomCustomerGrade: item.d,
      managerCompanyMainKindName: item.e,
      managerCompanySubKindName: item.f,
      atomCity: item.g,
      managerCompanyLocationName: item.h,
      managerCompanyLocationAddress: item.i,
      managerCompanyLocationTelephone: item.j,
      managerCompanyLocationStatus: item.k,
    });

    const responseData: MbsCrmPhnHttpGetEmployeePipelineCompanyRspMdl = {
      errorCode: rsp.aa,
      originalCompany: rsp.a ? mapCompanyItem(rsp.a) : null,
      company: rsp.b ? mapCompanyItem(rsp.b) : null,
    };

    return responseData;
  } catch (error) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-更新名單客戶 */
export const updatePhoneEmployeePipelineCompany = async (
  requestData: MbsCrmPhnHttpUpdateEmployeePipelineCompanyReqMdl
): Promise<MbsCrmPhnHttpUpdateEmployeePipelineCompanyRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlUpdateEmployeePipelineCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.managerCompanyID,
    c: requestData.managerCompanyLocationID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlUpdateEmployeePipelineCompanyRspMdl>(
        "/api/MbsCrmPhone/UpdateEmployeePipelineCompany",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpUpdateEmployeePipelineCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 窗口
//----------------------------------------------------------------------
/**  管理者後台-CRM-電銷管理-取得多筆名單窗口*/
export const getManyPhoneEmployeePipelineContacter = async (
  requestData: MbsCrmPhnHttpGetManyEmployeePipelineContacterReqMdl
): Promise<MbsCrmPhnHttpGetManyEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetManyEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlGetManyEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPhone/GetManyEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;
    const rsp = httpResponseData.data;

    const responseData: MbsCrmPhnHttpGetManyEmployeePipelineContacterRspMdl = {
      errorCode: rsp.aa,
      employeePipelineContacterList:
        rsp.a?.map(
          (item: MbsCrmPhnCtlGetManyEmployeePipelineContacterRspItemMdl) =>
            ({
              employeePipelineContacterID: item.a,
              managerContacterID: item.b,
              employeePipelineContacterIsPrimary: item.c,
              managerContacterName: item.d,
              managerContacterEmail: item.e,
              managerContacterPhone: item.f,
              managerContacterDepartment: item.g,
              managerContacterJobTitle: item.h,
              managerContacterTelephone: item.i,
              managerContacterIsConsent: item.j,
              managerContacterStatus: item.k,
              atomRatingKind: item.l,
              managerContacterRemark: item.m,
            }) satisfies MbsCrmPhnHttpGetManyEmployeePipelineContacterRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/**  管理者後台-CRM-電銷管理-取得原始名單窗口*/
export const getPhoneOriginalEmployeePipelineContacter = async (
  requestData: MbsCrmPhnHttpGetOriginalEmployeePipelineContacterReqMdl
): Promise<MbsCrmPhnHttpGetOriginalEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetOriginalEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlGetOriginalEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPhone/GetOriginalEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;
    const rsp = httpResponseData.data;

    const responseData: MbsCrmPhnHttpGetOriginalEmployeePipelineContacterRspMdl = {
      errorCode: rsp.aa,
      managerContacterName: rsp.a,
      managerContacterEmail: rsp.b,
      managerContacterPhone: rsp.c,
      managerContacterDepartment: rsp.d,
      managerContacterJobTitle: rsp.e,
      managerContacterTelephone: rsp.f,
      managerContacterIsConsent: rsp.g,
      managerContacterStatus: rsp.h,
      atomRatingKind: rsp.i,
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-新增名單窗口 */
export const addPhoneEmployeePipelineContacter = async (
  requestData: MbsCrmPhnHttpAddEmployeePipelineContacterReqMdl
): Promise<MbsCrmPhnHttpAddEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlAddEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.managerContacterID,
    c: requestData.employeePipelineContacterIsPrimary,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlAddEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPhone/AddEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    return {
      errorCode: httpResponseData.data.aa,
    };
  } catch (error) {
    console.error("API錯誤:", error);
    return null;
  }
};
//---------------------------------------------------------------------
/**  管理者後台-CRM-電銷管理-更新名單窗口*/
export const updatePhoneEmployeePipelineContacter = async (
  requestData: MbsCrmPhnHttpUpdateEmployeePipelineContacterReqMdl
): Promise<MbsCrmPhnHttpUpdateEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlUpdateEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineContacterID,
    b: requestData.managerContacterID,
    c: requestData.employeePipelineContacterIsPrimary,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlUpdateEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPhone/UpdateEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    return { errorCode: httpResponseData.data.aa };
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/**  管理者後台-CRM-電銷管理-刪除名單窗口*/
export const removePhoneEmployeePipelineContacter = async (
  requestData: MbsCrmPhnHttpRemoveEmployeePipelineContacterReqMdl
): Promise<MbsCrmPhnHttpRemoveEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlRemoveEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineContacterID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlRemoveEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPhone/RemoveEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;
    return { errorCode: httpResponseData.data.aa };
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};

//#endregion

//#region 指派業務紀錄
//----------------------------------------------------------------------
/**  管理者後台-CRM-電銷管理新增指派業務 */
export const addPhoneEmployeePipelineSaler = async (
  requestData: MbsCrmPhnHttpAddEmployeePipelineSalerReqMdl
): Promise<MbsCrmPhnHttpAddEmployeePipelineSalerRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlAddEmployeePipelineSalerReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeePipelineSalerEmployeeID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPhnCtlAddEmployeePipelineSalerRspMdl>(
      "/api/MbsCrmPhone/AddEmployeePipelineSaler",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpAddEmployeePipelineSalerRspMdl = {
      errorCode: httpResponseData.data.aa,
    };
    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/**  管理者後台-CRM-取得多筆指派業務 */
export const getManyPhoneEmployeePipelineSaler = async (
  requestData: MbsCrmPhnHttpGetManyEmployeePipelineSalerReqMdl
): Promise<MbsCrmPhnHttpGetManyEmployeePipelineSalerRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetManyEmployeePipelineSalerReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlGetManyEmployeePipelineSalerRspMdl>(
        "/api/MbsCrmPhone/GetManyEmployeePipelineSaler",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;
    const rsp = httpResponseData.data;

    const responseData: MbsCrmPhnHttpGetManyEmployeePipelineSalerRspMdl = {
      errorCode: rsp.aa,
      employeePipelineSalerList:
        rsp.a?.map(
          (item: MbsCrmPhnCtlGetManyEmployeePipelineSalerRspItemMdl) =>
            ({
              employeePipelineSalerID: item.a,
              employeePipelineSalerStatus: item.b,
              employeePipelineSalerCreateTime: item.c,
              employeePipelineSalerCreateEmployeeName: item.d,
              employeePipelineSalerReplyTime: item.e,
              employeePipelineSalerEmployeeName: item.f,
              employeePipelineSalerRemark: item.g,
            }) satisfies MbsCrmPhnHttpGetManyEmployeePipelineSalerRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//#endregion

//#region 電銷紀錄
//----------------------------------------------------------------------
/** 管理者後台-CRM-取得多筆電銷紀錄 */
export const getManyPhoneEmployeePipelinePhone = async (
  requestData: MbsCrmPhnHttpGetManyEmployeePipelinePhoneReqMdl
): Promise<MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetManyEmployeePipelinePhoneReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspMdl>(
        "/api/MbsCrmPhone/GetManyEmployeePipelinePhone",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;
    const rsp = httpResponseData.data;

    const responseData: MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspMdl = {
      errorCode: rsp.aa,
      employeePipelinePhoneList:
        rsp.a?.map(
          (item: MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspItemMdl) =>
            ({
              employeePipelinePhoneID: item.a,
              employeePipelinePhoneRecordTime: item.b,
              managerContacterName: item.c,
              employeePipelinePhoneTitle: item.d,
              employeePipelinePhoneCreateEmployeeName: item.e,
              employeePipelinePhoneRemark: item.f,
            }) satisfies MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-新增電銷紀錄 */
export const addPhoneEmployeePipelinePhone = async (
  requestData: MbsCrmPhnHttpAddEmployeePipelinePhoneReqMdl
): Promise<MbsCrmPhnHttpAddEmployeePipelinePhoneRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlAddEmployeePipelinePhoneReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeePipelinePhoneRecordTime,
    c: requestData.managerContacterID,
    d: requestData.employeePipelinePhoneTitle,
    e: requestData.employeePipelinePhoneRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPhnCtlAddEmployeePipelinePhoneRspMdl>(
      "/api/MbsCrmPhone/AddEmployeePipelinePhone",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpAddEmployeePipelinePhoneRspMdl = {
      errorCode: httpResponseData.data.aa,
    };
    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//#endregion

//#region 產品
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-取得多筆電銷產品 */
export const getManyPhoneEmployeePipelineProduct = async (
  requestData: MbsCrmPhnHttpGetManyEmployeePipelineProductReqMdl
): Promise<MbsCrmPhnHttpGetManyEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetManyEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlGetManyEmployeePipelineProductRspMdl>(
        "/api/MbsCrmPhone/GetManyEmployeePipelineProduct",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;
    const rsp = httpResponseData.data;

    const responseData: MbsCrmPhnHttpGetManyEmployeePipelineProductRspMdl = {
      errorCode: rsp.aa,
      employeePipelineProductList:
        rsp.a?.map(
          (item: MbsCrmPhnCtlGetManyEmployeePipelineProductRspItemMdl) =>
            ({
              employeePipelineProductID: item.a,
              managerProductID: item.b,
              managerProductName: item.c,
              managerProductMainKindID: item.d,
              managerProductMainKindName: item.e,
              managerProductSubKindID: item.f,
              managerProductSubKindName: item.g,
              managerProductSpecificationID: item.h,
              managerProductSpecificationName: item.i,
              managerProductSpecificationSellPrice: item.j,
            }) satisfies MbsCrmPhnHttpGetManyEmployeePipelineProductRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-取得單筆電銷產品 */
export const getPhoneEmployeePipelineProduct = async (
  requestData: MbsCrmPhnHttpGetEmployeePipelineProductReqMdl
): Promise<MbsCrmPhnHttpGetEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlGetEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineProductID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPhnCtlGetEmployeePipelineProductRspMdl>(
      "/api/MbsCrmPhone/GetEmployeePipelineProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPhnHttpGetEmployeePipelineProductRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineID: httpResponseData.data.a,
      managerProductMainKindID: httpResponseData.data.b,
      managerProductMainKindName: httpResponseData.data.c,
      managerProductSubKindID: httpResponseData.data.d,
      managerProductSubKindName: httpResponseData.data.e,
      managerProductID: httpResponseData.data.f,
      managerProductName: httpResponseData.data.g,
      managerProductSpecificationID: httpResponseData.data.h,
      managerProductSpecificationName: httpResponseData.data.i,
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//------------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-新增電銷產品 */
export const addPhoneEmployeePipelineProduct = async (
  requestData: MbsCrmPhnHttpAddEmployeePipelineProductReqMdl
): Promise<MbsCrmPhnHttpAddEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlAddEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.managerProductID,
    c: requestData.managerProductSpecificationID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPhnCtlAddEmployeePipelineProductRspMdl>(
      "/api/MbsCrmPhone/AddEmployeePipelineProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;
    const rsp = httpResponseData.data;

    const responseData: MbsCrmPhnHttpAddEmployeePipelineProductRspMdl = {
      errorCode: rsp.aa,
      employeePipelineProductID: rsp.a,
    };

    return responseData;
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-更新電銷產品 */
export const updatePhoneEmployeePipelineProduct = async (
  requestData: MbsCrmPhnHttpUpdateEmployeePipelineProductReqMdl
): Promise<MbsCrmPhnHttpUpdateEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlUpdateEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineProductID,
    b: requestData.managerProductID,
    c: requestData.managerProductSpecificationID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlUpdateEmployeePipelineProductRspMdl>(
        "/api/MbsCrmPhone/UpdateEmployeePipelineProduct",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    return { errorCode: httpResponseData.data.aa };
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-電銷管理-刪除電銷產品 */
export const removePhoneEmployeePipelineProduct = async (
  requestData: MbsCrmPhnHttpRemoveEmployeePipelineProductReqMdl
): Promise<MbsCrmPhnHttpRemoveEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPhnCtlRemoveEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineProductID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPhnCtlRemoveEmployeePipelineProductRspMdl>(
        "/api/MbsCrmPhone/RemoveEmployeePipelineProduct",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    return { errorCode: httpResponseData.data.aa };
  } catch (error) {
    console.error("API錯誤:", (error as AxiosError).response?.data ?? error);
    return null;
  }
};
//#endregion
