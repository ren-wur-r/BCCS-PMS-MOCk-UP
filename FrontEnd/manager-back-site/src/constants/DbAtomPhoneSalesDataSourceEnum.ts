export enum DbAtomPhoneSalesDataSourceEnum {
  Activity = 1,
  PhoneSales = 2,
  Import = 3
}

export const DbAtomPhoneSalesDataSourceLabels: Record<DbAtomPhoneSalesDataSourceEnum, string> = {
  [DbAtomPhoneSalesDataSourceEnum.Activity]: "活動管理",
  [DbAtomPhoneSalesDataSourceEnum.PhoneSales]: "電銷自建",
  [DbAtomPhoneSalesDataSourceEnum.Import]: "批次匯入"
};
