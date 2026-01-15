export enum DbEmployeePipelineStageCheckEnum {
  Undefined = 0,
  Pending = 1,
  Confirmed = 2,
  NotApplicable = 3,
}

export const pipelineStageCheckOptions = [
  { label: "未確認", value: DbEmployeePipelineStageCheckEnum.Pending },
  { label: "已確認", value: DbEmployeePipelineStageCheckEnum.Confirmed },
  { label: "不適用", value: DbEmployeePipelineStageCheckEnum.NotApplicable },
];
