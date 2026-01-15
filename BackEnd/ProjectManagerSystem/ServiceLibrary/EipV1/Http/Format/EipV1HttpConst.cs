namespace ServiceLibrary.EipV1.Http.Format;

/// <summary>EipV1-Http-常數</summary>
public static class EipV1HttpConst
{
    /// <summary>HttpClient名稱</summary>
    public static string HTTP_CLIENT_NAME { get; } = "EipV1";

    /// <summary>登入API</summary>
    public static string LOGIN { get; } = "/api/Login";

    /// <summary>PMS API - 查詢專案費用</summary>
    public static string PMS_GET_PROJECT_EXPENSES { get; } = "/api/PMS/GetProjectExpenses";

    /// <summary>PMS API - 查詢專案旅費</summary>
    public static string PMS_GET_PROJECT_TRAVEL_EXPENSES { get; } = "/api/PMS/GetProjectTravelExpenses";
}