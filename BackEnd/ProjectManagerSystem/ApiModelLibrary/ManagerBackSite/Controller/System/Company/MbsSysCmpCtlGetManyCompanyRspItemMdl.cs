using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomCustomerGrade;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司-回應項目模型</summary>
public class MbsSysCmpCtlGetManyCompanyRspItemMdl
{
    /// <summary>管理者公司-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerCompanyName { get; set; }

    /// <summary>客戶分級</summary>
    [JsonPropertyName("d")]
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>管理者公司主分類-名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者公司子分類-名稱</summary>
    [JsonPropertyName("f")]
    public string ManagerCompanySubKindName { get; set; }
}