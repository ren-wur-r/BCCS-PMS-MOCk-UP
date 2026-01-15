using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-更新專案-請求模型</summary>
public class MbsWrkPrjCtlUpdateProjectReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案-代碼</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案-名稱</summary>
    [JsonPropertyName("c")]
    public string EmployeeProjectName { get; set; }
}