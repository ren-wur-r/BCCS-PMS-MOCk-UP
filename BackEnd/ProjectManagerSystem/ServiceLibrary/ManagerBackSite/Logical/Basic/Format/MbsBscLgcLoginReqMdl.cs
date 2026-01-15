using System.Net;
using System.Text.Json.Serialization;
using CommonLibrary.CmnJson;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-登入-請求模型</summary>
public class MbsBscLgcLoginReqMdl
{
    /// <summary>員工-帳號</summary>
    public string EmployeeAccount { get; set; }

    /// <summary>員工密碼</summary>
    public string EmployeePassword { get; set; }

    /// <summary>操作者IP</summary>
    [JsonConverter(typeof(CmnIpAddressJsonConverter))]
    public IPAddress OperatorIP { get; set; }

}

