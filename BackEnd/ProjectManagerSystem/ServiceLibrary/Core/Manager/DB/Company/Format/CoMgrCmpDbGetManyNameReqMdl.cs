using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得多筆[名稱]-請求模型</summary>
public class CoMgrCmpDbGetManyNameReqMdl
{
    /// <summary>管理者公司ID列表</summary>
    public List<int> ManagerCompanyIdList { get; set; }
}