/** 資料庫-原子-權限-類型-列舉 */
export enum DbAtomPermissionKindEnum {
  /** 未定義 */
  Undefined = 0,
  /** 無權限 */
  Denied = 1,
  /** 自身 */
  Self = 2,
  /** 所有人 */
  Everyone = 3,
}
