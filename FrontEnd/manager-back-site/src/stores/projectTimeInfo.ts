import { defineStore } from "pinia";
//儲存專案起始時間結束時間   用在判斷新增里程碑驗證是否超過專案時間範圍
export const useProjectTimeInfoStore = defineStore("workProject", {
  state: () => ({
    employeeProjectName: "",
    employeeProjectStartTime: "",
    employeeProjectEndTime: "",
  }),

  actions: {
    /** 設定專案詳細資料 */
    setProjectTime(detail: {
      employeeProjectName: string;
      employeeProjectStartTime: string;
      employeeProjectEndTime: string;
    }) {
      this.employeeProjectName = detail.employeeProjectName;
      this.employeeProjectStartTime = detail.employeeProjectStartTime;
      this.employeeProjectEndTime = detail.employeeProjectEndTime;
    },
  },
});
