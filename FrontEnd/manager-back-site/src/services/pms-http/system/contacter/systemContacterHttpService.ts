import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import type {
  MbsSysCttCtlGetManyContacterReqMdl,
  MbsSysCttCtlGetManyContacterRspMdl,
  MbsSysCttCtlGetManyContacterRspItemMdl,
  MbsSysCttCtlGetContacterReqMdl,
  MbsSysCttCtlGetContacterRspMdl,
  MbsSysCttCtlAddContacterReqMdl,
  MbsSysCttCtlAddContacterRspMdl,
  MbsSysCttCtlUpdateContacterReqMdl,
  MbsSysCttCtlUpdateContacterRspMdl,
  MbsSysCttCtlGetManyContacterRatingReasonReqMdl,
  MbsSysCttCtlGetManyContacterRatingReasonRspMdl,
  MbsSysCttCtlGetManyContacterRatingReasonRspItemMdl,
  MbsSysCttCtlGetContacterRatingReasonReqMdl,
  MbsSysCttCtlGetContacterRatingReasonRspMdl,
  MbsSysCttCtlAddContacterRatingReasonReqMdl,
  MbsSysCttCtlAddContacterRatingReasonRspMdl,
  MbsSysCttCtlUpdateContacterRatingReasonReqMdl,
  MbsSysCttCtlUpdateContacterRatingReasonRspMdl,
  MbsSysCttCtlGetManyContacterRatingReqMdl,
  MbsSysCttCtlGetManyContacterRatingRspMdl,
  MbsSysCttCtlGetManyContacterRatingRspItemMdl,
  MbsSysCttCtlGetContacterRatingReqMdl,
  MbsSysCttCtlGetContacterRatingRspMdl,
  MbsSysCttCtlAddContacterRatingReqMdl,
  MbsSysCttCtlAddContacterRatingRspMdl,
  MbsSysCttCtlUpdateContacterRatingReqMdl,
  MbsSysCttCtlUpdateContacterRatingRspMdl,
  MbsSysCttCtlAddContacterReqRatingItemMdl,
  MbsSysCttCtlRemoveContacterRatingReqMdl,
  MbsSysCttCtlRemoveContacterRatingRspMdl,
} from "./systemContacterControllerFormat";
import type {
  MbsSysCttHttpGetManyContacterReqMdl,
  MbsSysCttHttpGetManyContacterRspMdl,
  MbsSysCttHttpGetManyContacterRspItemMdl,
  MbsSysCttHttpGetContacterReqMdl,
  MbsSysCttHttpGetContacterRspMdl,
  MbsSysCttHttpAddContacterReqMdl,
  MbsSysCttHttpAddContacterRspMdl,
  MbsSysCttHttpUpdateContacterReqMdl,
  MbsSysCttHttpUpdateContacterRspMdl,
  MbsSysCttHttpGetManyContacterRatingReasonReqMdl,
  MbsSysCttHttpGetManyContacterRatingReasonRspMdl,
  MbsSysCttHttpGetManyContacterRatingReasonRspItemMdl,
  MbsSysCttHttpGetContacterRatingReasonReqMdl,
  MbsSysCttHttpGetContacterRatingReasonRspMdl,
  MbsSysCttHttpAddContacterRatingReasonReqMdl,
  MbsSysCttHttpAddContacterRatingReasonRspMdl,
  MbsSysCttHttpUpdateContacterRatingReasonReqMdl,
  MbsSysCttHttpUpdateContacterRatingReasonRspMdl,
  MbsSysCttHttpGetManyContacterRatingReqMdl,
  MbsSysCttHttpGetManyContacterRatingRspMdl,
  MbsSysCttHttpGetManyContacterRatingRspItemMdl,
  MbsSysCttHttpGetContacterRatingReqMdl,
  MbsSysCttHttpGetContacterRatingRspMdl,
  MbsSysCttHttpAddContacterRatingReqMdl,
  MbsSysCttHttpAddContacterRatingRspMdl,
  MbsSysCttHttpUpdateContacterRatingReqMdl,
  MbsSysCttHttpUpdateContacterRatingRspMdl,
  MbsSysCttHttpAddContacterReqRatingItemMdl,
  MbsSysCttHttpRemoveContacterRatingReqMdl,
  MbsSysCttHttpRemoveContacterRatingRspMdl,
} from "./systemContacterHttpFormat";

