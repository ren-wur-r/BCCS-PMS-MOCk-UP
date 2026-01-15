using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-取得單筆-請求模型</summary>
public class MbsSysEmpCtlGetReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int EmployeeID { get; set; }
}