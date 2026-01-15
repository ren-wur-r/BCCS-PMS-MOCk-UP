/** 管理者後台-錯誤代碼-列舉 */
export enum MbsErrorCodeEnum {
  /** 未定義 */
  Undefined = 0,

  /** 成功 */
  Success = 1,

  /** 失敗 */
  Fail = 2,

  /** 帳號或密碼錯誤 */
  AccountOrPasswordError = 101,

  /** 登入令牌錯誤 */
  LoginTokenError = 103,

  /** 權限不足 */
  PermissionDenied = 104,

  /** 帳號停用 */
  AccountDisabled = 105,

  /** 帳號重複 */
  AccountDuplicate = 106,

  /** 名稱重複 */
  NameDuplicate = 107,

  /** 資料已存在 */
  DataAlreadyExists = 108,

  /** 附件格式錯誤 */
  FileFormatError = 109,

  /** 資料已存在 */
  FileContentError = 110,

  /** 專案代碼重複 */
  ProjectCodeDuplicate = 111,

  /** 統一編號重複 */
  UniformNumberDuplicate = 112,

  /** 商機已轉換為專案 */
  PipelineAlreadyTransferred = 113,

  /** 窗口Email重複 */
  ContacterEmailDuplicate = 114,

  /** 商機階段條件未達成 */
  PipelineStageConditionNotMet = 115,
}
