/** 資料庫-原子-管理者-公司狀態-列舉 */
export enum DbAtomManagerCompanyStatusEnum {
  /** 未選擇 */
  NotSelected = -1,
  /** 未定義 */
  Undefined = 0,
  /** 營運中 */
  Operating = 1,
  /** 停業 */
  Closed = 2,
  /** 不清楚 */
  Unclear = 3
}
