using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-新增子分類-回應模型</summary>
public class MbsSysPrdCtlAddSubKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-產品子分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductSubKindID { get; set; }
}