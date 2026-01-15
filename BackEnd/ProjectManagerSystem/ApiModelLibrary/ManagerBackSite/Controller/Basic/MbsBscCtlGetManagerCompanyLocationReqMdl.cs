using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得公司營業地點-請求模型</summary>
public class MbsBscCtlGetManagerCompanyLocationReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點-是否已刪除</summary>
    [JsonPropertyName("b")]
    public bool? ManagerCompanyLocationIsDeleted { get; set; }
}
