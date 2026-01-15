using System.Net;
using System.Text.Json.Serialization;
using CommonLibrary.CmnJson;

namespace ServiceLibrary.ManagerBackSite.Logical.Base.Format;

/// <summary>管理者後台-邏輯-基底-請求模型</summary>
public class MbsLgcBaseReqMdl
{
    /// <summary>員工登入令牌</summary>
    public string EmployeeLoginToken { get; set; }

    /// <summary>操作者IP</summary>
    [JsonConverter(typeof(CmnIpAddressJsonConverter))]
    public IPAddress OperatorIP { get; set; }
}
