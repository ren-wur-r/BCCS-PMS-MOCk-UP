using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機原始資料-資料庫-取得多筆[管理者後台-CRM-電銷管理]-回應模型</summary>
public class CoEmpPplDbGetManyFromMbsCrmPhnRspMdl
{
    /// <summary>商機列表</summary>
    public List<CoEmpPplDbGetManyFromMbsCrmPhnRspItemMdl> EmployeePipelineList { get; set; }
}