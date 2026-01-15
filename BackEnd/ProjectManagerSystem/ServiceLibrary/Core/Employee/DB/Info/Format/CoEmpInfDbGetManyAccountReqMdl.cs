using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆[帳號]-請求模型</summary>
public class CoEmpInfDbGetManyAccountReqMdl
{
    /// <summary>員工-ID列表</summary>
    public List<int> EmployeeIdList { get; set; }
}
