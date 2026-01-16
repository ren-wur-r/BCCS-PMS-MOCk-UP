import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  // 名單
  MbsCrmPplHttpGetManyEmployeePipelineReqMdl,
  MbsCrmPplHttpGetManyEmployeePipelineRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineRspItemMdl,
  MbsCrmPplHttpGetEmployeePipelineReqMdl,
  MbsCrmPplHttpGetEmployeePipelineRspMdl,
  MbsCrmPplHttpGetEmployeePipelineRspCompanyItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspContacterItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspPendingSalerItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspSalerItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspSalerTrackingItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspProductItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspDueItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspBillItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqMdl,
  MbsCrmPplHttpAddEmployeePipelineReqContacterItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqSalerTrackingItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqProductItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqBillItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqDueItemMdl,
  MbsCrmPplHttpAddEmployeePipelineRspMdl,
  MbsCrmPplHttpUpdatePipelineStatusReqMdl,
  MbsCrmPplHttpUpdatePipelineStatusRspMdl,
  MbsCrmPplHttpTransferPipelineToProjectReqMdl,
  MbsCrmPplHttpTransferPipelineToProjectReqItemMdl,
  MbsCrmPplHttpTransferPipelineToProjectRspMdl,
  // 客戶
  MbsCrmPplHttpGetEmployeePipelineCompanyReqMdl,
  MbsCrmPplHttpGetEmployeePipelineCompanyRspMdl,
  MbsCrmPplHttpUpdateEmployeePipelineCompanyReqMdl,
  MbsCrmPplHttpUpdateEmployeePipelineCompanyRspMdl,
  // 窗口
  MbsCrmPplHttpAddEmployeePipelineContacterReqMdl,
  MbsCrmPplHttpAddEmployeePipelineContacterRspMdl,
  MbsCrmPplHttpUpdateEmployeePipelineContacterReqMdl,
  MbsCrmPplHttpUpdateEmployeePipelineContacterRspMdl,
  MbsCrmPplHttpRemoveEmployeePipelineContacterReqMdl,
  MbsCrmPplHttpRemoveEmployeePipelineContacterRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineContacterReqMdl,
  MbsCrmPplHttpGetManyEmployeePipelineContacterRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineContacterRspItemMdl,
  // 業務紀錄
  MbsCrmPplHttpGetManyEmployeePipelineSalerReqMdl,
  MbsCrmPplHttpGetManyEmployeePipelineSalerRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineSalerRspItemMdl,
  MbsCrmPplHttpHandleEmployeePipelineSalerReqMdl,
  MbsCrmPplHttpHandleEmployeePipelineSalerRspMdl,
  // 業務開發紀錄
  MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingReqMdl,
  MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingRspItemMdl,
  MbsCrmPplHttpAddEmployeePipelineSalerTrackingReqMdl,
  MbsCrmPplHttpAddEmployeePipelineSalerTrackingRspMdl,
  // 履約期限
  MbsCrmPplHttpGetEmployeePipelineDueReqMdl,
  MbsCrmPplHttpGetEmployeePipelineDueRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineDueReqMdl,
  MbsCrmPplHttpGetManyEmployeePipelineDueRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineDueRspItemMdl,
  MbsCrmPplHttpAddEmployeePipelineDueReqMdl,
  MbsCrmPplHttpAddEmployeePipelineDueRspMdl,
  MbsCrmPplHttpUpdateEmployeePipelineDueReqMdl,
  MbsCrmPplHttpUpdateEmployeePipelineDueRspMdl,
  MbsCrmPplHttpRemoveEmployeePipelineDueReqMdl,
  MbsCrmPplHttpRemoveEmployeePipelineDueRspMdl,
  // 商機產品
  MbsCrmPplHttpGetEmployeePipelineProductReqMdl,
  MbsCrmPplHttpGetEmployeePipelineProductRspMdl,
  MbsCrmPplHttpAddEmployeePipelineProductReqMdl,
  MbsCrmPplHttpAddEmployeePipelineProductRspMdl,
  MbsCrmPplHttpUpdateEmployeePipelineProductReqMdl,
  MbsCrmPplHttpUpdateEmployeePipelineProductRspMdl,
  MbsCrmPplHttpRemoveEmployeePipelineProductReqMdl,
  MbsCrmPplHttpRemoveEmployeePipelineProductRspMdl,
  // 發票紀錄
  MbsCrmPplHttpGetEmployeePipelineBillReqMdl,
  MbsCrmPplHttpGetEmployeePipelineBillRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineBillReqMdl,
  MbsCrmPplHttpGetManyEmployeePipelineBillRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineBillRspItemMdl,
  MbsCrmPplHttpAddManyEmployeePipelineBillReqMdl,
  MbsCrmPplHttpAddManyEmployeePipelineBillReqItemMdl,
  MbsCrmPplHttpAddManyEmployeePipelineBillRspMdl,
  MbsCrmPplHttpUpdateEmployeePipelineBillReqMdl,
  MbsCrmPplHttpUpdateEmployeePipelineBillRspMdl,
  MbsCrmPplHttpUpdateManyEmployeePipelineBillReqMdl,
  MbsCrmPplHttpUpdateManyEmployeePipelineBillReqItemMdl,
  MbsCrmPplHttpUpdateManyEmployeePipelineBillRspMdl,
  MbsCrmPplHttpRemoveManyEmployeePipelineBillReqMdl,
  MbsCrmPplHttpRemoveManyEmployeePipelineBillRspMdl,
  MbsCrmPplHttpNotifyBillIssueReqMdl,
  MbsCrmPplHttpNotifyBillIssueRspMdl,
  MbsCrmPplHttpConfirmBillIssueReqMdl,
  MbsCrmPplHttpConfirmBillIssueRspMdl,
} from "@/services/pms-http/crm/pipeline/crmPipelineHttpFormat";
import type {
  // 名單
  MbsCrmPplCtlGetManyEmployeePipelineReqMdl,
  MbsCrmPplCtlGetManyEmployeePipelineRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineRspItemMdl,
  MbsCrmPplCtlGetEmployeePipelineReqMdl,
  MbsCrmPplCtlGetEmployeePipelineRspMdl,
  // MbsCrmPplCtlGetEmployeePipelineRspCompanyItemMdl,
  MbsCrmPplCtlGetEmployeePipelineRspContacterItemMdl,
  // MbsCrmPplCtlGetEmployeePipelineRspPendingSalerItemMdl,
  MbsCrmPplCtlGetEmployeePipelineRspSalerItemMdl,
  MbsCrmPplCtlGetEmployeePipelineRspSalerTrackingItemMdl,
  MbsCrmPplCtlGetEmployeePipelineRspProductItemMdl,
  MbsCrmPplCtlGetEmployeePipelineRspDueItemMdl,
  MbsCrmPplCtlGetEmployeePipelineRspBillItemMdl,
  MbsCrmPplCtlAddEmployeePipelineReqMdl,
  MbsCrmPplCtlAddEmployeePipelineReqContacterItemMdl,
  MbsCrmPplCtlAddEmployeePipelineReqSalerTrackingItemMdl,
  MbsCrmPplCtlAddEmployeePipelineReqProductItemMdl,
  MbsCrmPplCtlAddEmployeePipelineReqBillItemMdl,
  MbsCrmPplCtlAddEmployeePipelineReqDueItemMdl,
  MbsCrmPplCtlAddEmployeePipelineRspMdl,
  MbsCrmPplCtlUpdatePipelineStatusReqMdl,
  MbsCrmPplCtlUpdatePipelineStatusRspMdl,
  MbsCrmPplCtlTransferPipelineToProjectReqMdl,
  MbsCrmPplCtlTransferPipelineToProjectReqItemMdl,
  MbsCrmPplCtlTransferPipelineToProjectRspMdl,
  // 客戶
  MbsCrmPplCtlGetEmployeePipelineCompanyReqMdl,
  MbsCrmPplCtlGetEmployeePipelineCompanyRspMdl,
  MbsCrmPplCtlUpdateEmployeePipelineCompanyReqMdl,
  MbsCrmPplCtlUpdateEmployeePipelineCompanyRspMdl,
  // 窗口
  MbsCrmPplCtlAddEmployeePipelineContacterReqMdl,
  MbsCrmPplCtlAddEmployeePipelineContacterRspMdl,
  MbsCrmPplCtlUpdateEmployeePipelineContacterReqMdl,
  MbsCrmPplCtlUpdateEmployeePipelineContacterRspMdl,
  MbsCrmPplCtlRemoveEmployeePipelineContacterReqMdl,
  MbsCrmPplCtlRemoveEmployeePipelineContacterRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineContacterReqMdl,
  MbsCrmPplCtlGetManyEmployeePipelineContacterRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineContacterRspItemMdl,
  // 業務紀錄
  MbsCrmPplCtlGetManyEmployeePipelineSalerReqMdl,
  MbsCrmPplCtlGetManyEmployeePipelineSalerRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineSalerRspItemMdl,
  MbsCrmPplCtlHandleEmployeePipelineSalerReqMdl,
  MbsCrmPplCtlHandleEmployeePipelineSalerRspMdl,
  // 業務開發紀錄
  MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingReqMdl,
  MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspItemMdl,
  MbsCrmPplCtlAddEmployeePipelineSalerTrackingReqMdl,
  MbsCrmPplCtlAddEmployeePipelineSalerTrackingRspMdl,
  // 履約期限
  MbsCrmPplCtlGetEmployeePipelineDueReqMdl,
  MbsCrmPplCtlGetEmployeePipelineDueRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineDueReqMdl,
  MbsCrmPplCtlGetManyEmployeePipelineDueRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineDueRspItemMdl,
  MbsCrmPplCtlAddEmployeePipelineDueReqMdl,
  MbsCrmPplCtlAddEmployeePipelineDueRspMdl,
  MbsCrmPplCtlUpdateEmployeePipelineDueReqMdl,
  MbsCrmPplCtlUpdateEmployeePipelineDueRspMdl,
  MbsCrmPplCtlRemoveEmployeePipelineDueReqMdl,
  MbsCrmPplCtlRemoveEmployeePipelineDueRspMdl,
  // 商機產品
  MbsCrmPplCtlGetEmployeePipelineProductReqMdl,
  MbsCrmPplCtlGetEmployeePipelineProductRspMdl,
  MbsCrmPplCtlAddEmployeePipelineProductReqMdl,
  MbsCrmPplCtlAddEmployeePipelineProductRspMdl,
  MbsCrmPplCtlUpdateEmployeePipelineProductReqMdl,
  MbsCrmPplCtlUpdateEmployeePipelineProductRspMdl,
  MbsCrmPplCtlRemoveEmployeePipelineProductReqMdl,
  MbsCrmPplCtlRemoveEmployeePipelineProductRspMdl,
  // 發票紀錄
  MbsCrmPplCtlGetEmployeePipelineBillReqMdl,
  MbsCrmPplCtlGetEmployeePipelineBillRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineBillReqMdl,
  MbsCrmPplCtlGetManyEmployeePipelineBillRspMdl,
  MbsCrmPplCtlGetManyEmployeePipelineBillRspItemMdl,
  MbsCrmPplCtlAddManyEmployeePipelineBillReqMdl,
  MbsCrmPplCtlAddManyEmployeePipelineBillReqItemMdl,
  MbsCrmPplCtlAddManyEmployeePipelineBillRspMdl,
  MbsCrmPplCtlUpdateEmployeePipelineBillReqMdl,
  MbsCrmPplCtlUpdateEmployeePipelineBillRspMdl,
  MbsCrmPplCtlUpdateManyEmployeePipelineBillReqMdl,
  MbsCrmPplCtlUpdateManyEmployeePipelineBillReqItemMdl,
  MbsCrmPplCtlUpdateManyEmployeePipelineBillRspMdl,
  MbsCrmPplCtlRemoveManyEmployeePipelineBillReqMdl,
  MbsCrmPplCtlRemoveManyEmployeePipelineBillRspMdl,
  MbsCrmPplCtlNotifyBillIssueReqMdl,
  MbsCrmPplCtlNotifyBillIssueRspMdl,
  MbsCrmPplCtlConfirmBillIssueReqMdl,
  MbsCrmPplCtlConfirmBillIssueRspMdl,
} from "@/services/pms-http/crm/pipeline/crmPipelineControllerFormat";

