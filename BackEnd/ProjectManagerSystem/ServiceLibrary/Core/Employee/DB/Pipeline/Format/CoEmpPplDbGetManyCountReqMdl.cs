using System.Collections.Generic;
using DataModelLibrary.Database.AtomPipeline;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-取得[筆數]從[商機狀態]-請求模型</summary>
public class CoEmpPplDbGetManyCountReqMdl
{
    /// <summary>活動ID列表</summary>
    public List<int> ManagerActivityIDList { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum? AtomPipelineStatus { get; set; }
}