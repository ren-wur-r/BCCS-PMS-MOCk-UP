export enum DbAtomPhoneSalesCustomerValueEnum {
  High = 1,
  Medium = 2,
  Low = 3,
  Pending = 4
}

export const DbAtomPhoneSalesCustomerValueLabels: Record<DbAtomPhoneSalesCustomerValueEnum, string> = {
  [DbAtomPhoneSalesCustomerValueEnum.High]: "高價值",
  [DbAtomPhoneSalesCustomerValueEnum.Medium]: "中價值",
  [DbAtomPhoneSalesCustomerValueEnum.Low]: "低價值",
  [DbAtomPhoneSalesCustomerValueEnum.Pending]: "待評估"
};

export const DbAtomPhoneSalesCustomerValueColors: Record<DbAtomPhoneSalesCustomerValueEnum, string> = {
  [DbAtomPhoneSalesCustomerValueEnum.High]: "bg-yellow-100 text-yellow-800 border-yellow-300",
  [DbAtomPhoneSalesCustomerValueEnum.Medium]: "bg-blue-100 text-blue-800 border-blue-300",
  [DbAtomPhoneSalesCustomerValueEnum.Low]: "bg-gray-100 text-gray-800 border-gray-300",
  [DbAtomPhoneSalesCustomerValueEnum.Pending]: "bg-orange-100 text-orange-800 border-orange-300"
};
