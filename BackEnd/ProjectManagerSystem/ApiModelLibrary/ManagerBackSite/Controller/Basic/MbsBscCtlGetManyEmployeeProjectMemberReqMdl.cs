using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員-請求模型</summary>
public class MbsBscCtlGetManyEmployeeProjectMemberReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }
}