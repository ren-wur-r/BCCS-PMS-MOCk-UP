using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

/// <summary>核心-員工-商機產品-資料庫-取得多筆-回應模型</summary>
public class CoEmpPplPrdDbGetManyFromPhoneRspMdl
{
    /// <summary>商機產品清單</summary>
    public List<CoEmpPplPrdDbGetManyFromPhoneRspItemMdl> EmployeePipelineProductList { get; set; }
}