using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得子分類-請求模型</summary>
public class MbsSysPrdCtlGetSubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品子分類-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerProductSubKindID { get; set; }
}