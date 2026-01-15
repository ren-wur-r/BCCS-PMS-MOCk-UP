import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";

/**
 * 取得公司狀態文字標籤
 * @param value 公司狀態列舉
 * @returns 顯示文字（未選擇 / 未定義 / 營運中 / 停業 / 不清楚）
 */
export const getManagerCompanyStatusLabel = (
  value: DbAtomManagerCompanyStatusEnum | null
): string => {
  switch (value) {
    case DbAtomManagerCompanyStatusEnum.NotSelected:
      return "未選擇";
    case DbAtomManagerCompanyStatusEnum.Undefined:
      return "未定義";
    case DbAtomManagerCompanyStatusEnum.Operating:
      return "營運中";
    case DbAtomManagerCompanyStatusEnum.Closed:
      return "停業";
    case DbAtomManagerCompanyStatusEnum.Unclear:
      return "不清楚";
    default:
      return "-";
  }
};
