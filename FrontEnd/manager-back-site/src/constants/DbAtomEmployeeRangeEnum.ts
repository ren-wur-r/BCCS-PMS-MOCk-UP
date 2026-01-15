/** 資料庫-原子-人員規模-類型 */
export enum DbAtomEmployeeRangeEnum {
 /** 未選擇 */
  NotSelected = -1,
  /** 未定義 */
 Undefined = 0,
 /** 不清楚 */
 Unclear = 1,
 /** 少於100人 */
 LessThan100 = 2,
 /** 100-500人 */
 Range100To500 = 3,
 /** 500-1000人 */
 Range500To1000 = 4,
 /** 1000人以上 */
 MoreThan1000 = 5
}