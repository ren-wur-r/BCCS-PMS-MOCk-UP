using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

/// <summary>核心-員工-商機原始資料-資料庫-取得多筆公司名稱從[管理者後台-基本]-回應模型</summary>
public class CoEmpPplOgnDbGetManyMgrComNameFromMbsBscRspMdl
{
    /// <summary>管理者公司名稱列表</summary>
    public List<string> ManagerCompanyNameList { get; set; }
}
