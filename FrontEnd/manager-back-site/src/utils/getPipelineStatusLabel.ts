import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
/**
 * 取得商機狀態顯示文字
 */
export const getPipelineStatusLabel = (value: DbAtomPipelineStatusEnum | null): string => {
  switch (value) {
    case DbAtomPipelineStatusEnum.Undefined:
      return "未定義";
    case DbAtomPipelineStatusEnum.Hyphen:
      return "-";
    case DbAtomPipelineStatusEnum.Opened:
      return "eDM已開啟";
    case DbAtomPipelineStatusEnum.Clicked:
      return "eDM已點擊";
    case DbAtomPipelineStatusEnum.TransferredToSales:
      return "已轉電銷";
    case DbAtomPipelineStatusEnum.CompletedSales:
      return "已完成電銷";
    case DbAtomPipelineStatusEnum.TransferredToBusiness:
      return "已轉業務";
    case DbAtomPipelineStatusEnum.Business10Percent:
      return "商機10%";
    case DbAtomPipelineStatusEnum.Business30Percent:
      return "商機30%";
    case DbAtomPipelineStatusEnum.Business70Percent:
      return "商機70%";
    case DbAtomPipelineStatusEnum.Business100Percent:
      return "商機100%";
    case DbAtomPipelineStatusEnum.BusinessFailed:
      return "商機失敗";
    case DbAtomPipelineStatusEnum.TransferredToProject:
      return "已轉專案";
    default:
      return "未知";
  }
};

export default getPipelineStatusLabel;
