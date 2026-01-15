import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsCrmPhsHttpGetManyCustomerReqMdl,
  MbsCrmPhsHttpGetManyCustomerRspMdl,
  MbsCrmPhsHttpGetCustomerReqMdl,
  MbsCrmPhsHttpGetCustomerRspMdl,
  MbsCrmPhsHttpAddCustomerReqMdl,
  MbsCrmPhsHttpAddCustomerRspMdl,
  MbsCrmPhsHttpUpdateCustomerReqMdl,
  MbsCrmPhsHttpUpdateCustomerRspMdl,
  MbsCrmPhsHttpDeleteCustomerReqMdl,
  MbsCrmPhsHttpDeleteCustomerRspMdl,
  MbsCrmPhsHttpUpdateCustomerOrderReqMdl,
  MbsCrmPhsHttpUpdateCustomerOrderRspMdl,
} from "./phoneSalesHttpFormat";

export const getManyCustomer = async (
  requestData: MbsCrmPhsHttpGetManyCustomerReqMdl
): Promise<MbsCrmPhsHttpGetManyCustomerRspMdl | null> => {
  const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.atomPhoneSalesCustomerStatus,
    b: requestData.atomPhoneSalesCustomerValue,
    c: requestData.companyName,
    d: requestData.industry,
    e: requestData.contacterName,
    f: requestData.contacterEmail,
    g: requestData.isExistingCustomer,
    h: requestData.pageIndex,
    i: requestData.pageSize,
  };

  const cacheKey = "cache.phoneSales.customers";
  const readCache = () => {
    const raw = localStorage.getItem(cacheKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as MbsCrmPhsHttpGetManyCustomerRspMdl;
    } catch {
      return null;
    }
  };
  const writeCache = (data: MbsCrmPhsHttpGetManyCustomerRspMdl) => {
    localStorage.setItem(cacheKey, JSON.stringify(data));
  };
  const buildFallback = (): MbsCrmPhsHttpGetManyCustomerRspMdl => {
    const filterText = (value?: string | null) => (value ?? "").trim();
    const companyFilter = filterText(requestData.companyName).toLowerCase();
    const industryFilter = filterText(requestData.industry).toLowerCase();
    const contacterFilter = filterText(requestData.contacterName).toLowerCase();
    const filtered = mockDataSets.phoneSalesCustomers.filter((item) => {
      const matchesStatus =
        requestData.atomPhoneSalesCustomerStatus == null ||
        item.atomPhoneSalesCustomerStatus === requestData.atomPhoneSalesCustomerStatus;
      const matchesValue =
        requestData.atomPhoneSalesCustomerValue == null ||
        item.atomPhoneSalesCustomerValue === requestData.atomPhoneSalesCustomerValue;
      const matchesCompany =
        !companyFilter || item.companyName.toLowerCase().includes(companyFilter);
      const matchesIndustry =
        !industryFilter || item.industry.toLowerCase().includes(industryFilter);
      const matchesContacter =
        !contacterFilter ||
        item.contacterList.some((contacter) =>
          contacter.contacterName.toLowerCase().includes(contacterFilter)
        );
      return matchesStatus && matchesValue && matchesCompany && matchesIndustry && matchesContacter;
    });
    return {
      errorCode: MbsErrorCodeEnum.Success,
      customerList: filtered,
      totalCount: filtered.length,
    };
  };

  try {
    const response = await apiJsonClient.post(
      "/api/MbsCrmPhoneSales/GetManyCustomer",
      httpRequestData
    );
    if (response.data && typeof response.data.a !== "undefined") {
      const normalized: MbsCrmPhsHttpGetManyCustomerRspMdl = {
        errorCode: response.data.a,
        customerList: Array.isArray(response.data.b)
          ? response.data.b
              .filter(Boolean)
              .map((item: any) => ({
                id: item.a,
                companyName: item.b,
                industry: item.c,
                atomPhoneSalesCustomerStatus: item.d,
                atomPhoneSalesCustomerValue: item.e,
                atomPhoneSalesDataSource: item.f,
                isExistingCustomer: item.g,
                customOrder: item.h,
                contacterList: Array.isArray(item.i)
                  ? item.i
                      .filter(Boolean)
                      .map((contact: any) => ({
                        contacterName: contact.a,
                        contacterJobTitle: contact.b,
                        contacterPhone: contact.c,
                        contacterEmail: contact.d,
                      }))
                  : [],
              }))
          : [],
        totalCount: response.data.c ?? 0,
      };
      writeCache(normalized);
      const cached = readCache();
      if (useMockData && cached) {
        return cached;
      }
      return normalized;
    }
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  } catch (error) {
    const axiosError = error as AxiosError;
    console.warn("getManyCustomer fallback:", axiosError.response?.data || error);
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  }
};

