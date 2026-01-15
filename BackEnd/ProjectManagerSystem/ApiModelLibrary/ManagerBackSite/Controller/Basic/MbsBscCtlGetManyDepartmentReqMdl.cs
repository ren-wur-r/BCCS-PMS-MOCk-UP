using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-部門-取得多筆-請求模型</summary>
public class MbsBscCtlGetManyDepartmentReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>部門-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>是否啟用</summary>
    [JsonPropertyName("b")]
    public bool? ManagerDepartmentIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }
}