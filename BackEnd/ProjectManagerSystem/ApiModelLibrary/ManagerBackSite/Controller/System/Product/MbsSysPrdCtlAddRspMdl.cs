using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-新增-回應模型</summary>
public class MbsSysPrdCtlAddRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-產品-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductID { get; set; }
}