import { DbAtomEmployeePipelineSalerStatusEnum } from "@/constants/DbAtomEmployeePipelineSalerStatusEnum";

/**
 * 取得員工商機業務狀態文字標籤
 * @param value 員工商機業務狀態列舉
 * @returns 顯示文字（未定義 / 尚未回覆 / 接受 / 拒絕 / 轉指派業務）
 */
export const getEmployeePipelineSalerStatusLabel = (
  value: DbAtomEmployeePipelineSalerStatusEnum | null
): string => {
  switch (value) {
    case DbAtomEmployeePipelineSalerStatusEnum.Undefined:
      return "未定義";
    case DbAtomEmployeePipelineSalerStatusEnum.Pending:
      return "尚未回覆";
    case DbAtomEmployeePipelineSalerStatusEnum.Accepted:
      return "接受";
    case DbAtomEmployeePipelineSalerStatusEnum.Rejected:
      return "拒絕";
    case DbAtomEmployeePipelineSalerStatusEnum.Reassigned:
      return "轉指派業務";
    default:
      return "-";
  }
};