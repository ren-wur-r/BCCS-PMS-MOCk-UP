using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案-請求模型</summary>
public class MbsWrkPrjCtlGetManyProjectReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案狀態</summary>
    [JsonPropertyName("a")]
    public DbAtomEmployeeProjectStatusEnum? AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectName { get; set; }

    /// <summary>頁數</summary>
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [JsonPropertyName("d")]
    public int PageSize { get; set; }
}