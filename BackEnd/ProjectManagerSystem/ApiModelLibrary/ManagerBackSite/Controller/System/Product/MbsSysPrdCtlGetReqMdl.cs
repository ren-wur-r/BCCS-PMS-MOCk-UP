using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得-請求模型</summary>
public class MbsSysPrdCtlGetReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerProductID { get; set; }
}