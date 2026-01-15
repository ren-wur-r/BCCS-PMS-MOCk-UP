namespace DataModelLibrary.GlobalSystem.EnvVar;

/// <summary>全系統(社交工程控制)-環境參數-常數</summary>
public static class GsEnvironmentVariableConst
{
    #region ASP NET CORE

    /// <summary>ASP NET CORE環境</summary>
    public static string ASPNETCORE_ENVIRONMENT { get; } = "ASPNETCORE_ENVIRONMENT";

    #endregion

    #region DB

    /// <summary>DB-主要-連線字串</summary>
    public static string DB_MAIN_CONNECTION { get; } = "DB_MAIN_CONNECTION";

    #endregion

    #region Swagger

    /// <summary>Swagger-是否啟用</summary>
    public static string SWAGGER_IS_ENABLE { get; } = "SWAGGER_IS_ENABLE";

    #endregion

}
