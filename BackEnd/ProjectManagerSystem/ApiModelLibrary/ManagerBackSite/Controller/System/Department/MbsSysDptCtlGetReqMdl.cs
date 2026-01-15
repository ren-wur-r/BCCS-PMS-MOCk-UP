using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Department;

/// <summary>管理者後台-系統-部門-控制器-取得單筆-請求模型</summary>
public class MbsSysDptCtlGetReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-部門-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerDepartmentID { get; set; }
}