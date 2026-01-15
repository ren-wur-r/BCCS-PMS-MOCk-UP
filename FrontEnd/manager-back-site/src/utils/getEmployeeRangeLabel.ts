import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";

/**
 * 取得人員規模文字標籤
 * @param value 人員規模列舉
 * @returns 顯示文字（未選擇 / 未定義 / 不清楚 / 少於100人 / 100–500人 / 500–1000人 / 1000人以上）
 */
export const getEmployeeRangeLabel = (value: DbAtomEmployeeRangeEnum | null): string => {
  switch (value) {
    case DbAtomEmployeeRangeEnum.NotSelected:
      return "未選擇";
    case DbAtomEmployeeRangeEnum.Undefined:
      return "未定義";
    case DbAtomEmployeeRangeEnum.Unclear:
      return "不清楚";
    case DbAtomEmployeeRangeEnum.LessThan100:
      return "少於100人";
    case DbAtomEmployeeRangeEnum.Range100To500:
      return "100–500人";
    case DbAtomEmployeeRangeEnum.Range500To1000:
      return "500–1000人";
    case DbAtomEmployeeRangeEnum.MoreThan1000:
      return "1000人以上";
    default:
      return "-";
  }
};
