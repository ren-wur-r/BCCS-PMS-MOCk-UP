using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomCustomerGrade;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司-請求模型</summary>
public class MbsSysCmpCtlGetManyCompanyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>客戶分級</summary>
    [JsonPropertyName("a")]
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    [JsonPropertyName("b")]
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    [JsonPropertyName("c")]
    public int? ManagerCompanySubKindID { get; set; }

    /// <summary>管理者公司-名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerCompanyName { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("e")]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [Required]
    [JsonPropertyName("f")]
    public int PageSize { get; set; }
}