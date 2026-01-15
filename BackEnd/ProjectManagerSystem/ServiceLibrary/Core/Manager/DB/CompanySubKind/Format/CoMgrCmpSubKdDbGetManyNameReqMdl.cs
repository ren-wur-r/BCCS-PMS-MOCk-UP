using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

/// <summary>核心-管理者-公司子分類-資料庫-取得多筆[名稱]-請求模型</summary>
public class CoMgrCmpSubKdDbGetManyNameReqMdl
{
    /// <summary>公司子分類Id列表</summary>
    public List<int> ManagerCompanySubKindIdList { get; set; }
}