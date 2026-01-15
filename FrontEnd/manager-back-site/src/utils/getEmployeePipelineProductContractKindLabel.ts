import { DbAtomEmployeePipelineProductContractKindEnum } from "@/constants/DbAtomEmployeePipelineProductContractKindEnum";

/**
 * 取得合約類型文字標籤
 * @param value 合約類型列舉
 * @returns 顯示文字（未選擇 / 未定義 / 共契 / 標案 / 其他）
 */
export const getEmployeePipelineProductContractKindLabel = (
  value: DbAtomEmployeePipelineProductContractKindEnum | null
): string => {
  switch (value) {
    case DbAtomEmployeePipelineProductContractKindEnum.NotSelected:
      return "未選擇";
    case DbAtomEmployeePipelineProductContractKindEnum.Undefined:
      return "未定義";
    case DbAtomEmployeePipelineProductContractKindEnum.JointTendering:
      return "共契";
    case DbAtomEmployeePipelineProductContractKindEnum.Tender:
      return "標案";
    case DbAtomEmployeePipelineProductContractKindEnum.Other:
      return "其他";
    default:
      return "-";
  }
};
