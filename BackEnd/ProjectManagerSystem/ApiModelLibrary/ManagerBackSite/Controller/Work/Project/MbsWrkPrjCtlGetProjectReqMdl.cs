using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-邏輯服務-取得專案-請求模型</summary>
public class MbsWrkPrjCtlGetProjectReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }
}
