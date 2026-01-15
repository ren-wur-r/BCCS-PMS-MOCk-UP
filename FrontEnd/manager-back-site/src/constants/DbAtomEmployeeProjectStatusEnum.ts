/** 資料庫-原子-員工專案狀態 */
export enum DbAtomEmployeeProjectStatusEnum {
  /** 未定義 */
  Undefined = 0,
  /** 未指派 */
  NotAssigned = 1,
  /** 未開始 */
  NotStarted = 2,
  /** 如期 */
  OnSchedule = 3,
  /** 注意 */
  AtRisk = 4,
  /** 延遲 */
  Delayed = 5,
  /** 已完成 */
  Completed = 6,
}
