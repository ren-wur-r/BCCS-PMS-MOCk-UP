using System.Collections.Generic;
using DataModelLibrary.Database.AtomPipeline;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料邏輯層-批次更新商機狀態-請求模型</summary>
public class CoEmpPplDbUpdateManyAtomPipelineStatusReqMdl
{
    /// <summary>員工商機ID列表</summary>
    public List<int> EmployeePipelineIDList { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }
}
