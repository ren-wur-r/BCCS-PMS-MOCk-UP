/** 資料庫-原子-商機-狀態-列舉 */
export enum DbAtomPipelineStatusEnum {
  /** 未定義 */
  Undefined = 0,
  /** 連字號 */
  Hyphen = 1,
  /** eDM已開啟 */
  Opened = 2,
  /** eDM已點擊 */
  Clicked = 3,
  /** 已轉電銷 */
  TransferredToSales = 4,
  /** 已完成電銷 */
  CompletedSales = 5,
  /** 已轉業務 */
  TransferredToBusiness = 6,
  /** 商機10%：業務接受 或 booking 接受 */
  Business10Percent = 7,
  /** 商機30% */
  Business30Percent = 8,
  /** 商機70% */
  Business70Percent = 9,
  /** 商機100% */
  Business100Percent = 10,
  /** 商機失敗 */
  BusinessFailed = 11,
  /** 已轉專案 */
  TransferredToProject = 12,
}