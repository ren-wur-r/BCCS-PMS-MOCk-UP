using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-新增主分類-回應模型</summary>
public class MbsSysPrdCtlAddMainKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductMainKindID { get; set; }
}