//#region 名單
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得多筆商機 */
export const getManyEmployeePipeline = async (
  requestData: MbsCrmPplHttpGetManyEmployeePipelineReqMdl
): Promise<MbsCrmPplHttpGetManyEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetManyEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.atomPipelineStatus,
    b: requestData.managerCompanyName,
    c: requestData.managerContacterName,
    d: requestData.managerContacterEmail,
    e: requestData.pageIndex,
    f: requestData.pageSize,
    g: requestData.employeePipelineSalerEmployeeID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlGetManyEmployeePipelineRspMdl>(
      "/api/MbsCrmPipeline/GetManyEmployeePipeline",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsCrmPplHttpGetManyEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      employeePipelineList:
        httpResponseData.data.a?.map(
          (item: MbsCrmPplCtlGetManyEmployeePipelineRspItemMdl) =>
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
              employeePipelineSalerEmployeeName: item.j,
            }) satisfies MbsCrmPplHttpGetManyEmployeePipelineRspItemMdl
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
/** 管理者後台-CRM-商機管理-取得名單 */
export const getEmployeePipeline = async (
  requestData: MbsCrmPplHttpGetEmployeePipelineReqMdl
): Promise<MbsCrmPplHttpGetEmployeePipelineRspMdl | null> => {
  const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";
  const httpRequestData: MbsCrmPplCtlGetEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  const cacheKey = `cache.crm.pipeline.${requestData.employeePipelineID}`;
  const readCache = () => {
    const raw = localStorage.getItem(cacheKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as MbsCrmPplHttpGetEmployeePipelineRspMdl;
    } catch {
      return null;
    }
  };
  const writeCache = (payload: MbsCrmPplHttpGetEmployeePipelineRspMdl) => {
    localStorage.setItem(cacheKey, JSON.stringify(payload));
  };
  const buildFallback = (): MbsCrmPplHttpGetEmployeePipelineRspMdl => {
    const fallback =
      mockDataSets.crmPipelines.find((item) => item.id === requestData.employeePipelineID) ??
      mockDataSets.crmPipelines[0];
    return {
      errorCode: MbsErrorCodeEnum.Success,
      atomPipelineStatus: fallback?.status ?? 0,
      employeePipelineSalerEmployeeID: 0,
      employeePipelineSalerEmployeeName: fallback?.salerEmployeeName ?? "",
      employeePipelineSalerRegionID: 0,
      employeePipelineSalerRegionName: "",
      employeePipelineSalerDepartmentID: 0,
      employeePipelineSalerDepartmentName: "",
      company: {
        managerCompanyUnifiedNumber: "",
        managerCompanyID: 0,
        managerCompanyName: fallback?.companyName ?? "",
        atomEmployeeRange: null,
        atomCustomerGrade: null,
        managerCompanyMainKindName: "",
        managerCompanySubKindName: "",
        atomCity: null,
        managerCompanyLocationID: 0,
        managerCompanyLocationAddress: "",
        managerCompanyLocationTelephone: "",
        managerCompanyLocationStatus: 0,
      },
      contacterList: fallback
        ? [
            {
              managerContacterID: 0,
              employeePipelineContacterIsPrimary: true,
              managerContacterName: fallback.contacterName ?? "",
              managerContacterEmail: fallback.contacterEmail ?? "",
              managerContacterPhone: fallback.contacterPhone ?? "",
              managerContacterDepartment: fallback.contacterDepartment ?? "",
              managerContacterJobTitle: fallback.contacterJobTitle ?? "",
              managerContacterTelephone: fallback.contacterTelephone ?? "",
              managerContacterIsConsent: true,
              managerContacterStatus: 0,
              atomRatingKind: 0,
              managerContacterRemark: "",
            },
          ]
        : [],
      pendingEmployeePipelineSaler: null,
      employeePipelineSalerList: [],
      employeePipelineSalerTrackingList: [],
      employeePipelineProductList: [],
      employeePipelineDueList: [],
      employeePipelineBillList: [],
      stageStatus: {
        businessNeedStatus: null,
        businessTimelineStatus: null,
        businessBudgetStatus: null,
        businessPresentationStatus: null,
        businessQuotationStatus: null,
        businessNegotiationStatus: null,
        businessStageRemark: null,
      },
    };
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlGetEmployeePipelineRspMdl>(
      "/api/MbsCrmPipeline/GetEmployeePipeline",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      const cached = readCache();
      if (cached) return cached;
      return buildFallback();
    }

    const companyData = httpResponseData.data.h;
    const responseData: MbsCrmPplHttpGetEmployeePipelineRspMdl = {
      errorCode: httpResponseData.data.aa,
      atomPipelineStatus: httpResponseData.data.a,
      employeePipelineSalerEmployeeID: httpResponseData.data.b,
      employeePipelineSalerEmployeeName: httpResponseData.data.c,
      employeePipelineSalerRegionID: httpResponseData.data.d,
      employeePipelineSalerRegionName: httpResponseData.data.e,
      employeePipelineSalerDepartmentID: httpResponseData.data.f,
      employeePipelineSalerDepartmentName: httpResponseData.data.g,
      stageStatus: {
        businessNeedStatus: httpResponseData.data.p?.a ?? null,
        businessTimelineStatus: httpResponseData.data.p?.b ?? null,
        businessBudgetStatus: httpResponseData.data.p?.c ?? null,
        businessPresentationStatus: httpResponseData.data.p?.d ?? null,
        businessQuotationStatus: httpResponseData.data.p?.e ?? null,
        businessNegotiationStatus: httpResponseData.data.p?.f ?? null,
        businessStageRemark: httpResponseData.data.p?.g ?? null,
      },
      company: {
        managerCompanyUnifiedNumber: companyData?.a ?? null,
        managerCompanyID: companyData?.b ?? null,
        managerCompanyName: companyData?.c ?? null,
        atomEmployeeRange: companyData?.d ?? null,
        atomCustomerGrade: companyData?.e ?? null,
        managerCompanyMainKindName: companyData?.f ?? null,
        managerCompanySubKindName: companyData?.g ?? null,
        atomCity: companyData?.h ?? null,
        managerCompanyLocationID: companyData?.i ?? null,
        managerCompanyLocationAddress: companyData?.j ?? null,
        managerCompanyLocationTelephone: companyData?.k ?? null,
        managerCompanyLocationStatus: companyData?.l ?? null,
      } satisfies MbsCrmPplHttpGetEmployeePipelineRspCompanyItemMdl,
      contacterList: (httpResponseData.data.i ?? [])
        .filter(Boolean)
        .map(
          (item: MbsCrmPplCtlGetEmployeePipelineRspContacterItemMdl) =>
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
            }) satisfies MbsCrmPplHttpGetEmployeePipelineRspContacterItemMdl
        ),
      pendingEmployeePipelineSaler: httpResponseData.data.j
        ? ({
            employeePipelineSalerID: httpResponseData.data.j.a,
            employeePipelineSalerEmployeeName: httpResponseData.data.j.b,
            employeePipelineSalerCreateTime: httpResponseData.data.j.c,
            employeePipelineSalerStatus: httpResponseData.data.j.d,
            employeePipelineSalerCreateEmployeeName: httpResponseData.data.j.e,
            hasRejectPermissions: httpResponseData.data.j.f,
            hasAcceptPermissions: httpResponseData.data.j.g,
            hasReassignPermissions: httpResponseData.data.j.h,
          } satisfies MbsCrmPplHttpGetEmployeePipelineRspPendingSalerItemMdl)
        : null,
      employeePipelineSalerList: (httpResponseData.data.k ?? [])
        .filter(Boolean)
        .map(
          (item: MbsCrmPplCtlGetEmployeePipelineRspSalerItemMdl) =>
            ({
              employeePipelineSalerID: item.a,
              employeePipelineSalerEmployeeName: item.b,
              employeePipelineSalerReplyTime: item.c,
              employeePipelineSalerCreateTime: item.d,
              employeePipelineSalerStatus: item.e,
              employeePipelineSalerCreateEmployeeName: item.f,
              employeePipelineSalerRemark: item.g,
            }) satisfies MbsCrmPplHttpGetEmployeePipelineRspSalerItemMdl
        ),
      employeePipelineSalerTrackingList: (httpResponseData.data.l ?? [])
        .filter(Boolean)
        .map(
          (item: MbsCrmPplCtlGetEmployeePipelineRspSalerTrackingItemMdl) =>
            ({
              employeePipelineSalerTrackingID: item.a,
              employeePipelineSalerTrackingTime: item.b,
              managerContacterName: item.c,
              employeePipelineSalerTrackingRemark: item.d,
              employeePipelineSalerTrackingCreateEmployeeName: item.e,
            }) satisfies MbsCrmPplHttpGetEmployeePipelineRspSalerTrackingItemMdl
        ),
      employeePipelineProductList: (httpResponseData.data.m ?? [])
        .filter(Boolean)
        .map(
          (item: MbsCrmPplCtlGetEmployeePipelineRspProductItemMdl) =>
            ({
              employeePipelineProductID: item.a,
              managerProductName: item.b,
              managerProductMainKindName: item.c,
              managerProductSubKindName: item.d,
              managerProductSpecificationName: item.e,
              employeePipelineProductSellPrice: item.f,
              employeePipelineProductClosingPrice: item.g,
              employeePipelineProductCostPrice: item.h,
              employeePipelineProductGrossProfit: item.i,
              employeePipelineProductCount: item.j,
              employeePipelineProductPurchaseKind: item.k,
              employeePipelineProductContractKind: item.l,
              employeePipelineProductContractText: item.m,
              managerProductMainKindCommissionRate: item.n,
              managerProductID: item.o,
              managerProductSpecificationID: item.p,
            }) satisfies MbsCrmPplHttpGetEmployeePipelineRspProductItemMdl
        ),
      employeePipelineDueList: (httpResponseData.data.n ?? [])
        .filter(Boolean)
        .map(
          (item: MbsCrmPplCtlGetEmployeePipelineRspDueItemMdl) =>
            ({
              employeePipelineDueID: item.a,
              employeePipelineDueTime: item.b,
              employeePipelineDueRemark: item.c,
            }) satisfies MbsCrmPplHttpGetEmployeePipelineRspDueItemMdl
        ),
      employeePipelineBillList: (httpResponseData.data.o ?? [])
        .filter(Boolean)
        .map(
          (item: MbsCrmPplCtlGetEmployeePipelineRspBillItemMdl) =>
            ({
              employeePipelineBillID: item.a,
              employeePipelineBillPeriodNumber: item.b,
              employeePipelineBillBillTime: item.c,
              employeePipelineBillIsPreIssued: item.h ?? false,
              employeePipelineBillExecuteDate: item.i ?? null,
              employeePipelineBillBillNumber: item.d,
              employeePipelineBillNoTaxAmount: item.e,
              employeePipelineBillRemark: item.f,
              employeePipelineBillStatus: item.g,
            }) satisfies MbsCrmPplHttpGetEmployeePipelineRspBillItemMdl
        ),
    };

    writeCache(responseData);
    const cached = readCache();
    if (useMockData && cached) {
      return cached;
    }
    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.warn("API fallback:", axiosError.response?.data ?? error);
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-新增商機 */
export const addEmployeePipeline = async (
  requestData: MbsCrmPplHttpAddEmployeePipelineReqMdl
): Promise<MbsCrmPplHttpAddEmployeePipelineRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlAddEmployeePipelineReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
    b: requestData.managerCompanyLocationID,
    c: requestData.atomPipelineStatus,
    d: requestData.employeePipelineSalerEmployeeID,
    e: requestData.contacterList.map(
      (item: MbsCrmPplHttpAddEmployeePipelineReqContacterItemMdl) =>
        ({
          a: item.managerContacterID,
          b: item.employeePipelineContacterIsPrimary,
        }) satisfies MbsCrmPplCtlAddEmployeePipelineReqContacterItemMdl
    ),
    f: requestData.salerTrackingList.map(
      (item: MbsCrmPplHttpAddEmployeePipelineReqSalerTrackingItemMdl) =>
        ({
          a: item.employeePipelineSalerTrackingTime,
          b: item.managerContacterID,
          c: item.employeePipelineSalerTrackingRemark,
        }) satisfies MbsCrmPplCtlAddEmployeePipelineReqSalerTrackingItemMdl
    ),
    g: requestData.productList.map(
      (item: MbsCrmPplHttpAddEmployeePipelineReqProductItemMdl) =>
        ({
          a: item.managerProductID,
          b: item.managerProductSpecificationID,
          c: item.employeePipelineProductSellPrice,
          d: item.employeePipelineProductClosingPrice,
          e: item.employeePipelineProductCostPrice,
          f: item.employeePipelineProductCount,
          g: item.employeePipelineProductPurchaseKindID,
          h: item.employeePipelineProductContractKindID,
          i: item.employeePipelineProductContractText,
        }) satisfies MbsCrmPplCtlAddEmployeePipelineReqProductItemMdl
    ),
    h: requestData.billList.map(
      (item: MbsCrmPplHttpAddEmployeePipelineReqBillItemMdl) =>
        ({
          a: item.employeePipelineBillPeriodNumber,
          b: item.employeePipelineBillBillTime,
          f: item.employeePipelineBillIsPreIssued,
          g: item.employeePipelineBillExecuteDate,
          c: item.employeePipelineBillNoTaxAmount,
          d: item.employeePipelineBillRemark,
          e: item.employeePipelineBillStatus,
        }) satisfies MbsCrmPplCtlAddEmployeePipelineReqBillItemMdl
    ),
    i: requestData.dueList.map(
      (item: MbsCrmPplHttpAddEmployeePipelineReqDueItemMdl) =>
        ({
          a: item.employeePipelineDueTime,
          b: item.employeePipelineDueRemark,
        }) satisfies MbsCrmPplCtlAddEmployeePipelineReqDueItemMdl
    ),
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlAddEmployeePipelineRspMdl>(
      "/api/MbsCrmPipeline/AddEmployeePipeline",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpAddEmployeePipelineRspMdl = {
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
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-更新商機狀態 */
export const updatePipelineStatus = async (
  requestData: MbsCrmPplHttpUpdatePipelineStatusReqMdl
): Promise<MbsCrmPplHttpUpdatePipelineStatusRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlUpdatePipelineStatusReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.atomPipelineStatus,
    c: requestData.businessNeedStatus,
    d: requestData.businessTimelineStatus,
    e: requestData.businessBudgetStatus,
    f: requestData.businessPresentationStatus,
    g: requestData.businessQuotationStatus,
    h: requestData.businessNegotiationStatus,
    i: requestData.businessStageRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlUpdatePipelineStatusRspMdl>(
      "/api/MbsCrmPipeline/UpdatePipelineStatus",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpUpdatePipelineStatusRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-轉換商機至專案 */
export const transferPipelineToProject = async (
  requestData: MbsCrmPplHttpTransferPipelineToProjectReqMdl
): Promise<MbsCrmPplHttpTransferPipelineToProjectRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlTransferPipelineToProjectReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeeProjectCode,
    c: requestData.employeeProjectName,
    d: requestData.employeeProjectStartTime,
    e: requestData.employeeProjectEndTime,
    f: requestData.employeeProjectContractUrl,
    g: requestData.employeeProjectWorkUrl,
    h: requestData.employeeProjectMemberEmployeeList.map(
      (item: MbsCrmPplHttpTransferPipelineToProjectReqItemMdl) =>
        ({
          a: item.employeeID,
          b: item.employeeProjectMemberRole,
        }) satisfies MbsCrmPplCtlTransferPipelineToProjectReqItemMdl
    ),
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlTransferPipelineToProjectRspMdl>(
      "/api/MbsCrmPipeline/TransferPipelineToProject",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpTransferPipelineToProjectRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeeProjectID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
//#endregion

//#region 客戶
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得客戶 */
export const getEmployeePipelineCompany = async (
  requestData: MbsCrmPplHttpGetEmployeePipelineCompanyReqMdl
): Promise<MbsCrmPplHttpGetEmployeePipelineCompanyRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetEmployeePipelineCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlGetEmployeePipelineCompanyRspMdl>(
      "/api/MbsCrmPipeline/GetEmployeePipelineCompany",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetEmployeePipelineCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerCompanyUnifiedNumber: httpResponseData.data.a,
      managerCompanyName: httpResponseData.data.b,
      atomEmployeeRange: httpResponseData.data.c,
      atomCustomerGrade: httpResponseData.data.d,
      managerCompanyMainKindName: httpResponseData.data.e,
      managerCompanySubKindName: httpResponseData.data.f,
      atomCity: httpResponseData.data.g,
      managerCompanyLocationAddress: httpResponseData.data.h,
      managerCompanyLocationTelephone: httpResponseData.data.i,
      managerCompanyLocationStatus: httpResponseData.data.j,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-更新名單客戶 */
export const updateEmployeePipelineCompany = async (
  requestData: MbsCrmPplHttpUpdateEmployeePipelineCompanyReqMdl
): Promise<MbsCrmPplHttpUpdateEmployeePipelineCompanyRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlUpdateEmployeePipelineCompanyReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.managerCompanyID,
    c: requestData.managerCompanyLocationID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlUpdateEmployeePipelineCompanyRspMdl>(
        "/api/MbsCrmPipeline/UpdateEmployeePipelineCompany",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpUpdateEmployeePipelineCompanyRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
//#endregion

//#region 窗口
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-新增名單窗口 */
export const addEmployeePipelineContacter = async (
  requestData: MbsCrmPplHttpAddEmployeePipelineContacterReqMdl
): Promise<MbsCrmPplHttpAddEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlAddEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.managerContacterID,
    c: requestData.employeePipelineContacterIsPrimary,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlAddEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPipeline/AddEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpAddEmployeePipelineContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-更新名單窗口 */
export const updateEmployeePipelineContacter = async (
  requestData: MbsCrmPplHttpUpdateEmployeePipelineContacterReqMdl
): Promise<MbsCrmPplHttpUpdateEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlUpdateEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineContacterID,
    b: requestData.managerContacterID,
    c: requestData.employeePipelineContacterIsPrimary,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlUpdateEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPipeline/UpdateEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpUpdateEmployeePipelineContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-刪除名單窗口 */
export const removeEmployeePipelineContacter = async (
  requestData: MbsCrmPplHttpRemoveEmployeePipelineContacterReqMdl
): Promise<MbsCrmPplHttpRemoveEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlRemoveEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineContacterID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlRemoveEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPipeline/RemoveEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpRemoveEmployeePipelineContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得多筆名單窗口 */
export const getManyEmployeePipelineContacter = async (
  requestData: MbsCrmPplHttpGetManyEmployeePipelineContacterReqMdl
): Promise<MbsCrmPplHttpGetManyEmployeePipelineContacterRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetManyEmployeePipelineContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlGetManyEmployeePipelineContacterRspMdl>(
        "/api/MbsCrmPipeline/GetManyEmployeePipelineContacter",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetManyEmployeePipelineContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineContacterList:
        httpResponseData.data.a?.map(
          (item: MbsCrmPplCtlGetManyEmployeePipelineContacterRspItemMdl) =>
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
            }) satisfies MbsCrmPplHttpGetManyEmployeePipelineContacterRspItemMdl
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
//#endregion

//#region 業務紀錄
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得多筆商機業務 */
export const getManyEmployeePipelineSaler = async (
  requestData: MbsCrmPplHttpGetManyEmployeePipelineSalerReqMdl
): Promise<MbsCrmPplHttpGetManyEmployeePipelineSalerRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetManyEmployeePipelineSalerReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeePipelineSalerStatus,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlGetManyEmployeePipelineSalerRspMdl>(
        "/api/MbsCrmPipeline/GetManyEmployeePipelineSaler",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetManyEmployeePipelineSalerRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineSalerList:
        httpResponseData.data.a?.map(
          (item: MbsCrmPplCtlGetManyEmployeePipelineSalerRspItemMdl) =>
            ({
              employeePipelineSalerID: item.a,
              employeePipelineSalerEmployeeName: item.b,
              employeePipelineSalerReplyTime: item.c,
              employeePipelineSalerStatus: item.d,
              employeePipelineSalerCreateEmployeeName: item.e,
              employeePipelineSalerRemark: item.f,
              employeePipelineSalerCreateTime: item.g,
            }) satisfies MbsCrmPplHttpGetManyEmployeePipelineSalerRspItemMdl
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
/** 管理者後台-CRM-商機管理-處理商機業務 */
export const handleEmployeePipelineSaler = async (
  requestData: MbsCrmPplHttpHandleEmployeePipelineSalerReqMdl
): Promise<MbsCrmPplHttpHandleEmployeePipelineSalerRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlHandleEmployeePipelineSalerReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeePipelineSalerStatus,
    c: requestData.employeePipelineSalerRemark,
    d: requestData.employeePipelineSalerEmployeeID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlHandleEmployeePipelineSalerRspMdl>(
        "/api/MbsCrmPipeline/HandleEmployeePipelineSaler",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpHandleEmployeePipelineSalerRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
//#endregion

//#region 業務開發紀錄
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得多筆商機業務開發紀錄 */
export const getManyEmployeePipelineSalerTracking = async (
  requestData: MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingReqMdl
): Promise<MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspMdl>(
        "/api/MbsCrmPipeline/GetManyEmployeePipelineSalerTracking",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineSalerTrackingList:
        httpResponseData.data.a?.map(
          (item: MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspItemMdl) =>
            ({
              employeePipelineSalerTrackingID: item.a,
              employeePipelineSalerTrackingTime: item.b,
              managerContacterName: item.c,
              employeePipelineSalerTrackingRemark: item.d,
              employeePipelineSalerTrackingCreateEmployeeName: item.e,
            }) satisfies MbsCrmPplHttpGetManyEmployeePipelineSalerTrackingRspItemMdl
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
/** 管理者後台-CRM-商機管理-新增商機業務開發紀錄 */
export const addEmployeePipelineSalerTracking = async (
  requestData: MbsCrmPplHttpAddEmployeePipelineSalerTrackingReqMdl
): Promise<MbsCrmPplHttpAddEmployeePipelineSalerTrackingRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlAddEmployeePipelineSalerTrackingReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeePipelineSalerTrackingTime,
    c: requestData.managerContacterID,
    d: requestData.employeePipelineSalerTrackingRemark,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlAddEmployeePipelineSalerTrackingRspMdl>(
        "/api/MbsCrmPipeline/AddEmployeePipelineSalerTracking",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpAddEmployeePipelineSalerTrackingRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineSalerTrackingID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
//#endregion

//#region 履約期限通知
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得商機履約期限 */
export const getEmployeePipelineDue = async (
  requestData: MbsCrmPplHttpGetEmployeePipelineDueReqMdl
): Promise<MbsCrmPplHttpGetEmployeePipelineDueRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetEmployeePipelineDueReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineDueID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlGetEmployeePipelineDueRspMdl>(
      "/api/MbsCrmPipeline/GetEmployeePipelineDue",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetEmployeePipelineDueRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineDueID: httpResponseData.data.a,
      employeePipelineID: httpResponseData.data.b,
      employeePipelineDueTime: httpResponseData.data.c,
      employeePipelineDueRemark: httpResponseData.data.d,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得多筆商機履約期限 */
export const getManyEmployeePipelineDue = async (
  requestData: MbsCrmPplHttpGetManyEmployeePipelineDueReqMdl
): Promise<MbsCrmPplHttpGetManyEmployeePipelineDueRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetManyEmployeePipelineDueReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlGetManyEmployeePipelineDueRspMdl>(
      "/api/MbsCrmPipeline/GetManyEmployeePipelineDue",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetManyEmployeePipelineDueRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineDueList:
        httpResponseData.data.a?.map(
          (item: MbsCrmPplCtlGetManyEmployeePipelineDueRspItemMdl) =>
            ({
              employeePipelineDueID: item.a,
              employeePipelineID: item.b,
              employeePipelineDueTime: item.c,
              employeePipelineDueRemark: item.d,
            }) satisfies MbsCrmPplHttpGetManyEmployeePipelineDueRspItemMdl
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
/** 管理者後台-CRM-商機管理-新增商機履約期限 */
export const addEmployeePipelineDue = async (
  requestData: MbsCrmPplHttpAddEmployeePipelineDueReqMdl
): Promise<MbsCrmPplHttpAddEmployeePipelineDueRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlAddEmployeePipelineDueReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeePipelineDueTime,
    c: requestData.employeePipelineDueRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlAddEmployeePipelineDueRspMdl>(
      "/api/MbsCrmPipeline/AddEmployeePipelineDue",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpAddEmployeePipelineDueRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineDueID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-更新商機履約期限 */
export const updateEmployeePipelineDue = async (
  requestData: MbsCrmPplHttpUpdateEmployeePipelineDueReqMdl
): Promise<MbsCrmPplHttpUpdateEmployeePipelineDueRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlUpdateEmployeePipelineDueReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineDueID,
    b: requestData.employeePipelineDueTime,
    c: requestData.employeePipelineDueRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlUpdateEmployeePipelineDueRspMdl>(
      "/api/MbsCrmPipeline/UpdateEmployeePipelineDue",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpUpdateEmployeePipelineDueRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-刪除商機履約期限 */
export const removeEmployeePipelineDue = async (
  requestData: MbsCrmPplHttpRemoveEmployeePipelineDueReqMdl
): Promise<MbsCrmPplHttpRemoveEmployeePipelineDueRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlRemoveEmployeePipelineDueReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineDueID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlRemoveEmployeePipelineDueRspMdl>(
      "/api/MbsCrmPipeline/RemoveEmployeePipelineDue",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpRemoveEmployeePipelineDueRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
//#endregion

//#region 商機產品
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得商機產品 */
export const getEmployeePipelineProduct = async (
  requestData: MbsCrmPplHttpGetEmployeePipelineProductReqMdl
): Promise<MbsCrmPplHttpGetEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineProductID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlGetEmployeePipelineProductRspMdl>(
      "/api/MbsCrmPipeline/GetEmployeePipelineProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetEmployeePipelineProductRspMdl = {
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
      employeePipelineProductSellPrice: httpResponseData.data.j,
      employeePipelineProductClosingPrice: httpResponseData.data.k,
      employeePipelineProductCostPrice: httpResponseData.data.l,
      employeePipelineProductCount: httpResponseData.data.m,
      employeePipelineProductPurchaseKindID: httpResponseData.data.n,
      employeePipelineProductContractKindID: httpResponseData.data.o,
      employeePipelineProductContractText: httpResponseData.data.p,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-新增商機產品 */
export const addEmployeePipelineProduct = async (
  requestData: MbsCrmPplHttpAddEmployeePipelineProductReqMdl
): Promise<MbsCrmPplHttpAddEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlAddEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.managerProductID,
    c: requestData.managerProductSpecificationID,
    d: requestData.employeePipelineProductSellPrice,
    e: requestData.employeePipelineProductClosingPrice,
    f: requestData.employeePipelineProductCostPrice,
    g: requestData.employeePipelineProductCount,
    h: requestData.employeePipelineProductPurchaseKindID,
    i: requestData.employeePipelineProductContractKindID,
    j: requestData.employeePipelineProductContractText,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlAddEmployeePipelineProductRspMdl>(
      "/api/MbsCrmPipeline/AddEmployeePipelineProduct",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpAddEmployeePipelineProductRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineProductID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-更新商機產品 */
export const updateEmployeePipelineProduct = async (
  requestData: MbsCrmPplHttpUpdateEmployeePipelineProductReqMdl
): Promise<MbsCrmPplHttpUpdateEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlUpdateEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineProductID,
    b: requestData.managerProductID,
    c: requestData.managerProductSpecificationID,
    d: requestData.employeePipelineProductSellPrice,
    e: requestData.employeePipelineProductClosingPrice,
    f: requestData.employeePipelineProductCostPrice,
    g: requestData.employeePipelineProductCount,
    h: requestData.employeePipelineProductPurchaseKindID,
    i: requestData.employeePipelineProductContractKindID,
    j: requestData.employeePipelineProductContractText,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlUpdateEmployeePipelineProductRspMdl>(
        "/api/MbsCrmPipeline/UpdateEmployeePipelineProduct",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpUpdateEmployeePipelineProductRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-刪除商機產品 */
export const removeEmployeePipelineProduct = async (
  requestData: MbsCrmPplHttpRemoveEmployeePipelineProductReqMdl
): Promise<MbsCrmPplHttpRemoveEmployeePipelineProductRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlRemoveEmployeePipelineProductReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineProductID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlRemoveEmployeePipelineProductRspMdl>(
        "/api/MbsCrmPipeline/RemoveEmployeePipelineProduct",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpRemoveEmployeePipelineProductRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
//#endregion

//#region 發票紀錄
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得商機發票紀錄 */
export const getEmployeePipelineBill = async (
  requestData: MbsCrmPplHttpGetEmployeePipelineBillReqMdl
): Promise<MbsCrmPplHttpGetEmployeePipelineBillRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetEmployeePipelineBillReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineBillID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlGetEmployeePipelineBillRspMdl>(
      "/api/MbsCrmPipeline/GetEmployeePipelineBill",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetEmployeePipelineBillRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineBillID: httpResponseData.data.a,
      employeePipelineBillPeriodNumber: httpResponseData.data.b,
      employeePipelineBillBillTime: httpResponseData.data.c,
      employeePipelineBillBillNumber: httpResponseData.data.d,
      employeePipelineBillNoTaxAmount: httpResponseData.data.e,
      employeePipelineBillRemark: httpResponseData.data.f,
      employeePipelineBillStatus: httpResponseData.data.g,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-取得多筆商機發票紀錄 */
export const getManyEmployeePipelineBill = async (
  requestData: MbsCrmPplHttpGetManyEmployeePipelineBillReqMdl
): Promise<MbsCrmPplHttpGetManyEmployeePipelineBillRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlGetManyEmployeePipelineBillReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlGetManyEmployeePipelineBillRspMdl>(
        "/api/MbsCrmPipeline/GetManyEmployeePipelineBill",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpGetManyEmployeePipelineBillRspMdl = {
      errorCode: httpResponseData.data.aa,
      employeePipelineBillList:
        httpResponseData.data.a?.map(
          (item: MbsCrmPplCtlGetManyEmployeePipelineBillRspItemMdl) =>
            ({
              employeePipelineBillID: item.a,
              employeePipelineBillPeriodNumber: item.b,
              employeePipelineBillBillTime: item.c,
              employeePipelineBillNoTaxAmount: item.d,
              employeePipelineBillRemark: item.e,
            }) satisfies MbsCrmPplHttpGetManyEmployeePipelineBillRspItemMdl
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
/** 管理者後台-CRM-商機管理-新增多筆商機發票紀錄 */
export const addManyEmployeePipelineBill = async (
  requestData: MbsCrmPplHttpAddManyEmployeePipelineBillReqMdl
): Promise<MbsCrmPplHttpAddManyEmployeePipelineBillRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlAddManyEmployeePipelineBillReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeePipelineBillList.map(
      (item: MbsCrmPplHttpAddManyEmployeePipelineBillReqItemMdl) =>
        ({
          a: item.employeePipelineBillPeriodNumber,
          b: item.employeePipelineBillBillTime,
          c: item.employeePipelineBillNoTaxAmount,
          d: item.employeePipelineBillRemark,
        }) satisfies MbsCrmPplCtlAddManyEmployeePipelineBillReqItemMdl
    ),
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlAddManyEmployeePipelineBillRspMdl>(
        "/api/MbsCrmPipeline/AddManyEmployeePipelineBill",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpAddManyEmployeePipelineBillRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-更新商機發票紀錄 */
export const updateEmployeePipelineBill = async (
  requestData: MbsCrmPplHttpUpdateEmployeePipelineBillReqMdl
): Promise<MbsCrmPplHttpUpdateEmployeePipelineBillRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlUpdateEmployeePipelineBillReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineBillID,
    b: requestData.employeePipelineBillNumber,
    c: requestData.employeePipelineBillStatus,
    d: requestData.employeePipelineBillRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlUpdateEmployeePipelineBillRspMdl>(
      "/api/MbsCrmPipeline/UpdateEmployeePipelineBill",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpUpdateEmployeePipelineBillRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-更新多筆商機發票紀錄 */
export const updateManyEmployeePipelineBill = async (
  requestData: MbsCrmPplHttpUpdateManyEmployeePipelineBillReqMdl
): Promise<MbsCrmPplHttpUpdateManyEmployeePipelineBillRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlUpdateManyEmployeePipelineBillReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
    b: requestData.employeePipelineBillList.map(
      (item: MbsCrmPplHttpUpdateManyEmployeePipelineBillReqItemMdl) =>
        ({
          a: item.employeePipelineBillPeriodNumber,
          b: item.employeePipelineBillBillTime,
          e: item.employeePipelineBillIsPreIssued,
          f: item.employeePipelineBillExecuteDate,
          c: item.employeePipelineBillNoTaxAmount,
          d: item.employeePipelineBillRemark,
        }) satisfies MbsCrmPplCtlUpdateManyEmployeePipelineBillReqItemMdl
    ),
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlUpdateManyEmployeePipelineBillRspMdl>(
        "/api/MbsCrmPipeline/UpdateManyEmployeePipelineBill",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpUpdateManyEmployeePipelineBillRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-刪除多筆商機發票紀錄 */
export const removeManyEmployeePipelineBill = async (
  requestData: MbsCrmPplHttpRemoveManyEmployeePipelineBillReqMdl
): Promise<MbsCrmPplHttpRemoveManyEmployeePipelineBillRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlRemoveManyEmployeePipelineBillReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineID,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsCrmPplCtlRemoveManyEmployeePipelineBillRspMdl>(
        "/api/MbsCrmPipeline/RemoveManyEmployeePipelineBill",
        httpRequestData
      );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpRemoveManyEmployeePipelineBillRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-通知開立發票 */
export const notifyBillIssue = async (
  requestData: MbsCrmPplHttpNotifyBillIssueReqMdl
): Promise<MbsCrmPplHttpNotifyBillIssueRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlNotifyBillIssueReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineBillID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlNotifyBillIssueRspMdl>(
      "/api/MbsCrmPipeline/NotifyBillIssue",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpNotifyBillIssueRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
/** 管理者後台-CRM-商機管理-確認開立發票 */
export const confirmBillIssue = async (
  requestData: MbsCrmPplHttpConfirmBillIssueReqMdl
): Promise<MbsCrmPplHttpConfirmBillIssueRspMdl | null> => {
  const httpRequestData: MbsCrmPplCtlConfirmBillIssueReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.employeePipelineBillID,
    b: requestData.employeePipelineBillNumber,
    c: requestData.employeePipelineBillRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsCrmPplCtlConfirmBillIssueRspMdl>(
      "/api/MbsCrmPipeline/ConfirmBillIssue",
      httpRequestData
    );

    if (!httpResponseData?.data) {
      return null;
    }

    const responseData: MbsCrmPplHttpConfirmBillIssueRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//----------------------------------------------------------------------
//#endregion
