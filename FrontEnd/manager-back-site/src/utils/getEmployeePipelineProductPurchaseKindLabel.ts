import { DbAtomEmployeePipelineProductPurchaseKindEnum } from "@/constants/DbAtomEmployeePipelineProductPurchaseKindEnum";

/**
 * 取得採購類型文字標籤
 * @param value 採購類型列舉
 * @returns 顯示文字（未選擇 / 未定義 / 新購 / 續約）
 */
export const getEmployeePipelineProductPurchaseKindLabel = (
  value: DbAtomEmployeePipelineProductPurchaseKindEnum | null
): string => {
  switch (value) {
    case DbAtomEmployeePipelineProductPurchaseKindEnum.NotSelected:
      return "未選擇";
    case DbAtomEmployeePipelineProductPurchaseKindEnum.Undefined:
      return "未定義";
    case DbAtomEmployeePipelineProductPurchaseKindEnum.NewlyPurchase:
      return "新購";
    case DbAtomEmployeePipelineProductPurchaseKindEnum.Renewal:
      return "續約";
    default:
      return "-";
  }
};
