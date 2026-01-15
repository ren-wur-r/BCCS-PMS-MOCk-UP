using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案支出-請求模型</summary>
public class MbsWrkPrjCtlGetExpenseReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案支出ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectExpenseID { get; set; }
}