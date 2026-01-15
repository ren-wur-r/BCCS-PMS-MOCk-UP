using System;

namespace DataModelLibrary.GlobalSystem.EnvVar;

/// <summary>全系統(社交工程控制)-環境參數</summary>
public class GsEnvironmentVariable
{
    /// <summary>AspNetCore</summary>
    public string AspNetCore { get; } = Environment.GetEnvironmentVariable(GsEnvironmentVariableConst.ASPNETCORE_ENVIRONMENT);

    #region DB

    /// <summary>DB-主要-連線字串</summary>
    public string DbMain { get; } = Environment.GetEnvironmentVariable(GsEnvironmentVariableConst.DB_MAIN_CONNECTION);

    #endregion

    #region Swagger

    /// <summary>Swagger-是否啟用</summary>
    public string SwaggerIsEnable { get; } = Environment.GetEnvironmentVariable(GsEnvironmentVariableConst.SWAGGER_IS_ENABLE);

    #endregion

}
