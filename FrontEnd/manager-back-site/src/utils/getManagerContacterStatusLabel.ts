import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";

/**
 * 取得聯絡人狀態文字標籤
 * @param value 聯絡人狀態列舉
 * @returns 顯示文字（未定義 / 不清楚 / 在職 / 離職）
 */
export const getManagerContacterStatusLabel = (
  value: DbAtomManagerContacterStatusEnum | null
): string => {
  switch (value) {
    case DbAtomManagerContacterStatusEnum.Undefined:
      return "未定義";
    case DbAtomManagerContacterStatusEnum.Unknown:
      return "不清楚";
    case DbAtomManagerContacterStatusEnum.Employed:
      return "在職";
    case DbAtomManagerContacterStatusEnum.Unemployed:
      return "離職";
    default:
      return "-";
  }
};
