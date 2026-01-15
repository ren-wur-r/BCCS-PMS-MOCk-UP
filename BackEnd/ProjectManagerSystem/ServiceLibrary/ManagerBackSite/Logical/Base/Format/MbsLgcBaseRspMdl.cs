using ApiModelLibrary.ManagerBackSite.Common;

namespace ServiceLibrary.ManagerBackSite.Logical.Base.Format;

/// <summary>管理者後台-邏輯-基底-回應模型</summary>
public class MbsLgcBaseRspMdl
{
    /// <summary>錯誤代碼</summary>
    public MbsErrorCodeEnum ErrorCode { get; set; }
}
