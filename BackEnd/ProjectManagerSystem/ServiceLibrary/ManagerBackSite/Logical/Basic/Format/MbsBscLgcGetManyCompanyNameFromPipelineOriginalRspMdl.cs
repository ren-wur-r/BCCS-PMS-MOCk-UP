using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司名稱從[商機原始]-回應模型</summary>
public class MbsBscLgcGetManyCompanyNameFromPipelineOriginalRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者公司名稱-列表</summary>
    public List<string> ManagerCompanyNameList { get; set; }
}