export const getCustomer = async (
  requestData: MbsCrmPhsHttpGetCustomerReqMdl
): Promise<MbsCrmPhsHttpGetCustomerRspMdl | null> => {
  const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.id,
  };

  const cacheKey = `cache.phoneSales.customer.${requestData.id}`;
  const readCache = () => {
    const raw = localStorage.getItem(cacheKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as MbsCrmPhsHttpGetCustomerRspMdl;
    } catch {
      return null;
    }
  };
  const writeCache = (data: MbsCrmPhsHttpGetCustomerRspMdl) => {
    localStorage.setItem(cacheKey, JSON.stringify(data));
  };
  const buildFallback = (): MbsCrmPhsHttpGetCustomerRspMdl => {
    const fallback =
      mockDataSets.phoneSalesCustomers.find((item) => item.id === requestData.id) ??
      mockDataSets.phoneSalesCustomers[0];
    return {
      errorCode: MbsErrorCodeEnum.Success,
      id: fallback.id,
      companyName: fallback.companyName,
      unifiedNumber: fallback.unifiedNumber,
      industry: fallback.industry,
      companyAddress: fallback.companyAddress,
      companyPhone: fallback.companyPhone,
      companyWebsite: fallback.companyWebsite,
      atomPhoneSalesCustomerStatus: fallback.atomPhoneSalesCustomerStatus,
      atomPhoneSalesCustomerValue: fallback.atomPhoneSalesCustomerValue,
      atomPhoneSalesDataSource: fallback.atomPhoneSalesDataSource,
      isExistingCustomer: fallback.isExistingCustomer,
      remark: fallback.remark,
      contacterList: fallback.contacterList.map((item) => ({
        id: item.id,
        contacterName: item.contacterName,
        contacterJobTitle: item.contacterJobTitle,
        contacterPhone: item.contacterPhone,
        contacterEmail: item.contacterEmail,
        contacterLineID: item.contacterLineID,
        isPrimary: item.isPrimary,
      })),
    };
  };

  try {
    const response = await apiJsonClient.post(
      "/api/MbsCrmPhoneSales/GetCustomer",
      httpRequestData
    );
    if (response.data && typeof response.data.a !== "undefined") {
      const normalized: MbsCrmPhsHttpGetCustomerRspMdl = {
        errorCode: response.data.a,
        id: response.data.b,
        companyName: response.data.c,
        unifiedNumber: response.data.d,
        industry: response.data.e,
        companyAddress: response.data.f,
        companyPhone: response.data.g,
        companyWebsite: response.data.h,
        atomPhoneSalesCustomerStatus: response.data.i,
        atomPhoneSalesCustomerValue: response.data.j,
        atomPhoneSalesDataSource: response.data.k,
        isExistingCustomer: response.data.l,
        remark: response.data.m,
        contacterList: Array.isArray(response.data.n)
          ? response.data.n
              .filter(Boolean)
              .map((item: any) => ({
                id: item.a,
                contacterName: item.b,
                contacterJobTitle: item.c,
                contacterPhone: item.d,
                contacterEmail: item.e,
                contacterLineID: item.f,
                isPrimary: item.g,
              }))
          : [],
      };
      writeCache(normalized);
      const cached = readCache();
      if (useMockData && cached) {
        return cached;
      }
      return normalized;
    }
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  } catch (error) {
    const axiosError = error as AxiosError;
    console.warn("getCustomer fallback:", axiosError.response?.data || error);
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  }
};