//#region 管理者後台-系統-名單窗口
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-取得多筆 */
export const getManyContacter = async (
  requestData: MbsSysCttHttpGetManyContacterReqMdl
): Promise<MbsSysCttHttpGetManyContacterRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlGetManyContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
    b: requestData.atomManagerContacterRatingKind,
    c: requestData.managerContacterName,
    d: requestData.managerContacterEmail,
    e: requestData.pageIndex,
    f: requestData.pageSize,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlGetManyContacterRspMdl>(
      "/api/MbsSystemContacter/GetManyContacter",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysCttHttpGetManyContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerContacterList:
        httpResponseData.data.a?.map(
          (item: MbsSysCttCtlGetManyContacterRspItemMdl) =>
            ({
              managerContacterID: item.a,
              managerCompanyName: item.b,
              managerContacterName: item.c,
              managerContacterEmail: item.d,
              managerContacterPhone: item.e,
              managerContacterDepartment: item.f,
              managerContacterJobTitle: item.g,
              atomManagerContacterRatingKind: item.h,
            }) satisfies MbsSysCttHttpGetManyContacterRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-取得單筆 */
export const getContacter = async (
  requestData: MbsSysCttHttpGetContacterReqMdl
): Promise<MbsSysCttHttpGetContacterRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlGetContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlGetContacterRspMdl>(
      "/api/MbsSystemContacter/GetContacter",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const data = httpResponseData.data;

    const responseData: MbsSysCttHttpGetContacterRspMdl = {
      errorCode: data.aa,
      managerContacterID: data.a,
      managerCompanyID: data.b,
      managerContacterName: data.c,
      managerContacterEmail: data.d,
      managerContacterPhone: data.e,
      managerContacterDepartment: data.f,
      managerContacterTitle: data.g,
      managerContacterTelephone: data.h,
      atomManagerContacterStatus: data.i,
      managerContacterIsAgreeSurvey: data.j,
      atomManagerContacterRatingKind: data.k,
      managerContacterRemark: data.m,
      managerCompanyName: data.n,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-新增 */
export const addContacter = async (
  requestData: MbsSysCttHttpAddContacterReqMdl
): Promise<MbsSysCttHttpAddContacterRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlAddContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerCompanyID,
    b: requestData.managerContacterName,
    c: requestData.managerContacterEmail,
    d: requestData.managerContacterPhone,
    e: requestData.managerContacterDepartment,
    f: requestData.managerContacterTitle,
    g: requestData.managerContacterTelephone,
    h: requestData.atomManagerContacterStatus,
    i: requestData.managerContacterIsAgreeSurvey,
    j: requestData.atomManagerContacterRatingKind,
    k: requestData.managerContacterRatingList.map(
      (item: MbsSysCttHttpAddContacterReqRatingItemMdl) =>
        ({
          a: item.managerContacterRatingReasonID,
          b: item.managerContacterRatingRemark,
        }) satisfies MbsSysCttCtlAddContacterReqRatingItemMdl
    ),
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlAddContacterRspMdl>(
      "/api/MbsSystemContacter/AddContacter",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysCttHttpAddContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerContacterID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-更新 */
export const updateContacter = async (
  requestData: MbsSysCttHttpUpdateContacterReqMdl
): Promise<MbsSysCttHttpUpdateContacterRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlUpdateContacterReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterID,
    b: requestData.managerContacterName,
    c: requestData.managerContacterEmail,
    d: requestData.managerContacterPhone,
    e: requestData.managerContacterDepartment,
    f: requestData.managerContacterTitle,
    g: requestData.managerContacterTel,
    h: requestData.atomManagerContacterStatus,
    i: requestData.managerContacterIsAgreeSurvey,
    j: requestData.atomManagerContacterRatingKind,
    k: requestData.managerContacterRemark,
    l: requestData.managerCompanyID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlUpdateContacterRspMdl>(
      "/api/MbsSystemContacter/UpdateContacter",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysCttHttpUpdateContacterRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerContacterID: httpResponseData.data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//--------------------------------------------------------------------------------
//#endregion

//#region 管理者後台-系統-名單窗口-窗口開發評等原因
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-取得多筆窗口開發評等原因 */
export const getManyContacterRatingReason = async (
  requestData: MbsSysCttHttpGetManyContacterRatingReasonReqMdl
): Promise<MbsSysCttHttpGetManyContacterRatingReasonRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlGetManyContacterRatingReasonReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingReasonName,
    b: requestData.managerContacterRatingReasonStatus,
    c: requestData.pageIndex,
    d: requestData.pageSize,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsSysCttCtlGetManyContacterRatingReasonRspMdl>(
        "/api/MbsSystemContacter/GetManyContacterRatingReason",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysCttHttpGetManyContacterRatingReasonRspMdl = {
      errorCode: httpResponseData.data.aa,
      totalCount: httpResponseData.data.b,
      managerContacterRatingReasonList:
        httpResponseData.data.a?.map(
          (item: MbsSysCttCtlGetManyContacterRatingReasonRspItemMdl) =>
            ({
              managerContacterRatingReasonID: item.a,
              managerContacterRatingReasonName: item.b,
              managerContacterRatingReasonStatus: item.c,
            }) satisfies MbsSysCttHttpGetManyContacterRatingReasonRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-取得單筆窗口開發評等原因 */
export const getContacterRatingReason = async (
  requestData: MbsSysCttHttpGetContacterRatingReasonReqMdl
): Promise<MbsSysCttHttpGetContacterRatingReasonRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlGetContacterRatingReasonReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingReasonID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlGetContacterRatingReasonRspMdl>(
      "/api/MbsSystemContacter/GetContacterRatingReason",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const data = httpResponseData.data;

    const responseData: MbsSysCttHttpGetContacterRatingReasonRspMdl = {
      errorCode: data.aa,
      managerContacterRatingReasonName: data.a,
      managerContacterRatingReasonStatus: data.b,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-新增窗口開發評等原因 */
export const addContacterRatingReason = async (
  requestData: MbsSysCttHttpAddContacterRatingReasonReqMdl
): Promise<MbsSysCttHttpAddContacterRatingReasonRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlAddContacterRatingReasonReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingReasonName,
    b: requestData.managerContacterRatingReasonStatus,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlAddContacterRatingReasonRspMdl>(
      "/api/MbsSystemContacter/AddContacterRatingReason",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const data = httpResponseData.data;

    const responseData: MbsSysCttHttpAddContacterRatingReasonRspMdl = {
      errorCode: data.aa,
      managerContacterRatingReasonID: data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-更新窗口開發評等原因 */
export const updateContacterRatingReason = async (
  requestData: MbsSysCttHttpUpdateContacterRatingReasonReqMdl
): Promise<MbsSysCttHttpUpdateContacterRatingReasonRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlUpdateContacterRatingReasonReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingReasonID,
    b: requestData.managerContacterRatingReasonName,
    c: requestData.managerContacterRatingReasonStatus,
  };

  try {
    const httpResponseData =
      await apiJsonClient.post<MbsSysCttCtlUpdateContacterRatingReasonRspMdl>(
        "/api/MbsSystemContacter/UpdateContacterRatingReason",
        httpRequestData
      );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysCttHttpUpdateContacterRatingReasonRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//--------------------------------------------------------------------------------
//#endregion

//#region 管理者後台-系統-名單窗口-窗口開發評等
//----------------------------------------------------------------------------------
/** 管理者後台-系統-名單窗口-取得多筆窗口開發評等 */
export const getManyContacterRating = async (
  requestData: MbsSysCttHttpGetManyContacterRatingReqMdl
): Promise<MbsSysCttHttpGetManyContacterRatingRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlGetManyContacterRatingReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingReasonID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlGetManyContacterRatingRspMdl>(
      "/api/MbsSystemContacter/GetManyContacterRating",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysCttHttpGetManyContacterRatingRspMdl = {
      errorCode: httpResponseData.data.aa,
      managerContacterRatingList:
        httpResponseData.data.a?.map(
          (item: MbsSysCttCtlGetManyContacterRatingRspItemMdl) =>
            ({
              managerContacterRatingID: item.a,
              managerContacterRatingReasonName: item.b,
              managerContacterRatingRemark: item.c,
            }) satisfies MbsSysCttHttpGetManyContacterRatingRspItemMdl
        ) ?? [],
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-取得單筆窗口開發評等 */
export const getContacterRating = async (
  requestData: MbsSysCttHttpGetContacterRatingReqMdl
): Promise<MbsSysCttHttpGetContacterRatingRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlGetContacterRatingReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlGetContacterRatingRspMdl>(
      "/api/MbsSystemContacter/GetContacterRating",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const data = httpResponseData.data;

    const responseData: MbsSysCttHttpGetContacterRatingRspMdl = {
      errorCode: data.aa,
      managerContacterRatingReasonID: data.a,
      managerContacterRatingRemark: data.b,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-新增窗口開發評等 */
export const addContacterRating = async (
  requestData: MbsSysCttHttpAddContacterRatingReqMdl
): Promise<MbsSysCttHttpAddContacterRatingRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlAddContacterRatingReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingID,
    b: requestData.managerContacterRatingReasonID,
    c: requestData.managerContacterRatingRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlAddContacterRatingRspMdl>(
      "/api/MbsSystemContacter/AddContacterRating",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const data = httpResponseData.data;

    const responseData: MbsSysCttHttpAddContacterRatingRspMdl = {
      errorCode: data.aa,
      managerContacterRatingID: data.a,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-移除窗口開發評等 */
export const updateContacterRating = async (
  requestData: MbsSysCttHttpUpdateContacterRatingReqMdl
): Promise<MbsSysCttHttpUpdateContacterRatingRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlUpdateContacterRatingReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingID,
    b: requestData.managerContacterRatingReasonID,
    c: requestData.managerContacterRatingRemark,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlUpdateContacterRatingRspMdl>(
      "/api/MbsSystemContacter/UpdateContacterRating",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysCttHttpUpdateContacterRatingRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};

/** 管理者後台-系統-名單窗口-更新窗口開發評等 */
export const removeContacterRating = async (
  requestData: MbsSysCttHttpRemoveContacterRatingReqMdl
): Promise<MbsSysCttHttpRemoveContacterRatingRspMdl | null> => {
  const httpRequestData: MbsSysCttCtlRemoveContacterRatingReqMdl = {
    aa: requestData.employeeLoginToken,
    a: requestData.managerContacterRatingID,
  };

  try {
    const httpResponseData = await apiJsonClient.post<MbsSysCttCtlRemoveContacterRatingRspMdl>(
      "/api/MbsSystemContacter/RemoveContacterRating",
      httpRequestData
    );

    if (!httpResponseData?.data) return null;

    const responseData: MbsSysCttHttpRemoveContacterRatingRspMdl = {
      errorCode: httpResponseData.data.aa,
    };

    return responseData;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error("API錯誤:", axiosError.response?.data ?? error);
    return null;
  }
};
//--------------------------------------------------------------------------------
//#endregion
