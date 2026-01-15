using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-新增-回應模型</summary>
public class MbsSysEmpCtlAddRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeID { get; set; }
}