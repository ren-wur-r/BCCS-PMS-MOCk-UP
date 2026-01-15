namespace ApiModelLibrary.ManagerBackSite.Common;

/// <summary>管理者後台-錯誤代碼-列舉</summary>
public enum MbsErrorCodeEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>成功</summary>
    Success = 1,

    /// <summary>失敗</summary>
    Fail = 2,

    /// <summary>帳號或密碼錯誤</summary>
    AccountOrPasswordError = 101,

    /// <summary>登入令牌錯誤</summary>
    LoginTokenError = 103,

    /// <summary>權限不足</summary>
    PermissionDenied = 104,

    /// <summary>帳號停用</summary>
    AccountDisabled = 105,

    /// <summary>帳號重複</summary>
    AccountDuplicate = 106,

    /// <summary>名稱重複</summary>
    NameDuplicate = 107,

    /// <summary>資料已存在</summary>
    DataAlreadyExists = 108,

    /// <summary>附件格式錯誤</summary>
    FileFormatError = 109,

    /// <summary>附件內容錯誤</summary>
    FileContentError = 110,

    /// <summary>專案代碼重複</summary>
    ProjectCodeDuplicate = 111,

    /// <summary>統一編號重複</summary>
    UnifiedNumberDuplicate = 112,

    /// <summary>商機已轉換為專案</summary>
    PipelineAlreadyTransferred = 113,

    /// <summary>窗口Email重複</summary>
    ContacterEmailDuplicate = 114,

    /// <summary>商機階段條件未達成</summary>
    PipelineStageConditionNotMet = 115,
}
