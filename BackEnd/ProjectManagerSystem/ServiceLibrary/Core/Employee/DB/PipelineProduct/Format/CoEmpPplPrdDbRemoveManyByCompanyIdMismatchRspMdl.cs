namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

/// <summary>核心-員工-商機產品-資料庫-移除多筆根據公司ID不匹配-回應模型</summary>
public class CoEmpPplPrdDbRemoveManyByCompanyIdMismatchRspMdl
{
    /// <summary>影響筆數</summary>
    public int AffectedCount { get; set; }
}
