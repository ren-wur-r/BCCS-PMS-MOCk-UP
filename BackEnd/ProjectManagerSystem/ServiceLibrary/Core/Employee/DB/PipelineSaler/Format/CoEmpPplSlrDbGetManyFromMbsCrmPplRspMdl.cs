using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

/// <summary>核心-員工-商機業務-資料庫-取得多筆[管理者後台-CRM-商機管理]-回應模型</summary>
public class CoEmpPplSlrDbGetManyFromMbsCrmPplRspMdl
{
    /// <summary>商機業務紀錄列表</summary>
    public List<CoEmpPplSlrDbGetManyFromMbsCrmPplRspItemMdl> EmployeePipelineSalerList { get; set; }
}