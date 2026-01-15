import { defineStore } from "pinia";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import type { MbsWrkPrjHttpGetManyMemberRoleRspItemMdl } from "@/services/pms-http/work/project/workProjectHttpFormat";

/** 專案成員角色 Store
 *  用在判斷專案成員的角色，以決定【專案管理】頁面顯示內容
 */
export const useProjectMemberRoleStore = defineStore("projectMemberRole", {
  state: () => ({
    memberRoleList: [] as MbsWrkPrjHttpGetManyMemberRoleRspItemMdl[],
  }),

  actions: {
    /** 設定角色列表 */
    setMemberRoleList(list: MbsWrkPrjHttpGetManyMemberRoleRspItemMdl[]) {
      this.memberRoleList = list;
    },

    /** 清空資料 */
    clear() {
      this.memberRoleList = [];
    },

    /** 判斷角色是否可顯示元件 */
    canShow(requiredRoles: DbEmployeeProjectMemberRoleEnum | DbEmployeeProjectMemberRoleEnum[]) {
      const required = Array.isArray(requiredRoles) ? requiredRoles : [requiredRoles];

      return required.some((r) =>
        this.memberRoleList.some((item) => item.employeeProjectMemberRole === r)
      );
    },
  },
});