export const addCustomer = async (
  requestData: MbsCrmPhsHttpAddCustomerReqMdl
): Promise<MbsCrmPhsHttpAddCustomerRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.companyName,
    b: requestData.unifiedNumber,
    c: requestData.industry,
    d: requestData.companyAddress,
    e: requestData.companyPhone,
    f: requestData.companyWebsite,
    g: requestData.atomPhoneSalesCustomerValue,
    h: requestData.atomPhoneSalesDataSource,
    i: requestData.isExistingCustomer,
    j: requestData.remark,
    k: requestData.contacterList.map((item) => ({
      a: item.contacterName,
      b: item.contacterJobTitle,
      c: item.contacterPhone,
      d: item.contacterEmail,
      e: item.contacterLineID,
      f: item.isPrimary,
    })),
  };

  try {
    const response = await apiJsonClient.post(
      "/api/MbsCrmPhoneSales/AddCustomer",
      httpRequestData
    );
    if (response.data && response.data.a) {
      return {
        errorCode: response.data.a,
        customerID: response.data.b,
      };
    }
    return null;
  } catch (error) {
    const axiosError = error as AxiosError;
    console.error("addCustomer error:", axiosError.response?.data || error);
    return null;
  }
};

export const updateCustomer = async (
  requestData: MbsCrmPhsHttpUpdateCustomerReqMdl
): Promise<MbsCrmPhsHttpUpdateCustomerRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.id,
    b: requestData.companyName,
    c: requestData.unifiedNumber,
    d: requestData.industry,
    e: requestData.companyAddress,
    f: requestData.companyPhone,
    g: requestData.companyWebsite,
    h: requestData.atomPhoneSalesCustomerStatus,
    i: requestData.atomPhoneSalesCustomerValue,
    j: requestData.isExistingCustomer,
    k: requestData.remark,
    l: requestData.contacterList.map((item) => ({
      a: item.id,
      b: item.contacterName,
      c: item.contacterJobTitle,
      d: item.contacterPhone,
      e: item.contacterEmail,
      f: item.contacterLineID,
      g: item.isPrimary,
    })),
  };

  try {
    const response = await apiJsonClient.post(
      "/api/MbsCrmPhoneSales/UpdateCustomer",
      httpRequestData
    );
    if (response.data && response.data.a) {
      return {
        errorCode: response.data.a,
      };
    }
    return null;
  } catch (error) {
    const axiosError = error as AxiosError;
    console.error("updateCustomer error:", axiosError.response?.data || error);
    return null;
  }
};

export const deleteCustomer = async (
  requestData: MbsCrmPhsHttpDeleteCustomerReqMdl
): Promise<MbsCrmPhsHttpDeleteCustomerRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.id,
  };

  try {
    const response = await apiJsonClient.post(
      "/api/MbsCrmPhoneSales/DeleteCustomer",
      httpRequestData
    );
    if (response.data && response.data.a) {
      return {
        errorCode: response.data.a,
      };
    }
    return null;
  } catch (error) {
    const axiosError = error as AxiosError;
    console.error("deleteCustomer error:", axiosError.response?.data || error);
    return null;
  }
};

export const updateCustomerOrder = async (
  requestData: MbsCrmPhsHttpUpdateCustomerOrderReqMdl
): Promise<MbsCrmPhsHttpUpdateCustomerOrderRspMdl | null> => {
  const httpRequestData = {
    aa: requestData.employeeLoginToken,
    a: requestData.customerIDList,
  };

  try {
    const response = await apiJsonClient.post(
      "/api/MbsCrmPhoneSales/UpdateCustomerOrder",
      httpRequestData
    );
    if (response.data && response.data.a) {
      return {
        errorCode: response.data.a,
      };
    }
    return null;
  } catch (error) {
    const axiosError = error as AxiosError;
    console.error("updateCustomerOrder error:", axiosError.response?.data || error);
    return null;
  }
};
