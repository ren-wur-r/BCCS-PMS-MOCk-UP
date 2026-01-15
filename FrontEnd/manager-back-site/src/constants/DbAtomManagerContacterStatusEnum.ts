/** 資料庫-原子-管理者-聯絡人狀態-列舉 */
export enum DbAtomManagerContacterStatusEnum {
  // /** 未選擇 */
  // NotSelected = -1,
  /** 未定義 */
  Undefined = 0,
  /** 不清楚(預設) */
  Unknown = 1,
  /** 在職 */
  Employed = 2,
  /** 離職 */
  Unemployed = 3
}