/** 資料庫-原子-員工-商機發票紀錄-狀態 */
export enum DbAtomEmployeePipelineBillStatusEnum {
  /** 未定義 */
  Undefined = 0,
  /** 未結案 */
  NotCompleted = 1,
  /** 處理中 */
  InProgress = 2,
  /** 已結案 */
  Completed = 3,
}
