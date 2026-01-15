/** 資料庫-原子-員工商機業務-狀態 */
export enum DbAtomEmployeePipelineSalerStatusEnum {
  /** 未定義 */
  Undefined = 0,
  /** 尚未回覆  */
  Pending = 1,
  /** 接受  */
  Accepted = 2,
  /** 拒絕  */
  Rejected = 3,
  /** 轉指派業務  */
  Reassigned = 4,
}
