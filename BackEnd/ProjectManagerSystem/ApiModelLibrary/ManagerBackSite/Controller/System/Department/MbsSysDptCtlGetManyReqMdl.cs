using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Department;

/// <summary>管理者後台-系統-部門-控制器-取得多筆-請求模型</summary>
public class MbsSysDptCtlGetManyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-部門-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者-部門-是否啟用</summary>
    [JsonPropertyName("b")]
    public bool? ManagerDepartmentIsEnable { get; set; }

    /// <summary>頁數</summary>
    [JsonPropertyName("c")]
    [Required]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [JsonPropertyName("d")]
    [Required]
    public int PageSize { get; set; }
}