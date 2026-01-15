import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";

/**
 * 取得活動類型文字標籤
 * @param value 活動類型列舉
 * @returns 顯示文字（未定義 / 實體 / 線上）
 */
export const getManagerActivityKindLabel = (value: DbAtomManagerActivityKindEnum): string => {
  switch (value) {
    case DbAtomManagerActivityKindEnum.Undefined:
      return "未定義";
    case DbAtomManagerActivityKindEnum.PhysicalActivity:
      return "實體";
    case DbAtomManagerActivityKindEnum.OnlineActivity:
      return "線上";
    default:
      return "-";
  }
};

/**
 * 取得完整活動類型文字標籤
 * @param value 活動類型列舉
 * @returns 顯示文字（未定義 / 實體活動 / 線上活動）
 */
export const getFullManagerActivityKindLabel = (value: DbAtomManagerActivityKindEnum): string => {
  switch (value) {
    case DbAtomManagerActivityKindEnum.Undefined:
      return "未定義";
    case DbAtomManagerActivityKindEnum.PhysicalActivity:
      return "實體活動";
    case DbAtomManagerActivityKindEnum.OnlineActivity:
      return "線上活動";
    default:
      return "-";
  }
};
