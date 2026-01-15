export enum DbAtomPhoneSalesCustomerStatusEnum {
  PendingContact = 1,
  Contacted = 2,
  Appointed = 3,
  Dispatched = 4,
  NoContact = 5
}

export const DbAtomPhoneSalesCustomerStatusLabels: Record<DbAtomPhoneSalesCustomerStatusEnum, string> = {
  [DbAtomPhoneSalesCustomerStatusEnum.PendingContact]: "待聯繫",
  [DbAtomPhoneSalesCustomerStatusEnum.Contacted]: "已聯繫",
  [DbAtomPhoneSalesCustomerStatusEnum.Appointed]: "已約訪",
  [DbAtomPhoneSalesCustomerStatusEnum.Dispatched]: "已派送",
  [DbAtomPhoneSalesCustomerStatusEnum.NoContact]: "不再聯繫"
};

export const DbAtomPhoneSalesCustomerStatusColors: Record<DbAtomPhoneSalesCustomerStatusEnum, string> = {
  [DbAtomPhoneSalesCustomerStatusEnum.PendingContact]: "bg-gray-100 text-gray-800",
  [DbAtomPhoneSalesCustomerStatusEnum.Contacted]: "bg-blue-100 text-blue-800",
  [DbAtomPhoneSalesCustomerStatusEnum.Appointed]: "bg-green-100 text-green-800",
  [DbAtomPhoneSalesCustomerStatusEnum.Dispatched]: "bg-purple-100 text-purple-800",
  [DbAtomPhoneSalesCustomerStatusEnum.NoContact]: "bg-red-100 text-red-800"
};
