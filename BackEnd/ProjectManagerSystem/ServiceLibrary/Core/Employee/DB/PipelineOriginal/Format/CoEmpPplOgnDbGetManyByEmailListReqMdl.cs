using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

/// <summary>核心-員工-商機原始資料-資料庫-取得多筆從[Email列表]-請求模型</summary>
public class CoEmpPplOgnDbGetManyByEmailListReqMdl
{
    /// <summary>活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>窗口Email列表</summary>
    public List<string> ManagerContacterEmailList { get; set; }
}
