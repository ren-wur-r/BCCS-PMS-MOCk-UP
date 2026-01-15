using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機-聯絡人項目-請求模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineReqContacterItemMdl : MbsLgcBaseReqMdl
{
    /// <summary>經理人聯絡人ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>商機聯絡人是否為主要聯絡人</summary>
    public bool EmployeePipelineContacterIsPrimary { get; set; }
}
