using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

/// <summary>核心-員工-商機產品-資料庫-取得多筆從[管理者後台-CRM-商機管理]-回應模型</summary>
public class CoEmpPplPrdDbGetManyFromCrmPplRspMdl
{
    /// <summary>商機產品列表</summary>
    public List<CoEmpPplPrdDbGetManyFromCrmPplRspItemMdl> EmployeePipelineProductList { get; set; }
}