using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得公司營業地點-請求模型</summary>
public class MbsSysCmpCtlGetCompanyLocationReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyLocationID { get; set; }
}
