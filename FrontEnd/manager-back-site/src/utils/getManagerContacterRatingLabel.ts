import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";

/**
 * 取得開發評等文字標籤
 * @param value 開發評等列舉
 * @returns 顯示文字（未定義 / 白名單 / 灰名單 / 黑名單）
 */
export const getManagerContacterRatingLabel = (
  value: DbAtomManagerContacterRatingKindEnum | null
): string => {
  switch (value) {
    case DbAtomManagerContacterRatingKindEnum.Undefined:
      return "未定義";
    case DbAtomManagerContacterRatingKindEnum.Whitelist:
      return "白名單";
    case DbAtomManagerContacterRatingKindEnum.Graylist:
      return "灰名單";
    case DbAtomManagerContacterRatingKindEnum.Blacklist:
      return "黑名單";
    default:
      return "-";
  }
};
