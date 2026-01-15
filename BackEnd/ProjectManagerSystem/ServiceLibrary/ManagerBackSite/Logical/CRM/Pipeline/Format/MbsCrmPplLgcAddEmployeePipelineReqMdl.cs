using System.Collections.Generic;
using DataModelLibrary.Database.AtomPipeline;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增名單-請求模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineReqMdl : MbsLgcBaseReqMdl
{
    #region 基本資訊

    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司據點ID</summary>
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>商機-承辦業務員工ID</summary>
    public int EmployeePipelineSalerEmployeeID { get; set; }

    #endregion

    /// <summary>商機窗口列表</summary>
    public List<MbsCrmPplLgcAddEmployeePipelineReqContacterItemMdl> ContacterList { get; set; }

    /// <summary>商機業務開發紀錄列表</summary>
    public List<MbsCrmPplLgcAddEmployeePipelineReqSalerTrackingItemMdl> SalerTrackingList { get; set; }

    /// <summary>商機產品列表</summary>
    public List<MbsCrmPplLgcAddEmployeePipelineReqProductItemMdl> ProductList { get; set; }

    /// <summary>商機發票紀錄列表</summary>
    public List<MbsCrmPplLgcAddEmployeePipelineReqBillItemMdl> BillList { get; set; }

    /// <summary>商機履約期限列表</summary>
    public List<MbsCrmPplLgcAddEmployeePipelineReqDueItemMdl> DueList { get; set; }
}
