using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-新增商機-請求模型</summary>
public class MbsCrmPplCtlAddEmployeePipelineReqMdl : MbsCtlBaseReqMdl
{
    #region 基本資訊

    /// <summary>管理者公司ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司據點ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    [Required]
    [JsonPropertyName("c")]
    [Range(7, 10, ErrorMessage = "Invalid parameters")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>商機-承辦業務員工ID</summary>
    [Required]
    [JsonPropertyName("d")]
    public int EmployeePipelineSalerEmployeeID { get; set; }

    #endregion

    /// <summary>商機窗口列表</summary>
    [JsonPropertyName("e")]
    public List<MbsCrmPplCtlAddEmployeePipelineReqContacterItemMdl> ContacterList { get; set; }

    /// <summary>商機業務開發紀錄列表</summary>
    [JsonPropertyName("f")]
    public List<MbsCrmPplCtlAddEmployeePipelineReqSalerTrackingItemMdl> SalerTrackingList { get; set; }

    /// <summary>商機產品列表</summary>
    [JsonPropertyName("g")]
    public List<MbsCrmPplCtlAddEmployeePipelineReqProductItemMdl> ProductList { get; set; }

    /// <summary>商機發票紀錄列表</summary>
    [JsonPropertyName("h")]
    public List<MbsCrmPplCtlAddEmployeePipelineReqBillItemMdl> BillList { get; set; }

    /// <summary>商機履約期限列表</summary>
    [JsonPropertyName("i")]
    public List<MbsCrmPplCtlAddEmployeePipelineReqDueItemMdl> DueList { get; set; }
}
