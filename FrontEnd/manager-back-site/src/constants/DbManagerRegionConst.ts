/** 資料庫-管理者-地區-常數 */
export class DbManagerRegionConst {
  /** 無權限 */
  static readonly Denied = {
    /** ID */
    ID: -2,
    /** 名稱 */
    Name: "無權限",
    /** 是否啟用 */
    IsEnabled: true,
  } as const;

  /** 全區 */
  static readonly AllRegion = {
    /** ID */
    ID: -1,
    /** 名稱 */
    Name: "全區", 
    /** 是否啟用 */
    IsEnabled: true,
  } as const;
}